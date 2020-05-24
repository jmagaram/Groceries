module ItemEditor
open System
open DomainTypes

type ViewModel = 
    { Title : string
      Quantity : string
      Note : string
      Schedule : Schedule }
      member x.TitleError = 
        match String.IsNullOrWhiteSpace(x.Title) with
        | true -> Some "The title is required."
        | false ->
            match x.Title.Length with
            | 1 -> Some "That title seems a bit too short."
            | _ -> None
      member x.CanAddRecurrence =
        match x.Schedule with
        | Schedule.Repeat -> false
        | _ -> true
      member x.CanRemoveRecurrence =
        match x.Schedule with
        | Schedule.Repeat -> true
        | _ -> false
      member x.CanChangeRecurrence =
        match x.Schedule with
        | Schedule.Repeat -> true
        | _ -> false
      member x.CanPostpone =
        match x.Schedule with
        | Complete -> false
        | Incomplete -> true
        | Schedule.Repeat r -> 
            match r.PostponedUntil with
            | None -> true
            | _ -> false
        | _ -> false
        member x.CanRemovePostponement =
          match x.Schedule with
          | Postponed _ -> true
          | Schedule.Repeat r -> 
              match r.PostponedUntil with
              | Some dt -> true
              | _ -> false
          | _ -> false
      member x.IsPostponed = 
        match x.Schedule with
        | Schedule.Repeat r ->
            match r.PostponedUntil with
            | Some _ -> true
            | _ -> false
        | Postponed _ -> true
        | _ -> false
      member x.CanChooseSpecificPostponeDate = x.IsPostponed
      member x.CanSubmit =
        x.TitleError.IsNone      
      member x.IsRepeatOn(d) =
        match x.Schedule with
        | Repeat r when r.Frequency = d -> true
        | _ -> false
      member x.PostponedUntil =
        match x.Schedule with
        | Repeat r -> r.PostponedUntil
        | Postponed dt -> Some dt
        | _ -> None
      member x.CanComplete =
        match x.Schedule with
        | Complete -> false
        | _ -> x.CanSubmit
      member x.CanUncomplete =
        match x.Schedule with
        | Complete -> true
        | _ -> false

let createNew = 
    { ViewModel.Title = ""
      Quantity = ""
      Note = ""
      Schedule = Schedule.Incomplete }

let updateTitle title model  = { model with ViewModel.Title = title }
let updateNote note model = { model with ViewModel.Note = note }
let updateQuantity quantity model = { model with ViewModel.Quantity = quantity }
let complete (model:ViewModel) = 
    match model.Schedule with
    | Incomplete 
    | Complete
    | Postponed -> { model with Schedule = Schedule.Complete }
    | Repeat r ->
        let newPostponeDate = DateTime.Now.AddDays(r.Frequency |> Duration.asDays |> float)
        { model with Schedule = Schedule.Repeat { Repeat.Frequency = r.Frequency; Repeat.PostponedUntil = Some newPostponeDate } }
let incomplete model = { model with ViewModel.Schedule = Schedule.Incomplete }
let addRecurrence (model:ViewModel) = 
    { model with 
        Schedule = Repeat { 
            Repeat.Frequency = Duration.W1; 
            PostponedUntil = 
                match model.Schedule with
                | Postponed dt -> Some dt
                | _ -> None } }
let removeRecurrence (model:ViewModel) = 
    match model.Schedule with
    | Repeat r ->
        match r.PostponedUntil with
        | Some dt -> { model with Schedule = Postponed dt }
        | None -> { model with Schedule = Incomplete }
    | _ -> model
let removePostponement (model:ViewModel) = 
    match model.Schedule with
    | Repeat r -> { model with Schedule = Repeat { r with PostponedUntil = None }}
    | Postponed dt -> { model with Schedule = Schedule.Incomplete }
    | Complete -> model
    | Incomplete -> model
let changeRecurrence duration (model:ViewModel) =
    let repeat = 
        match model.Schedule with
        | Repeat r -> { r with Frequency = duration }
        | _ -> { Frequency = duration; PostponedUntil = None }
    { model with Schedule = Repeat repeat }
let postponeUntil dt (model:ViewModel)  =
    match model.Schedule with
    | Repeat r -> { model with Schedule = Repeat { r with PostponedUntil = Some dt }}
    | _ -> { model with Schedule = Postponed dt }
let postpone (model:ViewModel) (now:DateTime) = model |> postponeUntil (now.AddDays(7.0))