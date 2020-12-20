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
      Schedule: Schedule
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
        |> Seq.filter (fun i -> i.Schedule |> Schedule.isActive)
        |> Seq.length

    member me.Postponed =
        me.Items
        |> Seq.filter (fun i -> i.Schedule |> Schedule.isPostponed)
        |> Seq.length

    member me.Total = me.Items |> Seq.length
    member me.Inactive = me.Total - me.Active

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
      Schedule = item.Schedule
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
                let isCompletedMatch =
                    (settings.HideCompletedItems = false)
                    || (i.Schedule |> Schedule.isCompleted |> not)

                let isStoreMatch =
                    settings.StoreFilter
                    |> Option.map
                        (fun s ->
                            i.Availability
                            |> Seq.find (fun x -> x.Store.StoreId = s)
                            |> fun a -> a.IsSold)
                    |> Option.defaultValue true

                let isPostponedMatch =
                    i.Schedule
                    |> Schedule.postponedUntil
                    |> Option.map
                        (fun postponedUntil ->
                            let horizon =
                                now.AddDays(settings.PostponedViewHorizon |> float)

                            postponedUntil <= horizon)
                    |> Option.defaultValue true

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
                    isCompletedMatch
                    && isStoreMatch
                    && isPostponedMatch)
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
        let group =
            match item.Schedule with
            | Schedule.IsActive -> 0
            | Schedule.IsPostponedUntil _ -> 1
            | Schedule.IsComplete -> 2

        let date = item.Schedule |> Schedule.postponedUntil
        (group, date, item.ItemName)

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
