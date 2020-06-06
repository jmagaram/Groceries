[<AutoOpen>]
module String

let isNullOrWhiteSpace s = System.String.IsNullOrWhiteSpace(s)

let trim (s:string) = 
    match s |> isNullOrWhiteSpace with
    | true -> ""
    | false -> s.Trim()

let rec find (query:string) (source:string)  =
    seq {
        if source <> "" then
            let index = source.IndexOf(query, System.StringComparison.CurrentCultureIgnoreCase)
            if (index = -1) || query = "" then
                yield (source, false)
            else
                if (index = 0) then
                    let matchAtBeginning = source.Substring(0, query.Length)
                    yield (matchAtBeginning, true)
                    let remain = source.Substring(query.Length)
                    yield! find query remain
                else
                    let beforeMatch = source.Substring(0, index)
                    let matchInMiddle = source.Substring(index, query.Length)
                    let afterMatch = source.Substring(index + query.Length)
                    yield (beforeMatch, false)
                    yield (matchInMiddle, true)
                    yield! find query afterMatch 
    }

let tryParseWith (tryParseFunc: string -> bool * _) = tryParseFunc >> function
    | true, v    -> Some v
    | false, _   -> None

let tryParseDate = tryParseWith System.DateTime.TryParse

let tryParseInt = tryParseWith System.Int32.TryParse

let tryParseGuid = tryParseWith System.Guid.TryParse

module Tests = 

    open System
    open Xunit
    open FsUnit

    [<Theory>]
    [<InlineData("Once at beginning", "123xyz","123", "|123|_xyz_")>]
    [<InlineData("Once at end", "123xyz","xyz", "_123_|xyz|")>]
    [<InlineData("At beginning and end", "123xyz123","123", "|123|_xyz_|123|")>]
    [<InlineData("Case insensitive", "abcxyz","XYZ", "_abc_|xyz|")>]
    [<InlineData("Overlapping matches", "aaaaaa","aa", "|aa||aa||aa|")>]
    [<InlineData("An empty query is never found", "xyz","", "_xyz_")>]
    [<InlineData("A whitespace query is sometimes found", "a  a  a  a"," ", "_a_| || |_a_| || |_a_| || |_a_")>]
    [<InlineData("Complex case 1", "apple pleasant plum","pl", "_ap_|pl|_e _|pl|_easant _|pl|_um_")>]
    let ``Can find all matching text within a string`` (comment:string, source:string, query:string, expectedResult:string) =
        let rec parseExpectedResult (r:string) =
            seq {
                if r.Length > 0 then
                    let delimiter = r |> Seq.head
                    let endIndex = r.IndexOf(delimiter, 1)
                    let token = r.Substring(1, endIndex - 1)
                    let rest = r.Substring(token.Length + 2)
                    yield (token, delimiter = '|')
                    yield! parseExpectedResult rest
            }
        let expected = expectedResult |> parseExpectedResult
        let actual = source |> find query |> List.ofSeq
        actual |> should equivalent expected

    [<Theory>]
    [<InlineData("abc")>]
    [<InlineData("")>]
    let ``When search for something in empty string return empty`` (query:string) =
        let expected = Seq.empty
        let actual = "" |> find query
        actual |> should equivalent expected

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