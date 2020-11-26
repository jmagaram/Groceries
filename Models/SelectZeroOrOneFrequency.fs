module Models.SelectZeroOrOneFrequency

let create schedule =
    let freq =
        schedule 
        |> Schedule.tryAsRepeat
        |> Option.map (fun r -> r.Frequency)
    SelectZeroOrOne.create freq Frequency.commonFrequencyChoices

let createFromItemId itemId state =
    let item = state |> State.itemsTable |>DataTable.findCurrent itemId
    create item.Schedule

let asStateMessage item (s:SelectZeroOrOne.SelectZeroOrOne<CoreTypes.Frequency>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem (item, Item.Message.ScheduleOnce)
    | Some f -> StateTypes.ModifyItem (item, Item.Message.Repeat f)
    |> StateTypes.ItemMessage