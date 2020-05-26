module ViewModelTypes
open DomainTypes
open System

type ListItemViewModel =
    { Title : string
      Quantity : string option
      Note : string option
      IsComplete : bool
      Repeats : Repeat option }

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
      IsComplete = 
        match i.Schedule with 
        | Schedule.Complete -> true 
        | _ -> false
      Repeats = 
        match i.Schedule with
        | Repeat r -> Some r
        | _ -> None
    }

type ShoppingListViewModel =
    { Items : ListItemViewModel seq 
      ShowFutureItems : bool }

// maybe this is what it should be
type ShowFutureItems = ShoppingListViewModel -> ShoppingListViewModel
type HideFutureItems = ShoppingListViewModel -> ShoppingListViewModel

let showFutureItems (state:State) = 
    { ShowFutureItems = true
      Items = 
        state.Items
        |> Seq.map(fun i -> i.Value)
        |> Seq.map(fun i -> createListItemViewModel i)
    }

let hideFutureItems (state:State) now model = 
    { model with
        ShowFutureItems = false
        Items = 
            model.Items
            |> Seq.filter(fun i ->
                match i.Repeats with
                | Some r -> 
                    match r.Due with
                    | Some dt -> dt > now
                    | None -> false
                | None -> false)
    }