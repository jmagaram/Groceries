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

let orderBy r = 
    match r with
    | DoesNotRepeat -> 0
    | DailyInterval d -> d

type FormatRules =
    { NoRepeatText : string
      EveryDayText : string
      EveryWeekText : string
      EveryMonthText : string
      EveryNDaysText : int -> string
      EveryNWeeksText : int -> string
      EveryNMonthsText : int -> string }

let format (rules:FormatRules) r =
    match r with
    | DoesNotRepeat -> rules.NoRepeatText
    | DailyInterval d ->
        if (d % 30 = 0) then
            if d = 30 
            then rules.EveryMonthText 
            else rules.EveryNMonthsText (d / 30)
        elif (d % 7 = 0) then
            if d = 7
            then rules.EveryWeekText
            else rules.EveryNWeeksText (d / 7)
        else
            if d = 1
            then rules.EveryDayText
            else rules.EveryNDaysText d

let englishFormatRules =
    { FormatRules.NoRepeatText = "Does not repeat"
      EveryDayText = "Daily"
      EveryWeekText = "Weekly"
      EveryMonthText = "Monthly"
      EveryNDaysText = sprintf "Every %i days"
      EveryNWeeksText = sprintf "Every %i weeks"
      EveryNMonthsText = sprintf "Every %i months" }

let formatEnglish = format englishFormatRules

let standardRepeatIntervals =
    [1; 3; 7; 14; 21; 30; 60; 90; 180]
    |> Seq.map Repeat.DailyInterval

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