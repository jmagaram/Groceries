namespace Models

module Dto =

    let fromItem (userId: string) (i: StateTypes.Item) =
        let r = DtoTypes.Item()
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

        r

    let fromCategory (userId: string) (c: StateTypes.Category) =
        let r = DtoTypes.Category()
        r.UserId <- userId
        r.CategoryId <- c.CategoryId |> Id.categoryIdToGuid
        r.CategoryName <- c.CategoryName |> CategoryName.asText
        r

    let fromStore (userId: string) (c: StateTypes.Store) =
        let r = DtoTypes.Store()
        r.UserId <- userId
        r.StoreId <- c.StoreId |> Id.storeIdToGuid
        r.StoreName <- c.StoreName |> StoreName.asText
        r

    let fromNotSoldItem (userId: string) (c: StateTypes.NotSoldItem) =
        let r = DtoTypes.NotSoldItem()
        r.UserId <- userId
        r.ItemId <- c.ItemId |> Id.itemIdToGuid
        r.StoreId <- c.StoreId |> Id.storeIdToGuid

        r

module CosmosExperiment =

    let items userId (s: StateTypes.State) = s |> State.items |> Seq.map (Dto.fromItem userId)

    let categories userId (s: StateTypes.State) = s |> State.categories |> Seq.map (Dto.fromCategory userId)

    let stores userId (s: StateTypes.State) = s |> State.stores |> Seq.map (Dto.fromStore userId)

    let notSoldItems userId (s: StateTypes.State) =
        s
        |> State.notSoldItems
        |> Seq.map (Dto.fromNotSoldItem userId)
