module TitleTextBox

let normalize = trim

let validate t =
    match t |> normalize |> String.length with
    | 0 -> Some "The title is required."
    | _ -> None

let create = TextBoxOld.create validate normalize

let update = TextBoxOld.update validate normalize