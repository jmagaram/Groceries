module Models.DtoTypes

open System
open Newtonsoft.Json

// Issue: Should GUID be a GUID or string? Is GUID a Microsoft specific format?
// If so, how can the service manage items created on different platforms? If
// there are other types of GUID values then the core StateType Ids need to use
// string.

type GroceryDocument() =
    member val UserId: string = "" with get, set

type Item() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val ItemId: string = "" with get, set

    member val ItemName: string = "" with get, set
    member val Note: string = "" with get, set
    member val Quantity: string = "" with get, set
    member val CategoryId: string = null with get, set

type Store() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val StoreId: string = "" with get, set

    member val StoreName: string = "" with get, set

type Category() =
    inherit GroceryDocument()

    [<JsonProperty("id")>]
    member val CategoryId: string = "" with get, set

    member val CategoryName: string = "" with get, set

type NotSoldItem() =
    inherit GroceryDocument()
    member val StoreId: string = "" with get, set
    member val ItemId: string = "" with get, set

    [<JsonProperty("id")>]
    member me.Id = sprintf "(%s,%s)" me.StoreId me.ItemId

