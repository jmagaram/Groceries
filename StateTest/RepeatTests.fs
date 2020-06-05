module RepeatTests

open DomainTypes

open System
open Xunit
open FsUnit

[<Theory>]
[<InlineData(1)>]
[<InlineData(4)>]
[<InlineData(7)>]
[<InlineData(364)>]
[<InlineData(365)>]
let ``Create - when in range`` (i:int) =
    let actual = 
        i 
        |> Repeat.repeatEvery 
        |> sprintf "%A"
    let expected = 
        i
        |> Repeat.DailyInterval
        |> Ok
        |> sprintf "%A"
    actual
    |> should equal expected

[<Theory>]
[<InlineData(0)>]
[<InlineData(-1)>]
[<InlineData(366)>]
let ``Create - when out of range generate error`` (i:int) =
    let actual = 
        i
        |> Repeat.repeatEvery
        |> Result.mapError (fun i -> "Out of range")
    let actual = 
        i 
        |> Repeat.repeatEvery 
        |> sprintf "%A"
    let expected = 
        i
        |> Repeat.DailyInterval
        |> Ok
        |> sprintf "%A"
    actual
    |> should equal expected

