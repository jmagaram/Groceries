[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.StateQuery

open System
open Models
open Models.QueryTypes
open Models.StateTypes

let categoriesTable (s: State) = s.Categories
let storesTable (s: State) = s.Stores
let itemsTable (s: State) = s.Items
let notSoldItemsTable (s: State) = s.NotSoldItems
let settingsRow (s: State) = s.ShoppingListSettings

let categories = categoriesTable >> DataTable.current
let stores = storesTable >> DataTable.current
let items = itemsTable >> DataTable.current
let notSoldItems = notSoldItemsTable >> DataTable.current

let hasChanges s =
    (s |> itemsTable |> DataTable.hasChanges)
    || (s |> categoriesTable |> DataTable.hasChanges)
    || (s |> storesTable |> DataTable.hasChanges)
    || (s |> notSoldItemsTable |> DataTable.hasChanges)

let settings =
    settingsRow
    >> DataRow.currentValue
    >> Option.defaultValue ShoppingListSettings.create

let isItemSold (s: Store) (i: Item) state =
    state
    |> notSoldItemsTable
    |> DataTable.tryFindCurrent { ItemId = i.ItemId; StoreId = s.StoreId }
    |> Option.isNone

let itemQry (item: Item) state =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Etag = item.Etag
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun c -> state |> categoriesTable |> DataTable.findCurrent c)
      Availability =
          state
          |> stores
          |> Seq.map (fun s ->
              { ItemAvailability.Store = s
                IsSold = isItemSold s item state }) }

let itemQryFromGuid (itemId: Guid) state =
    let item = state |> itemsTable |> DataTable.findCurrent (ItemId itemId)

    itemQry item state

let categoryQry (cat: Category option) state =
    { Category = cat
      Items =
          state
          |> items
          |> Seq.filter (fun i ->
              Option.map2 (fun (c:Category) ic -> c.CategoryId = ic) cat i.CategoryId
              |> Option.defaultValue true)
          |> Seq.map (fun i ->
              { CategoryItem.ItemId = i.ItemId
                ItemName = i.ItemName
                Note = i.Note
                Quantity = i.Quantity
                Schedule = i.Schedule
                Availability =
                    state
                    |> stores
                    |> Seq.map (fun s ->
                        { ItemAvailability.Store = s
                          IsSold = isItemSold s i state }) }) }
