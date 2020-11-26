module Models.SelectZeroOrOneCategory

let create current state =
    SelectZeroOrOne.create current (state |> State.categories)

let asStateMessage item (s:SelectZeroOrOne.SelectZeroOrOne<CoreTypes.Category>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem (item, Item.Message.ClearCategory)
    | Some c -> StateTypes.ModifyItem (item, Item.Message.UpdateCategory c.CategoryId)
    |> StateTypes.ItemMessage

