[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ShoppingList

open System
open CoreTypes
open StateTypes
open ViewTypes

type Item =
    { ItemId: ItemId
      ItemName: FormattedText
      Note: FormattedText option
      Quantity: FormattedText option
      Category: Category option
      PostponeUntil: DateTimeOffset option
      Availability: ItemAvailability seq }

and ItemAvailability = { Store: Store; IsSold: bool }

type ShoppingList =
    { StoreFilter: Store option
      TextFilter: TextBox
      Categories: CategorySummary list
      Stores: Store list }
    member me.TotalItems =
        me.Categories
        |> Seq.sumBy (fun c -> c.Items |> List.length)

and CategorySummary =
    { Category: Category option
      Items: Item list }

type CategorySummary with
    member me.Active =
        me.Items
        |> Seq.filter (fun i -> i.PostponeUntil.IsNone)
        |> Seq.length

    member me.Postponed =
        me.Items
        |> Seq.filter (fun i -> i.PostponeUntil.IsSome)
        |> Seq.length

    member me.Total = me.Items |> Seq.length
    member me.Inactive = me.Total - me.Active

let postponeChangedCore (s1, s2) =
    s2
    |> State.items
    |> Seq.choose
        (fun i2 ->
            s1
            |> State.tryFindItem i2.ItemId
            |> Option.bind
                (fun i1 ->
                    match i1.PostponeUntil = i2.PostponeUntil with
                    | true -> None
                    | false -> Some i2.ItemId))
    |> Set.ofSeq

let private deletedCore (s1, s2) =
    let s1Items =
        s1
        |> State.items
        |> Seq.map (fun i -> i.ItemId)
        |> Set.ofSeq

    let s2Items =
        s2
        |> State.items
        |> Seq.map (fun i -> i.ItemId)
        |> Set.ofSeq

    s1Items - s2Items

let createItem find (item: CoreTypes.Item) state =
    let find =
        match find with
        | Some find -> find
        | None -> fun s -> s |> FormattedText.normal

    { ItemId = item.ItemId
      ItemName = item.ItemName |> ItemName.asText |> find
      Note = item.Note |> Option.map (Note.asText >> find)
      Quantity =
          item.Quantity
          |> Option.map (Quantity.asText >> find)
      Category =
          item.CategoryId
          |> Option.map
              (fun c ->
                  state
                  |> State.categoriesTable
                  |> DataTable.findCurrent c)
      PostponeUntil = item.PostponeUntil
      Availability =
          state
          |> State.stores
          |> Seq.map
              (fun st ->
                  { Store = st
                    IsSold =
                        state
                        |> State.notSoldTable
                        |> DataTable.tryFindCurrent
                            { StoreId = st.StoreId
                              ItemId = item.ItemId }
                        |> Option.isNone }) }

let createItemFromItemId (id: CoreTypes.ItemId) state =
    let item =
        state
        |> State.itemsTable
        |> DataTable.findCurrent id

    createItem None item state

let create now state =
    let settings =
        (state |> State.userSettingsForLoggedInUser)
            .ShoppingListSettings

    let find =
        let terms =
            settings.TextFilter.ValueTyping
            |> SearchTerm.splitOnSpace 3 SearchTerm.englishWordsToIgnore
            |> List.ofSeq

        match terms with
        | [] -> None
        | xs -> Highlighter.findAny xs |> Some

    let items (now: DateTimeOffset) =
        state
        |> State.items
        |> Seq.map (fun item -> createItem find item state)
        |> Seq.filter
            (fun i ->
                let isStoreMatch =
                    settings.StoreFilter
                    |> Option.map
                        (fun s ->
                            i.Availability
                            |> Seq.find (fun x -> x.Store.StoreId = s)
                            |> fun a -> a.IsSold)
                    |> Option.defaultValue true

                let isPostponedMatch =
                    match i.PostponeUntil with
                    | None -> true
                    | Some postponeUntil ->
                        match settings.PostponedViewHorizon with
                        | None -> false
                        | Some horizon ->
                            let horizon = now.AddDays(horizon |> float)
                            postponeUntil <= horizon

                let isTextMatch =
                    match find with
                    | None -> true
                    | Some _ ->
                        let name = i.ItemName |> FormattedText.hasHighlight

                        let note =
                            i.Note
                            |> Option.map (FormattedText.hasHighlight)
                            |> Option.defaultValue false

                        let qty =
                            i.Quantity
                            |> Option.map (FormattedText.hasHighlight)
                            |> Option.defaultValue false

                        name || note || qty

                if find.IsSome then
                    isTextMatch
                else
                    isStoreMatch && isPostponedMatch)
        |> Seq.toList

    let storeFilter =
        settings.StoreFilter
        |> Option.map
            (fun sid ->
                state
                |> State.storesTable
                |> DataTable.findCurrent sid)

    let stores = state |> State.stores |> List.ofSeq

    let items = items now

    let sortKey item =
        ((if item.PostponeUntil.IsNone then
              0
          else
              1),
         item.PostponeUntil,
         item.ItemName
         |> FormattedText.asString
         |> String.toLowerInvariant)

    { StoreFilter = storeFilter
      TextFilter = settings.TextFilter
      Stores = stores
      Categories =
          state
          |> State.categories
          |> Seq.map Some
          |> Seq.append (Seq.singleton None)
          |> Seq.sortBy
              (fun i ->
                  i
                  |> Option.map (fun s -> s.CategoryName |> CategoryName.asText)
                  |> Option.defaultValue "")
          |> Seq.map
              (fun c ->
                  { CategorySummary.Category = c
                    Items =
                        items
                        |> Seq.filter (fun j -> j.Category = c)
                        |> Seq.sortBy sortKey
                        |> Seq.toList })
          |> Seq.toList }
