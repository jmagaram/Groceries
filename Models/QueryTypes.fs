module Models.QueryTypes
open Models
open StateTypes

type ItemQry =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Category: ItemCategory option
      Schedule: Schedule
      NotSoldAt: ItemStore list }

and ItemCategory =
    { CategoryId: CategoryId
      CategoryName: CategoryName }

and ItemStore = { StoreId: StoreId; StoreName: StoreName }

type CategoryQry =
    { CategoryId: CategoryId
      CategoryName: CategoryName
      Items: CategoryItem list }

and CategoryItem =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Schedule: Schedule
      NotSoldAt: ItemStore list }

type StoreQry =
    { StoreId: StoreId
      StoreName: StoreName
      NotSoldItems: StoreItem list }

and StoreItem =
    { ItemId: ItemId
      ItemName: ItemName
      Note: Note option
      Quantity: Quantity option
      Category: ItemCategory option
      Schedule: Schedule }

type ShoppingListQry =
    { Stores : Store list
      Items : ItemQry list
      ShoppingListViewOptions : ShoppingListViewOptions }
