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
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      CategoryId: CategoryId option
      Schedule: Schedule }
    interface IKey<ItemId> with
        member this.Key = this.ItemId

type Store =
    { StoreId: StoreId
      StoreName: StoreName }
    interface IKey<StoreId> with
        member this.Key = this.StoreId

type Category =
    { CategoryId: CategoryId
      CategoryName: CategoryName }
    interface IKey<CategoryId> with
        member this.Key = this.CategoryId

type NotSoldItem =
    { StoreId: StoreId
      ItemId: ItemId }
    interface IKey<NotSoldItem> with
        member this.Key = this

// include "show empty categories"
type ShoppingListViewOptions = 
    { StoreFilter: StoreId option }
    interface IKey<string> with
        member this.Key = "singleton"

type ItemTable = DataTable<ItemId, Item>
type StoreTable = DataTable<StoreId, Store>
type CategoryTable = DataTable<CategoryId, Category>
type NotSoldItemsTable = DataTable<NotSoldItem, NotSoldItem>
type ShoppingListViewOptionsRow = DataRow<ShoppingListViewOptions>

type State =
    { Items: ItemTable
      Categories: CategoryTable
      Stores: StoreTable
      NotSoldItems: NotSoldItemsTable
      ShoppingListViewOptions: ShoppingListViewOptionsRow }

type CategoryReference =
    | ExistingCategory of CategoryId
    | NewCategory of Category

type StoreReference =
    | ExistingStore of StoreId
    | NewStore of Store

type ItemUpsert =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Category: CategoryReference option
      Schedule: Schedule 
      NotSoldAt : StoreReference list }

type ItemMessage =
    | InsertItem of ItemUpsert
    | UpdateItem of ItemUpsert
    | DeleteItem of ItemId

type CategoryMessage = 
    | InsertCategory of Category
    | DeleteCategory of CategoryId

type StoreMessage = 
    | DeleteStore of StoreId

type ShoppingListMessage =
    | ClearStoreFilter
    | SetStoreFilterTo of StoreId

type StateMessage =
    | ItemMessage of ItemMessage
    | StoreMessage of StoreMessage
    | CategoryMessage of CategoryMessage
    | ShoppingListMessage of ShoppingListMessage

