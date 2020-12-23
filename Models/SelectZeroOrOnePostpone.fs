module Models.SelectZeroOrOnePostpone

let createFromRelativeDate postponeUntil =
    let choices =
        Item.commonPostponeChoices
        |> Seq.map Some
        |> Seq.choose id
        |> Seq.distinctBy ItemForm.postponeDurationAsText

    SelectZeroOrOne.create postponeUntil choices

let createFromDate postponeUntil now =
    let current =
        postponeUntil
        |> Option.map (fun postponeUntil -> Item.postponeDaysAway now postponeUntil) // negative numbers?

    createFromRelativeDate current

let createFromItemId itemId now state =
    let item =
        state
        |> State.itemsTable
        |> DataTable.findCurrent itemId

    createFromDate item.PostponeUntil now

let asStateMessage item (s: SelectZeroOrOne.SelectZeroOrOne<int>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem(item, Item.Message.RemovePostpone)
    | Some f -> StateTypes.ModifyItem(item, Item.Message.Postpone(f * 1<days>))
    |> StateTypes.ItemMessage
