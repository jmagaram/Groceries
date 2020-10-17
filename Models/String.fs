[<AutoOpen>]
module Models.String

open System

let toString i = i.ToString()

let trim (s: String) = s.Trim()

let isNullOrWhiteSpace s = String.IsNullOrWhiteSpace(s)

let tryParseWith (tryParseFunc: string -> bool * _) =
    tryParseFunc
    >> function
    | true, v -> Some v
    | false, _ -> None
