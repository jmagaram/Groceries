module Models.Query

open System
open System.Reactive
open System.Reactive.Linq
open FSharp.Control.Reactive

open Models
open Models.QueryTypes
open Models.StateTypes

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

// distinctUntilChanged will do a value equality of lists and records this is
// slower than doing an object.equals comparison because if the object pointers
// are different there will be still be a field by field comparison. another
// option for speeding it up is giving a datatable a "last changed date" or
// "version" for comparison
let items (s: IObservable<StateTypes.State>) =
    let items = s |> Observable.map (fun s -> s.Items)
    let cats =  s |> Observable.map (fun s -> s.Categories)
    let ns =  s |> Observable.map (fun s -> s.NotSoldItems)
    let stores =  s |> Observable.map (fun s -> s.Stores)
    items
    |> Observable.combineLatest cats
    |> Observable.combineLatest ns
    |> Observable.combineLatest stores
    |> Observable.map (fun (stores, (ns, (cats, items)))-> 
        items
        |> DataTable.current
        |> Seq.map (fun i -> itemQry i cats ns stores)
        |> List.ofSeq)
    |> Observable.distinctUntilChanged

let categories (s: IObservable<StateTypes.State>) =
    let cats = s |> Observable.map (fun s -> s.Categories)
    let items = s |> Observable.map (fun s -> s.Items)
    let notSold = s |> Observable.map (fun s -> s.NotSoldItems)
    let stores = s |> Observable.map (fun s -> s.Stores)

    cats
    |> Observable.combineLatest items
    |> Observable.combineLatest notSold
    |> Observable.combineLatest stores
    |> Observable.map (fun (stores, (notSold, (items, cats))) ->
        cats
        |> DataTable.current
        |> Seq.map (fun cat -> categoryQry cat items notSold stores)
        |> Seq.toList)
    |> Observable.distinctUntilChanged

let stores (s: IObservable<StateTypes.State>) =
    let cats = s |> Observable.map (fun s -> s.Categories)
    let items = s |> Observable.map (fun s -> s.Items)
    let notSold = s |> Observable.map (fun s -> s.NotSoldItems)
    let stores = s |> Observable.map (fun s -> s.Stores)
    cats
    |> Observable.combineLatest items
    |> Observable.combineLatest notSold
    |> Observable.combineLatest stores
    |> Observable.map (fun (stores, (notSold, (items, cats))) ->
        stores
        |> DataTable.current
        |> Seq.map (fun store -> storeQry store items cats notSold)
        |> Seq.toList)
    |> Observable.distinctUntilChanged

let shoppingListViewOptions (s: IObservable<StateTypes.State>) =
    s
    |> Observable.map (fun i -> i.ShoppingListViewOptions)
    |> Observable.map DataRow.currentValue
    |> Observable.map (fun i ->
        i
        |> Option.defaultValue (ShoppingListViewOptions.defaultView))
    |> Observable.distinctUntilChanged

// other useful views like...
// items not sold at any store
// items available at a particular store with filter (shopping list)
