module StringTests

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
    let actual = source |> String.find query |> List.ofSeq
    actual |> should equivalent expected

[<Theory>]
[<InlineData("abc")>]
[<InlineData("")>]
let ``When search for something in empty string return empty`` (query:string) =
    let expected = Seq.empty
    let actual = "" |> String.find query
    actual |> should equivalent expected
