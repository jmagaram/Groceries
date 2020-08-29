module QuantityTextBox

let normalize = trim

let validate t =
    match t |> normalize |> String.length with
    | x when x > 10 -> Some "The quantity can't be that big."
    | _ -> None

let create = TextBoxOld.create validate normalize

let update = TextBoxOld.update validate normalize