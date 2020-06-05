module Repeat
open DomainTypes

let doesNotRepeat = DoesNotRepeat

let maxInterval = 365

let minInterval = 1

let outOfRangeError = 
    sprintf "The daily interval must be in the range %i..%i" minInterval maxInterval
    |> Error

let repeatEvery d =
    if d < minInterval || d > maxInterval
    then outOfRangeError
    else Ok (Repeat.DailyInterval d)

let serialize r =
    match r with
    | Repeat.DailyInterval d -> d.ToString()
    | Repeat.DoesNotRepeat -> "x"

let deserialize s =
    if s = "x"
    then Repeat.DoesNotRepeat |> Ok
    else 
        s
        |> tryParseInt
        |> Option.map repeatEvery
        |> Option.defaultValue (Error (sprintf "Could not deserialize %s" s))
