module Models.Query

open System
open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive

open Models
open Models.QueryTypes
open Models.StateTypes

let private itemStore storeId (state: State) =
    state.Stores
    |> DataTable.findCurrent storeId
    |> fun s -> { ItemStore.StoreId = s.StoreId; StoreName = s.StoreName }

let private itemCategory categoryId (state: State) =
    state.Categories
    |> DataTable.findCurrent categoryId
    |> fun cat ->
        { ItemCategory.CategoryId = cat.CategoryId
          CategoryName = cat.CategoryName }

let private itemQry (item: Item) (s: State) =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun cId -> itemCategory cId s)
      NotSoldAt =
          s.NotSoldItems
          |> DataTable.current
          |> Seq.choose (fun ns -> if ns.ItemId = item.ItemId then Some ns.StoreId else None)
          |> Seq.map (fun storeId -> itemStore storeId s)
          |> List.ofSeq }

let private categoryItem (item: Item) (state: State) =
    { CategoryItem.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      NotSoldAt =
          state.NotSoldItems
          |> DataTable.current
          |> Seq.choose (fun ns -> if ns.ItemId = item.ItemId then Some ns.StoreId else None)
          |> Seq.map (fun storeId -> itemStore storeId state)
          |> List.ofSeq }

let private categoryQry (category:Category) (s: State) =

    { CategoryQry.CategoryId = category.CategoryId
      CategoryName = category.CategoryName
      Items =
          s.Items
          |> DataTable.current
          |> Seq.choose (fun i -> if i.CategoryId = Some category.CategoryId then Some i else None)
          |> Seq.map (fun i -> categoryItem i s)
          |> List.ofSeq }

let private storeItem itemId (s: State) =
    let item = s.Items |> DataTable.findCurrent itemId

    { StoreItem.ItemId = itemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Category =
          item.CategoryId
          |> Option.map (fun catId -> itemCategory catId s)
      Schedule = item.Schedule }

let private storeQry (store:Store) (s: State) =

    { StoreQry.StoreId = store.StoreId
      StoreName = store.StoreName
      NotSoldItems =
          s.NotSoldItems
          |> DataTable.current
          |> Seq.choose (fun ns -> if ns.StoreId = store.StoreId then Some ns.ItemId else None)
          |> Seq.map (fun itemId -> storeItem itemId s)
          |> List.ofSeq }

let items (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map (fun s ->
        s.Items
        |> DataTable.current
        |> Seq.map (fun i -> itemQry i s)
        |> List.ofSeq)

let categories (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map (fun s ->
        s.Categories
        |> DataTable.current
        |> Seq.map (fun i -> categoryQry i s)
        |> List.ofSeq)

let stores (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map (fun s ->
        s.Stores
        |> DataTable.current
        |> Seq.map (fun i -> storeQry i s)
        |> List.ofSeq)
