namespace Models

open StringValidation
open DomainTypes

module ItemName =

    let rules = singleLine 3<chars> 50<chars>

    let validator = rules |> createValidator

    let create s =
        let s = s |> String.trim
        s
        |> validator
        |> List.ofSeq
        |> fun errors ->
            match errors with
            | [] -> s |> DomainTypes.ItemName |> Ok
            | _ -> Error errors

module Note =

    let rules = multipleLine 3<chars> 200<chars>

    let validator = rules |> createValidator

    let create s =
        let s = s |> String.trim
        s
        |> validator
        |> List.ofSeq
        |> fun errors ->
            match errors with
            | [] -> s |> DomainTypes.ItemName |> Ok
            | _ -> Error errors

module Quantity =

    let rules = singleLine 1<chars> 30<chars>

    let validator = rules |> createValidator

    let create s =
        let s = s |> String.trim
        s
        |> validator
        |> List.ofSeq
        |> fun errors ->
            match errors with
            | [] -> s |> DomainTypes.Quantity |> Ok
            | _ -> Error errors

module CategoryName =

    let rules = singleLine 1<chars> 30<chars>

    let validator = rules |> createValidator

    let create s =
        let s = s |> String.trim
        s
        |> validator
        |> List.ofSeq
        |> fun errors ->
            match errors with
            | [] -> s |> DomainTypes.CategoryName |> Ok
            | _ -> Error errors

module StoreName =

    let rules = singleLine 1<chars> 30<chars>

    let validator = rules |> createValidator

    let create s =
        let s = s |> String.trim
        s
        |> validator
        |> List.ofSeq
        |> fun errors ->
            match errors with
            | [] -> s |> DomainTypes.StoreName |> Ok
            | _ -> Error errors

module State =

    let empty =
        { Categories = DataTable.empty
          Items = DataTable.empty
          Stores = DataTable.empty
          NeverSells = DataTable.empty }

    let private updateCategories f s = { s with Categories = s.Categories |> f }
    let private updateStores f s = { s with Stores = s.Stores |> f }
    let private updateItems f s = { s with Items = s.Items |> f }
    let private updateNeverSells f s = { s with NeverSells = s.NeverSells |> f }

    let deleteCategory id (s:State) = 
        s 
        |> updateCategories (fun cs -> cs |> DataTable.deleteIf (fun x -> x.CategoryId = id))

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

