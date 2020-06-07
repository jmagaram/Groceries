module TitleTextBox

let normalize = trim

let validate t =
    match t |> normalize |> String.length with
    | 0 -> Some "The title is required."
    | _ -> None

let create = TextBox.create validate normalize

let update = TextBox.update validate normalize