module Models.Query

open System
open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive

open Models
open Models.QueryTypes
open Models.StateTypes
open Models.SynchronizationTypes

let private itemAvailability (item: Item) state =
    state
    |> State.stores
    |> Seq.map (fun store ->
        let isCategoryNotSold =
            item.CategoryId
            |> Option.map (fun c ->
                state
                |> State.notSoldCategoriesTable
                |> DataTable.currentContainsKey { NotSoldCategory.StoreId = store.StoreId; CategoryId = c })
            |> Option.defaultValue false

        let isItemNotSold =
            state
            |> State.notSoldItemsTable
            |> DataTable.currentContainsKey
                { NotSoldItem.StoreId = store.StoreId
                  ItemId = item.ItemId }

        match isCategoryNotSold, isItemNotSold with
        | true, false -> (store, CategoryIsNotSold)
        | false, true -> (store, ItemIsNotSold)
        | _, _ -> (store, ItemIsAvailable))
    |> List.ofSeq

let private itemQry (item: Item) state =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun c -> state |> State.categoriesTable |> DataTable.findCurrent c)
      StoreAvailability = itemAvailability item state }

let categoryQry (cat: Category option) state =
    { Category = cat
      Items =
          state
          |> State.items
          |> Seq.filter (fun i -> i.CategoryId = (cat |> Option.map (fun x -> x.CategoryId)))
          |> Seq.map (fun i ->
              { CategoryItem.ItemId = i.ItemId
                ItemName = i.ItemName
                Note = i.Note
                Quantity = i.Quantity
                Schedule = i.Schedule
                StoreAvailability = state |> itemAvailability i })
          |> List.ofSeq }

let shoppingListQry s =
    let storeFilter =
        (State.settings s).StoreFilter
        |> Option.map (fun x -> s |> State.storesTable |> DataTable.findCurrent x)

    { Stores = s.Stores |> DataTable.current |> List.ofSeq
      StoreFilter = storeFilter
      Items =
          s
          |> State.items
          |> Seq.map (fun item -> itemQry item s)
          |> Seq.filter (fun item ->
              match storeFilter with
              | None -> true
              | Some filterId ->
                  item.StoreAvailability
                  |> Seq.exists (fun (st, it) ->
                      st.StoreId = filterId.StoreId
                      && it = ItemAvailability.ItemIsAvailable))
          |> List.ofSeq }
