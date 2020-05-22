module Tests

open System
open Xunit

let areSequencesIdentical a b =
    let aArray = a |> Seq.toArray
    let bArray = b |> Seq.toArray
    Assert.Equal(aArray.Length, bArray.Length)
    Assert.True(
        aArray 
        |> Seq.zip bArray 
        |> Seq.forall(fun (x, y) -> x = y))

[<Theory>]
[<InlineData("Find once at beginning", "123xyz","123", "|123|_xyz_")>]
[<InlineData("Find once at end", "123xyz","xyz", "_123_|xyz|")>]
[<InlineData("Find twice", "123xyz123","123", "|123|_xyz_|123|")>]
[<InlineData("Find case insensitive", "abcxyz","XYZ", "_abc_|xyz|")>]
let ``Can find text`` (comment : string, source : string, query : string, expectedResult : string) =
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
    areSequencesIdentical actual expected