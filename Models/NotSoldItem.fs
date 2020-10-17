[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.NotSoldItem

open StateTypes

let private separator = '|'

let serialize (ns: NotSoldItem) =
    let storeId = ns.StoreId |> StoreId.serialize

    let itemId = ns.ItemId |> ItemId.serialize

    sprintf "%s%c%s" storeId separator itemId

let deserialize (s: string) =
    result {
        if s |> String.isNullOrWhiteSpace then
            return!
                "Could not deserialize an empty or null string to a NotSoldItem"
                |> Error
        else
            let parts = s.Split(separator)

            match parts.Length with
            | 2 ->
                let! storeId =
                    parts.[0]
                    |> StoreId.deserialize
                    |> Option.map Ok
                    |> Option.defaultValue
                        (sprintf "Could not deserialize the store ID in a NotSoldItem: %s" s
                         |> Error)

                let! itemId =
                    parts.[1]
                    |> ItemId.deserialize
                    |> Option.map Ok
                    |> Option.defaultValue
                        (sprintf "Could not deserialize the item ID in a NotSoldItem: %s" s
                         |> Error)

                return
                    { NotSoldItem.StoreId = storeId
                      NotSoldItem.ItemId = itemId }
            | _ ->
                return!
                    s
                    |> sprintf "Attempting to deserialize a NotSoldItem that does not have exactly two parts: %s"
                    |> Error
    }
