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

    let serializeFamily (i: CoreTypes.Family): DtoTypes.Document<DtoTypes.Family> =
        { CustomerId = i.FamilyId |> FamilyId.serialize
          Id = i.FamilyId |> FamilyId.serialize
          DocumentKind = DtoTypes.DocumentKind.Family
          Etag =
              i.Etag
              |> Option.map (Etag.tag)
              |> Option.defaultValue null
          IsDeleted = false
          Timestamp = Nullable<int>()
          Content =
              { FamilyName = i.FamilyName |> FamilyName.asText
                MemberEmails =
                    i.Members
                    |> Seq.map EmailAddress.asText
                    |> Array.ofSeq } }

    let deserializeFamily (i: DtoTypes.Document<DtoTypes.Family>) =
        result {
            let! verifyConsistentId =
                match i.Id = i.CustomerId with
                | true -> Ok()
                | false -> Error "The document ID does not match the family ID."

            let! verifyDocumentKind =
                match i.DocumentKind with
                | DtoTypes.DocumentKind.Family -> Ok()
                | _ -> Error "The document kind was supposed to indicate a family but is incorrect."

            let! familyName =
                i.Content.FamilyName
                |> FamilyName.tryParse
                |> Result.mapError (fun e -> $"Could not deserialize family name {e}")

            let! emails =
                i.Content.MemberEmails
                |> Seq.map EmailAddress.tryParse
                |> Seq.onlySome
                |> Option.map List.distinct
                |> Option.asResult "Could not deserialize the member list."

            let! verifyAtLeastOneMember =
                match emails with
                | [] -> Error "There must be at least one member of the family."
                | _ -> Ok()

            let! familyId =
                i.CustomerId
                |> FamilyId.deserialize
                |> Option.asResult $"Could not deserialize the family ID: {i.CustomerId}"

            let etag = CoreTypes.Etag i.Etag |> Some

            return
                { CoreTypes.FamilyId = familyId
                  CoreTypes.Family.FamilyName = familyName
                  CoreTypes.Family.Etag = etag
                  CoreTypes.Family.Members = emails }
        }

    let isMember userEmail (i: DtoTypes.Document<DtoTypes.Family>) =
        i.Content.MemberEmails
        |> Array.exists
            (fun j ->
                userEmail
                |> String.equalsInvariantCultureIgnoreCase j)

    let removeMember userEmail (i: DtoTypes.Document<DtoTypes.Family>) =
        { i with
              Content =
                  { i.Content with
                        MemberEmails =
                            i.Content.MemberEmails
                            |> Array.filter
                                (fun j ->
                                    userEmail
                                    |> String.equalsInvariantCultureIgnoreCase j
                                    |> not) } }

    let private affixFamilyIdToDocument id (i: DtoTypes.Document<_>) =
        if i.CustomerId = id then
            i
        else
            { i with CustomerId = id }

    //let clearEtags (i:DtoTypes.Changes) =
    //    { i with
    //        Items = i.Items |> Array.map (fun j -> { j with Etag = "" })
    //        Categories = i.Categories |> Array.map (fun j -> { j with Etag = "" })
    //        Purchases = i.Purchases |> Array.map (fun j -> { j with Etag = "" })
    //        Stores = i.Stores |> Array.map (fun j -> { j with Etag = "" })
    //        NotSoldItems = i.NotSoldItems |> Array.map (fun j -> { j with Etag = "" })
    //    }

    //let replaceAllIds (i:DtoTypes.Changes) =
    //    let allIds = 
    //        i.Items |> Seq.map (fun j -> j.Id)
    //        |> Seq.append (i.Categories |> Seq.map (fun j -> j.Id))
    //        |> Seq.append (i.Purchases |> Seq.map (fun j->j.Id))
    //        |> Seq.append (i.Stores |> Seq.map (fun j->j.Id ))
    //        |> Seq.append (i.Purchases |> Seq.map (fun j->j.)

    let affixFamilyId familyId (i: DtoTypes.Changes) =
        { i with
              Items =
                  i.Items
                  |> Array.map (affixFamilyIdToDocument familyId)
              Categories =
                  i.Categories
                  |> Array.map (affixFamilyIdToDocument familyId)
              Purchases =
                  i.Purchases
                  |> Array.map (affixFamilyIdToDocument familyId)
              Stores =
                  i.Stores
                  |> Array.map (affixFamilyIdToDocument familyId)
              NotSoldItems =
                  i.NotSoldItems
                  |> Array.map (affixFamilyIdToDocument familyId) }

    let emptyChanges =
        { DtoTypes.Changes.Items = [||]
          DtoTypes.Changes.Categories = [||]
          DtoTypes.Changes.Purchases = [||]
          DtoTypes.Changes.Stores = [||]
          DtoTypes.Changes.NotSoldItems = [||] }

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
                let timestamps = c |> timestamps

                { StateTypes.ImportChanges.ItemChanges = deserialize<_, _> deserializeItem c.Items
                  StateTypes.ImportChanges.CategoryChanges = deserialize<_, _> deserializeCategory c.Categories
                  StateTypes.ImportChanges.StoreChanges = deserialize<_, _> deserializeStore c.Stores
                  StateTypes.ImportChanges.NotSoldItemChanges = deserialize<_, _> deserializeNotSoldItem c.NotSoldItems
                  StateTypes.ImportChanges.PurchaseChanges = deserialize<_, _> deserializePurchase c.Purchases
                  StateTypes.ImportChanges.LatestTimestamp = timestamps |> Seq.max
                  StateTypes.ImportChanges.EarliestTimestamp = timestamps |> Seq.min }

            Some import
