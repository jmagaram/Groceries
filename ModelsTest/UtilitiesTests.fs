namespace ModelsTest

open Models
open Xunit
open FsUnit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit

module SeqTests =

    [<Property>]
    let ``takeAtMost returns min of seq size and take size`` (NonNegativeInt sourceSize) (NonNegativeInt takeSize) =
        let sourceSize = sourceSize % 5
        let takeSize = takeSize % 10
        let expectedResultSize = min sourceSize takeSize
        let source = Seq.replicate sourceSize "a"

        let actual =
            source
            |> Seq.takeAtMost expectedResultSize
            |> List.ofSeq

        let expected =
            source
            |> Seq.take expectedResultSize
            |> List.ofSeq

        actual |> should equal expected

    [<Property>]
    let ``takeAtMost throws if take size is less than 0`` (NegativeInt takeSize) (s:string) =
        let takeNegative () = s |> Seq.takeAtMost takeSize |> ignore
        takeNegative |> shouldFail