[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemQuickActions

open System
open CoreTypes

type ViewModel =
    { ItemId: ItemId
      ItemName: ItemName
      PostponeUntil: DateTimeOffset option
      PermitQuickNotSoldAt: Store option
      PermitStoresCustomization: bool }

let private create itemId activeStore state =
    let item =
        state
        |> State.itemsTable
        |> DataTable.findCurrent itemId

    let shoppingAtAndSellsItem =
        activeStore
        |> Option.bind (fun s -> state |> State.tryFindStore s)
        |> Option.filter (fun s -> state |> State.storeSellsItemById itemId s.StoreId)

    let storesExist =
        state |> State.stores |> Seq.isEmpty |> not

    { ItemId = item.ItemId
      ItemName = item.ItemName
      PostponeUntil = item.PostponeUntil
      PermitQuickNotSoldAt = shoppingAtAndSellsItem
      PermitStoresCustomization = storesExist }

let createNoActiveStore itemId state = create itemId None state
let createWithActiveStore itemId storeId state = create itemId (Some storeId) state
