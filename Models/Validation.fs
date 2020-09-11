﻿namespace Models

open System.Text.RegularExpressions
open Models.ValidationTypes

module StringValidation =

    let normalizeLineFeed (s: string) =
        s.Replace("\r\n", "\n").Replace("\r", "\n")

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

    let createValidator (r: StringRules) =
        let vs =
            [ (r.MinLength |> lengthAtLeast, TooShort)
              (r.MaxLength |> lengthAtMost, TooLong)
              (r.OnlyContains |> onlyContains, InvalidCharacters) ]

        fun s ->
            vs
            |> Seq.choose (fun (p, err) -> if p s then None else Some err)

    let parser tag rules =
        let validator = rules |> createValidator

        fun s ->
            let s = s |> String.trim |> normalizeLineFeed

            s
            |> validator
            |> List.ofSeq
            |> fun errors ->
                match errors with
                | [] -> s |> tag |> Ok
                | _ -> Error errors

module RangeValidation =

    let createValidator (r: Range<_>) v =
        if v < r.Min then RangeError.TooSmall |> Some
        elif v > r.Max then RangeError.TooBig |> Some
        else None
