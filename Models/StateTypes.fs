module Models.StateTypes

open System
open SynchronizationTypes

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
      PostponedUntil: DateTimeOffset option }

type Schedule =
    | Completed
    | Once
    | Repeat of Repeat

[<Struct>]
type CategoryId = CategoryId of Guid

[<Struct>]
type CategoryName = CategoryName of string

[<Struct>]
type StoreId = StoreId of Guid

[<Struct>]
type StoreName = StoreName of string

type Item =
    { ItemId: ItemId
      Name: ItemName
      Note: Note option
      Quantity: Quantity option
      CategoryId: CategoryId option
      Schedule: Schedule }
    interface IKey<ItemId> with
        member this.Key = this.ItemId

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

type NotSold =
    { StoreId: StoreId
      ItemId: ItemId }
    interface IKey<NotSold> with
        member this.Key = this

type State =
    { Items: DataTable<ItemId, Item>
      Categories: DataTable<CategoryId, Category>
      Stores: DataTable<StoreId, Store>
      NotSoldItems: DataTable<NotSold, NotSold> }

type StateMessage = DeleteCategory of CategoryId
