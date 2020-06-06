[<AutoOpen>]
module Seq

let takeAtMost n s = 
    s
    |> Seq.mapi (fun index item -> if index<n then Some item else None)
    |> Seq.takeWhile (fun i -> i.IsSome)
    |> Seq.choose id

let zeroOrOne s =
    let result =
        s
        |> takeAtMost 2
        |> Seq.toList
    match result with
    | [] -> None
    | [x] -> Some x
    | _ -> failwith "Too many items in the sequence. Expected zero or one."

module Tests = 

    open System
    open Xunit
    open FsUnit

    [<Fact>]
    let ``zero or one - when empty expect none`` () =
        []
        |> zeroOrOne
        |> should equal None

    [<Fact>]
    let ``zero or one - when exactly one expect it`` () =
        [1]
        |> zeroOrOne
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
            |> takeAtMost count
            |> Seq.map (fun c -> c.ToString())
            |> String.concat ""
        result
        |> should equal expected



