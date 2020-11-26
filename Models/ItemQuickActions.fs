[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemQuickActions

open CoreTypes

type ViewModel = { ItemId: ItemId; ItemName: ItemName; Schedule: Schedule }

let create itemId state =
    let item = state |> State.itemsTable |> DataTable.findCurrent itemId

    { ItemId = item.ItemId
      ItemName = item.ItemName
      Schedule = item.Schedule }

type ViewModel with
    member me.CanComplete = me.Schedule |> Schedule.isCompleted |> not
    member me.CanBuyAgain = me.Schedule |> Schedule.isCompleted
    member me.CanAddToShoppingListNow = me.Schedule |> Schedule.isPostponed 
    member me.CanPostpone = me.Schedule |> Schedule.isRepeat

//@if (!ItemQry.Schedule.IsCompleted) {
//    <elix-menu-item id="@MenuChoice.Complete">Complete</elix-menu-item>
//}
//@if (ItemQry.Schedule.IsCompleted) {
//    <elix-menu-item id="@MenuChoice.BuyAgain">Buy again</elix-menu-item>
//    <elix-menu-item id="@MenuChoice.BuyAgainWeekly">Buy again (weekly)</elix-menu-item>
//}
//@if (!ItemQry.Schedule.IsRepeat) {
//    <elix-menu-item id="@MenuChoice.Delete">Delete</elix-menu-item>
//}
//<elix-menu-separator></elix-menu-separator>
//@if (ItemQry.Schedule is Schedule.Repeat) {
//    @if (ItemQry.Schedule.IsPostponed()) {
//        <elix-menu-item id="@MenuChoice.Now">Add to shopping list now</elix-menu-item>
//    }
//    <elix-menu-item id="@MenuChoice.Plus1Wk">Postpone +1 Week</elix-menu-item>
//    <elix-menu-item id="@MenuChoice.Plus2Wk">Postpone +2 Weeks</elix-menu-item>
//    <elix-menu-separator></elix-menu-separator>
//}
//<elix-menu-item id="@MenuChoice.Details">Options...</elix-menu-item>
