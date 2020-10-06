namespace Models

open System
open SynchronizationTypes

module Dto =

    let serializeItem isDeleted (i: StateTypes.Item): DtoTypes.Document<DtoTypes.Item> =
        { Id = i.ItemId |> ItemId.serialize
          CustomerId = null
          DocumentKind = DtoTypes.DocumentKind.Item
          Etag =
              i.Etag
              |> Option.map (Etag.tag)
              |> Option.defaultValue null
          IsDeleted = isDeleted
          Timestamp = Nullable<int>()
          Content =
              { ItemName = i.ItemName |> ItemName.asText
                CategoryId =
                    i.CategoryId
                    |> Option.map CategoryId.serialize
                    |> Option.defaultValue null
                Note =
                    i.Note
                    |> Option.map Note.asText
                    |> Option.defaultValue null
                Quantity =
                    i.Quantity
                    |> Option.map Quantity.asText
                    |> Option.defaultValue null
                ScheduleKind =
                    match i.Schedule with
                    | StateTypes.Schedule.Completed -> DtoTypes.ScheduleKind.Completed
                    | StateTypes.Schedule.Once -> DtoTypes.ScheduleKind.Once
                    | StateTypes.Schedule.Repeat _ -> DtoTypes.ScheduleKind.Repeat
                ScheduleRepeat =
                    match i.Schedule with
                    | StateTypes.Schedule.Repeat r ->
                        { Frequency = r.Frequency |> Frequency.days
                          PostponedUntil = r.PostponedUntil |> Option.toNullable }
                    | _ -> Unchecked.defaultof<DtoTypes.Repeat> } }

    let deserializeItem (i: DtoTypes.Document<DtoTypes.Item>) =
        result {
            let! itemId =
                i.Id
                |> ItemId.deserialize
                |> Option.map Ok
                |> Option.defaultValue
                    (i.Id
                     |> sprintf "Could not deserialize the item ID '%s'"
                     |> Error)

            match i.IsDeleted with
            | true -> return (itemId |> Change.Delete)
            | false ->
                let! itemName =
                    i.Content.ItemName
                    |> ItemName.tryParse
                    |> Result.mapError (fun e ->
                        sprintf "Could not deserialize the item name '%s'; error: %A" i.Content.ItemName e)

                let! categoryId =
                    match i.Content.CategoryId |> String.isNullOrWhiteSpace with
                    | true -> None |> Ok
                    | false ->
                        i.Content.CategoryId
                        |> CategoryId.deserialize
                        |> Option.map (Some >> Ok)
                        |> Option.defaultValue
                            (i.Content.CategoryId
                             |> sprintf "Could not deserialize the category ID '%s'"
                             |> Error)

                let! note =
                    i.Content.Note
                    |> Note.tryParseOptional
                    |> Result.mapError (sprintf "Could not parse the note '%s'; error %A" i.Content.Note)

                let! quantity =
                    i.Content.Quantity
                    |> Quantity.tryParseOptional
                    |> Result.mapError (sprintf "Could not parse the quantity '%s'; error %A" i.Content.Quantity)

                let! schedule =
                    match i.Content.ScheduleKind with
                    | DtoTypes.ScheduleKind.Completed -> StateTypes.Schedule.Completed |> Ok
                    | DtoTypes.ScheduleKind.Once -> StateTypes.Schedule.Once |> Ok
                    | DtoTypes.ScheduleKind.Repeat ->
                        let frequency =
                            i.Content.ScheduleRepeat.Frequency
                            |> Frequency.create
                            |> Result.mapError (fun e ->
                                sprintf
                                    "Could not parse the frequency '%i'; error %A"
                                    i.Content.ScheduleRepeat.Frequency
                                    e)

                        let postponedUntil =
                            i.Content.ScheduleRepeat.PostponedUntil
                            |> Option.ofNullable

                        frequency
                        |> Result.map (fun f ->
                            StateTypes.Schedule.Repeat
                                { StateTypes.Repeat.Frequency = f
                                  StateTypes.Repeat.PostponedUntil = postponedUntil })

                    | _ ->
                        sprintf "An unexpected schedule type was found: '%A'" i.Content.ScheduleKind
                        |> Error

                let item =
                    { StateTypes.Item.ItemId = itemId
                      StateTypes.Item.ItemName = itemName
                      StateTypes.Item.CategoryId = categoryId
                      StateTypes.Item.Note = note
                      StateTypes.Item.Etag = StateTypes.Etag i.Etag |> Some
                      StateTypes.Item.Quantity = quantity
                      StateTypes.Item.Schedule = schedule }

                return (Upsert item)
        }

    // maybe should take what works, not throw
    let deserializeItems (i: DtoTypes.Document<DtoTypes.Item> seq) =
        i
        |> Seq.map deserializeItem
        |> Result.fromResults
        |> Result.okOrThrow

    let serializeCategory isDeleted (i: StateTypes.Category): DtoTypes.Document<DtoTypes.Category> =
        { Id = i.CategoryId |> CategoryId.serialize
          CustomerId = null
          DocumentKind = DtoTypes.DocumentKind.Category
          Etag =
              i.Etag
              |> Option.map (Etag.tag)
              |> Option.defaultValue null
          IsDeleted = isDeleted
          Timestamp = Nullable<int>()
          Content = { CategoryName = i.CategoryName |> CategoryName.asText } }

    let deserializeCategory (i: DtoTypes.Document<DtoTypes.Category>) =
        result {
            let! categoryId =
                i.Id
                |> CategoryId.deserialize
                |> Option.map Ok
                |> Option.defaultValue
                    (sprintf "Could not deserialize this category ID: %s" i.Id
                     |> Error)

            let! categoryName =
                i.Content.CategoryName
                |> CategoryName.tryParse
                |> Result.mapError (fun e ->
                    (sprintf "Could not deserialize '%s' as a category name; error: %A" i.Content.CategoryName e))

            match i.IsDeleted with
            | true -> return Change.Delete categoryId
            | false ->
                return
                    Change.Upsert
                        { StateTypes.Category.CategoryId = categoryId
                          StateTypes.Category.CategoryName = categoryName
                          StateTypes.Category.Etag = StateTypes.Etag i.Etag |> Some }
        }

    // maybe should take what works, not throw
    let deserializeCategories (cs: DtoTypes.Document<DtoTypes.Category> seq) =
        cs
        |> Seq.map deserializeCategory
        |> Result.fromResults
        |> Result.okOrThrow

    let serializeStore isDeleted (i: StateTypes.Store): DtoTypes.Document<DtoTypes.Store> =
        { Id = i.StoreId |> StoreId.serialize
          CustomerId = null
          DocumentKind = DtoTypes.DocumentKind.Store
          Etag =
              i.Etag
              |> Option.map (Etag.tag)
              |> Option.defaultValue null
          IsDeleted = isDeleted
          Timestamp = Nullable<int>()
          Content = { StoreName = i.StoreName |> StoreName.asText } }

    let deserializeStore (i: DtoTypes.Document<DtoTypes.Store>) =
        result {
            let! storeId =
                i.Id
                |> StoreId.deserialize
                |> Option.map Ok
                |> Option.defaultValue
                    (sprintf "Could not deserialize this store ID: %s" i.Id
                     |> Error)

            let! storeName =
                i.Content.StoreName
                |> StoreName.tryParse
                |> Result.mapError (fun e ->
                    (sprintf "Could not deserialize '%s' as a store name; error: %A" i.Content.StoreName e))

            match i.IsDeleted with
            | true -> return Change.Delete storeId
            | false ->
                return
                    Change.Upsert
                        { StateTypes.Store.StoreId = storeId
                          StateTypes.Store.StoreName = storeName
                          StateTypes.Store.Etag = StateTypes.Etag i.Etag |> Some }
        }

    // maybe should take what works, not throw
    let deserializeStores (cs: DtoTypes.Document<DtoTypes.Store> seq) =
        cs
        |> Seq.map deserializeStore
        |> Result.fromResults
        |> Result.okOrThrow

    let serializeNotSoldItem isDeleted (i: StateTypes.NotSoldItem): DtoTypes.Document<DtoTypes.NotSoldItem> =
        { Id = i |> NotSoldItem.serialize // use Json here
          CustomerId = null
          DocumentKind = DtoTypes.DocumentKind.NotSoldItem
          Etag = null
          IsDeleted = isDeleted
          Timestamp = Nullable<int>()
          Content = () }

    let deserializeNotSoldItem (i: DtoTypes.Document<DtoTypes.NotSoldItem>) =
        result {
            let! id = i.Id |> NotSoldItem.deserialize

            match i.IsDeleted with
            | true -> return id |> Change.Delete
            | false -> return id |> Change.Upsert
        }

    let deserializeNotSoldItems (i: DtoTypes.Document<DtoTypes.NotSoldItem> seq) =
        i
        |> Seq.map deserializeNotSoldItem
        |> Result.fromResults
        |> Result.okOrThrow

    let withCustomerId id (i: DtoTypes.Document<_>) = { i with CustomerId = id }

    let changes (s: Models.StateTypes.State) =
        let collect table f =
            let upsert =
                s
                |> table
                |> DataTable.isAddedOrModified
                |> Seq.map (fun i -> (i, false))

            let delete =
                s
                |> table
                |> DataTable.isDeleted
                |> Seq.map (fun i -> (i, true))

            upsert
            |> Seq.append delete
            |> Seq.map (fun (i, isDeleted) -> f isDeleted i)
            |> Seq.toArray

        { DtoTypes.Changes.Items = collect State.itemsTable serializeItem
          DtoTypes.Changes.Categories = collect State.categoriesTable serializeCategory
          DtoTypes.Changes.Stores = collect State.storesTable serializeStore
          DtoTypes.Changes.NotSoldItems = collect State.notSoldItemsTable serializeNotSoldItem }

    let processPull items categories stores notSoldItems (s: StateTypes.State) =
        s
        |> State.mapItems (SynchronizeState.merge items)
        |> State.mapCategories (SynchronizeState.merge categories)
        |> State.mapStores (SynchronizeState.merge stores)
        |> State.mapNotSoldItems (SynchronizeState.merge notSoldItems)
        |> State.fixBrokenForeignKeys
