namespace ModelsTest

open System
open Models
open Xunit
open FsUnit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit

module MemoizeTests =

    let add nums = nums |> Seq.sum
    let addMemoized = memoizeLast (add, (fun i j -> i = j))

    [<Property>]
    let ``memoizeLast - memoized result is same as non-memoized result`` (NonNegativeInt x)
                                                                         (NonNegativeInt y)
                                                                         (NonNegativeInt z)
                                                                         =
        let x = x % 5
        let y = y % 5
        let z = z % 5
        let expectedResult = add [ x; y; z ]
        let memoizedResult = addMemoized [ x; y; z ]
        memoizedResult |> should equal expectedResult

    [<Fact>]
    let ``memoizeLast - uses memoized result when available`` () =
        let mutable calculations = 0

        let multiply nums =
            calculations <- calculations + 1
            nums |> Seq.fold (fun total i -> total * i) 1

        let memoizedmultiply =
            memoizeLast (multiply, (fun i j -> i = j))

        let a = [ 1; 2; 3 ]
        let b = [ 4; 5; 6 ]
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

        let memoizedmultiply =
            memoizeLast (multiply, (fun i j -> System.Object.ReferenceEquals(i, j)))

        let a = [ 1; 2; 3 ]
        let b = [ 1; 2; 3 ]
        memoizedmultiply a |> ignore
        memoizedmultiply b |> ignore
        calculations |> should equal 2

module SeqTests =

    [<Fact>]
    let ``zero or one - when empty expect none`` () = [] |> zeroOrOne |> should equal None

    [<Fact>]
    let ``zero or one - when exactly one expect it`` () =
        [ 1 ] |> zeroOrOne |> should equal (Some 1)

    [<Fact>]
    let ``zero or one - when more than one throw`` () =
        let f () = [ 1; 2; 3 ] |> zeroOrOne |> ignore
        f |> shouldFail

    [<Fact>]
    let ``takeTo - no item before the last satisfies the predicate`` () =
        let isLessThanOrEqualTo75 x = x <= 75

        let failingTests =
            Gen.choose (0, 10)
            >>= (fun len -> Gen.listOfLength len (Gen.choose (50, 100)))
            |> Gen.sample 1 1000
            |> Seq.choose (fun items ->
                let result =
                    items
                    |> Seq.takeTo isLessThanOrEqualTo75
                    |> List.ofSeq

                let isOk =
                    items
                    |> Seq.truncate (max (result.Length - 1) 0)
                    |> Seq.tryFind isLessThanOrEqualTo75
                    |> Option.isNone

                match isOk with
                | true -> None
                | false -> Some items)
            |> List.ofSeq

        failingTests |> Seq.isEmpty |> should equal true

    [<Fact>]
    let ``takeTo - the last item matches predicate or is end of source`` () =
        let isLessThanOrEqualTo75 x = x <= 75

        let failingTests =
            Gen.choose (0, 10)
            >>= (fun len -> Gen.listOfLength len (Gen.choose (50, 100)))
            |> Gen.sample 1 1000
            |> Seq.choose (fun items ->
                let result =
                    items
                    |> Seq.takeTo isLessThanOrEqualTo75
                    |> List.ofSeq

                let isOk =
                    (result.Length = items.Length)
                    || (result |> List.last |> isLessThanOrEqualTo75)

                match isOk with
                | true -> None
                | false -> Some items)
            |> List.ofSeq

        failingTests |> Seq.isEmpty |> should equal true

module StringTests =

    let tryParseInt = tryParseWith System.Int32.TryParse

    [<Theory>]
    [<InlineData("1", 1)>]
    [<InlineData("2", 2)>]
    [<InlineData(" 3  ", 3)>]
    [<InlineData("+5", 5)>]
    [<InlineData("-7", -7)>]
    [<InlineData("0", 0)>]
    [<InlineData("+0", 0)>]
    [<InlineData("-0", 0)>]
    let ``tryParseWith when is an int `` (s: string) (expected: int) =
        s |> tryParseInt |> should equal (Some expected)

    [<Theory>]
    [<InlineData("banana")>]
    [<InlineData("")>]
    [<InlineData("1 apple")>]
    [<InlineData("2.4")>]
    [<InlineData("+2.4")>]
    [<InlineData("-2.4")>]
    let ``tryParseWith when is not an int should return none`` (s: string) = s |> tryParseInt |> should equal None

module OptionTests =

    [<Fact>]
    let ``computation expression - let! when each part returns some, return some`` () =
        let divide numerator denominator =
            match denominator with
            | 0 -> None
            | _ -> Some(numerator / denominator)

        let actual =
            option {
                let! a = divide 10 5
                let! b = divide 20 2
                let! c = divide 30 6
                return a + b + c
            }

        let expected = Some 17
        actual |> should equal expected

    [<Fact>]
    let ``computation expression - let! when some part returns none, return none`` () =
        let divide numerator denominator =
            match denominator with
            | 0 -> None
            | _ -> Some(numerator / denominator)

        let actual =
            option {
                let! a = divide 10 5
                let! b = divide 8 0 // should abort here
                let c = 3 / 0 // shouldn't get this far
                return a + b
            }

        let expected = None
        actual |> should equal expected

module ResultTests =

    type String5 = String5 of string

    let string5 (s: string) =
        if s.Length > 5 then Error "Too long" else Ok(String5 s)

    let len (String5 s) = s.Length

    [<Fact>]
    let ``computation expression - let! when some part fails, return error`` () =
        let actual =
            result {
                let! a = string5 "xx"
                let! b = string5 "yyyyyyyyyyyyy"
                let! c = string5 "yyyyyyyyy"
                let! d = string5 "xx"
                return (len a) + (len b) + (len c) + (len d)
            }

        let expected: Result<int, string> = Error "Too long"
        actual |> should equal expected

    [<Fact>]
    let ``computation expression - let! when each part ok, return ok`` () =
        let actual =
            result {
                let! a = string5 "xx"
                let! b = string5 "yyy"
                let! c = string5 "y"
                let! d = string5 "xx"
                return (len a) + (len b) + (len c) + (len d)
            }

        let expected: Result<int, string> = Ok 8
        actual |> should equal expected

    [<Fact>]
    let ``computation expression - can return! an error directly`` () =
        let actual: Result<int, string> =
            result { return! (Error "this is the error") }

        let expected: Result<int, string> = Error "this is the error"
        actual |> should equal expected

    [<Fact>]
    let ``computation expression - can return! an OK directly`` () =
        let actual: Result<int, string> = result { return! (Ok 3) }

        let expected: Result<int, string> = Ok 3
        actual |> should equal expected

    //[<Fact>]
    //let ``computation expression - can match!`` () =
    //    let actual =
    //        result {
    //            let a = match! (Ok 3) with
    //                    | 3 -> true
    //                    | _ -> false
    //            return a
    //        }
    //    true


    [<Fact>]
    let ``fromResults - return first Error, if any, or Ok list`` () =
        let isSmallerThan5 x = if x < 5 then Ok x else Error x
        let nums1To10 = Gen.choose (1, 10)

        let testData =
            Gen.choose (0, 5)
            >>= fun len -> Gen.listOfLength len nums1To10
            |> Gen.sample 1 1000

        let results =
            testData
            |> Seq.map (fun i ->
                let input =
                    i |> Seq.map isSmallerThan5 |> List.ofSeq

                let result = input |> fromResults

                let expected =
                    match input
                          |> List.tryPick (fun i ->
                              match i with
                              | Error e -> Some e
                              | _ -> None) with
                    | None -> Ok i
                    | Some r -> Error r

                {| Input = input
                   Expected = expected
                   Actual = result |})
            |> List.ofSeq

        let failing =
            results
            |> List.filter (fun i -> i.Actual <> i.Expected)

        failing |> Seq.isEmpty |> should equal true

module GuidTests =

    [<Property>]
    let ``serialize and tryDeserialize results in same thing`` () =
        let g = Guid.NewGuid()

        let actual =
            g |> Guid.serialize |> Guid.tryDeserialize

        let expected = Some g
        actual |> should equal expected

    [<Property>]
    let ``tryDeserialize trims whitespace`` (NonNegativeInt spaceBefore) (NonNegativeInt spaceAfter) =
        let before = new String(' ', spaceBefore)
        let after = new String(' ', spaceAfter)
        let g = Guid.NewGuid() |> Guid.serialize
        let g' = sprintf "%s%s%s" before g after

        (g |> Guid.tryDeserialize)
        |> should equal (g' |> Guid.tryDeserialize)

    // 7b9b544f-9fe2-42e8-b7f6-767f659e8866 is valid
    [<Theory>]
    [<InlineData("x7b9b544f-9fe2-42e8-b7f6-767f659e8866")>]
    [<InlineData("7b9b544f-9fe2-42e8-b7f6-767f659e8866x")>]
    [<InlineData("7b9b544f-9fe2-42e8-b7f6--67f659e8866")>]
    let ``tryDeserialize when invalid returns None`` s =
        s
        |> Guid.tryDeserialize
        |> Option.isNone
        |> should equal true
