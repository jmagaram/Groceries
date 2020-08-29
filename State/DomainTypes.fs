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

type TextBoxOld<'Error> =
    { Text : string
      NormalizedText : string
      Error : 'Error option 
      HasFocus : bool }

type TextBox<'Error> =
    { Text : string
      Error : 'Error option }

type TitleTextBox = TextBoxOld<string>

type QuantityTextBox = TextBoxOld<string>

type NoteTextBox = TextBoxOld<string>

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

type FormField<'Proposed, 'Validated, 'Error> =
    { Proposed : 'Proposed
      Normalized : 'Proposed
      ValidationResult : Result<'Validated, 'Error>
      InitialValue : 'Proposed }

type FormFieldMessage<'T> =
    | LostFocus
    | GainedFocus
    | Propose of 'T

type TextField<'Value, 'Error> =
    { BindToTextBox : string
      NormalizedText : string
      ValidationResult : Result<'Value, 'Error> }

type ChooseOne<'T> when 'T : comparison =
    { Choices : 'T list 
      SelectedItem : 'T 
      Serialize : 'T -> string
      Deserialize : string -> 'T }

type ChooseOneItem<'T> = 
    { Value : 'T 
      IsSelected : bool
      Key : string }

type Never = Never of unit

type ItemEditModel = 
    { Title : FormField<string, Title, TitleError>
      Quantity : FormField<string, Quantity, QuantityError>
      QuantityBigger : Quantity option
      QuantitySmaller : Quantity option 
      Note : FormField<string, Note, NoteError>
      Repeat : FormField<Repeat, Repeat, Never> 
      RelativeStatus : FormField<RelativeStatus, RelativeStatus, Never>
      CanSubmit : bool
      CanCancel : bool
      CanDelete : bool }

type ItemEditCommand = 
    | Submit
    | Delete
    | Cancel
    | QuantityIncrease
    | QuantityDecrease

type ItemEditMessage = 
    | TitleMessage of FormFieldMessage<string>
    | QuantityMessage of FormFieldMessage<string>
    | NoteMessage of FormFieldMessage<string>
    | RepeatMessage of FormFieldMessage<Repeat>
    | SetRelativeStatus of FormFieldMessage<RelativeStatus>
    | InvokeCommand of ItemEditCommand

type ItemEditView = 
    { Title : TextBox<TitleError>
      Quantity : TextBox<QuantityError>
      QuantitySpinner : Spinner
      Note : TextBox<NoteError>
      Repeat : ChooseOne<Repeat>
      RelativeStatus : ChooseOne<RelativeStatus>
      Commands : Set<ItemEditCommand> }

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
