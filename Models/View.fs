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

module FormattedText =

    let spans (FormattedText ft) = ft

    let fromList spans = FormattedText spans

    let normal s = s |> TextSpan.normal |> List.singleton |> fromList

    let hasHighlight ft =
        ft
        |> spans
        |> Seq.exists (fun i ->
            match i.Format with
            | TextFormat.Highlight -> true
            | _ -> false)

module Highlighter =
    let create: Highlighter =
        fun searchTerm ->
            let regex = searchTerm |> SearchTerm.toRegex

            fun s ->
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

                            if i < ms.Count - 1
                            then yield TextSpan.normal (s.Substring(regStart, ms.[i + 1].Index - regStart))
                            elif regStart < s.Length
                            then yield TextSpan.normal (s.Substring(regStart))

                    }
                    |> List.ofSeq
                    |> FormattedText.fromList

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

    let toItems normalize (splitOn: string []) (s:String) = 
        s.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)
        |> normalizeItems normalize
      

