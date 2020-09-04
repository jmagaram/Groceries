module Models.DomainTypes

open System

[<Measure>] type days

type ItemId = ItemId of Guid

type ItemName = ItemName of string

type Note = Note of string

type Quantity = Quantity of string

type Repeat =
    { Interval : int<days> 
      PostponedUntil : DateTime }

type Schedule =
    | Completed
    | Once
    | Repeat of Repeat

type CategoryId = CategoryId of Guid

type IKey<'TKey> =
    abstract member Key: 'TKey 

type Item = 
    { ItemId : ItemId
      Name : ItemName
      Note : Note option
      Quantity : Quantity option
      CategoryId : CategoryId option
      Schedule : Schedule }
    interface IKey<ItemId> with member this.Key = this.ItemId

type CategoryName = CategoryName of string

type StoreId = StoreId of Guid

type StoreName = StoreName of string

type Store = 
    { StoreId : StoreId 
      Name : StoreName }
    interface IKey<StoreId> with member this.Key = this.StoreId

type Category =
    { CategoryId : CategoryId 
      Name : CategoryName }
    interface IKey<CategoryId> with member this.Key = this.CategoryId

type StoreNeverStocksItem =
    { StoreId : StoreId 
      ItemId : ItemId }

type Changed<'T> =
    { Original : 'T
      Current : 'T }

type Modified<'T> =
    { Original : 'T 
      Current : 'T }

type DataRow<'T> =
    | Unchanged of 'T
    | Deleted of 'T
    | Modified of Modified<'T>
    | Added of 'T

type DataTable<'T, 'TKey> when 'TKey : comparison = DataTable of Map<'TKey, DataRow<'T>>

type State =
    { Categories : DataTable<CategoryId, Category> 
      Stores : DataTable<Store, StoreId>
      Items : DataTable<Item, ItemId>
      AntiInventory : DataTable<StoreNeverStocksItem, StoreNeverStocksItem> }
