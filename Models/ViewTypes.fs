module Models.ViewTypes
open Models.FormsTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

type SearchTerm = SearchTerm of string

type FormattedText = FormattedText of TextSpan list

type Highlighter = SearchTerm -> string -> FormattedText