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

let private itemCategory categoryId cats =
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
          |> Seq.choose (fun ns -> if ns.ItemId = item.ItemId then Some ns.StoreId else None)
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
          |> Seq.choose (fun ns -> if ns.ItemId = item.ItemId then Some ns.StoreId else None)
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
    let item = items |> DataTable.findCurrent itemId

    { StoreItem.ItemId = itemId
      ItemName = item.ItemName
      Note = item.Note
      Quantity = item.Quantity
      Category =
          item.CategoryId
          |> Option.map (fun catId -> itemCategory catId cats)
      Schedule = item.Schedule }

let private storeQry (store: Store) items cats notSold =

    { StoreQry.StoreId = store.StoreId
      StoreName = store.StoreName
      NotSoldItems =
          notSold
          |> DataTable.current
          |> Seq.choose (fun ns -> if ns.StoreId = store.StoreId then Some ns.ItemId else None)
          |> Seq.map (fun itemId -> storeItem itemId items cats)
          |> List.ofSeq }

let itemsFromState (s: State) =
    s
    |> State.items
    |> DataTable.current
    |> Seq.map (fun i -> itemQry i s.Categories s.NotSoldItems s.Stores)
    |> List.ofSeq

// distinctUntilChanged will do a value equality of lists and records this is
// slower than doing an object.equals comparison because if the object pointers
// are different there will be still be a field by field comparison.
//
// previously tried creating separate observables for the items, categories,
// etc. and doing a combine latest. was trying to only recalculate when
// something changed. but one of these fires before the rest. so if table a has
// a foreign key to table b and a row in b is removed, b might fire before a,
// causing errors when trying to find the source. need to trigger on foreign
// keys first.
let items (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map itemsFromState
    |> Observable.distinctUntilChanged

let private categoriesFromState (s: State) =
    s
    |> State.categories
    |> DataTable.current
    |> Seq.map (fun cat -> categoryQry cat s.Items s.NotSoldItems s.Stores)
    |> List.ofSeq

let categories (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map categoriesFromState
    |> Observable.distinctUntilChanged

let private storesFromState (s: State) =
    s
    |> State.stores
    |> DataTable.current
    |> Seq.map (fun store -> storeQry store s.Items s.Categories s.NotSoldItems)
    |> List.ofSeq

let stores (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map storesFromState
    |> Observable.distinctUntilChanged

let private shoppingListViewOptionsFromState (s: State) =
    s.ShoppingListViewOptions
    |> DataRow.currentValue
    |> Option.defaultValue (ShoppingListViewOptions.defaultView)

let shoppingListViewOptions (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map shoppingListViewOptionsFromState
    |> Observable.distinctUntilChanged

// other useful views like...
// items not sold at any store
// items available at a particular store with filter (shopping list)
