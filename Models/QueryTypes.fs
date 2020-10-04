module Models.QueryTypes
open Models
open StateTypes

type ItemAvailability =
    { Store : Store
      IsSold : bool }

type ItemQry =
    { ItemId: ItemId
      ItemName: ItemName
      Etag : Etag option
      Note: Note option
      Quantity: Quantity option
      Category: Category option
      Schedule: Schedule
      Availability: ItemAvailability seq }

type CategoryQry =
    { Category : Category option
      Items: CategoryItem seq }

and CategoryItem =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Schedule: Schedule
      Availability: ItemAvailability seq }

type ShoppingListQry =
    { Stores : Store list
      Items : ItemQry list
      StoreFilter : Store option }

type ItemFindQry =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Category: Category option
      Schedule: Schedule }