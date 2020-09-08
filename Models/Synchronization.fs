namespace Models

open DomainTypes

module DataRow =

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

    let deleted v = Deleted v

    let delete r =
        match r with
        | Unchanged v -> deleted v |> Some
        | Modified m -> deleted m.Original |> Some
        | Added _ -> None
        | Deleted _ -> failwith "Deleted rows can not be deleted again."

    let update v' r =
        match r with
        | Unchanged v -> modified v v'
        | Modified m -> modified m.Original v'
        | Added _ -> added v'
        | Deleted _ -> failwith "Deleted rows can not be updated."

    let map f r =
        match r with
        | Unchanged v -> modified v (f v)
        | Modified m -> modified m.Original (f m.Current)
        | Added v -> added (f v)
        | Deleted _ -> failwith "Deleted rows can not be updated."

    let currentValue r =
        match r with
        | Unchanged v -> v |> Some
        | Modified m -> m.Current |> Some
        | Added v -> v |> Some
        | Deleted _ -> None

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

    let private asMap dt =
        match dt with
        | DataTable dt -> dt

    let private fromMap dt = DataTable dt

    let empty<'Key, 'T when 'Key: comparison> =
        Map.empty<'Key, DataRow<'T>> |> DataTable

    let containsKey k dt = dt |> asMap |> Map.containsKey k

    let insert v dt =
        let k = v |> keyOf
        match dt |> containsKey k with
        | true -> failwith "A row with that key already exists."
        | false ->
            dt
            |> asMap
            |> Map.add k (DataRow.added v)
            |> fromMap

    let update v dt =
        let k = v |> keyOf
        let dt = dt |> asMap
        match dt |> Map.tryFind k with
        | None -> failwith "A row with the same key does not exist and thus could not be updated."
        | Some r ->
            let r = r |> DataRow.update v
            dt |> Map.add k r |> fromMap

    let upsert v dt =
        let k = v |> keyOf
        let dt = dt |> asMap

        let r =
            match dt |> Map.tryFind k with
            | None -> DataRow.added v
            | Some r -> r |> DataRow.update v

        dt |> Map.add k r |> fromMap

    let delete k dt =
        let dt = dt |> asMap
        match dt |> Map.tryFind k with
        | None -> failwith "A row with that key does not exist and thus no row could be deleted."
        | Some r ->
            match r |> DataRow.delete with
            | Some r -> dt |> Map.add k r
            | None -> dt |> Map.remove k
            |> fromMap

    let current dt =
        dt
        |> asMap
        |> Seq.choose (fun kv -> kv.Value |> DataRow.currentValue)

    let private chooseRow f dt =
        dt
        |> asMap
        |> Map.toSeq
        |> Seq.choose (fun (k, v) -> f v |> Option.map (fun v -> (k, v)))
        |> Map.ofSeq
        |> fromMap

    let acceptChanges dt = chooseRow DataRow.acceptChanges dt

    let rejectChanges dt = chooseRow DataRow.rejectChanges dt

    let deleteIf p dt =
        let chooser r =
            match r |> DataRow.currentValue with
            | None -> Some r
            | Some v -> if p v then r |> DataRow.delete else r |> Some

        chooseRow chooser dt
