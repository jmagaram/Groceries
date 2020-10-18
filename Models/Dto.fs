namespace Models

open System
open ChangeTrackerTypes

module Dto =

    type StateItem = StateTypes.Item
    type StateStore = StateTypes.Store

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
                    |> String.tryParseOptional Note.tryParse
                    |> Result.mapError (sprintf "Could not parse the note '%s'; error %A" i.Content.Note)

                let! quantity =
                    i.Content.Quantity
                    |> String.tryParseOptional Quantity.tryParse
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
                    { StateItem.ItemId = itemId
                      StateItem.ItemName = itemName
                      StateItem.CategoryId = categoryId
                      StateItem.Note = note
                      StateItem.Etag = StateTypes.Etag i.Etag |> Some
                      StateItem.Quantity = quantity
                      StateItem.Schedule = schedule }

                return (Upsert item)
        }

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
                        { StateStore.StoreId = storeId
                          StateStore.StoreName = storeName
                          StateStore.Etag = StateTypes.Etag i.Etag |> Some }
        }

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

    let withCustomerId id (i: DtoTypes.Document<_>) = { i with CustomerId = id }

    let pushRequest (s: Models.StateTypes.State) =
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

        { DtoTypes.Changes.Items = collect StateQuery.itemsTable serializeItem
          DtoTypes.Changes.Categories = collect StateQuery.categoriesTable serializeCategory
          DtoTypes.Changes.Stores = collect StateQuery.storesTable serializeStore
          DtoTypes.Changes.NotSoldItems = collect StateQuery.notSoldItemsTable serializeNotSoldItem }

    // maybe should take what works, not throw
    let private deserialize<'T, 'U> (f: DtoTypes.Document<'T> -> Result<'U, string>) (i: DtoTypes.Document<'T> seq) =
        i
        |> Seq.map f
        |> Result.fromResults
        |> Result.okOrThrow

    let pullResponse items categories stores notSoldItems =
        { StateTypes.ImportChanges.ItemChanges = deserialize<_, _> deserializeItem items
          StateTypes.ImportChanges.CategoryChanges = deserialize<_, _> deserializeCategory categories
          StateTypes.ImportChanges.StoreChanges = deserialize<_, _> deserializeStore stores
          StateTypes.ImportChanges.NotSoldItemChanges = deserialize<_, _> deserializeNotSoldItem notSoldItems
          StateTypes.ImportChanges.LatestTimestamp =
              [ items
                |> Seq.map (fun i -> i.Timestamp |> Option.ofNullable)
                stores
                |> Seq.map (fun i -> i.Timestamp |> Option.ofNullable)
                categories
                |> Seq.map (fun i -> i.Timestamp |> Option.ofNullable)
                notSoldItems
                |> Seq.map (fun i -> i.Timestamp |> Option.ofNullable) ]
              |> Seq.concat
              |> Seq.choose id
              |> Seq.toList
              |> Seq.fold (fun m i ->
                  m
                  |> Option.map (fun m -> max m i)
                  |> Option.orElse (Some i)) None }
