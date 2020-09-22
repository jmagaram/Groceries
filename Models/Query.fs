module Models.Query

open System
open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive

open Models
open Models.QueryTypes
open Models.StateTypes
open Models.SynchronizationTypes

let private itemAvailability (item: Item) (stores: StoresTable) notSoldItems notSoldCategories =
    stores
    |> DataTable.current
    |> Seq.map (fun store ->
        let isCategoryNeverStocked =
            item.CategoryId
            |> Option.map (fun c ->
                notSoldCategories
                |> DataTable.currentContainsKey { NotSoldCategory.StoreId = store.StoreId; CategoryId = c })
            |> Option.defaultValue false

        let isItemNeverStocked =
            notSoldItems
            |> DataTable.currentContainsKey
                { NotSoldItem.StoreId = store.StoreId
                  ItemId = item.ItemId }

        match isCategoryNeverStocked, isItemNeverStocked with
        | true, false -> (store, CategoryIsNeverStocked)
        | false, true -> (store, ItemIsNeverStocked)
        | _, _ -> (store, ItemIsAvailable))
    |> List.ofSeq

let private itemQry (item: Item) cats notSoldItems notSoldCategories (stores: StoresTable) =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun cId -> cats |> DataTable.findCurrent cId)
      Availability = itemAvailability item stores notSoldItems notSoldCategories }

let categoryQry (cat: Category option) (items: ItemsTable) notSoldItems notSoldCategories (stores: StoresTable) =
    { Category = cat
      Items =
          items
          |> DataTable.current
          |> Seq.filter (fun i -> i.CategoryId = (cat |> Option.map (fun x -> x.CategoryId)))
          |> Seq.map (fun i ->
              { CategoryItem.ItemId = i.ItemId
                ItemName = i.ItemName
                Note = i.Note
                Quantity = i.Quantity
                Schedule = i.Schedule
                Availability = itemAvailability i stores notSoldItems notSoldCategories })
          |> List.ofSeq }

// Get all the current data from State. methods?
// Avoids need for all the explicit typing too
// Potential for caching too

let shoppingListQry (s: State) =
    let storeFilter =
        s.Settings
        |> DataRow.currentValue
        |> Option.defaultValue (Settings.create)
        |> fun i ->
            i.StoreFilter
            |> Option.map (fun x -> s.Stores |> DataTable.findCurrent x)

    { Stores = s.Stores |> DataTable.current |> List.ofSeq
      StoreFilter = storeFilter
      Items =
          s.Items
          |> DataTable.current
          |> Seq.map (fun item -> itemQry item s.Categories s.NotSoldItems s.NotSoldCategories s.Stores)
          |> Seq.filter (fun item ->
              match storeFilter with
              | None -> true
              | Some filterId ->
                  item.Availability
                  |> Seq.exists (fun (st, it) ->
                      st.StoreId = filterId.StoreId
                      && it = StoreAvailability.ItemIsAvailable))
          |> List.ofSeq }
