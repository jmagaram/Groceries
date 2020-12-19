namespace Models

open System
open System.Reactive.Linq
open System.Text.RegularExpressions
open FSharp.Control.Reactive
open ViewTypes
open CoreTypes

module TextSpan =

    let private create f s =
        if s = "" then failwith "The text span is empty." else { Format = f; Text = s }

    let normal = create Normal

    let highlight = create Highlight

    let concatenate (s1: TextSpan) (s2: TextSpan) =
        if s1.Format = s2.Format then
            { TextSpan.Format = s1.Format
              TextSpan.Text = $"{s1.Text}{s2.Text}" }
            |> Some
        else
            None

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FormattedText =

    let spans (FormattedText ft) = ft

    let consolidate (source: TextSpan seq) =
        seq {
            use en = source.GetEnumerator()
            let mutable total = None

            while en.MoveNext() do
                match total with
                | None -> total <- Some en.Current
                | Some s ->
                    match TextSpan.concatenate s en.Current with
                    | None ->
                        yield s
                        total <- en.Current |> Some
                    | Some s -> total <- s |> Some

            match total with
            | Some s -> yield s
            | None -> ()
        }

    let fromSpans spans =
        spans
        |> consolidate
        |> List.ofSeq
        |> FormattedText

    let normal s =
        s
        |> TextSpan.normal
        |> List.singleton
        |> fromSpans

    let hasHighlight ft =
        ft
        |> spans
        |> Seq.exists
            (fun i ->
                match i.Format with
                | TextFormat.Highlight -> true
                | _ -> false)

    let asString s =
        String.Join("", s |> spans |> Seq.map (fun i -> i.Text))

module Highlighter =

    let private fromRegex (regex: Regex) s =
        match s |> String.length with
        | 0 -> [] |> FormattedText
        | _ ->
            seq {
                let ms = regex.Matches(s)

                if ms.Count = 0 then yield (TextSpan.normal s)
                elif ms.[0].Index > 0 then yield TextSpan.normal (s.Substring(0, ms.[0].Index))

                for i in 0 .. ms.Count - 1 do
                    yield TextSpan.highlight ms.[i].Value
                    let regStart = ms.[i].Index + ms.[i].Length

                    if i < ms.Count - 1 then
                        let len = ms.[i + 1].Index - regStart
                        if len > 0
                        then yield TextSpan.normal (s.Substring(regStart, len))
                    elif regStart < s.Length then
                        yield TextSpan.normal (s.Substring(regStart))

            }
            |> List.ofSeq
            |> FormattedText.fromSpans

    let find searchTerm =
        searchTerm 
        |> SearchTerm.toRegex 
        |> fromRegex

    let findAny searchTerms =
        let regexComponents =
            searchTerms
            |> Seq.sortByDescending SearchTerm.length
            |> Seq.map SearchTerm.toRegexComponents
            |> List.ofSeq

        let options =
            regexComponents
            |> Seq.map (fun (i, j) -> j)
            |> Seq.fold (fun t i -> t ||| i) RegexOptions.None

        let pattern =
            String.Join(
                '|',
                regexComponents
                |> Seq.map (fun (i, j) -> $"({i})")
            )

        new Regex(pattern, options) |> fromRegex

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetString =

    let private normalizeItems normalize items =
        items
        |> Seq.filter (String.isNullOrWhiteSpace >> not)
        |> Seq.map normalize
        |> Seq.distinctBy (fun (i: string) -> i.ToLowerInvariant())

    let fromItems normalize delimeter items =
        items
        |> normalizeItems normalize
        |> fun items -> String.Join(delimeter, items)

    let fromString normalize (splitOn: string []) delimeter (s: string) =
        s.Split(separator = splitOn, options = StringSplitOptions.RemoveEmptyEntries)
        |> fromItems normalize delimeter

    let toItems normalize (splitOn: string []) (s: String) =
        s.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)
        |> normalizeItems normalize
