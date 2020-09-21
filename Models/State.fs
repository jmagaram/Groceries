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
    let tryParse = parser ItemName rules
    let asText (ItemName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Note =

    let rules = multipleLine 3<chars> 200<chars>
    let tryParse = parser Note rules
    let asText (Note s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Quantity =

    let rules = singleLine 1<chars> 30<chars>
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
module CategoryReference =

    let id c =
        match c with
        | NewCategory c -> c.CategoryId
        | ExistingCategory c -> c

    let newCategory c =
        match c with
        | NewCategory c -> Some c
        | ExistingCategory c -> None

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreReference =

    let id s =
        match s with
        | NewStore c -> c.StoreId
        | ExistingStore c -> c

    let newStore s =
        match s with
        | NewStore c -> Some c
        | ExistingStore c -> None

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let fromItemUpsert (i: ItemUpsert) =
        { Item.ItemId = i.ItemId
          ItemName = i.ItemName
          CategoryId = i.Category |> Option.map CategoryReference.id
          Note = i.Note
          Quantity = i.Quantity
          Schedule = i.Schedule }

module StateCore =

    let mapCategories f s = { s with Categories = f s.Categories }
    let mapStores f s = { s with Stores = f s.Stores }
    let mapNotSoldCategories f s = { s with NotSoldCategories = f s.NotSoldCategories }
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

    let insertNotSoldCategory (nsc: NotSoldCategory) s =
        let category = s.Categories |> DataTable.tryFindCurrent nsc.CategoryId
        let store = s.Stores |> DataTable.tryFindCurrent nsc.StoreId

        match category, store with
        | Some _, Some _ -> s |> mapNotSoldCategories (DataTable.insert nsc)
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
        |> mapNotSoldCategories (DataTable.deleteIf (fun i -> i.StoreId = k))
        |> mapSettings (DataRow.mapCurrent (Settings.clearStoreFilterIf k))

    let deleteNotSoldItem k s = s |> mapNotSoldItems (DataTable.delete k)

    let deleteNotSoldCategory k s = s |> mapNotSoldCategories (DataTable.delete k)

    let deleteItem k s = s |> mapItems (DataTable.delete k)

    let deleteCategory k s =
        s
        |> mapCategories (DataTable.delete k)
        |> mapNotSoldCategories (DataTable.deleteIf (fun i -> i.CategoryId = k))
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
          NotSoldCategories = DataTable.empty
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

        let doesNotSellCategory store category (s: State) =
            let ns =
                { NotSoldCategory.CategoryId = (findCategory category s).CategoryId
                  StoreId = (findStore store s).StoreId }

            s |> mapNotSoldCategories (DataTable.insert ns)

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
        |> doesNotSellCategory "Walgreens" "Produce"
        |> doesNotSellCategory "Walgreens" "Dairy"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module State =

    let items (s: State) = s.Items

    let categories (s: State) = s.Categories

    let stores (s: State) = s.Stores

    let notSoldItems (s: State) = s.NotSoldItems

    let notSoldCategories (s: State) = s.NotSoldCategories

    let shoppingListViewOptions (s: State) = s.Settings

    let private editItems f s = { s with Items = f s.Items }

    let private editCategories f s = { s with Categories = f s.Categories }

    let private editStores f s = { s with Stores = f s.Stores }

    let private editNotSoldItems f s = { s with NotSoldItems = f s.NotSoldItems }

    let private editNotSoldCategories f s =
        s
        |> notSoldCategories
        |> f
        |> fun nsc -> { s with NotSoldCategories = nsc }

    let createDefault =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NotSoldItems = DataTable.empty
          NotSoldCategories = DataTable.empty
          Settings = DataRow.unchanged Settings.create }

    let private deleteCategory id (s: State) =
        s
        |> editCategories (DataTable.deleteIf (fun x -> x.CategoryId = id))
        |> editItems (fun dt ->
            dt
            |> DataTable.current
            |> Seq.choose (fun i -> if i.CategoryId = Some id then Some { i with CategoryId = None } else None)
            |> Seq.fold (fun dt i -> dt |> DataTable.update i) s.Items)

    let private insertCategory c (s: State) = s |> editCategories (DataTable.insert c)

    let private deleteItem id (s: State) =
        s
        |> editItems (DataTable.delete id)
        |> editNotSoldItems (DataTable.deleteIf (fun x -> x.ItemId = id))

    let private updateShoppingListViewOptions f (state: State) =
        let opt = state.Settings |> DataRow.mapCurrent f
        { state with Settings = opt }

    let private setShoppingListStoreFilterTo storeId (state: State) =
        state
        |> updateShoppingListViewOptions (fun i -> { i with StoreFilter = storeId })

    let private deleteStore id (s: State) =
        s
        |> editStores (DataTable.deleteIf (fun x -> x.StoreId = id))
        |> editNotSoldItems (DataTable.deleteIf (fun x -> x.StoreId = id))
        |> updateShoppingListViewOptions (fun v -> if v.StoreFilter = Some id then { v with StoreFilter = None } else v)

    let private insertStore store (s: State) = s |> editStores (DataTable.insert store)


    // PROBLEM!
    // if insert an item that points to a category AND that category does not exist,
    // it will silently strip the category and not generate an error

    let (insertItem, updateItem) =

        let go f (item: ItemUpsert) (s: State) =
            let itemToInsert = item |> Item.fromItemUpsert

            let items = s.Items |> f itemToInsert

            let categoryToInsert =
                item.Category
                |> Option.bind (fun c -> c |> CategoryReference.newCategory)

            let storesToInsert = item.NotSoldAt |> Seq.choose StoreReference.newStore

            let (nsToAdd, nsToRemove) =
                let proposed =
                    item.NotSoldAt
                    |> List.map (fun n ->
                        { NotSoldItem.StoreId = n |> StoreReference.id
                          ItemId = item.ItemId })

                let current =
                    s.NotSoldItems
                    |> DataTable.current
                    |> Seq.filter (fun i -> i.ItemId = item.ItemId)
                    |> List.ofSeq

                let nsToAdd = proposed |> Seq.except current
                let nsToRemove = current |> Seq.except proposed
                (nsToAdd, nsToRemove)

            let categories =
                categoryToInsert
                |> Option.map (fun c -> s.Categories |> DataTable.insert c)
                |> Option.defaultValue s.Categories

            let stores =
                storesToInsert
                |> Seq.fold (fun t i -> t |> DataTable.insert i) s.Stores

            let notSold =
                let ns = s.NotSoldItems
                let ns = nsToAdd |> Seq.fold (fun t i -> t |> DataTable.insert i) ns

                let ns =
                    nsToRemove
                    |> Seq.fold (fun t i -> t |> DataTable.delete i) ns

                ns

            { s with
                  Items = items
                  Categories = categories
                  Stores = stores
                  NotSoldItems = notSold }

        (go DataTable.insert, go DataTable.update)

    let createWithSampleData =
        let addCategory n =
            { CategoryId = Id.create CategoryId
              CategoryName = n |> CategoryName.tryParse |> Result.okOrThrow }
            |> insertCategory

        let addStore n =
            { StoreId = Id.create StoreId
              StoreName = n |> StoreName.tryParse |> Result.okOrThrow }
            |> insertStore

        let catReference n (s: State) =
            let catName = n |> CategoryName.tryParse |> Result.okOrThrow

            s.Categories
            |> DataTable.current
            |> Seq.tryFind (fun i -> i.CategoryName = catName)
            |> Option.map (fun c -> ExistingCategory c.CategoryId)
            |> Option.defaultValue
                (NewCategory
                    { CategoryId = Id.create CategoryId
                      CategoryName = catName })

        let addItem n cat qty note (s: State) =
            let item =
                { ItemUpsert.ItemId = Id.create ItemId
                  Category = if cat = "" then None else s |> catReference cat |> Some
                  ItemName = n |> ItemName.tryParse |> Result.okOrThrow
                  Note = note |> Note.tryParse |> Result.asOption
                  Quantity = qty |> Quantity.tryParse |> Result.asOption
                  NotSoldAt = []
                  Schedule = Schedule.Once }

            s |> insertItem item

        let findItem n (s: State) =
            let itemName = ItemName.tryParse n |> Result.okOrThrow

            s.Items
            |> DataTable.current
            |> Seq.filter (fun i -> i.ItemName = itemName)
            |> Seq.exactlyOne

        let setItemSchedule n sch (s: State) =
            let itemName = ItemName.tryParse n |> Result.okOrThrow

            let item =
                s.Items
                |> DataTable.current
                |> Seq.find (fun i -> i.ItemName = itemName)

            let item' = { item with Schedule = sch }
            { s with Items = s.Items |> DataTable.update item' }

        let markComplete n = setItemSchedule n Schedule.Completed

        let makeRepeat n interval postpone =
            let postpone =
                postpone
                |> Option.map (fun d -> DateTimeOffset.Now.AddDays(d |> float))

            let schedule =
                Repeat.create interval postpone
                |> Result.okOrThrow
                |> Schedule.Repeat

            setItemSchedule n schedule

        let findStore n (s: State) =
            let storeName = StoreName.tryParse n |> Result.okOrThrow

            s.Stores
            |> DataTable.current
            |> Seq.filter (fun i -> i.StoreName = storeName)
            |> Seq.exactlyOne

        let onlySoldAt itemName storeName (s: State) =
            let itemId = (s |> findItem itemName).ItemId
            let availableAtStoreId = (findStore storeName s).StoreId

            s.Stores
            |> DataTable.current
            |> Seq.filter (fun i -> i.StoreId <> availableAtStoreId)
            |> Seq.fold (fun state store ->
                let nsa = { NotSoldItem.StoreId = store.StoreId; ItemId = itemId }

                { state with
                      NotSoldItems = state.NotSoldItems |> DataTable.insert nsa }) s

        createDefault
        |> addItem "Bananas" "Produce" "1 bunch" ""
        |> addItem "Frozen mango chunks" "Frozen" "1 bag" ""
        |> addItem "Apples" "Produce" "6 large" ""
        |> addItem "Chocolate bars" "Dry" "Assorted; many" "Prefer Eco brand"
        |> addItem "Peanut butter" "Dry" "Several jars" "Like Santa Cruz brand"
        |> addItem "Nancy's lowfat yogurt" "Dairy" "1 tub" "Check the date"
        |> addItem "Ice cream" "Frozen" "2 pints" ""
        |> addItem "Dried flax seeds" "Dry" "1 bag" ""
        |> addCategory "Meat and Seafood"
        |> makeRepeat "Bananas" 7<days> None
        |> makeRepeat "Peanut butter" 14<days> (Some 3<days>)
        |> markComplete "Ice cream"
        |> addStore "QFC"
        |> addStore "Whole Foods"
        |> addStore "Trader Joe's"
        |> onlySoldAt "Dried flax seeds" "Trader Joe's"
        |> onlySoldAt "Frozen mango chunks" "Trader Joe's"


    let update msg s =
        match msg with
        | ItemMessage msg ->
            match msg with
            | DeleteItem i -> s |> deleteItem i
            | UpdateItem i -> s |> updateItem i
            | InsertItem i -> s |> insertItem i
        | StoreMessage msg ->
            match msg with
            | InsertStore i -> s |> insertStore i
            | DeleteStore i -> s |> deleteStore i
        | CategoryMessage msg ->
            match msg with
            | DeleteCategory i -> s |> deleteCategory i
            | InsertCategory i -> s |> insertCategory i
        | ShoppingListMessage msg ->
            match msg with
            | ClearStoreFilter -> s |> setShoppingListStoreFilterTo None
            | SetStoreFilterTo id -> s |> setShoppingListStoreFilterTo (Some id)
