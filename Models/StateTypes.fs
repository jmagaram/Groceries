module Models.StateTypes

open System
open ChangeTrackerTypes
open CoreTypes

type State =
    { Items: DataTable<ItemId, Item>
      Categories: DataTable<CategoryId, Category>
      Stores: DataTable<StoreId, Store>
      NotSoldItems: DataTable<NotSoldItem, NotSoldItem>
      ShoppingListSettings: DataRow<ShoppingListSettings>
      GlobalSettings : DataRow<GlobalSettings>
      LastCosmosTimestamp: int option
      ItemEditPage: ItemForm option }

type ReorganizeCategoriesMessage =
    { Delete : string list
      Create : string list
      Move : (string * string) list }

type ReorganizeStoresMessage =
    { Delete : string list
      Create : string list
      Move : (string * string) list }

type StateMessage =
    | ItemEditPageMessage of ItemEditPageMessage
    | ReorganizeCategoriesMessage of ReorganizeCategoriesMessage
    | ReorganizeStoresMessage of ReorganizeStoresMessage
    | ItemMessage of ItemMessage
    | ShoppingListSettingsMessage of ShoppingListSettings.Message
    | GlobalSettingsMessage of GlobalSettings.Message
    | Import of ImportChanges
    | AcceptAllChanges
    | ResetToSampleData
    | Transaction of StateMessage seq

and ImportChanges =
    { ItemChanges: Change<Item, ItemId> list
      CategoryChanges: Change<Category, CategoryId> list
      StoreChanges: Change<Store, StoreId> list
      NotSoldItemChanges: Change<NotSoldItem, NotSoldItem> list
      LatestTimestamp: int option }

and ItemEditPageMessage =
    | BeginEditItem of SerializedId
    | BeginCreateNewItem
    | BeginCreateNewItemWithName of string
    | ItemEditFormMessage of ItemForm.Message
    | SubmitItemEditForm
    | CancelItemEditForm
    | DeleteItem

and ItemMessage =
    | ModifyItem of ItemId * Item.Message
    | DeleteItem of ItemId

type Update = Clock -> StateMessage -> State -> State