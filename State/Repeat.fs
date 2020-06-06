module Repeat
open DomainTypes
open Xunit
open FsUnit

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

module Tests = 

    type RepeatResult = Result<Repeat, string>

    [<Theory>]
    [<InlineData(1)>]
    [<InlineData(4)>]
    [<InlineData(7)>]
    [<InlineData(364)>]
    [<InlineData(365)>]
    let ``Create - when in range`` (i:int) =
        let actual = i |> repeatEvery 
        let (expected : RepeatResult) = i |> Repeat.DailyInterval |> Ok
        actual |> should equal expected

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(-1)>]
    [<InlineData(366)>]
    let ``Create - when out of range generate error`` (i:int) =
        let actual = i |> repeatEvery
        let expected = outOfRangeError 
        actual |> should equal expected

    [<Theory>]
    [<InlineData(1)>]
    [<InlineData(19)>]
    [<InlineData(74)>]
    let ``Serialize - when daily interval`` (i:int) =
        let actual = 
            i 
            |> repeatEvery 
            |> Result.map serialize 
            |> Result.bind deserialize
        let (expected : RepeatResult) = Ok (Repeat.DailyInterval i)
        actual |> should equal expected

    [<Fact>]
    let ``Serialize - when does not repeat`` =
        let actual = 
            doesNotRepeat
            |> serialize 
            |> deserialize
        let (expected : RepeatResult) = Ok doesNotRepeat
        actual |> should equal expected
