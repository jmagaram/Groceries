module Models.ViewTypes
open Models.FormsTypes

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
      NotSoldItems: StoreItem list }

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

// OPTION 1 - Just keep ItemId in the list, sorted as I want, bind by ID
// OPTION 2 - Full list of items, whole list changes each time BUT distinctUntilChanged
// 
// Also, should the view contain several IObservables that change independently?

// Not sure this should be a view Maybe just have an IObservable of Items and
// let the UI figure out how to package it all together Maybe have a previous
// module that has the collections as predefined but useful queries and then
// combine them and fiilter them in other modules
type ShoppingList = 
    { Items: Item list 
      //StoreFilter : ChooseOne<ChooseOneItem<StateTypes.Store>>
    }

// or show for every store
