namespace Models

open System
open ValidationTypes
open StringValidation
open StateTypes

[<AutoOpen>]
module private StateUtilities =

    let parser tag rules =
        let validator = rules |> createValidator

        fun s ->
            let s = s |> String.trim |> normalizeLineFeed

            s
            |> validator
            |> List.ofSeq
            |> fun errors ->
                match errors with
                | [] -> s |> tag |> Ok
                | _ -> Error errors

module Id =

    let create tag = newGuid () |> tag

module ItemName =

    let rules = singleLine 3<chars> 50<chars>
    let tryParse = parser ItemName rules

module Note =

    let rules = multipleLine 3<chars> 200<chars>
    let tryParse = parser Note rules

module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let tryParse = parser Quantity rules

module CategoryName =

    let rules = singleLine 3<chars> 30<chars>
    let tryParse = parser CategoryName rules

module StoreName =

    let rules = singleLine 3<chars> 30<chars>
    let tryParse = parser StoreName rules

module Repeat =

    let rules = { Min = 1<days>; Max = 365<days> }

    let create interval postponedUntil =
        let validator = RangeValidation.createValidator rules

        match interval |> validator with
        | None ->
            { Interval = interval
              PostponedUntil = postponedUntil }
            |> Ok
        | Some e -> e |> Error

module State =

    let private editItems f s = { s with Items = f s.Items }

    let private editCategories f s = { s with Categories = f s.Categories }

    let private editStores f s = { s with Stores = f s.Stores }

    let private editNotSoldItems f s =
        { s with
              NotSoldItems = f s.NotSoldItems }

    let empty =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NotSoldItems = DataTable.empty }

    let addSampleData (s: State) =
        let addCat n s =
            n
            |> CategoryName.tryParse
            |> Result.okOrThrow
            |> fun n ->
                { CategoryId = Id.create CategoryId
                  Name = n }
            |> fun c ->
                { s with
                      Categories = s.Categories |> DataTable.insert c }

        let addStore n (s: State) =
            n
            |> StoreName.tryParse
            |> Result.okOrThrow
            |> fun n ->
                { StoreId = Id.create StoreId
                  Name = n }
            |> fun n ->
                { s with
                      Stores = s.Stores |> DataTable.insert n }

        let addItem name category qty note (s: State) =
            let categoryId =
                s.Categories
                |> DataTable.current
                |> Seq.tryPick (fun i -> if i.Name = CategoryName category then Some i.CategoryId else None)

            let qty =
                qty |> Quantity.tryParse |> Result.asOption

            let note = note |> Note.tryParse |> Result.asOption

            let item =
                name
                |> ItemName.tryParse
                |> Result.okOrThrow
                |> fun n ->
                    { ItemId = Id.create ItemId
                      Name = n
                      CategoryId = categoryId
                      Quantity = qty
                      Note = note
                      Schedule = Once }

            { s with
                  Items = s.Items |> DataTable.insert item }

        let markComplete itemName (s: State) =
            let name =
                ItemName.tryParse itemName |> Result.okOrThrow

            let item =
                s.Items
                |> DataTable.current
                |> Seq.find (fun i -> i.Name = name)
                |> fun i -> { i with Schedule = Completed }

            { s with
                  Items = s.Items |> DataTable.update item }

        let makeRepeat itemName interval postpone (s: State) =
            let name =
                ItemName.tryParse itemName |> Result.okOrThrow

            let postpone =
                postpone
                |> Option.map (fun d -> DateTimeOffset.Now.AddDays(d |> float))

            let schedule =
                Repeat.create interval postpone
                |> Result.okOrThrow
                |> Schedule.Repeat

            let item =
                s.Items
                |> DataTable.current
                |> Seq.find (fun i -> i.Name = name)
                |> fun i -> { i with Schedule = schedule }

            { s with
                  Items = s.Items |> DataTable.update item }

        let notSoldAt itemName storeName (s: State) =
            let itemName =
                ItemName.tryParse itemName |> Result.okOrThrow

            let storeName =
                StoreName.tryParse storeName |> Result.okOrThrow

            let storeId =
                s.Stores
                |> DataTable.current
                |> Seq.pick (fun i -> if i.Name = storeName then Some i.StoreId else None)

            let itemId =
                s.Items
                |> DataTable.current
                |> Seq.pick (fun i -> if i.Name = itemName then Some i.ItemId else None)

            let nsa =
                { NotSold.StoreId = storeId
                  ItemId = itemId }

            { s with
                  NotSoldItems = s.NotSoldItems |> DataTable.upsert nsa }

        s
        |> addCat "Produce"
        |> addCat "Dairy"
        |> addCat "Dry"
        |> addCat "Frozen"
        |> addItem "Bananas" "Produce" "1 bunch" ""
        |> addItem "Apples" "Produce" "6 large" ""
        |> addItem "Chocolate bars" "Dry" "Assorted; many" "Prefer Eco brand"
        |> addItem "Peanut butter" "Dry" "Several jars" "Like Santa Cruz brand"
        |> addItem "Nancy's lowfat yogurt" "Dairy" "1 tub" "Check the date"
        |> addItem "Ice cream" "Frozen" "2 pints" ""
        |> addItem "Dried flax seeds" "Dry" "1 bag" ""
        |> makeRepeat "Bananas" 7<days> None
        |> makeRepeat "Peanut butter" 14<days> (Some 3<days>)
        |> markComplete "Ice cream"
        |> notSoldAt "Dried flax seeds" "QFC"
        |> notSoldAt "Dried flax seeds" "Whole Foods"
        |> addStore "QFC"
        |> addStore "Whole Foods"
        |> addStore "Trader Joe's"

    let removeCategoryFromItem categoryId (i: Item) =
        match i.CategoryId with
        | None -> i
        | Some c -> if c = categoryId then { i with CategoryId = None } else i

    let deleteCategory id (s: State) =
        s
        |> editCategories (DataTable.deleteIf (fun x -> x.CategoryId = id))
        |> editItems (DataTable.mapCurrent (removeCategoryFromItem id))

    let update msg s =
        match msg with
        | DeleteCategory id -> s |> deleteCategory id

//let a (s:State) id =
//    let m = s.Items |> DataTable.current
//    let toChange = m |> Seq.filter (fun i -> i.CategoryId = id)
//    let withChange = toChange |> Seq.map (fun i -> { i with CategoryId = None })
// update Seq.map

//type State =
//    { Categories: DataTable<CategoryId, Category>
//      Stores: DataTable<StoreId, Store>
//      Items: DataTable<ItemId, Item>
//      NeverSells: DataTable<NeverSell, NeverSell> }
