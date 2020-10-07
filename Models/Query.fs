module Models.Query

open System
open Models
open Models.QueryTypes
open Models.StateTypes

let isItemSold (s: Store) (i: Item) state =
    state
    |> State.notSoldItemsTable
    |> DataTable.tryFindCurrent
        { ItemId = i.ItemId
          StoreId = s.StoreId }
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
          |> Option.map (fun c ->
              state
              |> State.categoriesTable
              |> DataTable.findCurrent c)
      Availability =
          state
          |> State.stores
          |> Seq.map (fun s ->
              { ItemAvailability.Store = s
                IsSold = isItemSold s item state }) }

let itemQryFromGuid (itemId: Guid) state =
    let item =
        state
        |> State.itemsTable
        |> DataTable.findCurrent (ItemId itemId)

    itemQry item state

let categoryQry (cat: Category option) state =
    { Category = cat
      Items =
          state
          |> State.items
          |> Seq.filter (fun i ->
              Option.map2 (fun c ic -> c.CategoryId = ic) cat i.CategoryId
              |> Option.defaultValue true)
          |> Seq.map (fun i ->
              { CategoryItem.ItemId = i.ItemId
                ItemName = i.ItemName
                Note = i.Note
                Quantity = i.Quantity
                Schedule = i.Schedule
                Availability =
                    state
                    |> State.stores
                    |> Seq.map (fun s ->
                        { ItemAvailability.Store = s
                          IsSold = isItemSold s i state }) }) }
