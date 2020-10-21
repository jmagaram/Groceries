namespace Models

open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive
open CoreTypes
open StateTypes

type Service(state, clock) =
    let stateSub = state |> Subject.behavior
    let stateObs = stateSub |> Observable.asObservable
    new() = Service(State.createDefault, clock)

    member me.Update(msg) = stateSub.OnNext(stateSub.Value |> State.update clock msg)
    member me.StoreEditPage = stateObs |> Observable.choose (fun i -> i.StoreEditPage)
    member me.CategoryEditPage = stateObs |> Observable.choose (fun i -> i.CategoryEditPage)
    member me.ItemEditPage = stateObs |> Observable.choose (fun i -> i.ItemEditPage)
    member me.ShoppingList = stateObs |> Observable.map (ShoppingList.create (clock ()))
    member me.Current = stateSub.Value
