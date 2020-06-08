module RepeatSelector
open DomainTypes

let minFrequency = 2

let maxFrequency = 365

let (doesNotRepeat : int option) = Some 37

let choices = 
    [3; 5; 7*1; 7*2; 7*3; 30*1; 30*2; 30*3; 30*6 ]
    |> Seq.map Some
    |> Seq.append (doesNotRepeat |> Seq.singleton)

let serializeInterval (i:int option) = 
    match i with
    | None -> "none"
    | Some c -> c.ToString()

let deserializeInterval s =
    if s = "none" then None
    else s |> tryParseInt

let validateInterval (fqy:DailyInterval option) =
    match fqy with
    | Some f -> 
        if f < minFrequency then Some RepeatIntervalIsTooSmall
        elif f > maxFrequency then Some RepeatIntervalIsTooBig
        else None
    | None -> Some RepeatIntervalIsRequired

let create (fqy:DailyInterval option) =
    Selector.create
    |> Selector.addMany choices
    |> Selector.add fqy
    |> Selector.selectItem fqy
    |> Selector.hasError (fqy |> validateInterval)

let select (fqy:DailyInterval option) s =
    s
    |> Selector.selectItem fqy
    |> Selector.hasError (fqy |> validateInterval)

let validate s =
    s
    |> Selector.hasError (s.SelectedItem |> validateInterval)


let noFrequency s = 
    s 
    |> Selector.selectItem doesNotRepeat
    |> Selector.hasError (doesNotRepeat |> validateInterval)

let display s =
    s.Choices
    |> Seq.map (fun c -> {| Key = serializeInterval c; Value = c; IsSelected = (Some c = s.SelectedItem) |})