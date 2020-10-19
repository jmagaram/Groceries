namespace Models

open ChangeTrackerTypes

module DataRow =

    let added v = Added v

    let unchanged v = Unchanged v

    let modified a b =
        match a = b with
        | true -> Unchanged b
        | false ->
            let aKey = a |> keyOf
            let bKey = b |> keyOf

            match aKey = bKey with
            | false -> failwith "It is not possible to change the key."
            | true -> Modified {| Original = a; Current = b |}

    let deleted v = Deleted v

    let current r =
        match r with
        | Unchanged v -> v |> Some
        | Modified m -> m.Current |> Some
        | Added v -> v |> Some
        | Deleted _ -> None

    let original r =
        match r with
        | Unchanged v -> v
        | Modified m -> m.Original
        | Added v -> v
        | Deleted v -> v

    let delete r =
        match r with
        | Unchanged v -> deleted v |> Some
        | Modified m -> deleted m.Original |> Some
        | Added _ -> None
        | Deleted _ -> r |> Some

    let tryUpdate v' r =
        match r with
        | Unchanged v -> modified v v' |> Ok
        | Modified m -> modified m.Original v' |> Ok
        | Added _ -> added v' |> Ok
        | Deleted v ->
            match v = v' with
            | false -> RowIsDeleted |> Error
            | true -> r |> Ok

    let tryMap f r =
        match r with
        | Unchanged v -> r |> tryUpdate (f v)
        | Modified m -> r |> tryUpdate (f m.Current)
        | Added v -> r |> tryUpdate (f v)
        | Deleted v -> r |> tryUpdate (f v)

    let isAddedOrModified r =
        match r with
        | Added v -> Some v
        | Modified m -> Some m.Current
        | _ -> None

    let isDeleted r =
        match r with
        | Deleted v -> Some v
        | _ -> None

    let hasChanges r =
        match r with
        | Unchanged _ -> false
        | _ -> true

    let acceptChanges r =
        match r with
        | Unchanged _ -> r |> Some
        | Modified m -> m.Current |> unchanged |> Some
        | Added v -> v |> unchanged |> Some
        | Deleted _ -> None

    let rejectChanges r =
        match r with
        | Unchanged _ -> r |> Some
        | Modified m -> m.Original |> unchanged |> Some
        | Added _ -> None
        | Deleted v -> v |> unchanged |> Some

module DataTable =

    let private fromMap dt = DataTable dt

    let asMap dt =
        match dt with
        | DataTable dt -> dt

    let empty<'Key, 'T when 'Key: comparison> = Map.empty<'Key, DataRow<'T>> |> DataTable

    let tryFindRow k dt = dt |> asMap |> Map.tryFind k

    let tryRemoveRow k dt =
        match dt |> asMap |> Map.containsKey k with
        | true -> dt |> asMap |> Map.remove k |> fromMap |> Ok
        | false -> Error RowToDeleteNotFound

    let tryInsert v dt =
        let k = v |> keyOf
        let rowHasKey k dt = dt |> asMap |> Map.containsKey k

        match dt |> rowHasKey k with
        | true -> Error DuplicateKey
        | false -> dt |> asMap |> Map.add k (DataRow.added v) |> fromMap |> Ok

    let insert v dt =
        match dt |> tryInsert v with
        | Ok dt -> dt
        | Error DuplicateKey -> failwith "A row with that key already exists."

    let tryUpdate v dt =
        result {
            let k = v |> keyOf
            let! row = dt |> tryFindRow k |> Option.asResult RowToUpdateNotFound
            let! row = row |> DataRow.tryUpdate v |> Result.mapError (fun _ -> RowIsDeletedInTable)
            return dt |> asMap |> Map.add k row |> fromMap
        }

    let update v dt = dt |> tryUpdate v |> Result.okOrThrow

    let tryUpsert v dt =
        result {
            let k = v |> keyOf
            let dt = dt |> asMap

            let! r =
                match dt |> Map.tryFind k with
                | None -> DataRow.added v |> Ok
                | Some r -> r |> DataRow.tryUpdate v

            return dt |> Map.add k r |> fromMap
        }

    let upsert v dt = dt |> tryUpsert v |> Result.okOrThrow

    let upsertUnchanged v dt =
        let key = keyOf v
        let row = DataRow.unchanged v
        dt |> asMap |> Map.add key row |> fromMap

    let current dt =
        dt
        |> asMap
        |> Seq.choose (fun kv -> kv.Value |> DataRow.current)

    let tryDelete k dt =
        result {
            let! r = dt |> tryFindRow k |> Option.asResult RowToDeleteNotFound

            return
                match r |> DataRow.delete with
                | None -> dt |> asMap |> Map.remove k |> fromMap
                | Some r -> dt |> asMap |> Map.add k r |> fromMap
        }

    let delete k dt =
        match dt |> tryDelete k with
        | Error RowToDeleteNotFound -> dt
        | Ok dt -> dt

    let deleteIf p dt =
        dt
        |> current
        |> Seq.choose (fun v -> if p v then Some(keyOf v) else None)
        |> Seq.fold (fun dt k -> dt |> delete k) dt

    let hasChanges dt =
        dt
        |> asMap
        |> Map.exists (fun k v -> v |> DataRow.hasChanges)

    let private choose f dt =
        dt
        |> asMap
        |> Map.toSeq
        |> Seq.choose (fun (k, v) -> f v |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> fromMap

    let acceptChanges dt = dt |> choose DataRow.acceptChanges

    let rejectChanges dt = dt |> choose DataRow.rejectChanges

    let isAddedOrModified dt =
        dt
        |> asMap
        |> Map.values
        |> Seq.choose DataRow.isAddedOrModified

    let isDeleted dt = dt |> asMap |> Map.values |> Seq.choose DataRow.isDeleted

    let tryFindCurrent k dt = dt |> asMap |> Map.tryFind k |> Option.bind DataRow.current

    let findCurrent k dt = tryFindCurrent k dt |> Option.get

    let acceptChange c dt =
        match c with
        | Delete k -> dt |> tryRemoveRow k |> Result.okOrDefaultValue dt
        | Upsert v -> dt |> upsertUnchanged v
