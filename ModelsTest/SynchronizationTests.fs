namespace ModelsTest

open System
open Xunit
open FsUnit
open FsUnit.Xunit
open Models
open Models.DomainTypes

module SynchronizationTestHelpers = 

    type Person =
        { Id: int
          Name: string }
        interface IKey<int> with
            member this.Key = this.Id

    let person id name = { Person.Id = id; Name = name }
    let bob = person 1 "robert"
    let bob' = person 1 "bob"
    let joe = person 2 "joseph"
    let joe' = person 2 "joe"

module DataRowTests =
    open SynchronizationTestHelpers

    [<Fact>]
    let ``when create a MODIFIED row, before and after values are set correctly`` () =
        let actual =
            match DataRow.modified bob bob' with
            | Modified m -> m.Current = bob' && m.Original = bob
            | _ -> false

        actual |> should equal true

    [<Fact>]
    let ``when update an UNCHANGED row with same value, return UNCHANGED`` () =
        let actual =
            bob |> DataRow.unchanged |> DataRow.update bob

        let expected = bob |> DataRow.unchanged
        actual |> should equal expected

    [<Fact>]
    let ``when update an UNCHANGED row with a different key, throw`` () =
        let updateWithDifferentKey () =
            bob
            |> DataRow.unchanged
            |> DataRow.update joe
            |> ignore

        updateWithDifferentKey |> shouldFail

    [<Fact>]
    let ``when update an UNCHANGED row, return MODIFIED`` () =
        let actual =
            bob |> DataRow.unchanged |> DataRow.update bob'

        let expected = DataRow.modified bob bob'
        actual |> should equal expected

    [<Fact>]
    let ``when delete an ADDED row, return None`` () =
        let actual = bob |> DataRow.added |> DataRow.delete
        let expected = None
        actual |> should equal expected

    [<Fact>]
    let ``when delete an UNCHANGED row, return Some unchanged value`` () =
        let actual =
            bob |> DataRow.unchanged |> DataRow.delete

        let expected = bob |> DataRow.deleted |> Some
        actual |> should equal expected

    [<Fact>]
    let ``when delete a MODIFIED row, return DELETED row with original value`` () =
        let actual =
            bob
            |> DataRow.unchanged
            |> DataRow.update bob'
            |> DataRow.delete

        let expected = bob |> DataRow.deleted |> Some
        actual |> should equal expected

    [<Fact>]
    let ``when delete row a DELETED row, throw`` () =
        let deleteTwice () =
            bob
            |> DataRow.unchanged
            |> DataRow.delete
            |> Option.bind DataRow.delete
            |> ignore

        deleteTwice |> shouldFail

module DataTableTests =
    open SynchronizationTestHelpers

    [<Fact>]
    let ``when delete row an ADDED row, remove it`` () =
        let actual = 
            DataTable.empty 
            |> DataTable.insert bob
            |> DataTable.delete bob.Id
        actual |> DataTable.current |> Seq.isEmpty |> should equal true
        actual |> DataTable.hasChanges |> not

    [<Fact>]
    let ``when delete an UNCHANGED row, mark it as deleted`` () =
        let actual = 
            DataTable.empty 
            |> DataTable.insert bob
            |> DataTable.acceptChanges
            |> DataTable.delete bob.Id
        actual |> DataTable.current |> Seq.isEmpty |> should equal true
        actual |> DataTable.hasChanges |> should equal true
