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
module ShoppingListViewOptions =

    let defaultView = { ShoppingListViewOptions.StoreFilter = None }

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

    let removeCategoryIfEqualTo (c: CategoryId) (i: Item) =
        match i.CategoryId with
        | Some c' when c = c' -> { i with CategoryId = None }
        | _ -> i

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module State =

    let items (s: State) = s.Items

    let categories (s: State) = s.Categories

    let stores (s: State) = s.Stores

    let notSoldItems (s: State) = s.NotSoldItems

    let shoppingListViewOptions (s: State) = s.ShoppingListViewOptions

    let private editItems f s = { s with Items = f s.Items }

    let private editCategories f s = { s with Categories = f s.Categories }

    let private editStores f s = { s with Stores = f s.Stores }

    let private editNotSoldItems f s = { s with NotSoldItems = f s.NotSoldItems }

    let createDefault =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NotSoldItems = DataTable.empty
          ShoppingListViewOptions = DataRow.unchanged ShoppingListViewOptions.defaultView }

    let createWithSampleData =
        let addCategory n s =
            let category =
                { CategoryId = Id.create CategoryId
                  CategoryName = n |> CategoryName.tryParse |> Result.okOrThrow }

            s |> editCategories (DataTable.insert category)

        let addStore n (s: State) =
            let store =
                { StoreId = Id.create StoreId
                  StoreName = n |> StoreName.tryParse |> Result.okOrThrow }

            s |> editStores (DataTable.insert store)

        let findItem n (s: State) =
            let itemName = ItemName.tryParse n |> Result.okOrThrow

            s.Items
            |> DataTable.current
            |> Seq.filter (fun i -> i.ItemName = itemName)
            |> Seq.exactlyOne

        let findCategory n (s: State) =
            s.Categories
            |> DataTable.current
            |> Seq.tryFind (fun i -> i.CategoryName = n)

        let addItem n cat qty note (s: State) =
            let (s, cat) =
                if cat = "" then
                    (s, None)
                else
                    let catName = cat |> CategoryName.tryParse |> Result.okOrThrow

                    match s |> findCategory catName with
                    | Some c -> (s, Some c)
                    | None ->
                        let c =
                            { CategoryId = Id.create CategoryId
                              CategoryName = catName }

                        let s = s |> editCategories (DataTable.insert c)
                        (s, Some c)

            let item =
                { Item.ItemName = n |> ItemName.tryParse |> Result.okOrThrow
                  ItemId = Id.create ItemId
                  CategoryId = cat |> Option.map (fun c -> c.CategoryId)
                  Quantity = qty |> Quantity.tryParse |> Result.asOption
                  Note = note |> Note.tryParse |> Result.asOption
                  Schedule = Once }

            s |> editItems (DataTable.insert item)

        let markComplete n (s: State) =
            let item =
                s
                |> findItem n
                |> fun i -> { i with Schedule = Schedule.Completed }

            { s with Items = s.Items |> DataTable.update item }

        let makeRepeat n interval postpone (s: State) =
            let postpone =
                postpone
                |> Option.map (fun d -> DateTimeOffset.Now.AddDays(d |> float))

            let schedule =
                Repeat.create interval postpone
                |> Result.okOrThrow
                |> Schedule.Repeat

            let item =
                s
                |> findItem n
                |> fun i -> { i with Schedule = schedule }

            { s with Items = s.Items |> DataTable.update item }

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
                let nsa = { StoreId = store.StoreId; ItemId = itemId }

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

    let private deleteCategory id (s: State) =
        s
        |> editCategories (DataTable.deleteIf (fun x -> x.CategoryId = id))
        |> editItems (DataTable.mapCurrent (Item.removeCategoryIfEqualTo id))

    let private insertCategory c (s:State) =
        s
        |> editCategories (DataTable.insert c)

    let private deleteItem id (s: State) =
        s
        |> editItems (DataTable.delete id)
        |> editNotSoldItems (DataTable.deleteIf (fun x -> x.ItemId = id))

    let private updateShoppingListViewOptions f (state: State) =
        let opt = state.ShoppingListViewOptions |> DataRow.mapCurrent f
        { state with ShoppingListViewOptions = opt }

    let private setShoppingListStoreFilterTo storeId (state: State) =
        state
        |> updateShoppingListViewOptions (fun i -> { i with StoreFilter = storeId })

    let private deleteStore id (s: State) =
        s
        |> editStores (DataTable.deleteIf (fun x -> x.StoreId = id))
        |> editNotSoldItems (DataTable.deleteIf (fun x -> x.StoreId = id))
        |> updateShoppingListViewOptions (fun v -> if v.StoreFilter = Some id then { v with StoreFilter = None } else v)

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

    let update msg s =
        match msg with
        | ItemMessage msg -> 
            match msg with
            | DeleteItem i -> s |> deleteItem i
            | UpdateItem i -> s |> updateItem i
            | InsertItem i -> s |> insertItem i
        | StoreMessage msg -> 
            match msg with 
            | DeleteStore i -> s |> deleteStore i
        | CategoryMessage msg ->
            match msg with
            | DeleteCategory i -> s |> deleteCategory i 
            | InsertCategory i -> s |> insertCategory i
        | ShoppingListMessage msg ->
            match msg with
            | ClearStoreFilter -> s |> setShoppingListStoreFilterTo None
            | SetStoreFilterTo id -> s |> setShoppingListStoreFilterTo (Some id)
