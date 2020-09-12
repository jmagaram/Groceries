module Models.ViewTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = 
    { Format : TextFormat
      Text : string }

type SearchTerm = | SearchTerm of string

type FormattedText = FormattedText of TextSpan list