[<AutoOpen>]
module Map

let values m =
    m
    |> Map.toSeq
    |> Seq.map (fun (k, v) -> v)

let keys m =
    m
    |> Map.toSeq
    |> Seq.map (fun (k, v) -> k)

module Tests = 

    open System
    open Xunit
    open FsUnit

    [<Fact>]
    let ``values - when some exist return them`` () = 
        Map.empty
        |> Map.add 1 "1"
        |> Map.add 2 "2"
        |> values
        |> should equivalent ["1"; "2"]

    [<Fact>]
    let ``keys - when some exist return them`` () = 
        Map.empty
        |> Map.add 1 "1"
        |> Map.add 2 "2"
        |> keys
        |> should equivalent [1; 2]

