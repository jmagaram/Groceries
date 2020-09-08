module Models.DomainTypes

open System

[<Measure>]
type days

[<Struct>]
type ItemId = ItemId of Guid

[<Struct>]
type ItemName = ItemName of string

[<Struct>]
type Note = Note of string

[<Struct>]
type Quantity = Quantity of string

type Repeat =
    { Interval: int<days>
      PostponedUntil: DateTime option }

type Schedule =
    | Completed
    | Once
    | Repeat of Repeat

[<Struct>]
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

[<Struct>]
type CategoryName = CategoryName of string

[<Struct>]
type StoreId = StoreId of Guid

[<Struct>]
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

type ActiveRow<'T> =
    | Unchanged of 'T
    | Modified of {| Original: 'T; Current: 'T |}
    | Added of 'T

type DataRow<'T> =
    | ActiveRow of ActiveRow<'T>
    | DeletedRow of 'T

type DataTable<'Key, 'T when 'Key: comparison> = DataTable of Map<'Key, DataRow<'T>>

type State =
    { Categories: DataTable<CategoryId, Category>
      Stores: DataTable<StoreId, Store>
      Items: DataTable<ItemId, Item>
      NeverSells: DataTable<NeverSell, NeverSell> }

type StateMessage = DeleteCategory of CategoryId
