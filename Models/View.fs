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

    let private consolidate (source: TextSpan seq) =
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
        searchTerm |> SearchTerm.toRegex |> fromRegex


    let findAny searchTerms =
        let highlighters =
            searchTerms
            |> Seq.map SearchTerm.toRegexComponents
            |> Seq.map (fun (r, o) -> new Regex(r, o) |> fromRegex)
            |> List.ofSeq

        let highlightedIndexes (f: FormattedText) =
            f
            |> FormattedText.spans
            |> Seq.collect (fun s -> Seq.replicate s.Text.Length s.Format)
            |> Seq.indexed
            |> Seq.choose (fun (i, j) -> if j = TextFormat.Highlight then Some i else None)
            |> Set.ofSeq

        fun (s: String) ->
            let highlightedCharIndexes =
                highlighters
                |> Seq.map (fun findOneTerm -> findOneTerm s)
                |> Seq.map highlightedIndexes
                |> Seq.fold Set.union Set.empty

            s
            |> Seq.indexed
            |> Seq.map
                (fun (i, j) ->
                    match highlightedCharIndexes |> Set.contains i with
                    | true -> TextSpan.highlight (j.ToString())
                    | false -> TextSpan.normal (j.ToString()))
            |> FormattedText.fromSpans

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

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module DurationEstimate =

    let fromTimeSpan (ts: TimeSpan) =
        let ts = max ts TimeSpan.Zero
        let daysTotal = ts.TotalDays
        let weeksTotal = ts.TotalDays / 7.0
        let monthsTotal = ts.TotalDays / 30.0
        let cutoff = 0.8

        if monthsTotal > cutoff then
            { DurationUnit = DurationUnit.Months
              Quantity = Convert.ToInt32(monthsTotal) |> uint }
        elif weeksTotal > cutoff then
            { DurationUnit = DurationUnit.Weeks
              Quantity = Convert.ToInt32(weeksTotal) |> uint }
        else
            { DurationUnit = DurationUnit.Days
              Quantity = Convert.ToInt32(daysTotal) |> uint }

    let fromDays (d: int) =
        TimeSpan.FromDays(d |> float) |> fromTimeSpan

    let toTimeSpan (d: DurationEstimate) =
        match d.DurationUnit with
        | Days -> TimeSpan.FromDays(d.Quantity |> float)
        | Weeks -> TimeSpan.FromDays((d.Quantity * 7u) |> float)
        | Months -> TimeSpan.FromDays((d.Quantity * 30u) |> float)

    let longLabel (d: DurationEstimate) =
        match d.DurationUnit, d.Quantity with
        | Days, 0u -> "Now"
        | Days, 1u -> "1 day"
        | Days, x -> $"{x} days"
        | Weeks, 1u -> "1 week"
        | Weeks, x -> $"{x} weeks"
        | Months, 1u -> "1 month"
        | Months, x -> $"{x} months"

    let shortLabel (soon: TimeSpan) (d: DurationEstimate) =
        if (d |> toTimeSpan) <= soon then
            "soon"
        else
            match d.DurationUnit with
            | Days -> $"{d.Quantity}d"
            | Weeks -> $"{d.Quantity}w"
            | Months -> $"{d.Quantity}m"
