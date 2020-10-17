module Models.QueryTypes
open Models
open StateTypes

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
