namespace Models

open System
open SynchronizationTypes

module Dto =

    let serializeItem isDeleted (i: StateTypes.Item): DtoTypes.Document<DtoTypes.Item> =
        { Id = i.ItemId |> Id.itemIdToGuid |> toString
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
                    |> Option.map Id.categoryIdToGuid
                    |> Option.toNullable
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
        let itemId =
            i.Id
            |> String.tryParseWith Guid.TryParse
            |> Option.get
            |> StateTypes.ItemId

        match i.IsDeleted with
        | true -> Change.Delete itemId
        | false ->
            Upsert
                { StateTypes.Item.ItemId = itemId
                  StateTypes.Item.ItemName =
                      i.Content.ItemName
                      |> ItemName.tryParse
                      |> Result.okOrThrow
                  StateTypes.Item.CategoryId =
                      i.Content.CategoryId
                      |> Option.ofNullable
                      |> Option.map StateTypes.CategoryId
                  StateTypes.Item.Note =
                      i.Content.Note
                      |> Note.tryParseOptional
                      |> Result.okOrThrow
                  StateTypes.Item.Etag = StateTypes.Etag i.Etag |> Some
                  StateTypes.Item.Quantity =
                      i.Content.Quantity
                      |> Quantity.tryParseOptional
                      |> Result.okOrThrow
                  StateTypes.Item.Schedule =
                      match i.Content.ScheduleKind with
                      | DtoTypes.ScheduleKind.Completed -> StateTypes.Schedule.Completed
                      | DtoTypes.ScheduleKind.Once -> StateTypes.Schedule.Once
                      | DtoTypes.ScheduleKind.Repeat ->
                          StateTypes.Repeat
                              { StateTypes.Repeat.Frequency =
                                    i.Content.ScheduleRepeat.Frequency
                                    |> Frequency.create
                                    |> Result.okOrThrow
                                StateTypes.Repeat.PostponedUntil =
                                    i.Content.ScheduleRepeat.PostponedUntil
                                    |> Option.ofNullable }
                      | _ -> failwith "Unexpected schedule kind." }

    let serializeCategory isDeleted (i: StateTypes.Category): DtoTypes.Document<DtoTypes.Category> =
        { Id = i.CategoryId |> Id.categoryIdToGuid |> toString
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
                |> Id.deserialize
                |> Option.map (StateTypes.CategoryId >> Ok)
                |> Option.defaultValue
                    (sprintf "Could not deserialize this category GUID: %s" i.Id
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

    let deserializeCategories (cs: DtoTypes.Document<DtoTypes.Category> seq) =
        cs
        |> Seq.map deserializeCategory
        |> Result.fromResults
        |> Result.okOrThrow

    let serializeStore isDeleted (i: StateTypes.Store): DtoTypes.Document<DtoTypes.Store> =
        { Id = i.StoreId |> Id.storeIdToGuid |> toString
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
        let storeId =
            i.Id
            |> String.tryParseWith Guid.TryParse
            |> Option.get
            |> StateTypes.StoreId

        match i.IsDeleted with
        | true -> Change.Delete storeId
        | false ->
            Change.Upsert
                { StateTypes.Store.StoreId = storeId
                  StateTypes.Store.StoreName =
                      i.Content.StoreName
                      |> StoreName.tryParse
                      |> Result.okOrThrow
                  StateTypes.Store.Etag = StateTypes.Etag i.Etag |> Some }

    let serializeNotSoldItem isDeleted (i: StateTypes.NotSoldItem): DtoTypes.Document<DtoTypes.NotSoldItem> =
        { Id = i |> NotSoldItem.serialize // use Json here
          CustomerId = null
          DocumentKind = DtoTypes.DocumentKind.NotSoldItem
          Etag = null
          IsDeleted = isDeleted
          Timestamp = Nullable<int>()
          Content = () }

    let deserializeNotSoldItem (i: DtoTypes.Document<DtoTypes.NotSoldItem>) =
        let id = i.Id |> NotSoldItem.deserialize

        match i.IsDeleted with
        | true -> Change.Delete id
        | false -> Change.Upsert id

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
