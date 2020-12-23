module Models.DtoTypes

open System
open System.Collections.Generic
open Newtonsoft.Json

// the information sent from client to server is a subset of what is sent from
// the server to cosmos. for example, the CustomerId is set on the server and
// does not need to be seen by the client. for simplicity, using the exact same
// structure for now.

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
      Note: string
      PostponeUntil : Nullable<DateTimeOffset>
      Quantity: string
      CategoryId: string }

type Store = { StoreName: string }

type Category = { CategoryName: string }

type NotSoldItem = unit

type Changes =
    { Items: Document<Item> []
      Categories: Document<Category> []
      Stores: Document<Store> []
      NotSoldItems: Document<NotSoldItem> [] }
