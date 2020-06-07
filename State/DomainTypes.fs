module DomainTypes
open System

type Repeat = 
    | DoesNotRepeat
    | DailyInterval of int

type Title = | Title of string

type Note = | Note of string

type Quantity = | Quantity of string

type ItemId = | ItemId of Guid

type Status =
    | Active
    | Complete
    | Postponed of DateTime

type Item = 
    { Id : ItemId
      Title : Title
      Note : Note option
      Quantity : Quantity option
      Repeat : Repeat
      Status : Status }

type StoreId = | StoreId of Guid

type StoreName = | StoreName of string

type State = 
    { Stores : Map<StoreId, StoreName> 
      Items : Map<ItemId, Item> 
      ItemIsUnavailableInStore : Set<StoreId * ItemId> }

type PickOne<'T> when 'T : comparison =
    { Choices : Set<'T>
      SelectedItem : 'T }

type PickOneMessage<'T> =
    | PickOneByPredicate of ('T -> bool)
    | PickOneByItem of 'T

type PickRepeat = PickOne<Repeat>

type PickRepeatMessage = Repeat

type Selector<'T, 'Error> when 'T : comparison =
    { Choices : Set<'T>
      SelectedItem : 'T option 
      Error : 'Error option }

type TextBox<'Error> =
    { Text : string
      NormalizedText : string
      Error : 'Error option 
      HasFocus : bool }

type TitleTextBox = TextBox<string>

type QuantityTextBox = TextBox<string>

type NoteTextBox = TextBox<string>

type TextBoxMessage = 
    | SetText of string
    | LoseFocus
    | GetFocus

type StatusKind =
    | Active
    | Complete
    | Postponed

type TimespanComponents =
    { Days : int
      Weeks : int
      Months : int }

type RelativeStatusChoice =
    { Kind : StatusKind 
      Description : string }

type PostponeDaysChoice = int

type RepeatError = 
    | RepeatIntervalIsTooSmall
    | RepeatIntervalIsTooBig
    | RepeatIntervalIsRequired

type DailyInterval = int

type Spinner = 
    { CanIncrease : bool
      CanDecrease : bool }

type SpinnerMessage = 
    | Increase
    | Decrease

type ItemEditorMessage =
    | TitleMessage of TextBoxMessage
    | QuantityMessage of TextBoxMessage
    | NoteMessage of TextBoxMessage
    | RepeatMessage of PickRepeatMessage
    | QuantitySpinner of SpinnerMessage

type ItemEditorModel =
    { Title : TitleTextBox
      Quantity : QuantityTextBox
      QuantitySpinner : Spinner
      Note : NoteTextBox
      Repeat : PickRepeat }