module Models.DomainTypes

open System

[<Measure>]
type days

type ItemId = ItemId of Guid

type ItemName = ItemName of string

type Note = Note of string

type Quantity = Quantity of string

type Repeat =
    { Interval: int<days>
      PostponedUntil: DateTime option }

type Schedule =
    | Completed
    | Once
    | Repeat of Repeat

type CategoryId = CategoryId of Guid

type IKey<'TKey> =
    abstract Key: 'TKey

type Item =
    { ItemId: ItemId
      Name: ItemName
      Note: Note option
      Quantity: Quantity option
      CategoryId: CategoryId option
      Schedule: Schedule }
    interface IKey<ItemId> with
        member this.Key = this.ItemId

type CategoryName = CategoryName of string

type StoreId = StoreId of Guid

type StoreName = StoreName of string

type Store =
    { StoreId: StoreId
      Name: StoreName }
    interface IKey<StoreId> with
        member this.Key = this.StoreId

type Category =
    { CategoryId: CategoryId
      Name: CategoryName }
    interface IKey<CategoryId> with
        member this.Key = this.CategoryId

type NeverSell = { StoreId: StoreId; ItemId: ItemId }

type Modified<'T> = { Original: 'T; Current: 'T }

type DataRow<'T> =
    | Unchanged of 'T
    | Deleted of 'T
    | Modified of Modified<'T>
    | Added of 'T

type DataTable<'Key, 'T when 'Key: comparison> = DataTable of Map<'Key, DataRow<'T>>

type State =
    { Categories: DataTable<CategoryId, Category>
      Stores: DataTable<StoreId, Store>
      Items: DataTable<ItemId, Item>
      NeverSells: DataTable<NeverSell, NeverSell> }

