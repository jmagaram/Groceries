namespace Models

open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive
open CoreTypes
open StateTypes
open System.Threading.Tasks

type ICosmosConnector =
    abstract CreateDatabaseAsync: unit -> Task
    abstract DeleteDatabaseAsync: unit -> Task
    abstract PullSinceAsync: lastSync:int -> Task<DtoTypes.Changes>
    abstract PullEverythingAsync: unit -> Task<DtoTypes.Changes>
    abstract PushAsync: DtoTypes.Changes -> Task

type Service(state: StateTypes.State, clock, cosmos: ICosmosConnector) =
    let stateSub = state |> Subject.behavior
    let stateObs = stateSub |> Observable.asObservable

    let update msg =
        let state' = stateSub.Value |> State.update clock msg
        stateSub |> Subject.onNext state' |> ignore

    let startAsyncUnit (a: Async<unit>) = Async.StartAsTask a :> Task

    let pull t =
        async {
            let! changes =
                match t with
                | None -> cosmos.PullEverythingAsync()
                | Some t -> cosmos.PullSinceAsync(t)
                |> Async.AwaitTask

            let import = changes |> Dto.changesAsImport
            let message = import |> Import
            update message
        }

    let push =
        async {
            match stateSub.Value |> Dto.pushRequest with
            | None -> ()
            | Some changes -> changes |> cosmos.PushAsync |> Async.AwaitTask |> ignore
        }

    new(cosmos) = Service(State.createDefault, clock, cosmos)

    member me.Update(msg) = update msg
    member me.Current = stateSub.Value
    member me.PushRequest() = stateSub.Value |> Dto.pushRequest
    member me.PullEverything() = pull None |> startAsyncUnit
    member me.PullIncremental() = pull (stateSub.Value.LastCosmosTimestamp) |> startAsyncUnit
    member me.Push() = push |> startAsyncUnit
    member me.StoreEditPage = stateObs |> Observable.choose (fun i -> i.StoreEditPage)
    member me.CategoryEditPage = stateObs |> Observable.choose (fun i -> i.CategoryEditPage)
    member me.ItemEditPage = stateObs |> Observable.choose (fun i -> i.ItemEditPage)
    member me.ShoppingList = stateObs |> Observable.map (ShoppingList.create (clock ()))
