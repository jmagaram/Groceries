module Models.DtoTypes

open System
open System.Collections.Generic
open Newtonsoft.Json

// this is not needed when communicating to server
// only needed from server to cosmos
// two levels of DTO needed
// same with userId

type DocumentKind =
    | Item = 1
    | Store = 2
    | Category = 3
    | NotSoldItem = 4

type Document<'T> =
    { DocumentKind: DocumentKind
      [<JsonProperty("CustomerId")>]
      CustomerId: string
      [<JsonProperty("_etag")>]
      Etag: string
      [<JsonProperty("_ts")>]
      Timestamp: Nullable<int>
      [<JsonProperty("id")>]
      Id: string
      IsDeleted: bool
      Content: 'T }

type Item =
    { ItemName: string
      ScheduleKind: ScheduleKind
      ScheduleRepeat: Repeat
      Note: string
      Quantity: string
      CategoryId: Nullable<Guid> }

and ScheduleKind =
    | Completed = 1
    | Once = 2
    | Repeat = 3

and Repeat =
    { Frequency: int<StateTypes.days>
      PostponedUntil: Nullable<DateTimeOffset> }

type Store = { StoreName: string }

type Category = { CategoryName: string }

type NotSoldItem = unit

type Changes =
    { Items: Document<Item> []
      Categories: Document<Category> []
      Stores: Document<Store> []
      NotSoldItems: Document<NotSoldItem> [] }
