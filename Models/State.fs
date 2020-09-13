namespace Models

open System
open ValidationTypes
open StringValidation
open StateTypes

module Id =

    let create tag = newGuid () |> tag

module ItemName =

    let rules = singleLine 3<chars> 50<chars>
    let tryParse = parser ItemName rules
    let asText (ItemName s) = s

module Note =

    let rules = multipleLine 3<chars> 200<chars>
    let tryParse = parser Note rules
    let asText (Note s) = s

module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let tryParse = parser Quantity rules
    let asText (Quantity s) = s

module CategoryName =

    let rules = singleLine 3<chars> 30<chars>
    let tryParse = parser CategoryName rules
    let asText (CategoryName s) = s

module StoreName =

    let rules = singleLine 3<chars> 30<chars>
    let tryParse = parser StoreName rules
    let asText (StoreName s) = s

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

    let items (s:State) = s.Items
    let categories (s:State) = s.Categories

    let empty =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NotSoldItems = DataTable.empty }

    let addSampleData (s: State) =
        let addCategory n s =
            { s with
                  Categories =
                      s.Categories
                      |> DataTable.insert
                          { CategoryId = Id.create CategoryId
                            CategoryName = n |> CategoryName.tryParse |> Result.okOrThrow } }

        let addStore n (s: State) =
            { s with
                  Stores =
                      s.Stores
                      |> DataTable.insert
                          { StoreId = Id.create StoreId
                            StoreName = n |> StoreName.tryParse |> Result.okOrThrow } }

        let addItem n cat qty note (s: State) =
            let item =
                { Item.ItemName = n |> ItemName.tryParse |> Result.okOrThrow
                  ItemId = Id.create ItemId
                  CategoryId =
                      s.Categories
                      |> DataTable.current
                      |> Seq.tryPick (fun i -> if i.CategoryName = CategoryName cat then Some i.CategoryId else None)
                  Quantity = qty |> Quantity.tryParse |> Result.asOption
                  Note = note |> Note.tryParse |> Result.asOption
                  Schedule = Once }

            { s with
                  Items = s.Items |> DataTable.insert item }

        let findItem n (s: State) =
            let itemName =
                ItemName.tryParse n |> Result.okOrThrow

            s.Items
            |> DataTable.current
            |> Seq.filter (fun i -> i.ItemName = itemName)
            |> Seq.exactlyOne

        let findStore n (s: State) =
            let storeName =
                StoreName.tryParse n |> Result.okOrThrow

            s.Stores
            |> DataTable.current
            |> Seq.filter (fun i -> i.StoreName = storeName)
            |> Seq.exactlyOne

        let markComplete n (s: State) =
            let item =
                s
                |> findItem n
                |> fun i -> { i with Schedule = Completed }

            { s with
                  Items = s.Items |> DataTable.update item }

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

            { s with
                  Items = s.Items |> DataTable.update item }

        let notSoldAt itemName storeName (s: State) =
            let nsa =
                { NotSoldItem.StoreId = (s |> findStore storeName).StoreId
                  ItemId = (s |> findItem itemName).ItemId }

            { s with
                  NotSoldItems = s.NotSoldItems |> DataTable.insert nsa }

        s
        |> addCategory "Produce"
        |> addCategory "Dairy"
        |> addCategory "Dry"
        |> addCategory "Frozen"
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