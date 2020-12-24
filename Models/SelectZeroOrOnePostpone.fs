module Models.SelectZeroOrOnePostpone

let create = SelectZeroOrOne.create None Item.commonPostponeChoices

let asStateMessage item (s: SelectZeroOrOne.SelectZeroOrOne<int>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem(item, Item.Message.RemovePostpone)
    | Some f -> StateTypes.ModifyItem(item, Item.Message.Postpone(f * 1<days>))
    |> StateTypes.ItemMessage
