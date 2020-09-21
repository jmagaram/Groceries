namespace Models
open System

[<AutoOpen>]
module Common =

    let keyOf<'T, 'TKey when 'T :> IKey<'TKey>> i = (i :> IKey<'TKey>).Key

    let refEquals<'T when 'T: not struct> x y = Object.ReferenceEquals(x, y)

    let newGuid () = System.Guid.NewGuid()

    let memoizeLast<'X, 'Result when 'Result : equality> (f: 'X -> 'Result, isEqual: 'X -> 'X -> bool) =
        let mutable cache = None

        let f' x =
            match cache with
            | None ->
                let result = f x
                cache <- Some(x, result)
                result
            | Some (cachedX, result) ->
                if (isEqual cachedX x) then
                    result
                else
                    let result = f x
                    cache <- Some(x, result)
                    result

        f'

[<AutoOpen>]
module Seq =

    let takeAtMost n source =
        if n < 0 then
            invalidArg "n" "The input must be non-negative."
        else
            source
            |> Seq.mapi (fun index item -> if index < n then Some item else None)
            |> Seq.takeWhile (fun i -> i.IsSome)
            |> Seq.choose id

[<AutoOpen>]
module Map =

    let values m = m |> Map.toSeq |> Seq.map snd

[<AutoOpen>]
module String =

    let trim (s: String) = s.Trim()

    let isNullOrWhiteSpace s = String.IsNullOrWhiteSpace(s)

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
