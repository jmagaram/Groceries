module Models.ViewTypes

[<Struct>]
type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

[<Struct>]
type SearchTerm = SearchTerm of string

type FormattedText = FormattedText of TextSpan list

type Highlighter = SearchTerm -> string -> FormattedText

type Item =
    { ItemId: StateTypes.ItemId
      ItemName: FormattedText
      Note: FormattedText option
      Quantity: FormattedText option
      Category: ItemCategory option
      Schedule: StateTypes.Schedule
      NotSoldAt: ItemStore list }

and ItemCategory =
    { CategoryId: StateTypes.CategoryId
      CategoryName: FormattedText }

and ItemStore = { StoreId: StateTypes.StoreId; StoreName: FormattedText }

type Category =
    { CategoryId: StateTypes.CategoryId
      CategoryName: FormattedText
      Items: CategoryItem list }

and CategoryItem =
    { ItemId: StateTypes.ItemId
      ItemName: FormattedText
      Note: FormattedText option
      Quantity: FormattedText option
      Schedule: StateTypes.Schedule
      NotSoldAt: ItemStore list }

type Store =
    { StoreId: StateTypes.StoreId
      StoreName: FormattedText
      NotSoldItems: StateTypes.Item list }

and StoreItem =
    { ItemId: StateTypes.ItemId
      ItemName: FormattedText
      Note: FormattedText option
      Quantity: FormattedText option
      Category: ItemCategory option
      Schedule: StateTypes.Schedule }
