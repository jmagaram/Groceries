namespace Models

module Dto =

    let fromItem (userId: string) (i: StateTypes.Item) =
        let r = DtoTypes.Item()
        r.DocumentKind <- DtoTypes.DocumentKind.Item
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

    let fromCategory (userId: string) (c: StateTypes.Category) =
        let r = DtoTypes.Category()
        r.DocumentKind <- DtoTypes.DocumentKind.Category
        r.UserId <- userId
        r.CategoryId <- c.CategoryId |> Id.categoryIdToGuid
        r.CategoryName <- c.CategoryName |> CategoryName.asText
        r

    let fromStore (userId: string) (c: StateTypes.Store) =
        let r = DtoTypes.Store()
        r.DocumentKind <- DtoTypes.DocumentKind.Store
        r.UserId <- userId
        r.StoreId <- c.StoreId |> Id.storeIdToGuid
        r.StoreName <- c.StoreName |> StoreName.asText
        r

    let fromNotSoldItem (userId: string) (c: StateTypes.NotSoldItem) =
        let r = DtoTypes.NotSoldItem()
        r.DocumentKind <- DtoTypes.DocumentKind.NotSoldItem
        r.UserId <- userId
        r.ItemId <- c.ItemId |> Id.itemIdToGuid
        r.StoreId <- c.StoreId |> Id.storeIdToGuid

        r

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
