module DomainTypes
open System

type Frequency = 
    | Daily
    | Weekly
    | Every2Weeks 
    | Every3Weeks 
    | Monthly 
    | Every2Months 
    | Every3Months 

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
      Repeat : Frequency option
      Status : Status }

type StoreId = | StoreId of Guid

type StoreName = | StoreName of string

type State = 
    { Stores : Map<StoreId, StoreName> 
      Items : Map<ItemId, Item> 
      ItemIsUnavailableInStore : Set<StoreId * ItemId> }

type UpDown<'v> = 
    { Increase : 'v -> 'v option
      Decrease : 'v -> 'v option }

type PickOne<'T> when 'T : comparison =
    { Choices : Set<'T>
      SelectedItem : 'T }

type PickOneMessage<'T> =
    | PickOneByPredicate of ('T -> bool)
    | PickOneByItem of 'T


type Selector<'T, 'Error> when 'T : comparison =
    { Choices : Set<'T>
      SelectedItem : 'T option 
      Error : 'Error option }

type Validated<'t,'error> =
    { Value : 't 
      Error : 'error option }

type TextBox<'Error> =
    { Text : string
      NormalizedText : string
      Error : 'Error option 
      HasFocus : bool }

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

type RepeatSelector = Selector<DailyInterval option, RepeatError>

type Model =
    { Title : Validated<string, string>
      Quantity : string 
      Note : string
      Status : Selector<RelativeStatusChoice,string>
      PostponedDays : int
      Repeat : RepeatSelector }