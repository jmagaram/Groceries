module Models.ViewTypes

type Item =
    { ItemId: StateTypes.ItemId
      Name: StateTypes.ItemName
      Note: StateTypes.Note option
      Quantity: StateTypes.Quantity option
      Category: StateTypes.Category option
      NotSold : StateTypes.Store list
      Schedule: StateTypes.Schedule }

