namespace Models

open System
open ValidationTypes
open StringValidation
open StateTypes

[<AutoOpen>]
module private X =

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

    let empty =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NeverSells = DataTable.empty }

    let private updateCategories f s =
        { s with
              Categories = s.Categories |> f }

    let private updateItems f s = { s with Items = s.Items |> f }
    let private updateStores f s = { s with Stores = s.Stores |> f }

    let private updateNeverSells f s =
        { s with
              NeverSells = s.NeverSells |> f }

    let removeCategoryFromItem categoryId (i: Item) =
        match i.CategoryId with
        | None -> i
        | Some c -> if c = categoryId then { i with CategoryId = None } else i

    let deleteCategory id (s: State) =
        s
        |> updateCategories (DataTable.deleteIf (fun x -> x.CategoryId = id))
        |> updateItems (DataTable.mapCurrent (removeCategoryFromItem id))

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
