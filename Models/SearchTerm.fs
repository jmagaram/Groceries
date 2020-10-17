[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.SearchTerm

open System.Text.RegularExpressions
open StateTypes
open ValidationTypes

let rules =
    { MinLength = 1<chars>
      MaxLength = 20<chars>
      StringRules.OnlyContains = [ Letter; Mark; Number; Punctuation; Space; Symbol ] }

let normalizer = String.trim

let validator = rules |> StringValidation.createValidator

let tryParse s =
    s
    |> normalizer
    |> fun s ->
        match validator s |> Seq.toList with
        | [] -> Ok s
        | errors -> Error errors
    |> Result.mapError List.head
    |> Result.map SearchTerm

let rec tryCoerce s =
    if s |> isNullOrWhiteSpace then
        None
    else
        match s |> normalizer |> tryParse with
        | Error IsRequired -> None
        | Error TooShort -> None
        | Ok t -> Some t
        | Error TooLong -> tryCoerce (s.Substring(0, rules.MaxLength |> int))
        | Error InvalidCharacters -> None // better to strip invalid chars

let value (SearchTerm s) = s

let toRegex (SearchTerm s) =
    let isRepeating s =
        let len = s |> String.length

        [ 1 .. len ]
        |> Seq.choose (fun i ->
            let endsWith = s.Substring(len - i)
            let n = len / i

            match (endsWith |> String.replicate n) = s with
            | true -> Some(endsWith, n)
            | false -> None)
        |> Seq.filter (fun (_, i) -> i > 1)
        |> Seq.tryHead

    let edgeMiddleEdge s =
        let len = String.length s
        let maxEdgeLength = (len - 1) / 2

        seq { maxEdgeLength .. (-1) .. 1 }
        |> Seq.choose (fun i ->
            let starts = s.Substring(0, i)
            let ends = s.Substring(len - i)

            if starts = ends then
                let middle = s.Substring(i, len - i * 2)
                Some {| Edge = starts; Middle = middle |}
            else
                None)
        |> Seq.tryHead

    let pattern =
        let s = s.ToLowerInvariant()
        let escape s = Regex.Escape(s)

        match s |> isRepeating with
        | Some (x, n) -> sprintf "(%s){%d,}" (escape x) n
        | None ->
            match s |> edgeMiddleEdge with
            | Some i -> sprintf "((%s)+(%s)*)+" (escape s) (escape (i.Middle + i.Edge))
            | None -> sprintf "(%s)+" (escape s)

    let options = RegexOptions.IgnoreCase ||| RegexOptions.CultureInvariant

    new Regex(pattern, options)
