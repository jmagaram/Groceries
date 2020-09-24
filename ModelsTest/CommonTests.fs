namespace ModelsTest

open Models
open Xunit
open FsUnit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit

module CommonTests =

    let add nums = nums |> Seq.sum
    let addMemoized = memoizeLast (add, fun i j -> i = j)

    [<Property>]
    let ``memoizeLast - memoized result is same as non-memoized result`` (NonNegativeInt x) (NonNegativeInt y) (NonNegativeInt z) =
        let x = x % 5 
        let y = y % 5 
        let z = z % 5 
        let expectedResult = add [x;y;z]
        let memoizedResult = addMemoized [x;y;z]
        memoizedResult |> should equal expectedResult

    [<Fact>]
    let ``memoizeLast - uses memoized result when available`` () =
        let mutable calculations = 0
        let multiply nums = 
            calculations <- calculations + 1
            nums |> Seq.fold (fun total i -> total * i) 1
        let memoizedmultiply = memoizeLast (multiply, fun i j -> i = j)
        let a = [1;2;3]
        let b = [4;5;6]
        memoizedmultiply a |> ignore
        memoizedmultiply a |> ignore
        memoizedmultiply b |> ignore
        memoizedmultiply b |> ignore
        memoizedmultiply a |> ignore
        memoizedmultiply a |> ignore
        calculations |> should equal 3

    [<Fact>]
    let ``memoizeLast - can use reference equals operator`` () =
        let mutable calculations = 0
        let multiply nums = 
            calculations <- calculations + 1
            nums |> Seq.fold (fun total i -> total * i) 1
        let memoizedmultiply = memoizeLast (multiply, fun i j -> System.Object.ReferenceEquals(i,j))
        let a = [1;2;3]
        let b = [1;2;3]
        memoizedmultiply a |> ignore
        memoizedmultiply b |> ignore
        calculations |> should equal 2

module SeqTests =

    [<Property>]
    let ``takeAtMost returns min of seq size and take size`` (NonNegativeInt sourceSize) (NonNegativeInt takeSize) =
        let sourceSize = sourceSize % 5
        let takeSize = takeSize % 10
        let expectedResultSize = min sourceSize takeSize
        let source = Seq.replicate sourceSize "a"

        let actual = source |> Seq.takeAtMost expectedResultSize |> List.ofSeq

        let expected = source |> Seq.take expectedResultSize |> List.ofSeq

        actual |> should equal expected

    [<Property>]
    let ``takeAtMost throws if take size is less than 0`` (NegativeInt takeSize) (s: string) =
        let takeNegative () = s |> Seq.takeAtMost takeSize |> ignore
        takeNegative |> shouldFail

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
        |> should equal [ [5;3;1]; [6;4;2]; [11;9;7]]

    [<Fact>]
    let ``chunk - when sequence has many items and just one resultant chunk`` () =
        let result = 
            [1;3;5;7;9]
            |> chunkIntoListsOfSameParity
            |> Seq.toList
        result 
        |> should equal [ [9;7;5;3;1]]

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
        |> should equal [ [1] ]

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

    [<Fact>]
    let ``zero or one - when more than one throw`` () =
        let f() = [1;2;3] |> zeroOrOne |> ignore
        f |> shouldFail

module StringTests = 

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