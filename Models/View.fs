namespace Models
open System
open System.Reactive.Linq
open System.Text.RegularExpressions
open FSharp.Control.Reactive
open ViewTypes

module SearchTerm =
    open ValidationTypes
    open StringValidation

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

module TextSpan =

    let private create f s =
        if s = "" then failwith "The text span is empty." else { Format = f; Text = s }

    let normal = create Normal

    let highlight = create Highlight

module FormattedText =

    let spans (FormattedText ft) = ft

    let fromList spans = FormattedText spans

    let normal s = s |> TextSpan.normal |> List.singleton |> fromList

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

//module ItemEditForm =
//    open StateTypes
//    open ValidationTypes
//    open FormsTypes

//    let init =
//        { ItemEditForm.ItemName = TextInput.init ItemName.tryParse String.trim "" }

//    // no way to store this in State variable because it is very early in the dependency list
//    // maybe core data is just one part of the application state

//    let handleMessage msg (form: ItemEditForm) =
//        let handleItemNameMsg = TextInput.handleMessage ItemName.tryParse String.trim

//        match msg with
//        | ItemNameMessage msg ->
//            { form with
//                  ItemName = form.ItemName |> handleItemNameMsg msg }

//type ItemEditFormMessage =
//    | ItemNameMessage of TextInputMessage

//type ItemEditForm =
//    { ItemName : TextInput<ItemName, StringError> }
