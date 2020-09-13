module Models.ViewTypes

[<Struct>]
type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

[<Struct>]
type SearchTerm = SearchTerm of string

type FormattedText = FormattedText of TextSpan list

type Highlighter = SearchTerm -> string -> FormattedText