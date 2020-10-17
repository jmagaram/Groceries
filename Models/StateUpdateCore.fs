[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.StateUpdateCore

open System
open System.Collections.Generic
open StateTypes

let mapCategories f s = { s with Categories = f s.Categories }
let mapStores f s = { s with State.Stores = f s.Stores }
let mapItems f s = { s with Items = f s.Items }

let mapNotSoldItems f s = { s with NotSoldItems = f s.NotSoldItems }

let mapSettings f s = { s with ShoppingListSettings = s.ShoppingListSettings |> DataRow.mapCurrent f }

let (insertCategory, updateCategory, upsertCategory) =
    let go f (c: Category) s = s |> mapCategories (f c)
    (go DataTable.insert, go DataTable.update, go DataTable.upsert)

let (insertStore, updateStore, upsertStore) =
    let go f (s: Store) = mapStores (f s)
    (go DataTable.insert, go DataTable.update, go DataTable.upsert)

let (insertItem, updateItem, upsertItem) =
    let go f (i: Item) s =
        let isCategoryReferenceValid =
            i.CategoryId
            |> Option.map (fun c -> s.Categories |> DataTable.tryFindCurrent c |> Option.isSome)
            |> Option.defaultValue true

        match isCategoryReferenceValid with
        | false -> failwith "A category is referenced that does not exist."
        | true -> s |> mapItems (f i)

    (go DataTable.insert, go DataTable.update, go DataTable.upsert)

let insertNotSoldItem (nsi: NotSoldItem) s =
    let item = s.Items |> DataTable.tryFindCurrent nsi.ItemId

    let store = s.Stores |> DataTable.tryFindCurrent nsi.StoreId

    match item, store with
    | Some _, Some _ -> s |> mapNotSoldItems (DataTable.insert nsi)
    | None, _ -> failwith "A store is referenced that does not exist."
    | _, None -> failwith "An item is referenced that does not exist."

let deleteStore k s =
    s
    |> mapStores (DataTable.delete k)
    |> mapNotSoldItems (DataTable.deleteIf (fun i -> i.StoreId = k))
    |> mapSettings (ShoppingListSettings.clearStoreFilterIf k)

let deleteNotSoldItem k s = s |> mapNotSoldItems (DataTable.delete k)

let deleteItem k s = s |> mapItems (DataTable.delete k)

let deleteCategory k s =
    s
    |> mapCategories (DataTable.delete k)
    |> mapItems (fun dt ->
        dt
        |> DataTable.current
        |> Seq.choose (fun i -> if i.CategoryId = Some k then Some { i with CategoryId = None } else None)
        |> Seq.fold (fun dt i -> dt |> DataTable.update i) s.Items)

let private setBrokenItemToCategoryLinksToNone (s: State) =
    s
    |> StateQuery.items
    |> Seq.filter (fun i ->
        match i.CategoryId with
        | None -> false
        | Some c ->
            s
            |> StateQuery.categoriesTable
            |> DataTable.tryFindCurrent c
            |> Option.isNone)
    |> Seq.fold (fun s i ->
        s
        |> mapItems (DataTable.upsert { i with CategoryId = None })) s

let private removeBrokenNotSoldItemLinks (s: State) =
    { s with
          NotSoldItems =
              s
              |> StateQuery.notSoldItemsTable
              |> DataTable.deleteIf (fun ns ->
                  let isBrokenStore =
                      s
                      |> StateQuery.itemsTable
                      |> DataTable.tryFindCurrent ns.ItemId
                      |> Option.isNone

                  let isBrokenItem =
                      s
                      |> StateQuery.storesTable
                      |> DataTable.tryFindCurrent ns.StoreId
                      |> Option.isNone

                  isBrokenStore || isBrokenItem) }

let private removeBrokenFilterLinks (s: State) =
    match (s |> StateQuery.settings).StoreFilter with
    | None -> s
    | Some sf ->
        match s |> StateQuery.storesTable |> DataTable.tryFindCurrent sf with
        | None -> s |> mapSettings (fun s -> { s with StoreFilter = None })
        | Some _ -> s

let fixBrokenForeignKeys (s: State) =
    s
    |> setBrokenItemToCategoryLinksToNone
    |> removeBrokenNotSoldItemLinks
    |> removeBrokenFilterLinks


let importChanges (i: ImportChanges) (s: StateTypes.State) =
    { s with
          Items =
              i.ItemChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Items
          Categories =
              i.CategoryChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Categories
          Stores =
              i.StoreChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Stores
          NotSoldItems =
              i.NotSoldItemChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.NotSoldItems
          LastCosmosTimestamp = i.LatestTimestamp }
    |> fixBrokenForeignKeys

let acceptAllChanges s =
    s
    |> mapItems DataTable.acceptChanges
    |> mapCategories DataTable.acceptChanges
    |> mapStores DataTable.acceptChanges
    |> mapNotSoldItems DataTable.acceptChanges