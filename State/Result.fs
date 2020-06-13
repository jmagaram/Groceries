[<AutoOpen>]
module Result

let okValue r =
    match r with
    | Ok v -> Some v
    | Error _ -> None

let okValueOrThrow r =
    r
    |> okValue
    |> Option.get

let errorValue r =
    match r with
    | Ok _ -> None
    | Error e -> Some e

let isOk r = r |> okValue |> Option.isSome

let isError r = r |> errorValue |> Option.isSome

module Tests = 

    open System
    open Xunit
    open FsUnit

    let errorBad = Result<int,string>.Error "Bad"
    let ok9 = Result<int,string>.Ok 9

    [<Fact>]
    let ``okValue - when exists return it`` () = 
        ok9
        |> okValue
        |> should equal (Some 9)

    [<Fact>]
    let ``okValue - when not exists return None`` () = 
        errorBad
        |> okValue
        |> should equal None

    [<Fact>]
    let ``errorValue - when exists return it`` () = 
        errorBad
        |> errorValue
        |> should equal (Some "Bad")

    [<Fact>]
    let ``errorValue - when not exists return None`` () = 
        ok9
        |> errorValue
        |> should equal None

    [<Fact>]
    let ``isOk - when Ok return true`` () = 
        ok9 |> isOk |> fun i -> i |> should equal true

    [<Fact>]
    let ``isOk - when Error return false`` () = 
        errorBad |> isOk |> fun i -> i |> should equal false

    [<Fact>]
    let ``isError - when Ok return false`` () = 
        ok9 |> isError |> fun i -> i |> should equal false

    [<Fact>]
    let ``isError - when Error return true`` () = 
        errorBad |> isError |> fun i -> i |> should equal true
