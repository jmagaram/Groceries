module ViewModelTypes
open DomainTypes
open System

type ListItemViewModel =
    { Title : string
      Quantity : string option
      Note : string option
      IsComplete : bool
      PostponedUntil : DateTime option
      Repeats : Duration option
    }

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
      PostponedUntil = 
        match i.Schedule with
        | Postponed dt -> Some dt
        | Repeat r ->
            match r.PostponedUntil with
            | Some dt -> Some dt
            | None -> None
        | _ -> None
      Repeats = 
        match i.Schedule with
        | Repeat r -> Some r.Frequency
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
                match i.PostponedUntil with
                | None -> true
                | Some dt -> dt <= now)
    }

