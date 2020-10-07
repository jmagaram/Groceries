module Models.ViewTypes
open Models.StateTypes
open Models.ValidationTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

type FormattedText = FormattedText of TextSpan list

type Highlighter = SearchTerm -> string -> FormattedText
