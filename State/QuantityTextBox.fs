module QuantityTextBox

let normalize = trim

let validate t =
    match t |> normalize |> String.length with
    | x when x > 10 -> Some "The quantity can't be that big."
    | _ -> None

let create = TextBox.create validate normalize

let update = TextBox.update validate normalize