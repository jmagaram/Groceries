[<AutoOpen>]
module String

let lengthIsAtLeast min s = (String.length s) >= min

let lengthIsAtMost max s = (String.length s) <= max

let startsWithOrEndsWithWhitespace s =
    match s |> String.length with
    | 0 -> false
    | len -> (s.[0] |> Char.isWhitespace) || (s.[len-1] |> Char.isWhitespace)

let containsNewLine (s:string) = s.Contains(System.Environment.NewLine)

let isNullOrWhiteSpace s = System.String.IsNullOrWhiteSpace(s)

let trim (s:string) = 
    match s |> isNullOrWhiteSpace with
    | true -> ""
    | false -> s.Trim()

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
    open FsCheck
    open FsCheck.Xunit

    type ZeroOrPositive =
        static member Integer() = 
            Arb.Default.Int32()
            |> Arb.mapFilter abs (fun t -> t >= 0)

    type NotNullString =
        static member String() =
            Arb.Default.String()
            |> Arb.filter (fun s -> s <> null)

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