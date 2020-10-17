module Models.DataRow

open ChangeTrackerTypes

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
    | Deleted _ -> r |> Some

let update v' r =
    match r with
    | Unchanged v -> modified v v'
    | Modified m -> modified m.Original v'
    | Added _ -> added v'
    | Deleted _ -> failwith "Deleted rows can not be updated."

let currentValue r =
    match r with
    | Unchanged v -> v |> Some
    | Modified m -> m.Current |> Some
    | Added v -> v |> Some
    | Deleted _ -> None

let originalValue r =
    match r with
    | Unchanged v -> v
    | Modified m -> m.Original
    | Added v -> v
    | Deleted v -> v

let mapCurrent f r =
    match r with
    | Unchanged v -> modified v (f v)
    | Modified m -> modified m.Original (f m.Current)
    | Added v -> added (f v)
    | Deleted _ -> r

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
