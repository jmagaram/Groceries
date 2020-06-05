module SeqTests

open System
open Xunit
open FsUnit

[<Fact>]
let ``zero or one - when empty expect none`` () =
    []
    |> Seq.zeroOrOne
    |> should equal None

[<Fact>]
let ``zero or one - when exactly one expect it`` () =
    [1]
    |> Seq.zeroOrOne
    |> should equal (Some 1)

[<Theory>]
[<InlineData("abc", 4, "abc")>]
[<InlineData("abc", 3, "abc")>]
[<InlineData("abc", 2, "ab")>]
[<InlineData("abc", 1, "a")>]
[<InlineData("abc", 0, "")>]
[<InlineData("", 2, "")>]
[<InlineData("", 1, "")>]
[<InlineData("", 0, "")>]
let ``takeAtMost `` (start:string) (count:int) (expected:string) =
    let result =
        start
        |> Seq.takeAtMost count
        |> Seq.map (fun c -> c.ToString())
        |> String.concat ""
    result
    |> should equal expected



