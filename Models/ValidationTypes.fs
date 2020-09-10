module Models.ValidationTypes

[<Measure>]
type chars

type CharacterKind =
    | Letter
    | Mark
    | Number
    | Punctuation
    | Symbol
    | Space
    | LineFeed

type StringRules =
    { MinLength: int<chars>
      MaxLength: int<chars>
      OnlyContains: CharacterKind list }

type StringError =
    | TooLong
    | TooShort
    | InvalidCharacters

type Range<'T> = { Min: 'T; Max: 'T }

type RangeError =
    | TooSmall
    | TooBig
