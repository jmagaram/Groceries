module ItemBuilder
open System
open DomainTypes

type Validated<'t,'error> =
    { Value : 't 
      Error : 'error option }

type Repeat = 
    { Frequency : Duration
      DueDays : int option }

type Schedule =
    | Complete
    | Incomplete
    | Repeat of Repeat
    member x.TryRepeat = match x with | Repeat r -> Some r | _ -> None

type Builder =
    { Title : Validated<string, string>
      Quantity : string 
      Note : string 
      Schedule : Schedule }

let trim (s:string) = 
    match System.String.IsNullOrWhiteSpace(s) with
    | true -> ""
    | false -> s.Trim()

let validateTitle t =
    let t = t |> trim
    { Value = t 
      Error = 
        match t.Length with
        | 0 -> Some "The title is required"
        | 1 | 2 -> Some "That seems a bit short for a title"
        | _ -> None }       

let setTitle t b = { b with Builder.Title = t |> validateTitle}

let setQuantity q b = { b with Builder.Quantity = q |> trim }

let increaseQty b = 
    b.Quantity 
    |> QuantityUpDown.instance.Increase 
    |> Option.map (fun q -> { b with Quantity = q })

let decreaseQty b = 
    b.Quantity 
    |> QuantityUpDown.instance.Decrease
    |> Option.map (fun q -> { b with Quantity = q })

let setNote n b = { b with Builder.Note = n |> trim }

let repeat b =
    match b.Schedule with
    | Incomplete -> Some { b with Schedule = Repeat { Frequency = Duration.W1; DueDays = None } }
    | _ -> None

let removeRepeat b =
    match b.Schedule with
    | Repeat -> Some { b with Schedule = Incomplete }
    | _ -> None

let setFrequency b =
    match b.Schedule with
    | Repeat r -> Some (fun dur -> { b with Schedule = Repeat { r with Frequency = dur }})
    | _ -> None

let activate b =
    match b.Schedule with
    | Complete -> Some { b with Schedule = Incomplete }
    | Incomplete -> None
    | Repeat r -> 
        match r.DueDays with
        | Some _ -> Some { b with Schedule = Repeat { r with DueDays = None }}
        | None -> None

let postpone b =
    match b.Schedule with
    | Incomplete -> None
    | Complete -> None
    | Repeat r ->
        fun days ->
            let days = System.Math.Max(days, 0)
            { b with Schedule = Repeat { r with DueDays = Some days }}
        |> Some

let canSubmit (b:Builder) = b.Title.Error.IsNone

let create = 
    { Title = "" |> validateTitle 
      Quantity = "" 
      Note = "" 
      Schedule = Schedule.Repeat { Frequency = W1; DueDays = Some 37 } }

let edit (item:DomainTypes.Item) (now:System.DateTime) =
    { Title = match item.Title with | Title t -> t |> validateTitle
      Quantity = 
        match item.Quantity with 
        | Some (Quantity q) -> q |> trim
        | None -> ""
      Note = 
        match item.Note with 
        | Some (Note n) -> n |> trim
        | None -> ""
      Schedule =
        match item.Schedule with
        | DomainTypes.Schedule.Incomplete -> Incomplete
        | DomainTypes.Schedule.Complete -> Complete
        | DomainTypes.Schedule.Repeat r ->
            Repeat { 
                Repeat.Frequency = r.Frequency; 
                Repeat.DueDays = 
                    r.Due
                    |> Option.map (fun dt -> Math.Max(0, Math.Round(dt.Subtract(now).TotalDays) |> int)) }
    }
