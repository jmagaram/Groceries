namespace Models

open DomainTypes

module ActiveRow =

    let added v = Added v

    let unchanged v = Unchanged v

    let modified v v' =
        match v = v' with
        | true -> Unchanged v'
        | false ->
            let vKey = v |> keyOf
            let vKey' = v' |> keyOf
            match vKey = vKey' with
            | false -> failwith "It is not possible to change the key when modifying a data row."
            | true -> Modified {| Original = v; Current = v' |}

    let update v' a =
        match a with
        | Unchanged v -> modified v v'
        | Modified m -> modified m.Original v'
        | Added _ -> added v'

    let map f a =
        match a with
        | Unchanged v -> modified v (f v)
        | Modified m -> modified m.Original (f m.Current)
        | Added v -> added (f v)

    let delete a =
        match a with
        | Unchanged v -> v |> Some
        | Modified m -> m.Original |> Some
        | Added _ -> None

    let value a =
        match a with
        | Unchanged v -> v
        | Modified m -> m.Current
        | Added v -> v

    let hasChanges a =
        match a with
        | Unchanged _ -> false
        | Modified _ -> true
        | Added _ -> true

    let acceptChanges a = a |> value |> Unchanged

    let rejectChanges a =
        match a with
        | Unchanged _ -> a |> Some
        | Modified m -> m.Original |> unchanged |> Some
        | Added _ -> None

module DataRow =

    type UpdateError = | DeletedRowsCanNotBeUpdated

    type DeleteError =
        | AddedRowsCanNotBeDeleted
        | DeletedRowsCanNotBeDeletedAgain

    let activeRow a = a |> ActiveRow

    let deletedRow v = v |> DeletedRow

    let update v r =
        match r with
        | DeletedRow _ -> failwith "Deleted rows can not be updated."
        | ActiveRow a -> a |> ActiveRow.update v |> activeRow

    let map f r =
        match r with
        | DeletedRow _ -> failwith "Deleted rows can not be updated."
        | ActiveRow a -> a |> ActiveRow.map f |> activeRow

    let delete r =
        match r with
        | DeletedRow _ -> failwith "Deleted rows can not be deleted again."
        | ActiveRow a ->
            a
            |> ActiveRow.delete
            |> Option.map (fun v -> DeletedRow v)

    let current r =
        match r with
        | DeletedRow _ -> None
        | ActiveRow a -> a |> ActiveRow.value |> Some

    let hasChanges r =
        match r with
        | DeletedRow _ -> true
        | ActiveRow a -> a |> ActiveRow.hasChanges

    let acceptChanges r =
        match r with
        | DeletedRow _ -> None
        | ActiveRow a -> a |> ActiveRow.acceptChanges |> activeRow |> Some

    let rejectChanges r =
        match r with
        | DeletedRow v -> v |> ActiveRow.unchanged |> activeRow |> Some
        | ActiveRow a ->
            a
            |> ActiveRow.rejectChanges
            |> Option.map activeRow

module DataTable =

    let empty<'Key, 'T when 'Key: comparison> =
        Map.empty<'Key, DataRow<'T>> |> DataTable

    let containsKey k (DataTable dt) = dt |> Map.containsKey k

    let tryFind k (DataTable dt) = dt |> Map.tryFind k

    let insert v dt =
        let k = v |> keyOf
        match dt |> containsKey k with
        | false ->
            v
            |> ActiveRow.added
            |> DataRow.activeRow
            |> fun r ->
                let (DataTable dt) = dt
                dt |> Map.add k r |> DataTable
        | true -> failwith "Attempt to insert an item with a duplicate key."

    let update v dt =
        let k = v |> keyOf
        match dt |> tryFind k with
        | None -> failwithf "A row with the key %A could not be found." k
        | Some r ->
            let (DataTable dt) = dt
            let r = r |> DataRow.update v
            dt |> Map.add k r |> DataTable

    let mapCurrent f (DataTable dt) =
        dt
        |> Map.map (fun _ r ->
            match r with
            | ActiveRow a -> a |> ActiveRow.map f |> ActiveRow
            | DeletedRow _ -> r)
        |> DataTable

    let upsert v dt =
        let key = v |> keyOf
        match dt |> containsKey key with
        | false -> dt |> insert v
        | true -> dt |> update v

    let delete k dt =
        match dt |> tryFind k with
        | None -> failwithf "A row with this key could not be found: %A" k
        | Some r ->
            let (DataTable dt) = dt
            match r |> DataRow.delete with
            | None -> dt |> Map.remove k |> DataTable
            | Some r -> dt |> Map.add k r |> DataTable

    let deleteIf p (DataTable dt) =
        let choose r =
            match r with
            | ActiveRow a ->
                match a |> ActiveRow.value |> p with
                | true -> a |> ActiveRow.delete |> Option.map DeletedRow
                | false -> r |> Some
            | DeletedRow _ -> r |> Some

        dt
        |> Map.toSeq
        |> Seq.choose (fun (k, v) -> v |> choose |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> DataTable

    let current (DataTable dt) =
        dt |> Map.values |> Seq.choose DataRow.current

    let acceptChanges (DataTable dt) =
        dt
        |> Map.toSeq
        |> Seq.choose (fun (k, v) ->
            v
            |> DataRow.acceptChanges
            |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> DataTable

    let rejectChanges (DataTable dt) =
        dt
        |> Map.toSeq
        |> Seq.choose (fun (k, v) ->
            v
            |> DataRow.rejectChanges
            |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> DataTable
