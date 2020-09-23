module Models.QueryTypes
open Models
open StateTypes

type ItemAvailability =
    | ItemIsAvailable
    | ItemIsNotSold

type ItemQry =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Category: Category option
      Schedule: Schedule
      Availability: (Store * ItemAvailability) list }

type CategoryQry =
    { Category : Category option
      Items: CategoryItem list }

and CategoryItem =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Schedule: Schedule
      Availability: (Store * ItemAvailability) list }

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