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
module TimeSpanEstimate =

    let fromTimeSpan (ts: TimeSpan) =
        let (ts, neg) =
            if ts < TimeSpan.Zero then (-ts, -1.0) else (ts, 1.0)

        let cutoff = 0.8
        let days = ts.TotalDays
        let months = ts.TotalDays / 30.0
        let weeks = ts.TotalDays / 7.0

        if months > cutoff
        then Convert.ToInt32(months * neg) |> int |> Months
        elif (weeks > cutoff)
        then Convert.ToInt32(weeks * neg) |> int |> Weeks
        else Convert.ToInt32(days * neg) |> int |> Days

    let fromDays d = TimeSpan.FromDays(d |> float) |> fromTimeSpan

    let between (a: DateTimeOffset) (b: DateTimeOffset) = (b - a) |> fromTimeSpan

    let toTimeSpan (ts: TimeSpanEstimate) =
        match ts with
        | Days d -> TimeSpan.FromDays(float d)
        | Weeks w -> TimeSpan.FromDays((float w) * 7.0)
        | Months m -> TimeSpan.FromDays((float m) * 30.0)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SelectZeroOrOne =

    let create choice items =
        { Choices = items |> Set.ofSeq
          CurrentChoice = choice
          OriginalChoice = choice }

    let select i z = 
        { z with CurrentChoice = Some i }

    let selectNone z = 
        { z with CurrentChoice = None }

    let hasChanges z = z.OriginalChoice <> z.CurrentChoice

module SelectZeroOrOneCategory =

    let createFromPickList current choices =
        SelectZeroOrOne.create current choices

    let create current state =
        createFromPickList current (state |> State.categories)

    let asStateMessage item (s:SelectZeroOrOne<CoreTypes.Category>) =
        match s.CurrentChoice with
        | None -> StateTypes.ModifyItem (item, Item.Message.ClearCategory)
        | Some c -> StateTypes.ModifyItem (item, Item.Message.UpdateCategory c.CategoryId)
        |> StateTypes.ItemMessage

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SelectMany =

    let create items =
        { Items = items |> Set.ofSeq
          SelectedOriginal = Set.empty
          Selected = Set.empty }

    let assertItemInSet i s =
        if s.Items |> Set.contains i |> not then failwith "The set does not contain that item."

    let withOriginalSelection items s =
        items
        |> Seq.fold
            (fun t i ->
                assertItemInSet i t

                { t with
                      SelectedOriginal = t.SelectedOriginal |> Set.add i
                      Selected = t.Selected |> Set.add i })
            s

    let select item s =
        s |> assertItemInSet item
        { s with Selected = s.Selected |> Set.add item }

    let selectMany items s = items |> Seq.fold (fun t i -> t |> select i) s

    let selectAll s = s |> selectMany s.Items

    let deselect item s =
        s |> assertItemInSet item
        { s with Selected = s.Selected |> Set.remove item }

    let deselectAll s = { s with Selected = Set.empty }

    let isSelected i s =
        s |> assertItemInSet i
        s.Selected |> Set.contains i

    let toggleSelected i s =
        match s |> isSelected i with
        | true -> s |> deselect i
        | false -> s |> select i

    let added s = Set.difference s.Selected s.SelectedOriginal

    let removed s = Set.difference s.SelectedOriginal s.Selected

    let hasChanges s =
        (s |> added |> Set.isEmpty |> not)
        || (s |> removed |> Set.isEmpty |> not)

    let selectionSummary s =
        s.Items
        |> Seq.map (fun i -> {| Item = i; IsSelected = s.Selected |> Set.contains i |})

    let allSelected s = s.Selected.Count = s.Items.Count

    let revertToOriginalSelection s = { s with Selected = s.SelectedOriginal }



