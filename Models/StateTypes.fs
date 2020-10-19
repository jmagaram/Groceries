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
      LastCosmosTimestamp: int option
      CategoryEditPage: CategoryEditForm option
      StoreEditPage: StoreEditForm option
      ItemEditPage: ItemForm option }

type StateMessage =
    | ItemEditPageMessage of ItemEditPageMessage
    | CategoryEditPageMessage of CategoryEditPageMessage
    | StoreEditPageMessage of StoreEditPageMessage
    | ItemMessage of ItemMessage
    | ShoppingListSettingsMessage of ShoppingListSettings.Message
    | Import of ImportChanges
    | AcceptAllChanges
    | ResetToSampleData

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

and CategoryEditPageMessage =
    | BeginEditCategory of SerializedId
    | BeginCreateNewCategory
    | CategoryEditFormMessage of CategoryEditForm.Message
    | SubmitCategoryEditForm
    | CancelCategoryEditForm
    | DeleteCategory

and StoreEditPageMessage =
    | BeginEditStore of SerializedId
    | BeginCreateNewStore
    | StoreEditFormMessage of StoreEditForm.Message
    | SubmitStoreEditForm
    | CancelStoreEditForm
    | DeleteStore

and ItemMessage =
    | ModifyItem of ItemId * Item.Message
    | DeleteItem of ItemId

type Update = Now -> StateMessage -> State -> State