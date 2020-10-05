namespace ModelsTest

open System
open Xunit
open FsUnit
open FsUnit.Xunit
open Models
open Models.SynchronizationTypes

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
    let ``when delete row a DELETED row, do nothing`` () =
        let expected = 
            bob
            |> DataRow.unchanged
            |> DataRow.delete

        let actual = 
            bob
            |> DataRow.unchanged
            |> DataRow.delete
            |> Option.bind DataRow.delete

        actual |> should equal expected

module DataTableTests =
    open SynchronizationTestHelpers
    open DataTable

    [<Fact>]
    let ``when delete an ADDED row, remove it`` () =
        let actual = empty |> insert bob |> delete bob.Id
        actual
        |> asMap
        |> Map.isEmpty
        |> should equal true

    [<Fact>]
    let ``when delete an UNCHANGED row, mark it as deleted`` () =
        let actual =
            empty
            |> insert bob
            |> acceptChanges
            |> delete bob.Id

        actual
        |> asMap
        |> Map.values
        |> Seq.exactlyOne
        |> should equal (DataRow.deleted bob)

    [<Fact>]
    let ``when deleteIf an ADDED row, remove it`` () =
        let actual =
            empty
            |> insert bob
            |> insert joe
            |> deleteIf (fun i -> i.Name.StartsWith(bob.Name.Substring(0,2)))

        actual
        |> asMap
        |> Map.values
        |> Seq.exactlyOne
        |> should equal (DataRow.added joe)

    [<Fact>]
    let ``when deleteIf an UNCHANGED row, mark it as deleted`` () =
        let actual =
            empty
            |> insert bob
            |> insert joe
            |> acceptChanges
            |> deleteIf (fun i -> i.Name.StartsWith(bob.Name.Substring(0,2)))
            |> asMap
            |> Map.values
            |> Seq.toList
        actual |> List.length |> should equal 2
        actual |> List.contains (DataRow.unchanged joe) |> should equal true
        actual |> List.contains (DataRow.deleted bob) |> should equal true
