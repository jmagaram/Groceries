namespace Models

open Models.FormsTypes

module TextBox =

    // Does the parser AND normalizer both normalize? If yes, why does TextInput.init need both?
    let init tryParse normalize value =
        { Value = value
          ValidationResult = value |> normalize |> tryParse }

    let setText tryParse value ti =
        { ti with
              Value = value
              ValidationResult = value |> tryParse }

    let loseFocus normalize ti = { ti with TextBox.Value = ti.Value |> normalize }

    let handleMessage tryParse normalize msg ti =
        match msg with
        | GainFocus -> ti
        | LoseFocus -> ti |> loseFocus normalize
        | TypeText s -> ti |> setText tryParse s

module ChooseZeroOrOne =

    let init items = { Choices = items; Selected = None }

    let selectNothing c = { c with Selected = None }

    let selectFirst p c = { c with Selected = c.Choices |> List.tryFind p }

    let selectByKey keyOf k c =
        { c with
              Selected = c.Choices |> List.find (fun i -> keyOf i = k) |> Some }

    let select i c =
        match c.Choices |> List.contains i with
        | true -> { c with Selected = Some i }
        | false -> failwith "The item is not in the available choices."

    let handleMessage keyOf msg c =
        match msg with
        | ClearSelection -> c |> selectNothing
        | ChooseByKey k -> c |> selectByKey keyOf k
