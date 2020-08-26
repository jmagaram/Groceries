[<AutoOpen>]
module String

let isNullOrWhiteSpace s = System.String.IsNullOrWhiteSpace(s)

let trim (s:string) = s.Trim() 

let tryParseWith (tryParseFunc: string -> bool * _) = tryParseFunc >> function
    | true, v    -> Some v
    | false, _   -> None

let tryParseDate = tryParseWith System.DateTime.TryParse

let tryParseInt = tryParseWith System.Int32.TryParse

let tryParseGuid = tryParseWith System.Guid.TryParse

let tryParseTimeSpan = tryParseWith System.TimeSpan.TryParse

module Tests = 

    open System
    open Xunit
    open FsUnit

    [<Theory>]
    [<InlineData("1", 1)>]
    [<InlineData("2", 2)>]
    [<InlineData(" 3  ", 3)>]
    [<InlineData("+5", 5)>]
    [<InlineData("-7", -7)>]
    [<InlineData("0", 0)>]
    [<InlineData("+0", 0)>]
    [<InlineData("-0", 0)>]
    let ``tryParseInt when is an int `` (s:string) (expected:int) =
        s 
        |> tryParseInt
        |> should equal (Some expected)

    [<Theory>]
    [<InlineData("banana")>]
    [<InlineData("")>]
    [<InlineData("1 apple")>]
    [<InlineData("2.4")>]
    [<InlineData("+2.4")>]
    [<InlineData("-2.4")>]
    let ``tryParseInt when is not an int should return none`` (s:string) =
        s 
        |> tryParseInt
        |> should equal None