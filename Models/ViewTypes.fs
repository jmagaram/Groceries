module Models.ViewTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

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

// store filter and picker?
// future item status filter and picker?
// order of display?
// filter?

// stores to choose from
// currently selected store
// any errors in a form?
type ShoppingList = { Items: Item list }
