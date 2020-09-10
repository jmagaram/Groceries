module Models.View

open ViewTypes

module Item = 

    let create itemId (s:StateTypes.State) =
        let item = s.Items |> DataTable.findCurrent itemId
        let category = 
            item
            |> Option.bind (fun i -> i.CategoryId)
            |> Option.bind (fun i -> s.Categories |> DataTable.findCurrent i)
        let notSold = 
            s.NotSoldItems
            |> DataTable.current
            |> Seq.choose (fun i ->
                if i.ItemId = itemId
                then s.Stores |> DataTable.findCurrent i.StoreId
                else None)
            |> List.ofSeq
        match item with
        | None -> None
        | Some item ->
            { ItemId = item.ItemId 
              Name = item.Name 
              Note = item.Note
              Quantity = item.Quantity
              Category = category
              NotSold = notSold
              Schedule = item.Schedule
            }
            |> Some
//type Item =
//    { ItemId: StateTypes.ItemId
//      Name: StateTypes.ItemName
//      Note: StateTypes.Note option
//      Quantity: StateTypes.Quantity option
//      Category: StateTypes.Category option
//      NotSold : StateTypes.NotSold list
//      Schedule: StateTypes.Schedule }

