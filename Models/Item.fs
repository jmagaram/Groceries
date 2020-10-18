namespace Models

open System
open System.Text.RegularExpressions
open System.Collections.Generic
open StateTypes
open StringValidation
open ValidationTypes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemId =

    let create () = newGuid () |> ItemId

    let serialize i =
        match i with
        | ItemId g -> g.ToString()

    let deserialize s =
        s
        |> String.tryParseWith Guid.TryParse
        |> Option.map ItemId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemName =

    let rules = singleLine 3<chars> 50<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator ItemName List.head

    let asText (ItemName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Note =

    let rules = multipleLine 3<chars> 200<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Note List.head

    let tryParseOptional s =
        if s |> String.isNullOrWhiteSpace then Ok None else s |> tryParse |> Result.map Some

    let asText (Note s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Quantity List.head

    let tryParseOptional s =
        if s |> String.isNullOrWhiteSpace then Ok None else s |> tryParse |> Result.map Some

    let asText (Quantity s) = s

    type private KnownUnit = { OneOf: string; ManyOf: string }

    let private knownUnits =
        [ { OneOf = "jar"; ManyOf = "jars" }
          { OneOf = "can"; ManyOf = "cans" }
          { OneOf = "ounce"; ManyOf = "ounces" }
          { OneOf = "pound"; ManyOf = "pounds" }
          { OneOf = "gram"; ManyOf = "grams" }
          { OneOf = "head"; ManyOf = "heads" }
          { OneOf = "bunch"; ManyOf = "bunches" }
          { OneOf = "pack"; ManyOf = "packs" }
          { OneOf = "bag"; ManyOf = "bags" }
          { OneOf = "package"
            ManyOf = "packages" }
          { OneOf = "box"; ManyOf = "boxes" }
          { OneOf = "pint"; ManyOf = "pints" }
          { OneOf = "gallon"; ManyOf = "gallons" }
          { OneOf = "container"
            ManyOf = "containers" } ]

    let private manyOf u =
        knownUnits
        |> Seq.where (fun i -> i.OneOf = u || i.ManyOf = u)
        |> Seq.map (fun i -> i.ManyOf)
        |> Seq.tryHead
        |> Option.defaultValue u

    let private oneOf u =
        knownUnits
        |> Seq.where (fun i -> i.OneOf = u || i.ManyOf = u)
        |> Seq.map (fun i -> i.OneOf)
        |> Seq.tryHead
        |> Option.defaultValue u

    let private grammar =
        new Regex("^\s*(\d+)\s*(.*)", RegexOptions.Compiled)

    type private ParsedQuantity = { Quantity: int; Units: string }

    let private format q =
        match q with
        | { Units = "" } -> sprintf "%i" q.Quantity
        | _ -> sprintf "%i %s" q.Quantity q.Units

    let private parse s =
        let m = grammar.Match(s)

        match m.Success with
        | false -> None
        | true ->
            let qty = System.Int32.Parse(m.Groups.[1].Value)
            let units = m.Groups.[2].Value
            Some { Quantity = qty; Units = units }

    let increase qty =
        match isNullOrWhiteSpace qty with
        | true -> Some "2"
        | false ->
            match parse qty with
            | None -> None
            | Some i ->
                let qty = i.Quantity + 1

                let result =
                    { Quantity = qty
                      Units = i.Units |> manyOf }
                    |> format

                Some result

    let decrease qty =
        match parse qty with
        | None -> None
        | Some i ->
            match i.Quantity with
            | x when x <= 1 -> None
            | _ ->
                let qty = i.Quantity - 1

                let result =
                    { Quantity = qty
                      Units =
                          match qty with
                          | 1 -> i.Units |> oneOf
                          | _ -> i.Units |> manyOf }
                    |> format

                Some result

    let increaseQty qty =
        qty |> asText |> increase |> Option.map Quantity // not good logic; Quantity.create makes a Result

    let decreaseQty qty =
        qty |> asText |> decrease |> Option.map Quantity

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Frequency =

    let rules = { Min = 1<days>; Max = 365<days> }

    let create =
        let normalizer = id
        let validator = RangeValidation.createValidator rules
        let onSuccess = Frequency
        let onFailure = id
        RangeValidation.toResult normalizer validator onSuccess onFailure

    let goodDefault = 7<days> |> create |> Result.okOrThrow

    let common =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days> |> create |> Result.okOrThrow)

    let days (Frequency v) = v

    let fromNow (now: DateTimeOffset) f = now.AddDays(f |> days |> float)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Repeat =

    let commonPostponeDays =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days>)

    let create frequency postponedUntil =
        { Frequency = frequency
          PostponedUntil = postponedUntil }

    let due (now: DateTimeOffset) r =
        r.PostponedUntil
        |> Option.map (fun future ->
            let duration = future - now

            round (duration.TotalDays) |> int |> (*) 1<days>)

    let dueWithin (now: DateTimeOffset) (d: int<days>) r =
        r
        |> due now
        |> Option.map (fun d' -> d' <= d)
        |> Option.defaultValue true

    let completeOne (now: DateTimeOffset) r =
        { r with
              PostponedUntil = r.Frequency |> Frequency.fromNow now |> Some }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Schedule =

    let due (now: DateTimeOffset) s =
        match s with
        | Schedule.Once -> now
        | Schedule.Completed -> DateTimeOffset.MaxValue
        | Schedule.Repeat r -> r.PostponedUntil |> Option.defaultValue now

    let effectiveDueDateComparer (now: DateTimeOffset): IComparer<Schedule> =
        { new IComparer<Schedule> with
            member this.Compare(x, y) =
                let xDue = x |> due now
                let yDue = y |> due now
                DateTimeOffset.Compare(xDue, yDue) }

    let isPostponed s =
        match s with
        | Schedule.Repeat { PostponedUntil = Some _ } -> true
        | _ -> false

    let isCompleted (s: Schedule) =
        match s with
        | Schedule.Completed -> true
        | _ -> false

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let markComplete (now: DateTimeOffset) (i: Item) =
        match i.Schedule with
        | Schedule.Completed -> i
        | Schedule.Once -> { i with Schedule = Schedule.Completed }
        | Schedule.Repeat r ->
            { i with
                  Schedule = r |> Repeat.completeOne now |> Schedule.Repeat }

    let buyAgain (i: Item) =
        match i.Schedule with
        | Schedule.Completed -> { i with Schedule = Schedule.Once }
        | Schedule.Once -> i
        | Schedule.Repeat _ -> i

    let isPostponed (i: Item) = i.Schedule |> Schedule.isPostponed

    let isCompleted (i: Item) = i.Schedule |> Schedule.isCompleted

    let removePostpone (i: Item) =
        match i.Schedule with
        | Schedule.Repeat ({ PostponedUntil = Some _ } as r) ->
            { i with
                  Schedule = Schedule.Repeat { r with PostponedUntil = None } }
        | _ -> i

    let postpone (now: DateTimeOffset) (d: int<days>) (i: Item) =
        match i.Schedule with
        | Schedule.Repeat r ->
            let r =
                { r with
                      PostponedUntil = now.AddDays(d |> float) |> Some }

            { i with Schedule = Schedule.Repeat r }
        | _ -> failwith "A non-repeating item can not be postponed."

    type Message =
        | MarkComplete of ItemId
        | BuyAgain of ItemId
        | RemovePostpone of ItemId
        | Postpone of ItemId * int<days>
        | DeleteItem of ItemId

    let map id f (s: State) =
        let item = s.Items |> DataTable.findCurrent id |> f

        { s with
              Items = s.Items |> DataTable.update item }

    let reduce now msg (s: State) =
        match msg with
        | MarkComplete i -> map i (markComplete now) s
        | RemovePostpone i -> map i removePostpone s
        | Postpone (id, d) -> map id (postpone now d) s
        | BuyAgain i -> map i buyAgain s
        | DeleteItem k -> s |> StateUpdateCore.deleteItem k
