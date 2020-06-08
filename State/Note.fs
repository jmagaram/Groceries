module Note
open DomainTypes

let normalize : NormalizeString = trim

let maxLength = 10

type private Create = string -> Result<Note, NoteError>
let create : Create = fun n ->
    let n = n |> normalize
    if n.Length > maxLength then Error (NoteError.NoteIsOverMaxLength maxLength)
    else Ok (Note n)

type private AsString = Note -> string
let asString : AsString = fun n -> 
    match n with | Note n -> n