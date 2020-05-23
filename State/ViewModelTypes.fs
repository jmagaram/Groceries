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
