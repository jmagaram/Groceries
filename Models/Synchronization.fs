namespace Models
open DomainTypes

module Modified = 

    let current (m:Modified<_>) = m.Current

    let original (m:Modified<_>) = m.Original

    let create original current =
        match original = current with
        | true -> ModifiedError.ValuesAreTheSame |> Error
        | false -> 
            if (original |> keyOf) <> (current |> keyOf)
            then ModifiedError.KeysAreDifferent |> Error
            else { Modified.Current = current; Modified.Original = original } |> Ok

    let updateCurrent v (m:Modified<_>) = create m.Original v

module DataRow =

    let unchanged v = Unchanged v

    let acceptChanges row =
        match row with
        | Unchanged _ -> row |> Some
        | Deleted _ -> None
        | Modified m -> m |> Modified.current |> Unchanged |> Some
        | Added i -> Unchanged i |> Some

    let rejectChanges row =
        match row with
        | Unchanged _ -> row |> Some
        | Deleted v -> Unchanged v |> Some
        | Modified m -> m |> Modified.original |> Unchanged |> Some
        | Added _ -> None

    let current row = 
        match row with
        | Unchanged t -> Some t
        | Added t -> Some t
        | Modified m -> m |> Modified.current |> Some
        | Deleted _ -> None

    let update v' row =
        match row with
        | Unchanged v -> 
            match Modified.create v v' with
            | Ok m -> m |> Modified |> Ok
            | Error ModifiedError.KeysAreDifferent -> DataRowUpdateError.KeyCanNotBeChanged |> Error
            | Error ModifiedError.ValuesAreTheSame -> row |> Ok
        | Deleted _ -> DataRowUpdateError.DeletedRowsCanNotBeUpdated |> Error
        | Modified m -> 
            match m |> Modified.updateCurrent v' with
            | Ok m -> m |> Modified |> Ok
            | Error ModifiedError.KeysAreDifferent -> DataRowUpdateError.KeyCanNotBeChanged |> Error
            | Error ModifiedError.ValuesAreTheSame -> row |> Ok
        | Added v -> Added v' |> Ok

    let delete row =
        match row with
        | Unchanged v -> Deleted v |> Ok
        | Deleted _ -> DataRowDeleteError.DeletedRowsCanNotBeDeletedAgain |> Error
        | Modified m -> m |> Modified.original |> Deleted |> Ok
        | Added _ -> DataRowDeleteError.AddedRowsCanNotBeDeleted |> Error

module DataTable = 

    let empty = Map.empty

    let insert i dt = 
        let key = i |> keyOf
        match dt |> Map.tryFind key with
        | None -> dt |> Map.add key (Added i) |> DataTable |> Ok
        | Some _ -> DataTableInsertError.DuplicateKey |> Error

    let update i dt =
        let key = i |> keyOf
        match dt |> Map.tryFind key with
        | None -> DataTableUpdateError.RowNotFound |> Error
        | Some r -> 
            match r |> DataRow.update i with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRowUpdateError.DeletedRowsCanNotBeUpdated -> 
                DataTableUpdateError.DeletedRowsCanNotBeUpdated |> Error
            | Error DataRowUpdateError.KeyCanNotBeChanged -> 
                DataTableUpdateError.KeyCanNotBeChanged |> Error

    let upsert i dt =
        match dt |> insert i with
        | Ok dt -> dt |> Ok
        | Error DataTableInsertError.DuplicateKey -> dt |> update i

    let delete key dt =
        match dt |> Map.tryFind key with
        | None -> DataTableDeleteError.RowNotFound |> Error
        | Some r ->
            match r |> DataRow.delete with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRowDeleteError.DeletedRowsCanNotBeDeletedAgain -> 
                DataTableDeleteError.DeletedRowsCanNotBeDeletedAgain |> Error
            | Error DataRowDeleteError.AddedRowsCanNotBeDeleted -> 
                dt |> Map.remove key |> DataTable |> Ok

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

