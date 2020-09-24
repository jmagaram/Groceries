namespace Models

open System
open ValidationTypes
open StringValidation
open StateTypes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Id =

    let create tag = newGuid () |> tag

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemName =

    let rules = singleLine 3<chars> 50<chars>
    let normalizer = String.trim
    let tryParse = parser ItemName rules
    let asText (ItemName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Note =

    let rules = multipleLine 3<chars> 200<chars>
    let normalizer = String.trim
    let tryParse = parser Note rules
    let asText (Note s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let normalizer = String.trim
    let tryParse = parser Quantity rules
    let asText (Quantity s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CategoryName =

    let rules = singleLine 3<chars> 30<chars>
    let tryParse = parser CategoryName rules
    let asText (CategoryName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreName =

    let rules = singleLine 3<chars> 30<chars>
    let tryParse = parser StoreName rules
    let asText (StoreName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Repeat =

    let rules = { Min = 1<days>; Max = 365<days> }

    let commonIntervals =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days>)

    let create interval postponedUntil =
        interval
        |> RangeValidation.createValidator rules
        |> Option.map Error
        |> Option.defaultValue (Ok { Interval = interval; PostponedUntil = postponedUntil })

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Settings =

    let create = { Settings.StoreFilter = None }

    let setStoreFilter f s = { s with StoreFilter = f }

    let clearStoreFilterIf k s =
        if s.StoreFilter = Some k then s |> setStoreFilter None else s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module State =

    let categoriesTable (s: State) = s.Categories
    let storesTable (s: State) = s.Stores
    let itemsTable (s: State) = s.Items
    let notSoldItemsTable (s: State) = s.NotSoldItems
    let settingsRow (s: State) = s.Settings

    let categories = categoriesTable >> DataTable.current
    let stores = storesTable >> DataTable.current
    let items = itemsTable >> DataTable.current
    let notSoldItems = notSoldItemsTable >> DataTable.current

    let settings =
        settingsRow
        >> DataRow.currentValue
        >> Option.defaultValue Settings.create

    let mapCategories f s = { s with Categories = f s.Categories }
    let mapStores f s = { s with Stores = f s.Stores }
    let mapItems f s = { s with Items = f s.Items }
    let mapNotSoldItems f s = { s with NotSoldItems = f s.NotSoldItems }
    let mapSettings f s = { s with Settings = f s.Settings }

    let (insertCategory, updateCategory, upsertCategory) =
        let go f (c: Category) s = s |> mapCategories (f c)
        (go DataTable.insert, go DataTable.update, go DataTable.upsert)

    let (insertStore, updateStore, upsertStore) =
        let go f (s: Store) = mapStores (f s)
        (go DataTable.insert, go DataTable.update, go DataTable.upsert)

    let (insertItem, updateItem, upsertItem) =
        let go f (i: Item) s =
            let isCategoryReferenceValid =
                i.CategoryId
                |> Option.map (fun c -> s.Categories |> DataTable.currentContainsKey c)
                |> Option.defaultValue true

            match isCategoryReferenceValid with
            | false -> failwith "A category is referenced that does not exist."
            | true -> s |> mapItems (f i)

        (go DataTable.insert, go DataTable.update, go DataTable.upsert)

    let insertNotSoldItem (nsi: NotSoldItem) s =
        let item = s.Items |> DataTable.tryFindCurrent nsi.ItemId
        let store = s.Stores |> DataTable.tryFindCurrent nsi.StoreId

        match item, store with
        | Some _, Some _ -> s |> mapNotSoldItems (DataTable.insert nsi)
        | None, _ -> failwith "A store is referenced that does not exist."
        | _, None -> failwith "An item is referenced that does not exist."

    let updateSettingsStoreFilter k s =
        let isStoreReferenceValid =
            k
            |> Option.map (fun k -> s.Stores |> DataTable.currentContainsKey k)
            |> Option.defaultValue true

        match isStoreReferenceValid with
        | false -> failwith "A store is referenced that does not exist."
        | true -> mapSettings (DataRow.mapCurrent (Settings.setStoreFilter k)) s

    let deleteStore k s =
        s
        |> mapStores (DataTable.delete k)
        |> mapNotSoldItems (DataTable.deleteIf (fun i -> i.StoreId = k))
        |> mapSettings (DataRow.mapCurrent (Settings.clearStoreFilterIf k))

    let deleteNotSoldItem k s = s |> mapNotSoldItems (DataTable.delete k)

    let deleteItem k s = s |> mapItems (DataTable.delete k)

    let deleteCategory k s =
        s
        |> mapCategories (DataTable.delete k)
        |> mapItems (fun dt ->
            dt
            |> DataTable.current
            |> Seq.choose (fun i -> if i.CategoryId = Some k then Some { i with CategoryId = None } else None)
            |> Seq.fold (fun dt i -> dt |> DataTable.update i) s.Items)

    let createDefault =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NotSoldItems = DataTable.empty
          Settings = DataRow.unchanged Settings.create }

    let createSampleData =

        let newCategory n s =
            s
            |> insertCategory
                { Category.CategoryId = Id.create CategoryId
                  CategoryName = n |> CategoryName.tryParse |> Result.okOrThrow }

        let newStore n s =
            s
            |> insertStore
                { Store.StoreId = Id.create StoreId
                  StoreName = n |> StoreName.tryParse |> Result.okOrThrow }

        let findCategory n (s: State) =
            let n = CategoryName.tryParse n |> Result.okOrThrow

            s.Categories
            |> DataTable.current
            |> Seq.find (fun i -> i.CategoryName = n)

        let findItem n (s: State) =
            let n = ItemName.tryParse n |> Result.okOrThrow

            s.Items
            |> DataTable.current
            |> Seq.find (fun i -> i.ItemName = n)

        let findStore n (s: State) =
            let n = StoreName.tryParse n |> Result.okOrThrow

            s.Stores
            |> DataTable.current
            |> Seq.find (fun i -> i.StoreName = n)

        let newItem name cat qty note s =
            s
            |> insertItem
                { Item.ItemId = Id.create ItemId
                  ItemName = name |> ItemName.tryParse |> Result.okOrThrow
                  Quantity = if qty = "" then None else qty |> Quantity.tryParse |> Result.okOrThrow |> Some
                  Note = if note = "" then None else note |> Note.tryParse |> Result.okOrThrow |> Some
                  Item.Schedule = Schedule.Once
                  Item.CategoryId = if cat = "" then None else Some (findCategory cat s).CategoryId }

        let now = System.DateTimeOffset.Now

        let markComplete n (s: State) =
            let item =
                s
                |> findItem n
                |> fun i -> { i with Schedule = Completed }

            s |> mapItems (DataTable.update item)

        let makeRepeat n days postpone (s: State) =
            let postpone = postpone |> Option.map (fun d -> now.AddDays(d |> float))
            let repeat = Repeat.create days postpone |> Result.okOrThrow

            let item =
                s
                |> findItem n
                |> fun i -> { i with Schedule = Repeat repeat }

            s |> mapItems (DataTable.update item)

        let doesNotSellItem store item (s: State) =
            let ns =
                { NotSoldItem.ItemId = (findItem item s).ItemId
                  StoreId = (findStore store s).StoreId }

            s |> mapNotSoldItems (DataTable.insert ns)

        createDefault
        |> newCategory "Meat and Seafood"
        |> newCategory "Dairy"
        |> newCategory "Frozen"
        |> newCategory "Produce"
        |> newCategory "Dry"
        |> newItem "Bananas" "Produce" "1 bunch" ""
        |> newItem "Frozen mango chunks" "Frozen" "1 bag" ""
        |> newItem "Apples" "Produce" "6 large" ""
        |> newItem "Chocolate bars" "Dry" "Assorted; many" "Prefer Eco brand"
        |> newItem "Peanut butter" "Dry" "Several jars" "Like Santa Cruz brand"
        |> newItem "Nancy's lowfat yogurt" "Dairy" "1 tub" "Check the date"
        |> newItem "Ice cream" "Frozen" "2 pints" ""
        |> newItem "Dried flax seeds" "Dry" "1 bag" ""
        |> makeRepeat "Bananas" 7<days> None
        |> makeRepeat "Peanut butter" 14<days> (Some 3<days>)
        |> markComplete "Ice cream"
        |> newStore "QFC"
        |> newStore "Whole Foods"
        |> newStore "Trader Joe's"
        |> newStore "Costco"
        |> newStore "Walgreens"
        |> doesNotSellItem "QFC" "Dried flax seeds"
        |> doesNotSellItem "Costco" "Chocolate bars"

    let rec update msg s =
        match msg with
        | ItemMessage msg ->
            match msg with
            | InsertItem i -> s |> insertItem i
            | UpdateItem i -> s |> updateItem i
            | DeleteItem k -> s |> deleteItem k
        | StoreMessage msg ->
            match msg with
            | InsertStore i -> s |> insertStore i
            | DeleteStore k -> s |> deleteStore k
        | CategoryMessage msg ->
            match msg with
            | InsertCategory i -> s |> insertCategory i
            | DeleteCategory k -> s |> deleteCategory k
        | NotSoldItemMessage msg ->
            match msg with
            | InsertNotSoldItem i -> s |> insertNotSoldItem i
            | DeleteNotSoldItem i -> s |> deleteNotSoldItem i
        | SettingsMessage msg ->
            match msg with
            | ClearStoreFilter -> s |> updateSettingsStoreFilter None
            | SetStoreFilterTo id -> s |> updateSettingsStoreFilter (Some id)
        | Transaction msgs -> msgs |> Seq.fold (fun t i -> t |> update i) s
