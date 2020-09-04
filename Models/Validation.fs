namespace Models

open System.Text.RegularExpressions

module StringValidation =

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

    let lengthIsAtLeast c s = String.length s >= (c |> int)

    let lengthIsAtMost c s = String.length s <= (c |> int)

    let onlyContains cs =
        match cs with
        | [] -> fun s -> (String.length s) = 0
        | cs ->
            let characterClass c =
                match c with
                | Letter -> "\p{L}"
                | Mark -> "\p{M}"
                | Number -> "\p{N}"
                | Punctuation -> "\p{P}"
                | Symbol -> "\p{S}"
                | Space -> "\p{Zs}"
                | LineFeed -> "\n"

            let regex =
                cs
                |> Seq.map characterClass
                |> String.concat ""
                |> fun p -> new Regex(sprintf "[^%s]" p, RegexOptions.Compiled)

            fun s -> regex.IsMatch(s) |> not

//let private lengthIsAtLeast (n:int<chars>) s = String.length s >= (n |> int)

//let private lengthIsAtMost (n:int<chars>) s = String.length s <= (n |> int)

//let private characterClass c =
//    match c with
//    | Letter       -> "\p{L}"
//    | Mark         -> "\p{M}"
//    | Number       -> "\p{N}"
//    | Punctuation  -> "\p{P}"
//    | Symbol       -> "\p{S}"
//    | Space        -> "\p{Zs}"
//    | LineFeed     -> "\n"

//let private onlyContains cs =
//    match cs with
//    | [] -> fun s -> (String.length s) = 0
//    | cs ->
//        let pattern =
//            cs
//            |> Seq.map characterClass
//            |> String.concat ""
//            |> fun s -> sprintf "[%s]*" s
//        let regex = new Regex(pattern)
//        fun s -> regex.IsMatch(s)

//let private normalizeLineFeeds =
//    let regex = new Regex("\r\n")
//    fun s -> regex.Replace(s,"\n")

//type private CreateValidator = PlainTextRules -> (string -> Result<PlainText, PlainTextRules>)
//let createValidator : CreateValidator = fun rs ->
//    let onlyContainsPermittedChars = rs.PermittedCharacters |> onlyContains
//    fun s ->
//        let s =
//            s
//            |> trim
//            |> normalizeLineFeeds
//        let isValid =
//            lengthIsAtMost rs.MaximumLength s
//            && lengthIsAtLeast rs.MinimumLength s
//            && (onlyContainsPermittedChars s)
//        match isValid with
//        | true -> Ok (PlainText s)
//        | false -> Error rs

//type PlainTextRules =
//    { MinimumLength: int<chars>
//      MaximumLength: int<chars>
//      PermittedCharacters: PlainTextCharacter list }
