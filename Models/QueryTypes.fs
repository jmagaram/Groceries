module Models.QueryTypes

type ItemDenormalized = 
    { Item : StateTypes.Item
      Category : StateTypes.Category
      NotSold : StateTypes.Store list }

type StoreDenormalized = 
    { Store : StateTypes.Store
      NotSold : StateTypes.Item list }

type CategoryDenormalized =
    { Category : StateTypes.Category
      Items : StateTypes.Item list }