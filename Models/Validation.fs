namespace Models

open System.Text.RegularExpressions
open Models.ValidationTypes

module StringValidation =

    let normalizeLineFeed (s: string) = s.Replace("\r\n", "\n").Replace("\r", "\n")

    let singleLine minLength maxLength =
        { MinLength = minLength
          MaxLength = maxLength
          OnlyContains = [ Letter; Mark; Number; Punctuation; Space; Symbol ] }

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

    let lengthAtLeast min s = (String.length s) >= (min |> int)

    let lengthAtMost max s = (String.length s) <= (max |> int)

    let existsIfRequired min s =
        let isRequired = (min |> int) > 0
        let isEmpty = s |> String.isNullOrWhiteSpace
        isRequired && not isEmpty

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

    let createValidator (r: StringRules) =
        let vs =
            [ (r.MinLength |> existsIfRequired, IsRequired)
              (r.MinLength |> lengthAtLeast, TooShort)
              (r.MaxLength |> lengthAtMost, TooLong)
              (r.OnlyContains |> onlyContains, InvalidCharacters) ]

        fun s ->
            vs
            |> Seq.choose (fun (p, err) -> if p s then None else Some err)

    let createParser normalizer validator onSuccess onError s =
        s
        |> normalizer
        |> fun s ->
            match validator s |> Seq.toList with
            | [] -> s |> onSuccess |> Ok
            | errors -> errors |> onError |> Error

module RangeValidation =

    let createValidator (r: Range<_>) v =
        if v < r.Min then RangeError.TooSmall |> Some
        elif v > r.Max then RangeError.TooBig |> Some
        else None

    let forceIntoBounds v (r: Range<_>) =
        if v < r.Min then r.Min
        elif v > r.Max then r.Max
        else v

    let toResult normalizer validator onSuccess onError v = 
        match v |> normalizer |> validator with
        | None -> v |> onSuccess |> Ok
        | Some error -> error |> onError |> Error

