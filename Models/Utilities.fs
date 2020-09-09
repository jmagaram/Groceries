namespace Models
open System

[<AutoOpen>]
module Seq =

    let takeAtMost n s =
        s
        |> Seq.mapi (fun index item -> if index < n then Some item else None)
        |> Seq.takeWhile (fun i -> i.IsSome)
        |> Seq.choose id

    let zeroOrOne s =
        let result = s |> takeAtMost 2 |> Seq.toList

        match result with
        | [] -> None
        | [ x ] -> Some x
        | _ -> failwith "Too many items in the sequence. Expected zero or one."

[<AutoOpen>]
module Map =

    let values m = m |> Map.toSeq |> Seq.map snd

[<AutoOpen>]
module String =

    let trim (s: String) = s.Trim()

    let isNullOrWhiteSpace s = System.String.IsNullOrWhiteSpace(s)

    let tryParseWith (tryParseFunc: string -> bool * _) =
        tryParseFunc
        >> function
        | true, v -> Some v
        | false, _ -> None

    let tryParseDate = tryParseWith System.DateTime.TryParse

    let tryParseInt = tryParseWith System.Int32.TryParse

    let tryParseGuid = tryParseWith System.Guid.TryParse

    let tryParseTimeSpan = tryParseWith System.TimeSpan.TryParse
