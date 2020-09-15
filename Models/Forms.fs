namespace Models

module ChooseOne =

    let y = 4

//type ChooseOne<'T> when 'T : comparison =
//    { Choices : 'T list 
//      SelectedItem : 'T 
//      Serialize : 'T -> string
//      Deserialize : string -> 'T }

//type ChooseOneItem<'T> = 
//    { Value : 'T 
//      IsSelected : bool
//      Key : string }


//let init serialize deserialize items =
//    match items |> List.tryHead with
//    | None -> failwith "The list of items was empty. At least one, but usually three or more items are necessary for a list-based choice."
//    | Some h ->
//        { Choices = items
//          SelectedItem = h
//          Serialize = serialize
//          Deserialize = deserialize }
    
//let select i c = { c with ChooseOne.SelectedItem = i }
    
//let selectBy p c = 
//    { c with ChooseOne.SelectedItem = c.Choices |> Seq.filter p |> Seq.exactlyOne}

//let items c =
//    let selectedKey = c.SelectedItem |> c.Serialize
//    c.Choices
//    |> Seq.map (fun i -> 
//        let itemKey = i |> c.Serialize
//        let isSelected = itemKey = selectedKey
//        { ChooseOneItem.Value = i
//          Key = itemKey
//          IsSelected = isSelected })

//let selectedItemFromKey key c =
//    key |> c.Deserialize



