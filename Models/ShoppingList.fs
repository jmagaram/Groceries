[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ShoppingList

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
    { Items: Item list
      ItemEdited: ItemForm option
      StoreFilter: Store option
      Stores: Store list
      SearchTerm: SearchTerm option
      HasChanges: bool }

let private createItem find (item: StateTypes.Item) state =
    let find =
        match find with
        | Some find -> find
        | None -> fun s -> s |> FormattedText.normal

    { ItemId = item.ItemId
      ItemName = item.ItemName |> ItemName.asText |> find
      Note = item.Note |> Option.map (Note.asText >> find)
      Quantity = item.Quantity |> Option.map (Quantity.asText >> find)
      Category =
          item.CategoryId
          |> Option.map (fun c ->
              state
              |> StateQuery.categoriesTable
              |> DataTable.findCurrent c)
      Schedule = item.Schedule
      Availability =
          state
          |> StateQuery.stores
          |> Seq.map (fun st ->
              { Store = st
                IsSold =
                    state
                    |> StateQuery.notSoldItemsTable
                    |> DataTable.tryFindCurrent { StoreId = st.StoreId; ItemId = item.ItemId }
                    |> Option.isNone }) }

let create now state =
    let settings = state |> StateQuery.settings

    let find = settings.ItemTextFilter |> Option.map Highlighter.create

    let items now =
        state
        |> StateQuery.items
        |> Seq.map (fun item -> createItem find item state)
        |> Seq.filter (fun i ->
            let isCompletedMatch =
                (settings.HideCompletedItems = false)
                || (i.Schedule |> Schedule.isCompleted |> not)

            let isStoreMatch =
                settings.StoreFilter
                |> Option.map (fun s ->
                    i.Availability
                    |> Seq.find (fun x -> x.Store.StoreId = s)
                    |> fun a -> a.IsSold)
                |> Option.defaultValue true

            let isPostponedMatch = // hack
                match i.Schedule with
                | StateTypes.Schedule.Repeat r ->
                    let due = i.Schedule |> Schedule.effectiveDueDate now

                    let horizon = now.AddDays(settings.PostponedViewHorizon |> float)

                    due <= horizon
                | _ -> true

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

            if find.IsSome then isTextMatch else isCompletedMatch && isStoreMatch && isPostponedMatch)
        |> Seq.toList

    let storeFilter =
        settings.StoreFilter
        |> Option.map (fun sid -> state |> StateQuery.storesTable |> DataTable.findCurrent sid)

    let stores = state |> StateQuery.stores |> List.ofSeq

    { Items = items now
      StoreFilter = storeFilter
      SearchTerm = settings.ItemTextFilter
      Stores = stores
      HasChanges =
          state |> StateQuery.hasChanges
          || state.LastCosmosTimestamp.IsNone
      ItemEdited = None }
