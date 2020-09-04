namespace Models

open DomainTypes

module Modified =

    type CreateError =
        | ValuesAreEqual
        | KeysAreDifferent

    let create v v' =
        match v = v' with
        | true -> ValuesAreEqual |> Error
        | false ->
            let vKey = v |> keyOf
            let vKey' = v' |> keyOf
            match vKey = vKey' with
            | true -> { Original = v; Current = v' } |> Ok
            | false -> KeysAreDifferent |> Error

    let current (m: Modified<_>) = m.Current

    let original (m: Modified<_>) = m.Original

    let update v (m: Modified<_>) = create (m |> original) v

module DataRow =

    type UpdateError =
        | DeletedRowsCanNotBeUpdated
        | KeysAreDifferent

    type DeleteError =
        | DeletedRowsCanNotBeDeletedAgain
        | AddedRowsCanNotBeDeleted

    let unchanged v = Unchanged v

    let current r =
        match r with
        | Unchanged v -> Some v
        | Added v -> Some v
        | Modified m -> m |> Modified.current |> Some
        | Deleted _ -> None

    let update v' r =
        match r with
        | Unchanged v ->
            match Modified.create v v' with
            | Ok m -> m |> Modified |> Ok
            | Error Modified.KeysAreDifferent -> KeysAreDifferent |> Error
            | Error Modified.ValuesAreEqual -> r |> Ok
        | Deleted _ -> DeletedRowsCanNotBeUpdated |> Error
        | Modified m ->
            match m |> Modified.update v' with
            | Ok m -> m |> Modified |> Ok
            | Error Modified.ValuesAreEqual -> v' |> Unchanged |> Ok
            | Error Modified.KeysAreDifferent -> KeysAreDifferent |> Error
        | Added _ -> Added v' |> Ok

    let delete r =
        match r with
        | Unchanged v -> Deleted v |> Ok
        | Deleted _ -> DeletedRowsCanNotBeDeletedAgain |> Error
        | Modified m -> m |> Modified.original |> Deleted |> Ok
        | Added _ -> AddedRowsCanNotBeDeleted |> Error

    let hasChanges r =
        match r with
        | Unchanged _ -> false
        | _ -> true

    let acceptChanges r =
        match r with
        | Unchanged _ -> r |> Some
        | Deleted _ -> None
        | Modified m -> m |> Modified.current |> Unchanged |> Some
        | Added v -> Unchanged v |> Some

    let rejectChanges r =
        match r with
        | Unchanged _ -> r |> Some
        | Deleted v -> Unchanged v |> Some
        | Modified m -> m |> Modified.original |> Unchanged |> Some
        | Added _ -> None

module DataTable =

    type InsertError = | DuplicateKey

    type DeleteError =
        | RowNotFoundToDelete
        | DeletedRowsCanNotBeDeletedAgain

    type UpdateError =
        | RowNotFoundToUpdate
        | DeletedRowsCanNotBeUpdated
        | KeyCanNotBeChanged

    let empty = Map.empty

    let insert v dt =
        let key = v |> keyOf
        match dt |> Map.tryFind key with
        | None -> dt |> Map.add key (Added v) |> DataTable |> Ok
        | Some _ -> DuplicateKey |> Error

    let update v dt =
        let key = v |> keyOf
        match dt |> Map.tryFind key with
        | None -> RowNotFoundToUpdate |> Error
        | Some r ->
            match r |> DataRow.update v with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRow.DeletedRowsCanNotBeUpdated -> DeletedRowsCanNotBeUpdated |> Error
            | Error DataRow.KeysAreDifferent -> KeyCanNotBeChanged |> Error

    let upsert v dt =
        match dt |> insert v with
        | Ok dt -> dt |> Ok
        | Error DuplicateKey -> dt |> update v

    let delete key dt =
        match dt |> Map.tryFind key with
        | None -> RowNotFoundToDelete |> Error
        | Some r ->
            match r |> DataRow.delete with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRow.DeletedRowsCanNotBeDeletedAgain -> DeletedRowsCanNotBeDeletedAgain |> Error
            | Error DataRow.AddedRowsCanNotBeDeleted -> dt |> Map.remove key |> DataTable |> Ok

    let current dt =
        let (DataTable dt) = dt
        dt |> Map.values |> Seq.choose DataRow.current

    let acceptChanges dt =
        let (DataTable dt) = dt
        dt
        |> Map.toSeq
        |> Seq.choose (fun (k, v) ->
            v
            |> DataRow.acceptChanges
            |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> DataTable

    let rejectChanges dt =
        let (DataTable dt) = dt
        dt
        |> Map.toSeq
        |> Seq.choose (fun (k, v) ->
            v
            |> DataRow.rejectChanges
            |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> DataTable
