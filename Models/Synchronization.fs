namespace Models

open DomainTypes

module Modified =

    type Error<'T> =
        | ValuesAreEqual of 'T 
        | KeysAreDifferent

    let create v v' =
        match v = v' with
        | true -> ValuesAreEqual v |> Error
        | false ->
            let vKey = v |> keyOf
            let vKey' = v' |> keyOf
            match vKey = vKey' with
            | true -> { Original = v; Current = v' } |> Ok
            | false -> KeysAreDifferent |> Error

    let current (m: Modified<_>) = m.Current

    let original (m: Modified<_>) = m.Original

    let update v (m: Modified<_>) = create (m |> original) v

    let map f (m: Modified<_>) =
        create (m |> original) (m |> current |> f)

module CurrentModule = 

    // active?

    type Current<'T> = 
        | Unchanged of 'T
        | Added of 'T
        | Modified of Modified<'T>

    type Row<'T> =
        | Deleted of 'T
        | Active of Current<'T>

    let value c = 
        match c with
        | Unchanged v -> v
        | Added v -> v
        | Modified m -> m |> Modified.current

    let updateCurrent v' c =
        match c with
        | Unchanged v -> 
            match Modified.create v v' with
            | Ok m -> Modified m
            | Error Modified.KeysAreDifferent -> failwith "aa"
            | Error (Modified.ValuesAreEqual _) -> c
        | Added _ -> Added v'
        | Modified m -> 
            match m |> Modified.update v' with
            | Ok m -> Modified m
            | Error Modified.KeysAreDifferent -> failwith "aa"
            | Error (Modified.ValuesAreEqual _) -> Unchanged v'

    let mapCurrent f c =
        let v' = c |> value |> f
        c |> updateCurrent v'

    let acceptChangesCurrent c =
        match c with
        | Unchanged v -> c
        | Added v -> Unchanged v
        | Modified m -> m |> Modified.current |> Unchanged

    let mapCurrentRow f r =
        match r with
        | Active c -> c |> mapCurrent f |> Active
        | Deleted _ -> r

module DataRow =

    type UpdateRowError =
        | DeletedRowsCanNotBeUpdated
        | KeysAreDifferent

    type DeleteRowError =
        | DeletedRowsCanNotBeDeletedAgain
        | AddedRowsCanNotBeDeleted

    let unchanged v = v |> Unchanged

    let current r =
        match r with
        | DataRow.Unchanged v -> v |> Some
        | Added v -> v |>  Some 
        | Modified m -> m |> Modified.current|> Some
        | Deleted _ -> None

    let update v' r =
        match r with
        | Unchanged v -> 
            match Modified.create v v' with
            | Ok m -> m |> Modified |>  Ok
            | Error Modified.KeysAreDifferent -> KeysAreDifferent |> Error
            | Error (Modified.ValuesAreEqual _) -> r |> Ok
        | Deleted _ -> DeletedRowsCanNotBeUpdated |> Error
        | Modified m ->
            match m |> Modified.update v' with
            | Ok m -> m  |> Modified |> Ok
            | Error Modified.KeysAreDifferent -> KeysAreDifferent |> Error
            | Error (Modified.ValuesAreEqual v) -> v |> Unchanged |>  Ok
        | Added _ -> v' |> Added |> Ok

    let delete r =
        match r with
        | Unchanged v -> v |> Deleted |>  Ok
        | Deleted _ -> DeletedRowsCanNotBeDeletedAgain |> Error
        | Modified m -> m |> Modified.original |> Deleted |>  Ok
        | Added _ -> AddedRowsCanNotBeDeleted |> Error

    let map f r =
        match r with
        | Deleted _ -> DeletedRowsCanNotBeUpdated |> Error
        | Unchanged v -> 
            let v' = f v // better as Current |> Map
            match Modified.create v v' with
            | Ok m -> m |> Modified |>  Ok
            | Error Modified.KeysAreDifferent -> KeysAreDifferent |> Error
            | Error (Modified.ValuesAreEqual _) -> r |> Ok
        | Modified m ->
            match m |> Modified.map f with
            | Ok m -> m  |> Modified |> Ok
            | Error Modified.KeysAreDifferent -> KeysAreDifferent |> Error
            | Error (Modified.ValuesAreEqual v') -> v' |> Unchanged |>  Ok
        | Added v -> v |> f |> Added |> Ok

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

    let empty<'Key, 'T when 'Key: comparison> =
        Map.empty<'Key, DataRow<'T>> |> DataTable

    let insert v dt =
        let (DataTable dt) = dt
        let key = v |> keyOf
        match dt |> Map.tryFind key with
        | None -> dt |> Map.add key (Added v) |> DataTable |> Ok
        | Some _ -> DuplicateKey |> Error

    let current dt =
        let (DataTable dt) = dt
        dt |> Map.values |> Seq.choose DataRow.current

    let update v dt =
        let (DataTable dt) = dt
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
        let (DataTable dt) = dt
        match dt |> Map.tryFind key with
        | None -> RowNotFoundToDelete |> Error
        | Some r ->
            match r |> DataRow.delete with
            | Ok r -> dt |> Map.add key r |> DataTable |> Ok
            | Error DataRow.DeletedRowsCanNotBeDeletedAgain -> DeletedRowsCanNotBeDeletedAgain |> Error
            | Error DataRow.AddedRowsCanNotBeDeleted -> dt |> Map.remove key |> DataTable |> Ok

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
