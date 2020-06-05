module ItemBuilder
open System
open DomainTypes
open Selector

module Title =

    let normalize = trim

    let validate t =
        match t |> normalize |> String.length with
        | 0 -> Some "The title is required."
        | _ -> None

    let create t =
        let t = t |> normalize
        match t |> validate with
        | Some error -> Error error
        | _ -> Ok t

    let textBox = 
        let initial = TextBox.create validate normalize
        let update = TextBox.update validate normalize
        {| Initial = initial
           Update = update |}

    let q = 
        textBox.Initial
        |> textBox.Update DomainTypes.TextBoxMessage.GetFocus
        |> textBox.Update (DomainTypes.TextBoxMessage.SetText "abc")
        |> textBox.Update DomainTypes.TextBoxMessage.LoseFocus

let validateTitle title =
    let t = title |> trim
    { Value = t 
      Error = 
        match t.Length with
        | 0 -> Some "The title is required"
        | 1 | 2 -> Some "That seems a bit short for a title"
        | _ -> None }

let setTitle t m = { m with Model.Title = t |> validateTitle }

let setNote n m = { m with Model.Note = n |> trim }

let setQuantity q m = { m with Model.Quantity = q |> trim }

let adjustQty f m = 
    m.Quantity
    |> f
    |> Option.map(fun q -> { m with Quantity = q })
   
let increaseQty = adjustQty QuantityUpDown.instance.Increase

let decreaseQty = adjustQty QuantityUpDown.instance.Decrease

let setRepeat (f:DailyInterval) m = { m with Model.Repeat = m.Repeat |> RepeatSelector.select (Some f) }

let removeRepeat m = { m with Model.Repeat = m.Repeat |> RepeatSelector.noFrequency } 

let changeStatusKind (k:StatusKind) m =
    match m.Status.SelectedItem with
    | None -> None
    | Some i ->
        if i.Kind = k 
            then None
            else Some { m with Status = m.Status |> Selector.select (fun i -> i.Kind = k)}

let activate m = m |> changeStatusKind Active

let complete m = m |> changeStatusKind Complete

let setPostpone d (m:Model) = 
    m
    |> changeStatusKind Postponed
    |> Option.map (fun m -> { m with PostponedDays = d })

let purchase m = Some m
    //match m.Status.SelectedItem with
    //| RelativeStatus.Complete -> None
    //| _ -> 
    //    match m.Repeat.SelectedItem with
    //    | None -> { m with Status = RelativeStatus.Complete }
    //    | Some choice -> 
    //        match choice.Frequency with
    //        | None -> { m with Status = RelativeStatus.Complete }
    //        | Some fqy -> m |> setPostpone (fqy |> Frequency.asDays)
    //    |> Some

let hasErrors m = m.Title.Error.IsSome


type TextInput<'Error> =
    { Value : string 
      Error : 'Error option }

module TitleTextInput =

    let create s = { TextInput.Value = s; Error = None }

    let edit s t = { TextInput.Value = s; Error = match t with | "" -> Some "empty!" | _ -> None }

let create = 
    { Title = "" |> validateTitle 
      Quantity = "" 
      Note = "" 
      Repeat = RepeatSelector.create (Some 13)(*RepeatSelector.doesNotRepeat*)
      Status = 
        Selector.create 
        |> Selector.add { RelativeStatusChoice.Kind = Active; Description = "Active" }
        |> Selector.add { RelativeStatusChoice.Kind = Complete; Description = "Complete" }
        |> Selector.add { RelativeStatusChoice.Kind = Postponed; Description = "Postponed" }
      PostponedDays = 7
    }

let trimmedCapitalized s = (s |> String.trim).ToUpper()

let betweenThreeFiveChars s =
    match s |> String.length with
    | x when x >= 3 && x <= 5 -> None
    | x when x <3 -> Some "Too short"
    | _ -> Some "Too long"

let textBoxInitial = TextBox.create betweenThreeFiveChars trimmedCapitalized 

let textBoxUpdate = TextBox.update betweenThreeFiveChars trimmedCapitalized

//let edit (item:DomainTypes.Item) (now:System.DateTime) =
//    { Title = match item.Title with | Title t -> t |> validateTitle
//      Quantity = 
//        match item.Quantity with 
//        | Some (Quantity q) -> q |> trim
//        | None -> ""
//      Note = 
//        match item.Note with 
//        | Some (Note n) -> n |> trim
//        | None -> ""
//      Repeat = Selector.create
//      Status = 
//        match item.Status with
//        | Status.Active -> RelativeStatus.Active
//        | Status.Postponed dt -> RelativeStatus.PostponedDays (Math.Round(dt.Subtract(now).TotalDays) |> int)
//        | Status.Complete -> RelativeStatus.Complete 
//      PostponedDays = item. 7 }


//let serializeStatus s =
//    match s with
//    | RelativeStatus.Active -> "active"
//    | Complete -> "complete"
//    | PostponedDays d -> d.ToString()

//let deserializeStatus s =
//    match s with
//    | "active" -> RelativeStatus.Active
//    | "complete" -> RelativeStatus.Complete
//    | s -> RelativeStatus.PostponedDays (Int32.Parse(s))

//let chunk days =
//    let months = days / 30
//    let weeks = (days - months * 30) / 7
//    let days = days - months * 30 - weeks * 7
//    {| Months = months; Weeks = weeks; Days = days |}

//let statusChoices (schedule:Schedule) (days:int seq) =
//    match schedule with
//    | Repeat r ->
//        days
//        |> Seq.map Some
//        |> Seq.append (r.DueDays |> Seq.singleton)
//        |> Seq.distinct
//        |> Seq.choose id
//        |> Seq.map postponed    
//    | _ -> Seq.empty
//    |> Seq.append (purchased |> Seq.singleton)
//    |> Seq.append (onShoppingList |> Seq.singleton)
//    |> Seq.sortBy (fun i ->
//        match i with
//        | Completed -> -2
//        | Active -> -1
//        | Postponed x -> x)