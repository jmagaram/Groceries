namespace Models

open System
open SynchronizationTypes

module Dto =

    let fromItem (userId: string) (i: StateTypes.Item) =
        let r = DtoTypes.Item()
        r.DocumentKind <- DtoTypes.DocumentKind.Item
        r.Etag <- i.Etag |> Option.map (Etag.tag) |> Option.defaultValue null
        r.UserId <- userId
        r.ItemId <- i.ItemId |> Id.itemIdToGuid
        r.ItemName <- i.ItemName |> ItemName.asText
        r.Note <- i.Note |> Option.map Note.asText |> Option.defaultValue ""

        r.Quantity <-
            i.Quantity
            |> Option.map Quantity.asText
            |> Option.defaultValue ""

        r.CategoryId <-
            i.CategoryId
            |> Option.map Id.categoryIdToGuid
            |> Option.toNullable

        match i.Schedule with
        | StateTypes.Schedule.Once -> r.ScheduleKind <- DtoTypes.ScheduleKind.Once
        | StateTypes.Schedule.Completed -> r.ScheduleKind <- DtoTypes.ScheduleKind.Completed
        | StateTypes.Schedule.Repeat rpt ->
            r.ScheduleKind <- DtoTypes.ScheduleKind.Repeat

            r.Repeat <-
                let frequency = rpt.Frequency |> Frequency.days |> int
                let postponedUntil = rpt.PostponedUntil |> Option.toNullable
                DtoTypes.Repeat(Frequency = frequency, PostponedUntil = postponedUntil)

        r

    let toItem (i: DtoTypes.Item) =
        let itemId = StateTypes.ItemId i.ItemId

        match i.IsDeleted with
        | true -> Change.Delete itemId
        | false ->
            Upsert
                { StateTypes.Item.ItemId = itemId
                  StateTypes.Item.ItemName = i.ItemName |> ItemName.tryParse |> Result.okOrThrow
                  StateTypes.Item.CategoryId =
                      i.CategoryId
                      |> Option.ofNullable
                      |> Option.map StateTypes.CategoryId
                  StateTypes.Item.Note = i.Note |> Note.tryParseOptional |> Result.okOrThrow
                  StateTypes.Item.Etag = StateTypes.Etag i.Etag |> Some
                  StateTypes.Item.Quantity = i.Quantity |> Quantity.tryParseOptional |> Result.okOrThrow
                  StateTypes.Item.Schedule =
                      match i.ScheduleKind with
                      | DtoTypes.ScheduleKind.Completed -> StateTypes.Schedule.Completed
                      | DtoTypes.ScheduleKind.Once -> StateTypes.Schedule.Once
                      | DtoTypes.ScheduleKind.Repeat ->
                          StateTypes.Repeat
                              { StateTypes.Repeat.Frequency =
                                    match i.Repeat with
                                    | null -> failwith "Should not be null."
                                    | r ->
                                        (r.Frequency * 1<StateTypes.days>)
                                        |> Frequency.create
                                        |> Result.okOrThrow
                                StateTypes.Repeat.PostponedUntil = i.Repeat.PostponedUntil |> Option.ofNullable }
                      | _ -> failwith "Unexpected schedule kind." }

    let fromCategory (userId: string) (c: StateTypes.Category) =
        let r = DtoTypes.Category()
        r.Etag <- c.Etag |> Option.map (Etag.tag) |> Option.defaultValue null
        r.DocumentKind <- DtoTypes.DocumentKind.Category
        r.UserId <- userId
        r.CategoryId <- c.CategoryId |> Id.categoryIdToGuid
        r.CategoryName <- c.CategoryName |> CategoryName.asText
        r

    let toCategory (c: DtoTypes.Category) =
        let id = StateTypes.CategoryId c.CategoryId

        match c.IsDeleted with
        | true -> Change.Delete id
        | false ->
            Change.Upsert
                { StateTypes.Category.CategoryId = id
                  StateTypes.Category.CategoryName = c.CategoryName |> CategoryName.tryParse |> Result.okOrThrow
                  StateTypes.Category.Etag = StateTypes.Etag c.Etag |> Some }

    let fromStore (userId: string) (c: StateTypes.Store) =
        let r = DtoTypes.Store()
        r.DocumentKind <- DtoTypes.DocumentKind.Store
        r.Etag <- c.Etag |> Option.map (Etag.tag) |> Option.defaultValue null
        r.UserId <- userId
        r.StoreId <- c.StoreId |> Id.storeIdToGuid
        r.StoreName <- c.StoreName |> StoreName.asText
        r

    let toStore (c: DtoTypes.Store) =
        let id = StateTypes.StoreId c.StoreId

        match c.IsDeleted with
        | true -> Change.Delete id
        | false ->
            Change.Upsert
                { StateTypes.Store.StoreId = id
                  StateTypes.Store.StoreName = c.StoreName |> StoreName.tryParse |> Result.okOrThrow
                  StateTypes.Store.Etag = StateTypes.Etag c.Etag |> Some }

    let fromNotSoldItem (userId: string) (c: StateTypes.NotSoldItem) =
        let r = DtoTypes.NotSoldItem()
        r.DocumentKind <- DtoTypes.DocumentKind.NotSoldItem
        r.UserId <- userId
        r.ItemId <- c.ItemId |> Id.itemIdToGuid
        r.StoreId <- c.StoreId |> Id.storeIdToGuid
        r

    let toNotSoldItem (i: DtoTypes.NotSoldItem) =
        let id =
            { StateTypes.NotSoldItem.StoreId = StateTypes.StoreId i.StoreId
              StateTypes.NotSoldItem.ItemId = StateTypes.ItemId i.ItemId }

        match i.IsDeleted with
        | true -> Change.Delete id
        | false -> Change.Upsert id

    let private changes<'T, 'TKey, 'U when 'U :> DtoTypes.GroceryDocument and 'TKey: comparison> (table: SynchronizationTypes.DataTable<'TKey, 'T>)
                                                                                                 (createDto: 'T -> 'U)
                                                                                                 =
        let mapItems isDeleted source =
            source
            |> Seq.map createDto
            |> Seq.map (fun i -> 
                i.IsDeleted <- isDeleted
                i)

        let toDelete = table |> DataTable.isDeleted |> mapItems true
        let toUpsert = table |> DataTable.isAddedOrModified |> mapItems false

        System.Collections.Generic.List<_>(Seq.append toDelete toUpsert)

    let pushChanges userId (s: Models.StateTypes.State) =
        let c = DtoTypes.Changes()

        c.Items <- changes (s |> State.itemsTable) (fromItem userId)
        c.Categories <- changes (s |> State.categoriesTable) (fromCategory userId)
        c.Stores <- changes (s |> State.storesTable) (fromStore userId)
        c.NotSoldItems <- changes (s |> State.notSoldItemsTable) (fromNotSoldItem userId)
        c

    let pull items categories stores notSoldItems =
        { DtoTypes.Pull.Items = items |> Seq.map toItem |> List.ofSeq
          DtoTypes.Pull.Categories = categories |> Seq.map toCategory |> List.ofSeq
          DtoTypes.Pull.Stores = stores |> Seq.map toStore |> List.ofSeq
          DtoTypes.Pull.NotSoldItems = notSoldItems |> Seq.map toNotSoldItem |> List.ofSeq
        }

    let processPull (p:DtoTypes.Pull) (s:StateTypes.State) =
        s 
        |> State.mapItems (SynchronizeState.merge p.Items)
        |> State.mapCategories (SynchronizeState.merge p.Categories)
        |> State.mapStores (SynchronizeState.merge p.Stores)
        |> State.mapNotSoldItems (SynchronizeState.merge p.NotSoldItems)
        // then fix broken foreign keys