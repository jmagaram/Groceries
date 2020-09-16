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
