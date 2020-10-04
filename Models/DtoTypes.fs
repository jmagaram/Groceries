module Models.DtoTypes

open System
open System.Collections.Generic;
open Newtonsoft.Json

type DocumentKind =
    | Undefined = 0
    | Item = 1
    | Store = 2
    | Category = 3
    | NotSoldItem = 4

type GroceryDocument() =
    member val UserId = "" with get, set
    member val DocumentKind = DocumentKind.Undefined with get, set
    member val IsDeleted = false with get, set
    
    [<JsonProperty("_etag")>]
    member val _etag = "" with get, set
    [<JsonProperty("_ts")>]
    member val _ts = 0 with get, set // seconds since 1970

[<AllowNullLiteral>]
type Repeat() =
    member val Frequency = 0 with get, set
    member val PostponedUntil = Nullable<DateTimeOffset>() with get, set

type ScheduleKind =
    | Undefined = 0
    | Completed = 1
    | Once = 2
    | Repeat = 3

type Item() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val ItemId: Guid = Guid.Empty with get, set

    member val ItemName = "" with get, set
    member val Note = "" with get, set
    member val Quantity = "" with get, set
    member val CategoryId = Nullable<Guid>() with get, set
    member val ScheduleKind = ScheduleKind.Once with get, set
    member val Repeat : Repeat = null with get, set

type Store() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val StoreId = Guid.Empty with get, set

    member val StoreName = "" with get, set

type Category() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val CategoryId = Guid.Empty with get, set

    member val CategoryName = "" with get, set

type NotSoldItem() =
    inherit GroceryDocument()
    member val StoreId = Guid.Empty with get, set
    member val ItemId = Guid.Empty with get, set

    [<JsonProperty("id")>]
    member me.Id =
        sprintf "(%s,%s)" (me.StoreId |> Id.serialize) (me.ItemId |> Id.serialize)

type PushChanges() =
    member val Items = List<Item>() with get, set
    member val Categories = List<Category>() with get, set
    member val Stores = List<Store>() with get, set
    member val NotSoldItems = List<NotSoldItem>() with get, set