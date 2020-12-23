[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemQuickActions

open System
open CoreTypes

type ViewModel = { ItemId: ItemId; ItemName: ItemName; PostponeUntil: DateTimeOffset option }

let create itemId state =
    let item = state |> State.itemsTable |> DataTable.findCurrent itemId

    { ItemId = item.ItemId
      ItemName = item.ItemName
      PostponeUntil = item.PostponeUntil }

type ViewModel with
    member me.CanComplete = true
    member me.CanPostpone = true
    member me.CanAddToShoppingListNow = me.PostponeUntil.IsSome