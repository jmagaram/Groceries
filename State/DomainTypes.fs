module DomainTypes
open System

type Duration = 
    | D3 
    | W1 
    | W2 
    | W3 
    | M1 
    | M2 
    | M3 
    | M4 
    | M6 
    | M9

type Title = | Title of string

type Note = | Note of string

type Quantity = | Quantity of string

type ItemId = | ItemId of Guid

type Repeat =
    { Frequency : Duration
      PostponedUntil : DateTime option }

type Schedule =
    | Complete
    | Incomplete
    | Postponed of DateTime
    | Repeat of Repeat

type Item = 
    { Id : ItemId
      Title : Title
      Note : Note option
      Quantity : Quantity option
      Schedule : Schedule }

type StoreId = | StoreId of Guid

type StoreName = | StoreName of string

type State = 
    { Stores : Map<StoreId, StoreName> 
      Items : Map<ItemId, Item> 
      ItemIsUnavailableInStore : Set<StoreId * ItemId> }

