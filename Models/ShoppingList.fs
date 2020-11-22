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
    { Items: Item list
      ItemEdited: ItemForm option
      StoreFilter: Store option
      TextFilter : TextBox
      Stores: Store list }

let private createItem find (item: CoreTypes.Item) state =
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
          |> Option.map (fun c -> state |> State.categoriesTable |> DataTable.findCurrent c)
      Schedule = item.Schedule
      Availability =
          state
          |> State.stores
          |> Seq.map (fun st ->
              { Store = st
                IsSold =
                    state
                    |> State.notSoldTable
                    |> DataTable.tryFindCurrent { StoreId = st.StoreId; ItemId = item.ItemId }
                    |> Option.isNone }) }

let create now state =
    let settings = state |> State.shoppingListSettings

    let find = settings.TextFilter.ValueTyping |> SearchTerm.tryParse |> Result.map Highlighter.create |> Result.asOption

    let items (now: DateTimeOffset) =
        state
        |> State.items
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

            let isPostponedMatch =
                i.Schedule
                |> Schedule.postponedUntil
                |> Option.map (fun postponedUntil ->
                    let horizon = now.AddDays(settings.PostponedViewHorizon |> float)
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

            if find.IsSome then isTextMatch else isCompletedMatch && isStoreMatch && isPostponedMatch)
        |> Seq.toList

    let storeFilter =
        settings.StoreFilter
        |> Option.map (fun sid -> state |> State.storesTable |> DataTable.findCurrent sid)

    let stores = state |> State.stores |> List.ofSeq

    { Items = items now
      StoreFilter = storeFilter
      TextFilter = settings.TextFilter
      Stores = stores
      ItemEdited = None }
