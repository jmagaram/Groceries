[<AutoOpen>]
module String

open System

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

let repeat n s = 
    if n < 0
        then raise (ArgumentOutOfRangeException("n","The repeat count must be greater than or equal to zero."))
    elif s = null
        then raise (ArgumentNullException("s"))
    else
        String.Join("", Seq.replicate (n+1) s)

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

    [<Fact>]
    let ``repeat when n < 0 should throw`` () =
        (fun() -> repeat -1 "aaa" |> ignore)
        |> should throw typeof<ArgumentOutOfRangeException>

    [<Fact>]
    let ``repeat when passed null should throw`` () =
        (fun() -> repeat 3 null |> ignore)
        |> should throw typeof<ArgumentNullException>

    [<Property(Arbitrary=[| typeof<ZeroOrPositive>; typeof<NotNullString> |])>]
    let ``repeat n times is repeat n-1 plus original, but if n = 0 then original`` (s:string) (n:int) = 
        let actual = s |> repeat n
        let expected = 
            match n with
            | 0 -> s
            | x -> sprintf("%s%s") (s |> repeat (x-1)) s
        actual = expected

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