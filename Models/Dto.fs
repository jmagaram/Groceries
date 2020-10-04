namespace Models

open System

module Dto =

    let fromItem (userId: string) (i: StateTypes.Item) =
        let r = DtoTypes.Item()
        r.DocumentKind <- DtoTypes.DocumentKind.Item
        r._etag <- i.Etag |> Option.map (Etag.tag) |> Option.defaultValue null
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
        { StateTypes.Item.ItemId = StateTypes.ItemId i.ItemId
          StateTypes.Item.ItemName = i.ItemName |> ItemName.tryParse |> Result.okOrThrow
          StateTypes.Item.CategoryId =
              i.CategoryId
              |> Option.ofNullable
              |> Option.map StateTypes.CategoryId
          StateTypes.Item.Note = i.Note |> Note.tryParseOptional |> Result.okOrThrow
          StateTypes.Item.Etag = StateTypes.Etag i._etag |> Some
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
        r._etag <- c.Etag |> Option.map (Etag.tag) |> Option.defaultValue null 
        r.DocumentKind <- DtoTypes.DocumentKind.Category
        r.UserId <- userId
        r.CategoryId <- c.CategoryId |> Id.categoryIdToGuid
        r.CategoryName <- c.CategoryName |> CategoryName.asText
        r

    let toCategory (c: DtoTypes.Category) =
        { StateTypes.Category.CategoryId = StateTypes.CategoryId c.CategoryId
          StateTypes.Category.CategoryName = c.CategoryName |> CategoryName.tryParse |> Result.okOrThrow
          StateTypes.Category.Etag = StateTypes.Etag c._etag |> Some }

    let fromStore (userId: string) (c: StateTypes.Store) =
        let r = DtoTypes.Store()
        r.DocumentKind <- DtoTypes.DocumentKind.Store
        r._etag <- c.Etag |> Option.map (Etag.tag) |> Option.defaultValue null 
        r.UserId <- userId
        r.StoreId <- c.StoreId |> Id.storeIdToGuid
        r.StoreName <- c.StoreName |> StoreName.asText
        r

    let toStore (c: DtoTypes.Store) =
        { StateTypes.Store.StoreId = StateTypes.StoreId c.StoreId
          StateTypes.Store.StoreName = c.StoreName |> StoreName.tryParse |> Result.okOrThrow
          StateTypes.Store.Etag = StateTypes.Etag c._etag |> Some }

    let fromNotSoldItem (userId: string) (c: StateTypes.NotSoldItem) =
        let r = DtoTypes.NotSoldItem()
        r.DocumentKind <- DtoTypes.DocumentKind.NotSoldItem
        r.UserId <- userId
        r.ItemId <- c.ItemId |> Id.itemIdToGuid
        r.StoreId <- c.StoreId |> Id.storeIdToGuid
        r

    let toNotSoldItem (i: DtoTypes.NotSoldItem) =
        { StateTypes.NotSoldItem.StoreId = StateTypes.StoreId i.StoreId
          StateTypes.NotSoldItem.ItemId = StateTypes.ItemId i.ItemId }

    let private changes<'T, 'TKey, 'U when 'U :> DtoTypes.GroceryDocument and 'TKey: comparison> (table: SynchronizationTypes.DataTable<'TKey, 'T>)
                                                                                                 (createDto: 'T -> 'U)
                                                                                                 =

        let toDelete =
            table
            |> DataTable.isDeleted
            |> Seq.map createDto
            |> Seq.map (fun i ->
                i.IsDeleted <- true
                i)

        let toUpsert =
            table
            |> DataTable.isAddedOrModified
            |> Seq.map createDto
            |> Seq.map (fun i ->
                i.IsDeleted <- false
                i)

        System.Collections.Generic.List<_>(Seq.append toDelete toUpsert)


    let pushChanges userId (s: Models.StateTypes.State) =
        let c = DtoTypes.PushChanges()

        c.Items <- changes (s |> State.itemsTable) (fromItem userId)
        c.Categories <- changes (s |> State.categoriesTable) (fromCategory userId)
        c.Stores <- changes (s |> State.storesTable) (fromStore userId)
        c.NotSoldItems <- changes (s |> State.notSoldItemsTable) (fromNotSoldItem userId)
        c
