module Models.StateTypes

open System
open ChangeTrackerTypes
open CoreTypes

type State =
    { Items: DataTable<ItemId, Item>
      Categories: DataTable<CategoryId, Category>
      Stores: DataTable<StoreId, Store>
      NotSoldItems: DataTable<NotSoldItem, NotSoldItem>
      UserSettings : DataTable<UserId, UserSettings>
      Purchases : DataTable<Purchase, Purchase>
      LoggedInUser : UserId
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
    | RecordPurchase of ItemId
    | ReorganizeCategoriesMessage of ReorganizeCategoriesMessage
    | ReorganizeStoresMessage of ReorganizeStoresMessage
    | ItemMessage of ItemMessage
    | ItemAvailabilityMessage of ItemId * ItemAvailability seq
    | ItemOnlySoldAt of ItemId * StoreId seq
    | ItemNotSoldAt of ItemId * StoreId
    | UserSettingsMessage of UserSettings.Message
    | Import of ImportChanges
    | AcceptAllChanges
    | ResetToSampleData
    | Transaction of StateMessage seq

and ImportChanges =
    { ItemChanges: Change<Item, ItemId> list
      CategoryChanges: Change<Category, CategoryId> list
      StoreChanges: Change<Store, StoreId> list
      NotSoldItemChanges: Change<NotSoldItem, NotSoldItem> list
      PurchaseChanges : Change<Purchase,Purchase> list
      LatestTimestamp: int option 
      EarliestTimestamp : int option
    }

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