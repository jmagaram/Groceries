namespace Models

module Dto =

    let fromItem (userId: string) (i: StateTypes.Item) =
        let r = DtoTypes.Item()
        r.UserId <- userId

        r.ItemId <-
            match i.ItemId with
            | StateTypes.ItemId id -> id.ToString()

        r.ItemName <- i.ItemName |> ItemName.asText
        r.Note <- i.Note |> Option.map Note.asText |> Option.defaultValue ""

        r.Quantity <-
            i.Quantity
            |> Option.map Quantity.asText
            |> Option.defaultValue ""

        r.CategoryId <-
            match i.CategoryId with
            | None -> null
            | Some (StateTypes.CategoryId c) -> c.ToString()

        r

    let fromCategory (userId: string) (c: StateTypes.Category) =
        let r = DtoTypes.Category()
        r.UserId <- userId

        r.CategoryId <-
            match c.CategoryId with
            | StateTypes.CategoryId id -> id.ToString()

        r.CategoryName <- c.CategoryName |> CategoryName.asText
        r

    let fromStore (userId: string) (c: StateTypes.Store) =
        let r = DtoTypes.Store()
        r.UserId <- userId

        r.StoreId <-
            match c.StoreId with
            | StateTypes.StoreId id -> id.ToString()

        r.StoreName <- c.StoreName |> StoreName.asText
        r

    let fromNotSoldItem (userId: string) (c: StateTypes.NotSoldItem) =
        let r = DtoTypes.NotSoldItem()
        r.UserId <- userId

        r.ItemId <-
            match c.ItemId with
            | StateTypes.ItemId id -> id.ToString()

        r.StoreId <-
            match c.StoreId with
            | StateTypes.StoreId id -> id.ToString()

        r

module CosmosExperiment =

    let items userId (s: StateTypes.State) = s |> State.items |> Seq.map (Dto.fromItem userId)

    let categories userId (s: StateTypes.State) = s |> State.categories |> Seq.map (Dto.fromCategory userId)

    let stores userId (s: StateTypes.State) = s |> State.stores |> Seq.map (Dto.fromStore userId)

    let notSoldItems userId (s: StateTypes.State) =
        s
        |> State.notSoldItems
        |> Seq.map (Dto.fromNotSoldItem userId)

