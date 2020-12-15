module Models.SelectZeroOrOneCategory

let createFromPickList current choices =
    SelectZeroOrOne.create current choices

let create current state =
    createFromPickList current (state |> State.categories)

let asStateMessage item (s:SelectZeroOrOne.SelectZeroOrOne<CoreTypes.Category>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem (item, Item.Message.ClearCategory)
    | Some c -> StateTypes.ModifyItem (item, Item.Message.UpdateCategory c.CategoryId)
    |> StateTypes.ItemMessage

