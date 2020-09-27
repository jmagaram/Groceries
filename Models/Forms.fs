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

    let (|Mode1|Mode2|Mode3|) (m: Modes3<_, _, _>) =
        match m.CurrentMode with
        | Mode1Of3Tag -> Mode1 m.Mode1
        | Mode2Of3Tag -> Mode2 m.Mode2
        | Mode3Of3Tag -> Mode3 m.Mode3

    let setMode t (m: Modes3<_, _, _>) = { m with CurrentMode = t }
    let mapMode1 f (m: Modes3<_, _, _>) = { m with Mode1 = f m.Mode1 }
    let mapMode2 f (m: Modes3<_, _, _>) = { m with Mode2 = f m.Mode2 }
    let mapMode3 f (m: Modes3<_, _, _>) = { m with Mode3 = f m.Mode3 }

module ChooseZeroOrOne =

    let init items = { Choices = items; Selected = None}

    let selectNothing c = { c with Selected = None }

    let selectFirst p c = { c with Selected = c.Choices |> List.tryFind p }

    let selectByKey keyOf k c = { c with Selected = c.Choices |> List.find (fun i -> keyOf i = k) |> Some }

    let select i c = 
        match c.Choices |> List.contains i with
        | true -> { c with Selected = Some i }
        | false -> failwith "The item is not in the available choices."

    let handleMessage keyOf msg c = 
        match msg with
        | ClearSelection -> c |> selectNothing
        | ChooseByKey k ->  c |> selectByKey keyOf k