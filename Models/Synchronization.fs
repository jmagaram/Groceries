﻿namespace Models
open DomainTypes

module DataRow =

    let unchanged v = Unchanged v

    let acceptChanges row =
        match row with
        | Unchanged _ -> row |> Some
        | Deleted _ -> None
        | Modified m -> Unchanged m.Current |> Some
        | Added i -> Unchanged i |> Some

    let rejectChanges row =
        match row with
        | Unchanged _ -> row |> Some
        | Deleted v -> Unchanged v |> Some
        | Modified m -> Unchanged m.Original |> Some
        | Added i -> None

    let current row = 
        match row with
        | Unchanged t -> Some t
        | Added t -> Some t
        | Modified m -> Some m.Current
        | Deleted _ -> None

    let update v' row = 
        match row with
        | Unchanged v -> 
            if v <> v'
            then Modified { Original = v; Current = v' } |> Ok
            else row |> Ok
        | Deleted _ -> DataRowUpdateError.DeletedRowsCanNotBeUpdated |> Error
        | Modified m -> 
            if m.Original = v'
            then Unchanged v' |> Ok
            else { m with Current = v' } |> Modified |> Ok
        | Added v -> Added v' |> Ok

    let delete row =
        match row with
        | Unchanged v -> Deleted v |> Ok
        | Deleted _ -> DataRowDeleteError.DeletedRowsCanNotBeDeletedAgain |> Error
        | Modified m -> m.Original |> Deleted |> Ok
        | Added _ -> DataRowDeleteError.AddedRowsCanNotBeDeleted |> Error

module DataTable = 

    let empty = Map.empty

    let insert pk i dt = 
        let key = pk i
        match dt |> Map.tryFind key with
        | None -> dt |> Map.add key (Added i) |> DataTable |> Ok
        | Some _ -> DataTableInsertError.DuplicateKey |> Error

    let update pk i dt =
        let key = pk i
        match dt |> Map.tryFind key with
        | None -> DataTableUpdateError.RowNotFound |> Error
        | Some r -> 
            match r |> DataRow.update i with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRowUpdateError.DeletedRowsCanNotBeUpdated -> DataTableUpdateError.DeletedRowsCanNotBeUpdated |> Error

    let upsert pk i dt =
        match dt |> insert pk i with
        | Ok dt -> dt |> Ok
        | Error DataTableInsertError.DuplicateKey -> dt |> update pk i

    let delete key dt =
        match dt |> Map.tryFind key with
        | None -> DataTableDeleteError.RowNotFound |> Error
        | Some r ->
            match r |> DataRow.delete with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRowDeleteError.DeletedRowsCanNotBeDeletedAgain -> DataTableDeleteError.DeletedRowsCanNotBeDeletedAgain |> Error
            | Error DataRowDeleteError.AddedRowsCanNotBeDeleted -> dt |> Map.remove key |> DataTable |> Ok

    let current (dt:DataTable<_,_>) =
        match dt with
        | DataTable dt ->
            dt
            |> Map.values
            |> Seq.choose DataRow.current

    let acceptChanges (dt:DataTable<_,_>) =
        match dt with
        | DataTable dt ->
            dt
            |> Map.toSeq
            |> Seq.choose (fun (k, v) -> v |> DataRow.acceptChanges |> Option.map (fun v -> (k, v)))
            |> Map.ofSeq
            |> DataTable

    let rejectChanges dt =
        let (DataTable dt) = dt
        dt
        |> Map.toSeq
        |> Seq.choose (fun (k, v) -> v |> DataRow.rejectChanges |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> DataTable
