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

let chunk create add source =
    seq {
        let mutable chunk = None
        for i in source do
            let (chunk', isComplete) = 
                chunk
                |> Option.map (fun c -> 
                        match c |> add i with
                        | None -> (i |> create, true)
                        | Some c -> (c, false))
                |> Option.defaultWith(fun () -> (i |> create, false))
            if isComplete then
                yield chunk
            chunk <- Some chunk'
        yield chunk }
    |> Seq.choose id

module Tests = 

    open System
    open Xunit
    open FsUnit

    let chunkIntoListsOfSameParity = 
        let createList i = [i]
        let consIfSameParity i list =
            let isListEven = list |> List.tryHead |> Option.map (fun i -> i % 2 = 0) |> Option.defaultValue true
            let isItemEven = i % 2 = 0
            if isListEven = isItemEven 
            then Some (i :: list)
            else None        
        chunk createList consIfSameParity

    [<Fact>]
    let ``chunk - when sequence has many items and many resultant chunks`` () =
        let result = 
            [1;3;5;2;4;6;7;9;11]
            |> chunkIntoListsOfSameParity
            |> Seq.toList
        result 
        |> should equivalent [ [5;3;1]; [6;4;2]; [11;9;7]]

    [<Fact>]
    let ``chunk - when sequence has many items and just one resultant chunk`` () =
        let result = 
            [1;3;5;7;9]
            |> chunkIntoListsOfSameParity
            |> Seq.toList
        result 
        |> should equivalent [ [9;7;5;3;1]]

    [<Fact>]
    let ``chunk - when sequence has nothing in it`` () =
        let result = 
            []
            |> chunkIntoListsOfSameParity
        result 
        |> Seq.isEmpty
        |> should equal true

    [<Fact>]
    let ``chunk - when sequence has exactly one item`` () =
        let result = 
            [1]
            |> chunkIntoListsOfSameParity
            |> Seq.toList
        result 
        |> should equivalent [ [1] ]

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



