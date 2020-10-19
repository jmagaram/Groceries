namespace Models

open System
open UtilitiesTypes

[<AutoOpen>]
module Miscellaneous =

    let keyOf<'T, 'TKey when 'T :> IKey<'TKey>> i = (i :> IKey<'TKey>).Key

    let clock: Clock = fun () -> DateTimeOffset.Now

    let refEquals<'T when 'T: not struct> x y = Object.ReferenceEquals(x, y)

    let newGuid () = Guid.NewGuid()

[<AutoOpen>]
module Map =

    let values m = m |> Map.toSeq |> Seq.map snd

[<AutoOpen>]
module Seq =

    /// <summary>Returns a sequence that, when iterated, yields elements of the underlying sequence until and
    /// including the first element where the given predicate returns True, and then returns no further
    /// elements.</summary>
    let takeTo<'T> predicate (source: 'T seq) =
        seq {
            use en = source.GetEnumerator()
            let mutable isDone = false

            while isDone = false && en.MoveNext() do
                yield en.Current
                isDone <- predicate en.Current
        }

    let zeroOrOne s =
        let result = s |> Seq.truncate 2 |> Seq.toList

        match result with
        | [] -> None
        | [ x ] -> Some x
        | _ -> failwith "Too many items in the sequence. Expected zero or one."

    let leftJoin xs ys f =
        xs
        |> Seq.map (fun x -> (x, ys |> Seq.filter (fun y -> f x y)))

    let join xs ys f =
        leftJoin xs ys f
        |> Seq.collect (fun (x, ys) -> ys |> Seq.map (fun y -> (x, y)))

[<AutoOpen>]
module Option =

    type OptionBuilder() =
        member this.Return(x) = Some x
        member this.Bind(x, f) = Option.bind f x
        member this.ReturnFrom r = r

    let option = OptionBuilder()

    let asResult e o =
        o
        |> Option.map Ok
        |> Option.defaultValue (Error e)

[<AutoOpen>]
module Result =

    let okOrThrow r =
        match r with
        | Ok v -> v
        | Error e -> failwithf "Could not get the Ok value; this was the Error: %A" e

    let asOption r =
        match r with
        | Ok v -> Some v
        | Error _ -> None

    let isOk r =
        match r with
        | Ok _ -> true
        | Error _ -> false

    let isError r = r |> isOk |> not

    let error r =
        match r with
        | Ok v -> None
        | Error e -> Some e

    type ResultBuilder() =
        member this.Return(x) = Ok x
        member this.Bind(x, f) = Result.bind f x
        member this.ReturnFrom r = r

    let result = ResultBuilder()

    let fromResults rs =
        rs
        |> Seq.scan (fun (vs, err) i ->
            match i with
            | Ok v -> (v :: vs, err)
            | Error e -> (vs, Some e)) ([], None)
        |> Seq.takeTo (fun (vs, err) -> err.IsSome)
        |> Seq.last
        |> fun (vs, err) ->
            match err with
            | None -> vs |> List.rev |> Ok
            | Some err -> Error err

[<AutoOpen>]
module String =

    let toString i = i.ToString()

    let trim (s: String) = s.Trim()

    let isNullOrWhiteSpace s = String.IsNullOrWhiteSpace(s)

    let tryParseWith (tryParseFunc: string -> bool * _) =
        tryParseFunc
        >> function
        | true, v -> Some v
        | false, _ -> None

    let tryParseOptional parser s =
        if s |> isNullOrWhiteSpace then Ok None else s |> parser |> Result.map Some

[<AutoOpen>]
module Math =

    let divRem divisor dividend =
        if divisor = 0 then
            None
        else
            Some
                {| Quotient = dividend / divisor
                   Remainder = dividend % divisor |}

[<AutoOpen>]
module Memoize =

    let memoize<'Cache, 'X, 'Y when 'Y: equality> empty tryFind (add: 'X -> 'Y -> 'Cache -> 'Cache) f =
        let mutable cache = empty

        let f' x =
            match cache |> tryFind x with
            | Some y -> y
            | None ->
                let y = f x
                cache <- add x y cache
                y

        f'

    let memoizeAll f =
        let empty = Map.empty
        let tryFind = Map.tryFind
        let add = Map.add
        memoize empty tryFind add f

    let memoizeLast (f, isEqual) =
        let empty = None

        let tryFind x' cache =
            cache
            |> Option.filter (fun (x, _) -> isEqual x x')
            |> Option.map (fun (_, y) -> y)

        let add x y cache = Some(x, y)
        memoize empty tryFind add f

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Guid =

    let serialize (g: Guid) = g.ToString()

    let tryDeserialize s = s |> String.tryParseWith Guid.TryParse
