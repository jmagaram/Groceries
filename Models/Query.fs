module Models.Query

open System
open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive

open Models
open Models.QueryTypes
open Models.StateTypes
open Models.SynchronizationTypes

let private itemStore storeId stores =
    stores
    |> DataTable.findCurrent storeId
    |> fun (s: Store) -> { ItemStore.StoreId = s.StoreId; StoreName = s.StoreName }

let private itemCategory categoryId (cats:CategoryTable) =
    cats
    |> DataTable.findCurrent categoryId
    |> fun cat ->
        { ItemCategory.CategoryId = cat.CategoryId
          CategoryName = cat.CategoryName }

let private itemQry (item: Item) cats notSoldItems stores =
    { ItemQry.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      Category =
          item.CategoryId
          |> Option.map (fun cId -> itemCategory cId cats)
      NotSoldAt =
          notSoldItems
          |> DataTable.current
          |> Seq.choose (fun (ns:NotSoldItem) -> if ns.ItemId = item.ItemId then Some ns.StoreId else None)
          |> Seq.map (fun storeId -> itemStore storeId stores)
          |> List.ofSeq }

let private categoryItem (item: Item) notSoldItems stores =
    { CategoryItem.ItemId = item.ItemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Schedule = item.Schedule
      NotSoldAt =
          notSoldItems
          |> DataTable.current
          |> Seq.choose (fun (ns:NotSoldItem) -> if ns.ItemId = item.ItemId then Some ns.StoreId else None)
          |> Seq.map (fun storeId -> itemStore storeId stores)
          |> List.ofSeq }

let private categoryQry (category: Category) items notSold stores =
    { CategoryQry.CategoryId = category.CategoryId
      CategoryName = category.CategoryName
      Items =
          items
          |> DataTable.current
          |> Seq.choose (fun (i: Item) -> if i.CategoryId = Some category.CategoryId then Some i else None)
          |> Seq.map (fun i -> categoryItem i notSold stores)
          |> List.ofSeq }

let private storeItem itemId items cats =
    let (item:Item) = items |> DataTable.findCurrent itemId

    { StoreItem.ItemId = itemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Category =
          item.CategoryId
          |> Option.map (fun catId -> itemCategory catId cats)
      Schedule = item.Schedule }

let private storeQry (store: Store) items cats (notSold:NotSoldItemsTable) =
    { StoreQry.StoreId = store.StoreId
      StoreName = store.StoreName
      NotSoldItems =
          notSold
          |> DataTable.current
          |> Seq.choose (fun ns -> if ns.StoreId = store.StoreId then Some ns.ItemId else None)
          |> Seq.map (fun itemId -> storeItem itemId items cats)
          |> List.ofSeq }

let private items categories items notSold stores =
    items
    |> DataTable.current
    |> Seq.map (fun i -> itemQry i categories notSold stores)

let private categories categories items notSold stores =
    categories
    |> DataTable.current
    |> Seq.map (fun cat -> categoryQry cat items notSold stores)

let private stores stores items categories notSold =
    stores
    |> DataTable.current
    |> Seq.map (fun store -> storeQry store items categories notSold)

let private shoppingListViewOptions viewOptionRow =
    viewOptionRow
    |> DataRow.currentValue
    |> Option.defaultValue (ShoppingListViewOptions.defaultView)

let private isSoldAt store (itemQry: ItemQry) =
    store
    |> Option.map (fun store ->
        itemQry.NotSoldAt
        |> Seq.forall (fun ns -> ns.StoreId <> store))
    |> Option.defaultValue true

let shoppingListQry (s: State) =
    let viewOpt = s.ShoppingListViewOptions |> shoppingListViewOptions

    { Stores = s.Stores |> DataTable.current |> List.ofSeq
      Items =
          s
          |> State.items
          |> DataTable.current
          |> Seq.map (fun item -> itemQry item s.Categories s.NotSoldItems s.Stores)
          |> Seq.filter (fun item -> item |> isSoldAt viewOpt.StoreFilter)
          |> List.ofSeq
      ShoppingListViewOptions = viewOpt }
