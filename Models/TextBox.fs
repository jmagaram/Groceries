[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.TextBox

open StateTypes

let create s = { ValueTyping = s; ValueCommitted = s }

let typeText s t = { t with ValueTyping = s }

let loseFocus normalize t =
    { t with
          ValueCommitted = normalize t.ValueTyping }

let handle normalize msg t = 
    match msg with
    | TextBoxMessage.TypeText s -> t |> typeText s
    | TextBoxMessage.LoseFocus -> t |> loseFocus normalize