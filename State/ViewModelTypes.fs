module ViewModelTypes
open DomainTypes
open System

type ListItemViewModel =
    { Title : string
      Quantity : string option
      Note : string option
      Status : Status
      Repeat : Frequency option }

let createListItemViewModel (i:DomainTypes.Item) =
    { ListItemViewModel.Title = match i.Title with | Title t -> t
      Quantity = 
        match i.Quantity with 
        | Some (Quantity q) -> Some q
        | None -> None
      Note = 
        match i.Note with 
        | Some (Note n) -> Some n
        | None -> None
      Repeat = i.Repeat
      Status = i.Status }

type ShoppingListViewModel =
    { Items : ListItemViewModel seq 
      ShowFutureItems : bool }

// maybe this is what it should be
type ShowFutureItems = ShoppingListViewModel -> ShoppingListViewModel
type HideFutureItems = ShoppingListViewModel -> ShoppingListViewModel

//let showFutureItems (state:Result) = 
//    { ShowFutureItems = true
//      Items = 
//        state.Items
//        |> Seq.map(fun i -> i.Value)
//        |> Seq.map(fun i -> createListItemViewModel i)
//    }

let isFutureItem now model  =
    match model.Status with
    | Status.Postponed dt -> dt > now
    | _ -> false

//let hideFutureItems (state:Result) now model = 
//    { model with
//        ShowFutureItems = false
//        Items = 
//            model.Items
//            |> Seq.filter(fun i -> i |> isFutureItem now) }