module DomainTypes
open System

[<Measure>]
type chars

type NowUtc = unit -> DateTime

type PlainText = PlainText of string

type PlainTextCharacter =
    | Letter
    | Mark
    | Number
    | Punctuation
    | Symbol
    | Space
    | LineFeed

type PlainTextRules =
    { MinimumLength : int<chars>
      MaximumLength : int<chars>
      PermittedCharacters : PlainTextCharacter list }

type Repeat = 
    | DoesNotRepeat
    | DailyInterval of int

type NormalizeString = string -> string

type Title = Title of string

type TitleError =
    | TitleIsRequired
    | TitleIsOverMaxLength of int

type Note = Note of string

type NoteError =
    | NoteIsOverMaxLength of int

type Quantity = Quantity of string

type QuantityError =
    | QuantityIsOverMaxLength of int

type ItemId = ItemId of Guid

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

type StoreId = StoreId of Guid

type StoreName = StoreName of string

type PickOne<'T> when 'T : comparison =
    { Choices : Set<'T>
      SelectedItem : 'T }

type PickOneMessage<'T> =
    | PickOneByPredicate of ('T -> bool)
    | PickOneByItem of 'T

type RepeatSelector = PickOne<Repeat>

type PickRepeatMessage = Repeat

type RelativeStatus =
    | Active
    | Complete
    | PostponedDays of int

type RelativeStatusSelector = PickOne<RelativeStatus>

type StatusSelectorMessage = RelativeStatus

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
    | RelativeStatusSelectorMessage of StatusSelectorMessage

type ItemEditorModel =
    { Title : TitleTextBox
      Quantity : QuantityTextBox
      QuantitySpinner : Spinner
      Note : NoteTextBox
      Repeat : RepeatSelector 
      Status : RelativeStatusSelector }

type ItemSummary =
    { Id : ItemId
      Title : string
      Quantity : string 
      Note : string
      Repeat : Repeat
      Status : Status }

type PostponedItemFilter =
    | ExcludePostponedItems
    | IncludeOverdueOnly
    | IncludeOverdueAndFutureItems of TimeSpan
    | AllPostponedItems

type ItemFilter =
    { PostponedItemFilter : PostponedItemFilter }

type ItemListView =
    { Items : ItemSummary seq
      Filter : ItemFilter }

type ItemListViewMessage = 
    | SetFilter of ItemFilter

type State = 
    { Stores : Map<StoreId, StoreName> 
      Items : Map<ItemId, Item> 
      ItemIsUnavailableInStore : Set<StoreId * ItemId> 
      ItemListView : ItemListView }

type StateMessage =
    | InsertItem of ItemEditorModel
    | DeleteItem of ItemId
    | ItemListViewMessage of ItemListViewMessage
