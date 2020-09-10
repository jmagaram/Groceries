namespace Models
open System

[<AutoOpen>]
module Common =

    let keyOf<'T, 'TKey when 'T :> IKey<'TKey>> i = (i :> IKey<'TKey>).Key

    let newGuid () = System.Guid.NewGuid()

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
        | Error e -> failwithf "The Result was an error: %A" e

    let asOption r =
        match r with
        | Ok v -> Some v
        | Error _ -> None