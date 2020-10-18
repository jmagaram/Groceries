namespace Models

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices
open StateTypes
open StringValidation
open ValidationTypes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemId =

    let create () = newGuid () |> ItemId

    let serialize i =
        match i with
        | ItemId g -> g |> Guid.serialize

    let deserialize s =
        s |> Guid.tryDeserialize |> Option.map ItemId

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

    let asText (Note s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Quantity List.head

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

    let days (Frequency v) = v

    let create =
        let normalizer = id
        let validator = RangeValidation.createValidator rules
        let onSuccess = Frequency
        let onFailure = id
        RangeValidation.toResult normalizer validator onSuccess onFailure

    let frequencyDefault = 7<days> |> create |> Result.okOrThrow

    let commonFrequencyChoices =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days> |> create |> Result.okOrThrow)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Schedule =

    let commonPostponeChoices =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days>)

    let dueDate (now: DateTimeOffset) s =
        match s with
        | Schedule.Completed -> None
        | Schedule.Once -> Some now
        | Schedule.Repeat r -> r.PostponedUntil |> Option.orElse (Some now)

    let isPostponed s =
        match s with
        | Schedule.Repeat { PostponedUntil = Some _ } -> true
        | _ -> false

    let isCompleted (s: Schedule) =
        match s with
        | Schedule.Completed -> true
        | _ -> false

    let postponedUntil s =
        match s with
        | Schedule.Repeat r -> r.PostponedUntil
        | _ -> None

    let postponedUntilDays (now: DateTimeOffset) s =
        match s with
        | Schedule.Repeat r ->
            r.PostponedUntil
            |> Option.map (fun future ->
                let duration = future - now
                round (duration.TotalDays) |> int |> (*) 1<days>)
        | _ -> None

    let completeNext (now: DateTimeOffset) s =
        match s with
        | Schedule.Completed -> s
        | Schedule.Once -> Schedule.Completed
        | Schedule.Repeat r ->
            { r with
                  PostponedUntil =
                      now.AddDays(r.Frequency |> Frequency.days |> float)
                      |> Some }
            |> Schedule.Repeat

    let activate s =
        match s with
        | Schedule.Completed -> Schedule.Once
        | Schedule.Once -> s
        | Schedule.Repeat _ -> s

    let withoutPostpone s =
        match s with
        | Schedule.Repeat ({ PostponedUntil = Some _ } as r) ->
            { r with PostponedUntil = None }
            |> Schedule.Repeat
        | _ -> s

    let tryPostpone (now: DateTimeOffset) (d: int<days>) s =
        match s with
        | Schedule.Repeat r ->
            { r with
                  PostponedUntil = now.AddDays(d |> float) |> Some }
            |> Schedule.Repeat
            |> Ok
        | _ -> Error "Only repeating items can be postponed."

    [<Extension>]
    type ScheduleExtensions =
        [<Extension>]
        static member IsPostponed(me: Schedule) = me |> isPostponed

        [<Extension>]
        static member DueDate(me: Schedule, now: DateTimeOffset) = me |> dueDate now

        [<Extension>]
        static member PostponedUntilDays(me: Schedule, now: DateTimeOffset) = me |> postponedUntilDays now

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let mapSchedule f i = { i with Schedule = i.Schedule |> f }

    let markComplete (now: DateTimeOffset) i =
        i |> mapSchedule (Schedule.completeNext now)

    let buyAgain i = i |> mapSchedule Schedule.activate

    let removePostpone i =
        i |> mapSchedule Schedule.withoutPostpone

    let postpone now days i =
        i
        |> mapSchedule (Schedule.tryPostpone now days >> Result.okOrThrow)

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
