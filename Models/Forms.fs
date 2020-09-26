namespace Models

open Models.FormsTypes

module TextInput =

    let init tryParse normalize value =
        { Value = value
          ValidationResult = value |> normalize |> tryParse }

    let setText tryParse value ti =
        { ti with
              Value = value
              ValidationResult = value |> tryParse }

    let loseFocus normalize ti = { ti with TextInput.Value = ti.Value |> normalize }

    let handleMessage tryParse normalize msg ti =
        match msg with
        | GainFocus -> ti
        | LoseFocus -> ti |> loseFocus normalize
        | TypeText s -> ti |> setText tryParse s

module Modes2 =

    let init tag m1 m2 = { CurrentMode = tag; Mode1 = m1; Mode2 = m2 }

    let (|Mode1|Mode2|) (m: Modes2<_, _>) =
        match m.CurrentMode with
        | Mode1Of2Tag -> Mode1 m.Mode1
        | Mode2Of2Tag -> Mode2 m.Mode2

    let setMode t (m: Modes2<_, _>) = { m with CurrentMode = t }
    let mapMode1 f (m: Modes2<_, _>) = { m with Mode1 = f m.Mode1 }
    let mapMode2 f (m: Modes2<_, _>) = { m with Mode2 = f m.Mode2 }

module Modes3 =

    let init tag m1 m2 = { CurrentMode = tag; Mode1 = m1; Mode2 = m2 }

    let (|Mode1|Mode2|) (m: Modes3<_, _, _>) =
        match m.CurrentMode with
        | Mode1Of3Tag -> Mode1 m.Mode1
        | Mode2Of3Tag -> Mode2 m.Mode2
        | Mode3Of3Tag -> Mode2 m.Mode3

    let setMode t (m: Modes3<_, _, _>) = { m with CurrentMode = t }
    let mapMode1 f (m: Modes3<_, _, _>) = { m with Mode1 = f m.Mode1 }
    let mapMode2 f (m: Modes3<_, _, _>) = { m with Mode2 = f m.Mode2 }
    let mapMode3 f (m: Modes3<_, _, _>) = { m with Mode3 = f m.Mode3 }

module ChooseZeroOrOne =

    let init items = { Choices = items; Selected = None}

    let selectFirst p c = { c with Selected = c.Choices |> List.tryFind p }

    let select i c = 
        match c.Choices |> List.contains i with
        | true -> { c with Selected = Some i }
        | false -> failwith "The item is not in the available choices."
    
//type ChooseZeroOrOne<'T> = { Choices: 'T list; Selected: 'T option }

//module Modes3 =

//    let init tag m1 m2 m3 = { CurrentMode = tag; Mode1 = m1; Mode2 = m2; Mode3 = m3 }

//    let (|Mode1|Mode2|Mode3|) (m:Modes3<_,_,_>) =
//        match m.CurrentMode with
//        | Mode1Of3 -> m.Mode1
//        | Mode2Of3 -> m.Mode2
//        | Mode3Of3 -> m.Mode3

//    let setMode t (m:Modes3<_,_,_>) = { m with CurrentMode = t }

//    let mapMode1 f (m:Modes3<_,_,_>) = { m with Mode1 = f m.Mode1 }
//    let mapMode2 f (m:Modes3<_,_,_>) = { m with Mode2 = f m.Mode2 }
//    let mapMode3 f (m:Modes3<_,_,_>) = { m with Mode3 = f m.Mode3 }

//module ChooseOne =

//    let create choices selected =
//        if choices |> List.contains selected |> not
//        then failwith "The selected item is not in the available choices."
//        else { Choices = choices; Selected = selected }

//    let setSelected selected chooseOne =
//        if chooseOne.Choices |> List.contains selected |> not
//        then failwith "The selected item is not in the available choices."
//        else { chooseOne with Selected = selected }

//type TextInput<'T, 'Error> =
//    { Value : string
//      ValidationResult : Result<'T,'Error> }

//type TextInputMessage =
//    | LoseFocus
//    | GainFocus
//    | TypeText of string


//let lostFocus (f:FormField<_,_,_>) =
//    match f.Proposed <> f.Normalized with
//    | true -> { f with Proposed = f.Normalized }
//    | false -> f

//let proposed (f:FormField<_,_,_>) = f.Proposed

//let gainedFocus f = f

//let propose normalize validate =
//    fun p f ->
//        let n =
//            match normalize with
//            | None -> p
//            | Some normalize -> p |> normalize
//        let r = n |> validate
//        { f with
//            Proposed = p
//            Normalized = n
//            ValidationResult = r }

//let init normalize validate proposed =
//    let n =
//        match normalize with
//        | None -> proposed
//        | Some normalize -> proposed |> normalize
//    let r = n |> validate
//    { InitialValue = n
//      Proposed = n
//      Normalized = n
//      ValidationResult = r }

//let handleMessage normalize validate =
//    let propose = propose normalize validate
//    let lostFocus = lostFocus
//    let gainedFocus = gainedFocus
//    fun msg f ->
//        match msg with
//        | LostFocus -> f |> lostFocus
//        | GainedFocus -> f |> gainedFocus
//        | Propose p -> f |> propose p

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
