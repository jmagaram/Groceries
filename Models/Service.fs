namespace Models

open System
open System.Threading.Tasks
open System.Threading
open System.Runtime.CompilerServices
open FSharp.Control.Reactive
open StateTypes
open ServiceTypes

type Service(state: StateTypes.State, clock, cosmos: ICosmosConnector) =
    // Hack for now; probably better to make this step a part of the state observable
    let mutable isInitialized = false
    let stateSub = state |> Subject.behavior
    // Eventaully might want to use DistinctUntilChanged with
    // System.Collections.Generic.ReferenceEqualityComparer
    let stateObs = stateSub |> Observable.asObservable
    let isSynchronizing = false |> Subject.behavior

    let statusObs =
        isSynchronizing
        |> Observable.withLatestFrom (fun i j ->
            if i = true then Synchronizing
            elif j |> State.hasChanges then HasChanges
            else NoChanges) stateObs

    let update msg =
        let state' = stateSub.Value |> State.update clock msg
        stateSub |> Subject.onNext state' |> ignore
        state'

    // When using Blazor Server it seems to be ok to use the thread pool. I
    // don't know if this is good or bad performance-wise. It does require that
    // calls to StateHasChanged within Blazor components get wrapped in
    // InvokeAsync though. I do not yet know how this works with WebAssembly.
    let useThreadPool = false

    let startAsyncUnit (a: Async<unit>) =
        match useThreadPool with
        | true -> Async.StartAsTask a :> Task
        | false -> Async.StartImmediateAsTask a :> Task

    let pull since =
        async {
            let timeout =
                match since with
                | None -> TimeSpan.FromSeconds(10.0)
                | Some _ -> TimeSpan.FromSeconds(2.0)

            use source = new CancellationTokenSource(timeout)

            let! changes =
                match since with
                | None ->
                    "Synchronization: pulling everything" |> dprintln
                    cosmos.PullEverythingAsync source.Token
                | Some since ->
                    "Synchronization: pulling incremental" |> dprintln
                    cosmos.PullSinceAsync since source.Token
                |> Async.AwaitTask

            return changes |> Dto.changesAsImport
        }

    let push s =
        async {
            use source = new CancellationTokenSource(TimeSpan.FromSeconds(2.0))
            "Synchronization: pushing" |> dprintln

            match s |> Dto.pushRequest with
            | None -> ()
            | Some c -> return! cosmos.PushAsync c source.Token |> Async.AwaitTask
        }

    let sync since =
        async {
            isSynchronizing.OnNext(true)

            try
                let state = stateSub.Value
                let unsaved = stateSub.Value |> State.hasChanges
                if unsaved then do! push state

                if unsaved || (since |> Option.isNone) then
                    let! p = pull since

                    // Note that resolving the pulled changes can cause
                    // additional changes, such as if there are foreign key
                    // problems that are fixed after import.
                    match p with
                    | Some p -> update (Import p) |> ignore
                    | None -> ()
            with e -> "Synchronization: exception thrown" |> dprintln

            isSynchronizing.OnNext(false)
        }

    let syncIncremental = sync stateSub.Value.LastCosmosTimestamp
    let syncEverything = sync None

    let updateAsync msg =
        async {
            let state = update msg
            do! sync (state.LastCosmosTimestamp)
        }

    let initialize =
        async {
            if isInitialized = false then
                isInitialized <- true
                do! syncIncremental
        }

    new(cosmos) = Service(State.createDefault, clock, cosmos)

    member me.InitializeAsync() = initialize |> startAsyncUnit
    member me.UpdateAsync(msg) = updateAsync msg |> startAsyncUnit
    member me.SyncEverythingAsync() = syncEverything |> startAsyncUnit
    member me.SyncIncrementalAsync() = syncIncremental |> startAsyncUnit

    member me.State = stateObs
    member me.CurrentState = stateSub.Value
    member me.SyncronizationStatus = statusObs
    
