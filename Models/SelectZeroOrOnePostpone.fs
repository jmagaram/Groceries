module Models.SelectZeroOrOnePostpone

let create schedule now =
    let current =
        schedule
        |> Schedule.postponedUntilDays now
        |> Option.map (fun i -> max i 1<days>)

    let choices =
        Schedule.commonPostponeChoices
        |> Seq.map Some
        //|> Seq.append (current |> Seq.singleton)
        |> Seq.choose id
        |> Seq.distinctBy ItemForm.postponeDurationAsText

    SelectZeroOrOne.create current choices

let createFromItemId itemId now state =
    let item = state |> State.itemsTable |> DataTable.findCurrent itemId
    create item.Schedule now

let asStateMessage item (s: SelectZeroOrOne.SelectZeroOrOne<int>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem(item, Item.Message.RemovePostpone)
    | Some f -> StateTypes.ModifyItem(item, Item.Message.Postpone(f * 1<days>))
    |> StateTypes.ItemMessage
