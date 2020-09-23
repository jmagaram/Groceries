module Models.Query

open System

open Models
open Models.QueryTypes
open Models.StateTypes
open Models.SynchronizationTypes

let itemStoreAvailability (stores: Store seq) (items: Item seq) state =
    Seq.allPairs stores items
    |> Seq.map (fun (s, i) ->
        let availability =
            let categoryIsNotSold =
                i.CategoryId
                |> Option.map (fun c ->
                    state
                    |> State.notSoldCategoriesTable
                    |> DataTable.tryFindCurrent { CategoryId = c; StoreId = s.StoreId }
                    |> Option.isSome)
                |> Option.defaultValue false

            let itemIsNotSold =
                state
                |> State.notSoldItemsTable
                |> DataTable.currentContainsKey { ItemId = i.ItemId; StoreId = s.StoreId }

            match categoryIsNotSold, itemIsNotSold with
            | false, false -> ItemIsAvailable
            | true, false -> CategoryIsNotSold
            | false, true -> ItemIsNotSold
            | true, true -> CategoryAndItemAreNotSold

        (i, s, availability))

let private itemQry (item: Item) state =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun c -> state |> State.categoriesTable |> DataTable.findCurrent c)
      StoreAvailability =
          itemStoreAvailability (state |> State.stores) (item |> Seq.singleton) state
          |> Seq.map (fun (_, s, a) -> (s, a))
          |> List.ofSeq } // Seq.cache? Just a Seq?

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
                StoreAvailability =
                    itemStoreAvailability (state |> State.stores) (i |> Seq.singleton) state
                    |> Seq.map (fun (_, s, a) -> (s, a))
                    |> List.ofSeq })
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
