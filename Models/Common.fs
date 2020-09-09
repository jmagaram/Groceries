namespace Models
open System

[<AutoOpen>]
module Common =

    let keyOf<'T, 'TKey when 'T :> IKey<'TKey>> i = (i :> IKey<'TKey>).Key

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
