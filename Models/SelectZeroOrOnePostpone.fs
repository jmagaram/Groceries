module Models.SelectZeroOrOnePostpone

let create schedule now =
    let current =
        schedule 
        |> Schedule.postponedUntilDays now
        |> Option.map (fun i -> max i 0<days>)
    let choices =
        Schedule.commonPostponeChoices
        |> Seq.map Some
        |> Seq.append (current |> Seq.singleton)
        |> Seq.choose id
        |> Seq.distinctBy ItemForm.postponeDurationAsText
    SelectZeroOrOne.create current choices

let asStateMessage item (s:SelectZeroOrOne.SelectZeroOrOne<int>) =
    match s.CurrentChoice with
    | None -> StateTypes.ModifyItem (item, Item.Message.RemovePostpone)
    | Some f -> StateTypes.ModifyItem (item, Item.Message.Postpone (f * 1<days>))
    |> StateTypes.ItemMessage

