module Title
open DomainTypes

let rules =
    [ PlainTextRule.MaximumLength 20<chars>
      PlainTextRule.MinimumLength 1<chars>
      PlainTextRule.NoLeadingOrTrailingWhitespace
      PlainTextRule.SingleLineOnly ]

let normalize : NormalizeString = trim

let maxLength = 20

type private Create = string -> Result<Title, TitleError>
let create : Create = fun t ->
    let t = t |> normalize
    if t.Length = 0 then Error (TitleError.TitleIsRequired)
    elif t.Length > maxLength then Error (TitleError.TitleIsOverMaxLength maxLength)
    else Ok (Title t)

type private AsString = Title -> string
let asString : AsString = fun t -> 
    match t with | Title t -> t