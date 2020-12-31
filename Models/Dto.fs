namespace Models

open System
open ChangeTrackerTypes

module Dto =

    type StateItem = CoreTypes.Item
    type StateStore = CoreTypes.Store
    type StateCategory = CoreTypes.Category

    let serializeItem isDeleted (i: CoreTypes.Item): DtoTypes.Document<DtoTypes.Item> =
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
                PostponeUntil = i.PostponeUntil |> Option.toNullable } }

    let deserializeItem (i: DtoTypes.Document<DtoTypes.Item>) =
        result {
            let! itemId =
                i.Id
                |> ItemId.deserialize
                |> Option.map Ok
                |> Option.defaultValue (
                    i.Id
                    |> sprintf "Could not deserialize the item ID '%s'"
                    |> Error
                )

            match i.IsDeleted with
            | true -> return (itemId |> Change.Delete)
            | false ->
                let! itemName =
                    i.Content.ItemName
                    |> ItemName.tryParse
                    |> Result.mapError
                        (fun e -> sprintf "Could not deserialize the item name '%s'; error: %A" i.Content.ItemName e)

                let! categoryId =
                    match i.Content.CategoryId |> String.isNullOrWhiteSpace with
                    | true -> None |> Ok
                    | false ->
                        i.Content.CategoryId
                        |> CategoryId.deserialize
                        |> Option.map (Some >> Ok)
                        |> Option.defaultValue (
                            i.Content.CategoryId
                            |> sprintf "Could not deserialize the category ID '%s'"
                            |> Error
                        )

                let! note =
                    i.Content.Note
                    |> String.tryParseOptional Note.tryParse
                    |> Result.mapError (sprintf "Could not parse the note '%s'; error %A" i.Content.Note)

                let! quantity =
                    i.Content.Quantity
                    |> String.tryParseOptional Quantity.tryParse
                    |> Result.mapError (sprintf "Could not parse the quantity '%s'; error %A" i.Content.Quantity)

                let postponeUntil =
                    i.Content.PostponeUntil |> Option.ofNullable

                let item =
                    { StateItem.ItemId = itemId
                      StateItem.ItemName = itemName
                      StateItem.CategoryId = categoryId
                      StateItem.Note = note
                      StateItem.Etag = CoreTypes.Etag i.Etag |> Some
                      StateItem.Quantity = quantity
                      StateItem.PostponeUntil = postponeUntil }

                return (Upsert item)
        }

    let serializeCategory isDeleted (i: CoreTypes.Category): DtoTypes.Document<DtoTypes.Category> =
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
                |> Option.defaultValue (
                    sprintf "Could not deserialize this category ID: %s" i.Id
                    |> Error
                )

            let! categoryName =
                i.Content.CategoryName
                |> CategoryName.tryParse
                |> Result.mapError
                    (fun e ->
                        (sprintf "Could not deserialize '%s' as a category name; error: %A" i.Content.CategoryName e))

            match i.IsDeleted with
            | true -> return Change.Delete categoryId
            | false ->
                return
                    Change.Upsert
                        { StateCategory.CategoryId = categoryId
                          StateCategory.CategoryName = categoryName
                          StateCategory.Etag = CoreTypes.Etag i.Etag |> Some }
        }

    let serializeStore isDeleted (i: CoreTypes.Store): DtoTypes.Document<DtoTypes.Store> =
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
                |> Option.defaultValue (
                    sprintf "Could not deserialize this store ID: %s" i.Id
                    |> Error
                )

            let! storeName =
                i.Content.StoreName
                |> StoreName.tryParse
                |> Result.mapError
                    (fun e -> (sprintf "Could not deserialize '%s' as a store name; error: %A" i.Content.StoreName e))

            match i.IsDeleted with
            | true -> return Change.Delete storeId
            | false ->
                return
                    Change.Upsert
                        { StateStore.StoreId = storeId
                          StateStore.StoreName = storeName
                          StateStore.Etag = CoreTypes.Etag i.Etag |> Some }
        }

    let serializeNotSoldItem isDeleted (i: CoreTypes.NotSoldItem): DtoTypes.Document<DtoTypes.NotSoldItem> =
        { Id = i |> NotSoldItem.serialize // Could use Json implementation
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

    let serializePurchase isDeleted (i: CoreTypes.Purchase): DtoTypes.Document<DtoTypes.Purchase> =
        { Id = i |> Purchase.serialize // Could use Json implementation
          CustomerId = null
          DocumentKind = DtoTypes.DocumentKind.Purchase
          Etag = null
          IsDeleted = isDeleted
          Timestamp = Nullable<int>()
          Content = () }

    let deserializePurchase (i: DtoTypes.Document<DtoTypes.Purchase>) =
        result {
            let! id = i.Id |> Purchase.deserialize

            match i.IsDeleted with
            | true -> return id |> Change.Delete
            | false -> return id |> Change.Upsert
        }

    let withCustomerId id (i: DtoTypes.Document<_>) = { i with CustomerId = id }

    let hasChanges (changes: DtoTypes.Changes) =
        changes.Items.Length > 0
        || changes.Categories.Length > 0
        || changes.Stores.Length > 0
        || changes.NotSoldItems.Length > 0
        || changes.Purchases.Length > 0

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

        let changes =
            { DtoTypes.Changes.Items = collect State.itemsTable serializeItem
              DtoTypes.Changes.Categories = collect State.categoriesTable serializeCategory
              DtoTypes.Changes.Stores = collect State.storesTable serializeStore
              DtoTypes.Changes.NotSoldItems = collect State.notSoldTable serializeNotSoldItem
              DtoTypes.Changes.Purchases = collect State.purchasesTable serializePurchase }

        match changes |> hasChanges with
        | true -> Some changes
        | false -> None

    // maybe should take what works, not throw
    let private deserialize<'T, 'U> (f: DtoTypes.Document<'T> -> Result<'U, string>) (i: DtoTypes.Document<'T> seq) =
        i
        |> Seq.map f
        |> Result.fromResults
        |> Result.okOrThrow

    let private timestamp (d: DtoTypes.Document<_>) = d.Timestamp |> Option.ofNullable

    let private timestamps (c: DtoTypes.Changes) =
        [ c.Items |> Seq.map timestamp
          c.Purchases |> Seq.map timestamp
          c.Categories |> Seq.map timestamp
          c.NotSoldItems |> Seq.map timestamp
          c.Stores |> Seq.map timestamp ]
        |> Seq.concat

    let changesAsImport (c: DtoTypes.Changes) =
        match c |> hasChanges with
        | false -> None
        | true ->
            let import =
                let timestamps = c |> timestamps |> Seq.choose id

                { StateTypes.ImportChanges.ItemChanges = deserialize<_, _> deserializeItem c.Items
                  StateTypes.ImportChanges.CategoryChanges = deserialize<_, _> deserializeCategory c.Categories
                  StateTypes.ImportChanges.StoreChanges = deserialize<_, _> deserializeStore c.Stores
                  StateTypes.ImportChanges.NotSoldItemChanges = deserialize<_, _> deserializeNotSoldItem c.NotSoldItems
                  StateTypes.ImportChanges.PurchaseChanges = deserialize<_, _> deserializePurchase c.Purchases
                  StateTypes.ImportChanges.LatestTimestamp =
                      timestamps
                      |> Seq.fold
                          (fun m i ->
                              m
                              |> Option.map (fun m -> max m i)
                              |> Option.orElse (Some i))
                          None
                  StateTypes.ImportChanges.EarliestTimestamp =
                      timestamps
                      |> Seq.fold
                          (fun m i ->
                              m
                              |> Option.map (fun m -> min m i)
                              |> Option.orElse (Some i))
                          None

                }

            Some import
