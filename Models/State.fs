namespace Models

open System
open System.Text.RegularExpressions
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
    let validator = rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator ItemName List.head

    let asText (ItemName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Note =

    let rules = multipleLine 3<chars> 200<chars>
    let normalizer = String.trim
    let validator = rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Note List.head

    let tryParseOptional s =
        if s |> String.isNullOrWhiteSpace then Ok None else s |> tryParse |> Result.map Some

    let asText (Note s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let normalizer = String.trim
    let validator = rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Quantity List.head

    let tryParseOptional s =
        if s |> String.isNullOrWhiteSpace then Ok None else s |> tryParse |> Result.map Some

    let asText (Quantity s) = s

    type private KnownUnit = { OneOf: string; ManyOf: string }

    let private knownUnits =
        [ { OneOf = "jar"; ManyOf = "jars" }
          { OneOf = "can"; ManyOf = "cans" }
          { OneOf = "ounce"; ManyOf = "ounces" }
          { OneOf = "pound"; ManyOf = "pounds" }
          { OneOf = "gram"; ManyOf = "grams" }
          { OneOf = "head"; ManyOf = "heads" }
          { OneOf = "bunch"; ManyOf = "bunches" }
          { OneOf = "pack"; ManyOf = "packs" }
          { OneOf = "bag"; ManyOf = "bags" }
          { OneOf = "package"; ManyOf = "packages" }
          { OneOf = "box"; ManyOf = "boxes" }
          { OneOf = "pint"; ManyOf = "pints" }
          { OneOf = "gallon"; ManyOf = "gallons" }
          { OneOf = "container"; ManyOf = "containers" } ]

    let private manyOf u =
        knownUnits
        |> Seq.where (fun i -> i.OneOf = u || i.ManyOf = u)
        |> Seq.map (fun i -> i.ManyOf)
        |> Seq.tryHead
        |> Option.defaultValue u

    let private oneOf u =
        knownUnits
        |> Seq.where (fun i -> i.OneOf = u || i.ManyOf = u)
        |> Seq.map (fun i -> i.OneOf)
        |> Seq.tryHead
        |> Option.defaultValue u

    let private grammar = new Regex("^\s*(\d+)\s*(.*)", RegexOptions.Compiled)

    type private ParsedQuantity = { Quantity: int; Units: string }

    let private format q =
        match q with
        | { Units = "" } -> sprintf "%i" q.Quantity
        | _ -> sprintf "%i %s" q.Quantity q.Units

    let private parse s =
        let m = grammar.Match(s)

        match m.Success with
        | false -> None
        | true ->
            let qty = System.Int32.Parse(m.Groups.[1].Value)
            let units = m.Groups.[2].Value
            Some { Quantity = qty; Units = units }

    let increase qty =
        match isNullOrWhiteSpace qty with
        | true -> Some "2"
        | false ->
            match parse qty with
            | None -> None
            | Some i ->
                let qty = i.Quantity + 1
                let result = { Quantity = qty; Units = i.Units |> manyOf } |> format
                Some result

    let decrease qty =
        match parse qty with
        | None -> None
        | Some i ->
            match i.Quantity with
            | x when x <= 1 -> None
            | _ ->
                let qty = i.Quantity - 1

                let result =
                    { Quantity = qty
                      Units =
                          match qty with
                          | 1 -> i.Units |> oneOf
                          | _ -> i.Units |> manyOf }
                    |> format

                Some result

    let increaseQty qty = qty |> asText |> increase |> Option.map Quantity // not good logic; Quantity.create makes a Result

    let decreaseQty qty = qty |> asText |> decrease |> Option.map Quantity

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CategoryName =

    let rules = singleLine 3<chars> 30<chars>
    let normalizer = String.trim
    let validator = rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator CategoryName List.head

    let asText (CategoryName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreName =

    let rules = singleLine 3<chars> 30<chars>
    let normalizer = String.trim
    let validator = rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator StoreName List.head

    let asText (StoreName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Frequency =

    let rules = { Min = 1<days>; Max = 365<days> }

    let create =
        let normalizer = id
        let validator = RangeValidation.createValidator rules
        let onSuccess = Frequency
        let onFailure = id
        RangeValidation.toResult normalizer validator onSuccess onFailure

    let goodDefault = 7<days> |> create |> Result.okOrThrow

    let common =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days> |> create |> Result.okOrThrow)

    let days (Frequency v) = v

    let fromNow (now: DateTimeOffset) f = now.AddDays(f |> days |> float)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Repeat =

    let commonPostponeDays =
        [ 1; 3; 7; 14; 30; 60; 90 ]
        |> List.map (fun i -> i * 1<days>)

    let create frequency postponedUntil = { Frequency = frequency; PostponedUntil = postponedUntil }

    let due (now: DateTimeOffset) r =
        r.PostponedUntil
        |> Option.map (fun future ->
            let duration = future - now

            round (duration.TotalDays) |> int |> (*) 1<StateTypes.days>)

    let completeOne (now: DateTimeOffset) r =
        { r with
              PostponedUntil = r.Frequency |> Frequency.fromNow now |> Some }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let markComplete (now: DateTimeOffset) (i: Item) =
        match i.Schedule with
        | Completed -> i
        | Once -> { i with Schedule = Completed }
        | Repeat r -> { i with Schedule = r |> Repeat.completeOne now |> Repeat }

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

        let makeRepeat n freq postpone (s: State) =
            let freq = Frequency.create freq |> Result.okOrThrow
            let postpone = postpone |> Option.map (fun d -> now.AddDays(d |> float))
            let repeat = Repeat.create freq postpone

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

    let submitStoreForm msg s =
        match msg with
        | StoreFormMessage.InsertStore n ->
            s
            |> insertStore { StoreId = Id.create StoreId; StoreName = n }
        | StoreFormMessage.UpdateStore i -> s |> updateStore i

    let submitCategoryForm msg s =
        match msg with
        | CategoryFormMessage.InsertCategory n ->
            s
            |> insertCategory { CategoryId = Id.create CategoryId; CategoryName = n }
        | CategoryFormMessage.UpdateCategory i -> s |> updateCategory i

    let markItemComplete now id s =
        let item =
            s
            |> itemsTable
            |> DataTable.findCurrent id
            |> Item.markComplete now

        s |> mapItems (DataTable.update item)

    let rec update msg s =
        match msg with
        | SubmitStoreForm msg -> s |> submitStoreForm msg
        | SubmitCategoryForm msg -> s |> submitCategoryForm msg
        | ItemMessage msg ->
            match msg with
            | MarkComplete i -> s |> markItemComplete DateTimeOffset.Now i
            | InsertItem i -> s |> insertItem i
            | UpdateItem i -> s |> updateItem i
            | UpsertItem i -> s |> upsertItem i
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
