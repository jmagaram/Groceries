module Models.Query

open Models
open Models.QueryTypes
open Models.StateTypes

let itemAvailability (s: Store) (i: Item) state =
    if state
       |> State.notSoldItemsTable
       |> DataTable.currentContainsKey { ItemId = i.ItemId; StoreId = s.StoreId } then
        ItemIsAvailable
    else
        ItemIsNotSold

let itemQry (item: Item) state =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun c -> state |> State.categoriesTable |> DataTable.findCurrent c)
      Availability =
          state
          |> State.stores
          |> Seq.map (fun s -> (s, itemAvailability s item state)) }

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
                    |> Seq.map (fun s -> (s, itemAvailability s i state)) }) }

let shoppingListQry s =
    let sf =
        (s |> State.settings).StoreFilter
        |> Option.map (fun storeId -> s |> State.storesTable |> DataTable.findCurrent storeId)

    { Stores = s |> State.stores |> List.ofSeq
      StoreFilter = sf
      Items =
          s
          |> State.items
          |> Seq.map (fun i -> itemQry i s)
          |> Seq.filter (fun i ->
              sf
              |> Option.map (fun sf ->
                  i.Availability
                  |> Seq.exists (fun (s, a) -> s = sf && a = ItemIsAvailable))
              |> Option.defaultValue true)
          |> List.ofSeq }
