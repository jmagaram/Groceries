module TextBox
open DomainTypes

let hasFocus tb = tb.HasFocus

let text tb = tb.Text

let error tb = tb.Error

let normalizedText tb = tb.NormalizedText

let create v n =
    let nt = "" |> n
    { Text = nt
      NormalizedText = nt
      Error = nt |> v 
      HasFocus = false }

let setText v n s tb =
    let nt = s |> n
    { tb with 
        Text = 
            match tb |> hasFocus with 
            | true -> s
            | false -> nt
        NormalizedText = nt
        Error = nt |> v }

let getFocus tb = 
    { tb with 
        HasFocus = true }

let loseFocus tb =
    { tb with 
        HasFocus = false; 
        Text = tb.NormalizedText }

let update v n msg tb = 
    match msg with
    | GetFocus -> tb |> getFocus
    | LoseFocus -> tb |> loseFocus
    | SetText t -> tb |> (setText v n t)