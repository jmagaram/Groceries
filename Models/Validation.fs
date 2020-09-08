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

    type Rules =
        { MinLength: int<chars>
          MaxLength: int<chars>
          OnlyContains: CharacterKind list }

    let singleLine minLength maxLength =
        { MinLength = minLength
          MaxLength = maxLength
          OnlyContains =
              [ Letter
                Mark
                Number
                Punctuation
                Space
                Symbol ] }

    let multipleLine minLength maxLength =
        { MinLength = minLength
          MaxLength = maxLength
          OnlyContains =
              [ Letter
                Mark
                Number
                Punctuation
                Space
                Symbol
                LineFeed ] }

    type StringError =
        | TooLong
        | TooShort
        | InvalidCharacters

    let lengthAtLeast c s = (String.length s) >= (c |> int)

    let lengthAtMost c s = (String.length s) <= (c |> int)

    let onlyContains cs =
        match cs with
        | [] -> failwith "No list of valid characters was provided."
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

    let createValidator (r: Rules) =
        let vs =
            [ (r.MinLength |> lengthAtLeast, TooShort)
              (r.MaxLength |> lengthAtMost, TooLong)
              (r.OnlyContains |> onlyContains, InvalidCharacters) ]

        fun s ->
            vs
            |> Seq.choose (fun (p, err) -> if p s then None else Some err)
