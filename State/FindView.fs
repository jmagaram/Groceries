module FindView

open System
open DomainTypes
open System.Text.RegularExpressions

type Format =
    | Highlight
    | Regular

type Span =
    { Format : Format 
      Text : string }

type HighlightedText = Span seq

type TextFilter = | CaseInsensitive of string

type TextFilterError = | FilterIsEmptyOrWhitespace

type ItemSummary =
    { Id : ItemId
      Title : Title
      Quantity : Quantity
      Note : Note
      Repeat : Repeat
      Status : Status }

type Model =
    { TitleFilter : string
    }

module Span = 

    let private create f s =
        match s |> String.length with
        | 0 -> failwith "A span must have at least 1 character."
        | _ -> { Text = s; Format = f }

    let highlight s = s |> create Highlight
    
    let regular s = s |> create Regular

    let isHighlight s = 
        match s.Format with 
        | Highlight _ -> true 
        | _ -> false

module TextFilter =

    type private Create = string -> Result<TextFilter, TextFilterError>
    let create : Create = fun s -> 
        match s |> isNullOrWhiteSpace with
        | true -> Error FilterIsEmptyOrWhitespace
        | false -> Ok (TextFilter.CaseInsensitive s)

    let toRegexPattern s =
        let len = s |> String.length
        let div = len/2 + (len%2)
        let a = s.Substring(0, div)
        let b = s.Substring(div)
        if ((a+a+b).StartsWith(a+b)) 
        then sprintf "(%s)+%s" a b
        else sprintf "(%s)+" s

module HighlightedText =

    type private ApplyTextFilter = TextFilter -> string -> HighlightedText
    let applyFilter : ApplyTextFilter = fun q s ->
        let (TextFilter.CaseInsensitive filter) = q
        let regex = 
            Regex.Escape(filter)
            |> TextFilter.toRegexPattern
            |> fun pattern -> new Regex(pattern, RegexOptions.IgnoreCase)
        let endIndex (m:Match) = m.Index + m.Length
        seq {
            let ms = regex.Matches(s)
            if ms.Count = 0 then
                if s |> String.length > 0 then
                    yield Span.regular s
            else
                for i = 0 to (ms.Count-1) do
                    let curr = ms.[i]
                    if i = 0 then
                        if curr.Index > 0 then
                            yield Span.regular (s.Substring(0, curr.Index))
                    else
                        let prevEnd = ms.[i-1] |> endIndex
                        let currStart = ms.[i].Index
                        yield Span.regular (s.Substring(prevEnd, currStart - prevEnd))
                    yield Span.highlight (curr.Value)
                let lastEnd = ms.[ms.Count-1] |> endIndex
                if lastEnd < s.Length then
                    yield Span.regular (s.Substring(lastEnd, s.Length - lastEnd))
        }

module Tests = 

    open Xunit
    open FsUnit

    module SpanTests = 

        [<Fact>]
        let ``create - if empty string throw`` () =
            (fun () -> Span.highlight "" |> ignore) |> should throw typeof<Exception>
            (fun () -> Span.regular "" |> ignore) |> should throw typeof<Exception>

        [<Fact>]
        let ``create - can include a bunch of whitespace`` () =
            let whitespace = "   "
            (Span.highlight whitespace).Text |> should equal whitespace
            (Span.regular whitespace).Text |> should equal whitespace

    module TextFilterTests =

        [<Theory>]
        [<InlineData("")>]
        [<InlineData("    ")>]
        let ``create - if only whitespace return error`` (s:string) =
            s
            |> TextFilter.create
            |> Result.errorValue
            |> should equal (Some FilterIsEmptyOrWhitespace)

        [<Theory>]
        [<InlineData("a")>]
        [<InlineData("ab")>]
        [<InlineData("ab cd")>]
        [<InlineData("  abcd  ")>]
        let ``create - store text exactly as specified without trimming`` (s:string) =
            s
            |> TextFilter.create
            |> Result.okValue
            |> should equal (Some (TextFilter.CaseInsensitive s))

        [<Theory>]
        [<InlineData("a", "(a)+")>]
        [<InlineData("ab", "(ab)+")>]
        [<InlineData("abc", "(abc)+")>]
        [<InlineData("aa", "(a)+a")>]
        [<InlineData("aba", "(ab)+a")>] 
        [<InlineData("abcabc", "(abc)+abc")>]
        [<InlineData("aaa", "(aa)+a")>]
        let ``toRegexPattern`` (filter:string) (expected:string) =
            filter 
            |> TextFilter.toRegexPattern
            |> should equal expected

    module HighlightedTextTests =

        let private parseSpan (s:String) =
            if s.[0] = '!' then (s.Substring(1) |> Span.highlight)
            else s |> Span.regular

        let private parseFormattedText (s:String) = 
            s.Split(',',System.StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map parseSpan

        [<Fact>]
        let ``applyFilter - when search an empty string find nothing``() =
            let filter = TextFilter.create "abc" |> Result.okValueOrThrow
            let highlighter = HighlightedText.applyFilter filter
            let source = ""
            let actual = highlighter source
            actual |> Seq.isEmpty |> should equal true

        [<Theory>]
        [<InlineData("Once at beginning", "123xyz","123", "!123,xyz")>]
        [<InlineData("Once at end", "123xyz","xyz", "123,!xyz")>]
        [<InlineData("At beginning and end", "123xyz123","123", "!123,xyz,!123")>]
        [<InlineData("Case insensitive", "abcxyz","XYZ", "abc,!xyz")>]
        [<InlineData("Overlapping matches 1", "aaaaaa","aa", "!aaaaaa")>]
        [<InlineData("Overlapping matches 2", "ababaccaba","aba", "!ababa,cc,!aba")>]
        [<InlineData("Overlapping matches 3", "aaabaaaab","aa","!aaa,b,!aaaa,b")>]
        [<InlineData("Complex case 1", "apple pleasant plum","pl", "ap,!pl,e ,!pl,easant ,!pl,um")>]
        [<InlineData("Not found at all", "abc","x","abc")>]
        [<InlineData("Query is same as entire source", "abc","abc","!abc")>]
        [<InlineData("Search for regex characters", "abc^$()abc","^$()", "abc,!^$(),abc")>]
        let ``applyFilter`` (comment:string) (source:string) (filter:string) (expected:string) =
            let format (s:Span) = 
                match s.Format with
                | Highlight -> sprintf "!%s" s.Text
                | Regular -> s.Text
            let expected = 
                expected
                |> parseFormattedText
                |> Seq.map format
            let actual =
                filter
                |> TextFilter.create
                |> Result.map HighlightedText.applyFilter
                |> Result.map (fun h -> h source)
                |> Result.map (fun r -> r |> Seq.map format)
                |> Result.okValueOrThrow
            expected
            |> should equivalent actual
