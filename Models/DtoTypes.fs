module Models.DtoTypes

open System
open Newtonsoft.Json

type GroceryDocument() =
    member val UserId: string = "" with get, set

type Item() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val ItemId: Guid = Guid.Empty with get, set

    member val ItemName: string = "" with get, set
    member val Note: string = "" with get, set
    member val Quantity: string = "" with get, set
    member val CategoryId: Nullable<Guid> = Nullable<Guid>() with get, set

type Store() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val StoreId: Guid = Guid.Empty with get, set

    member val StoreName: string = "" with get, set

type Category() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val CategoryId: Guid = Guid.Empty with get, set

    member val CategoryName: string = "" with get, set

type NotSoldItem() =
    inherit GroceryDocument()
    member val StoreId: Guid = Guid.Empty with get, set
    member val ItemId: Guid = Guid.Empty with get, set

    [<JsonProperty("id")>]
    member me.Id = sprintf "(%s,%s)" (me.StoreId |> Id.serialize) (me.ItemId |> Id.serialize)

