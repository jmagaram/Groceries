namespace ModelsTest

open System
open Xunit
open FsUnit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit
open Models
open Models.ChangeTrackerTypes

module TestHelpers =

    type Person =
        { Id: int
          Name: string }
        interface IKey<int> with
            member this.Key = this.Id

    let person id name = { Person.Id = id; Name = name }
    let changeNameTo n p = { p with Person.Name = n }

    let bob1 = person 1 "robert"
    let bob2 = person 1 "bob"
    let bob3 = person 1 "bobby"
    let joe1 = person 2 "joseph"
    let joe2 = person 2 "joe"

    let isModified x y r =
        match r with
        | Modified m -> m.Original = x && m.Current = y
        | _ -> false

    let assertIsModified x y r = isModified x y r |> should equal true

    let isAdded x r =
        match r with
        | Added y -> x = y
        | _ -> false

    let assertIsAdded x r = isAdded x r |> should equal true

    let isUnchanged x r =
        match r with
        | Unchanged y -> x = y
        | _ -> false

    let assertisUnchanged x r = isUnchanged x r |> should equal true

    let isDeleted x r =
        match r with
        | Deleted y -> x = y
        | _ -> false

    let assertisDeleted x r = isDeleted x r |> should equal true

    let assertIsSomeDeleted x r =
        match r with
        | Some r -> r |> isDeleted x
        | None -> failwith "Expected Some"

module DataRowTests =
    open TestHelpers

    [<Property>]
    let ``added - creates added row with proper value stored`` (x: int) = DataRow.added x |> assertIsAdded x

    [<Property>]
    let ``unchanged - creates unchanged row with proper value stored`` (x: int) =
        DataRow.unchanged x |> assertisUnchanged x

    [<Property>]
    let ``deleted - creates deleted row with proper value stored`` (x: int) = DataRow.deleted x |> assertisDeleted x

    [<Fact>]
    let ``modified - when values are equal, is unchanged`` () = DataRow.modified bob1 bob1 |> assertisUnchanged bob1

    [<Fact>]
    let ``modified - when values are different, record both`` () =
        DataRow.modified bob1 bob2 |> assertIsModified bob1 bob2

    [<Fact>]
    let ``modified - when keys are different, throw`` () =
        let f () = DataRow.modified bob1 joe1 |> ignore
        f |> shouldFail

    [<Fact>]
    let ``delete - when added return none`` () =
        DataRow.added bob1
        |> DataRow.delete
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``delete - when unchanged set to deleted`` () =
        DataRow.unchanged bob1
        |> DataRow.delete
        |> assertIsSomeDeleted bob1

    [<Fact>]
    let ``delete - when deleted already return self`` () =
        DataRow.deleted bob1
        |> DataRow.delete
        |> assertIsSomeDeleted bob1

    [<Fact>]
    let ``delete - when modified return deleted original`` () =
        DataRow.modified bob1 bob2
        |> DataRow.delete
        |> assertIsSomeDeleted bob1

    [<Fact>]
    let ``tryUpdate - when unchanged and different value, return modified`` () =
        DataRow.unchanged bob1
        |> DataRow.tryUpdate bob2
        |> Result.map (isModified bob1 bob2)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryUpdate - when unchanged and same value, return unchanged`` () =
        DataRow.unchanged bob1
        |> DataRow.tryUpdate bob1
        |> Result.map (isUnchanged bob1)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryUpdate - when added, return added`` () =
        DataRow.added bob1
        |> DataRow.tryUpdate bob2
        |> Result.map (isAdded bob2)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryUpdate - when deleted and different value, return error`` () =
        match DataRow.deleted bob1 |> DataRow.tryUpdate bob2 with
        | Ok r -> false
        | Error RowIsDeleted -> true
        |> should equal true

    [<Fact>]
    let ``tryUpdate - when deleted and same value, return self`` () =
        DataRow.deleted bob1
        |> DataRow.tryUpdate bob1
        |> Result.map (isDeleted bob1)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryUpdate - when modified and different value, return modified`` () =
        DataRow.modified bob1 bob2
        |> DataRow.tryUpdate bob3
        |> Result.map (isModified bob1 bob3)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryUpdate - when modified and original value, return unchanged`` () =
        DataRow.modified bob1 bob2
        |> DataRow.tryUpdate bob1
        |> Result.map (isUnchanged bob1)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryMap - when unchanged usually returns modified`` () =
        DataRow.unchanged bob1
        |> DataRow.tryMap (changeNameTo bob2.Name)
        |> Result.map (isModified bob1 bob2)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryMap - when unchanged but new value same, return unchanged`` () =
        DataRow.unchanged bob1
        |> DataRow.tryMap id
        |> Result.map (isUnchanged bob1)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryMap - when added return added`` () =
        DataRow.added bob1
        |> DataRow.tryMap (changeNameTo bob2.Name)
        |> Result.map (isAdded bob2)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryMap - when deleted and different value return error`` () =
        match DataRow.deleted bob1
              |> DataRow.tryMap (changeNameTo bob2.Name) with
        | Error RowIsDeleted -> true
        | _ -> false
        |> should equal true

    [<Fact>]
    let ``tryMap - when deleted and same value return self`` () =
        match DataRow.deleted bob1 |> DataRow.tryMap id with
        | Ok r -> r |> isDeleted bob1
        | Error RowIsDeleted -> false
        |> should equal true

    [<Fact>]
    let ``tryMap - when modified usually return modified`` () =
        DataRow.modified bob1 bob2
        |> DataRow.tryMap (changeNameTo bob3.Name)
        |> Result.map (isModified bob1 bob3)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``tryMap - when modified can sometimes return to unchanged`` () =
        DataRow.modified bob1 bob2
        |> DataRow.tryMap (changeNameTo bob1.Name)
        |> Result.map (isUnchanged bob1)
        |> Result.okOrDefaultValue false
        |> should equal true

    [<Fact>]
    let ``current - when unchanged return it`` () =
        DataRow.unchanged bob1
        |> DataRow.current
        |> Option.get
        |> should equal bob1

    [<Fact>]
    let ``current - when added return it`` () =
        DataRow.added bob1
        |> DataRow.current
        |> Option.get
        |> should equal bob1

    [<Fact>]
    let ``current - when deleted return none`` () =
        DataRow.deleted bob1
        |> DataRow.current
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``current - when modified return current`` () =
        DataRow.modified bob1 bob2
        |> DataRow.current
        |> Option.get
        |> should equal bob2

    [<Fact>]
    let ``original - when unchanged return it`` () =
        DataRow.unchanged bob1
        |> DataRow.original
        |> should equal bob1

    [<Fact>]
    let ``original - when added return it`` () = DataRow.added bob1 |> DataRow.original |> should equal bob1

    [<Fact>]
    let ``original - when deleted return none`` () =
        DataRow.deleted bob1
        |> DataRow.original
        |> should equal bob1

    [<Fact>]
    let ``original - when modified return original`` () =
        DataRow.modified bob1 bob2
        |> DataRow.original
        |> should equal bob1

    [<Fact>]
    let ``accept changes - when unchanged return self`` () =
        DataRow.unchanged bob1
        |> DataRow.acceptChanges
        |> Option.map (isUnchanged bob1)
        |> Option.defaultValue false
        |> should equal true

    [<Fact>]
    let ``accept changes - when modified return unchanged`` () =
        DataRow.modified bob1 bob2
        |> DataRow.acceptChanges
        |> Option.map (isUnchanged bob2)
        |> Option.defaultValue false
        |> should equal true

    [<Fact>]
    let ``accept changes - when deleted return none`` () =
        DataRow.deleted bob1
        |> DataRow.acceptChanges
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``accept changes - when added return unchanged`` () =
        DataRow.added bob1
        |> DataRow.acceptChanges
        |> Option.map (isUnchanged bob1)
        |> Option.defaultValue false
        |> should equal true

module DataTableTests =
    open TestHelpers
    open DataTable

    type Integer =
        { UniqueId: Guid
          Value: int }
        interface IKey<Guid> with
            member this.Key = this.UniqueId

    let rnd = new Random()
    let rndIntValue () = rnd.Next(0, 20)
    let rndInt () = { UniqueId = Guid.NewGuid(); Value = rndIntValue () }

    type Command =
        | Insert of Integer
        | Delete of Guid
        | Update of Guid * int
        | Upsert of Integer

    [<Property>]
    let ``current - after modifications, contains same items as a non-change-tracking collection`` () =
        let x = Map.empty
        let y = DataTable.empty

        let fold (x, y) _ =
            let rndExist () =
                match x |> Map.isEmpty with
                | true -> None
                | false ->
                    x
                    |> Map.values
                    |> Seq.item (rnd.Next(0, x |> Map.count))
                    |> Some

            let find k s = s |> Map.find k

            let change =
                match rnd.Next() % 5 with
                | 0 -> rndInt () |> Insert |> Some
                | 1 -> rndInt () |> Upsert |> Some
                | 2 ->
                    rndExist ()
                    |> Option.map (fun i -> Update(i.UniqueId, rndIntValue ()))
                | 3 ->
                    rndExist ()
                    |> Option.map (fun i -> Upsert { i with Value = rndIntValue () })
                | _ -> rndExist () |> Option.map (fun n -> n.UniqueId |> Delete)

            match change with
            | Some (Insert n) -> (x |> Map.add n.UniqueId n, y |> DataTable.tryInsert n |> Result.okOrThrow)
            | Some (Delete k) -> (x |> Map.remove k, y |> DataTable.tryDelete k |> Result.okOrThrow)
            | Some (Upsert n) -> (x |> Map.add n.UniqueId n, y |> DataTable.tryUpsert n |> Result.okOrThrow)
            | Some (Update (k, v)) ->
                let existing = x |> find k
                let changed = { existing with Value = v }
                (x |> Map.add k changed, y |> DataTable.tryUpdate changed |> Result.okOrThrow)
            | None -> (x, y)

        let (x, y) = seq { 1 .. 1000 } |> Seq.fold fold (x, y)
        let x = x |> Map.values |> Set.ofSeq
        let y = y |> DataTable.current |> Set.ofSeq
        x |> should equal y

    let tryDeleteOrThrow k dt = dt |> tryDelete k |> Result.okOrThrow
    let tryUpdateOrThrow k dt = dt |> tryUpdate k |> Result.okOrThrow

    [<Fact>]
    let ``tryDelete - when delete an added row, remove it`` () =
        empty
        |> insert bob1
        |> tryDeleteOrThrow bob1.Id
        |> tryFindRow bob1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``tryDelete - when delete an unchanged row, mark it as deleted`` () =
        empty
        |> insert bob1
        |> acceptChanges
        |> tryDeleteOrThrow bob1.Id
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisDeleted bob1

    [<Fact>]
    let ``tryDelete - when delete a modified row, mark it as deleted`` () =
        empty
        |> insert bob1
        |> acceptChanges
        |> update bob2
        |> tryDeleteOrThrow bob1.Id
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisDeleted bob1

    [<Fact>]
    let ``tryDelete - when delete a deleted row many times, do nothing`` () =
        empty
        |> insert bob1
        |> acceptChanges
        |> tryDeleteOrThrow bob1.Id
        |> tryDeleteOrThrow bob1.Id
        |> tryDeleteOrThrow bob1.Id
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisDeleted bob1

    [<Fact>]
    let ``deleteIf - has no effect on deleted rows`` () =
        empty
        |> insert bob1
        |> acceptChanges
        |> tryDeleteOrThrow bob1.Id
        |> deleteIf (fun i -> i.Id = bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisDeleted bob1

    [<Fact>]
    let ``deleteIf - when matches an added row, remove it`` () =
        empty
        |> insert bob1
        |> deleteIf (fun i -> i.Id = bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``deleteIf - when matches an unchanged row, remove it`` () =
        empty
        |> insert bob1
        |> acceptChanges
        |> deleteIf (fun i -> i.Id = bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisDeleted bob1

    [<Fact>]
    let ``deleteIf - when matches a modified row, delete it`` () =
        empty
        |> insert bob1
        |> acceptChanges
        |> tryUpdateOrThrow bob2
        |> deleteIf (fun i -> i.Id = bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisDeleted bob1

    [<Fact>]
    let ``acceptChange delete - when unchanged remove the row entirely``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> acceptChange (Change.Delete bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``acceptChange delete - when modified remove the row entirely``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> tryUpdateOrThrow bob2
        |> acceptChange (Change.Delete bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``acceptChange delete - when added remove the row entirely``()=
        empty
        |> insert bob1
        |> acceptChange (Change.Delete bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``acceptChange delete - when deleted remove the row entirely``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> tryDeleteOrThrow bob1.Id
        |> acceptChange (Change.Delete bob1.Id)
        |> tryFindRow bob1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``acceptChange delete - when row does not exist do nothing``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> acceptChange (Change.Delete joe1.Id)
        |> tryFindRow joe1.Id
        |> Option.isNone
        |> should equal true

    [<Fact>]
    let ``acceptChange upsert - when unchanged, replace and make unchanged``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> acceptChange (Change.Upsert bob2)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisUnchanged bob2

    [<Fact>]
    let ``acceptChange upsert - when added, replace and make unchanged``()=
        empty
        |> insert bob1
        |> acceptChange (Change.Upsert bob2)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisUnchanged bob2

    [<Fact>]
    let ``acceptChange upsert - when deleted, replace and make unchanged``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> tryDeleteOrThrow bob1.Id
        |> acceptChange (Change.Upsert bob2)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisUnchanged bob2

    [<Fact>]
    let ``acceptChange upsert - when modified, replace and make unchanged``()=
        empty
        |> insert bob1
        |> acceptChanges
        |> update bob2
        |> acceptChange (Change.Upsert bob3)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisUnchanged bob3

    [<Fact>]
    let ``acceptChange upsert - when not exist, insert and make unchanged``()=
        empty
        |> acceptChange (Change.Upsert bob1)
        |> tryFindRow bob1.Id
        |> Option.get
        |> assertisUnchanged bob1
