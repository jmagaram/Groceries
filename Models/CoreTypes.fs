module Models.CoreTypes

open System

[<Struct>]
type ItemId = ItemId of Guid

[<Struct>]
type CategoryId = CategoryId of Guid

[<Struct>]
type StoreId = StoreId of Guid

[<Struct>]
type UserId = UserId of Guid

[<Struct>]
type ItemName = ItemName of string

type SerializedId = string

[<Struct>]
type Note = Note of string

[<Struct>]
type Quantity = Quantity of string

type Schedule =
    | Completed
    | Once
    | Repeat of Repeat

and Repeat =
    { Frequency: Frequency
      PostponedUntil: DateTimeOffset option }

and Frequency = Frequency of int<days>

[<Struct>]
type CategoryName = CategoryName of string

[<Struct>]
type StoreName = StoreName of string

type Etag = Etag of string

type Item =
    { ItemId: ItemId
      ItemName: ItemName
      Etag: Etag option
      Note: Note option
      Quantity: Quantity option
      CategoryId: CategoryId option
      Schedule: Schedule }
    interface IKey<ItemId> with
        member this.Key = this.ItemId

type Store =
    { StoreId: StoreId
      StoreName: StoreName
      Etag: Etag option }
    interface IKey<StoreId> with
        member this.Key = this.StoreId

type Category =
    { CategoryId: CategoryId
      CategoryName: CategoryName
      Etag: Etag option }
    interface IKey<CategoryId> with
        member this.Key = this.CategoryId

type NotSoldItem =
    { StoreId: StoreId
      ItemId: ItemId }
    interface IKey<NotSoldItem> with
        member this.Key = this

type TextBox =
    { ValueTyping: string
      ValueCommitted: string }

type TextBoxMessage =
    | TypeText of string
    | LoseFocus

type SearchTerm = SearchTerm of string

type ShoppingListSettings =
    { StoreFilter: StoreId option
      PostponedViewHorizon: int<days>
      HideCompletedItems: bool
      TextFilter : TextBox }

type FontSize = 
    | NormalFontSize
    | LargeFontSize

type UserSettings =
    { UserId : UserId
      FontSize : FontSize
      ShoppingListSettings : ShoppingListSettings
    }
    interface IKey<UserId> with
        member this.Key = this.UserId

// If this was a generic add-on to State, maybe could define this and it's
// extension methods in one file (using implicit extensions) later in the
// dependency order.
type ItemForm =
    { ItemId: ItemId option
      ItemName: TextBox
      Etag: Etag option
      Quantity: TextBox
      Note: TextBox
      ScheduleKind: ScheduleKind
      IsComplete : bool
      Frequency: Frequency
      Postpone: int<days> option
      CategoryMode: CategoryMode
      NewCategoryName: TextBox
      CategoryChoice: Category option
      CategoryChoiceList: Category list
      Stores: ItemAvailability list }

and CategoryMode =
    | ChooseExisting
    | CreateNew

and ScheduleKind =
    | Once
    | Completed
    | Repeat

and ItemAvailability = { Store: Store; IsSold: bool }

type ItemDenormalized =
    { ItemId: ItemId
      ItemName: ItemName
      Etag : Etag option
      Note: Note option
      Quantity: Quantity option
      Category: Category option
      Schedule: Schedule
      Availability: ItemAvailability seq }