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

type FormattedText = Span seq

type Query = | CaseInsensitive of string

type QueryError = | QueryIsOnlyWhitespace

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

module Query =

    type private Create = string -> Result<Query, QueryError>
    let create : Create = fun s -> 
        match s |> isNullOrWhiteSpace with
        | true -> Error QueryIsOnlyWhitespace
        | false -> Ok (Query.CaseInsensitive s)

module FormattedText =

    let createRegexPatternFromQuery s =
        let len = s |> String.length
        let div = len/2 + (len%2)
        let a = s.Substring(0, div)
        let b = s.Substring(div)
        if ((a+a+b).StartsWith(a+b)) 
        then sprintf "(%s)+%s" a b
        else sprintf "(%s)+" s

    type private HighlightMatches = Query -> string -> FormattedText
    let highlightMatches : HighlightMatches = fun q s ->
        let (Query.CaseInsensitive query) = q
        let regex = 
            Regex.Escape(query)
            |> createRegexPatternFromQuery
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

    module QueryTests =

        [<Theory>]
        [<InlineData("")>]
        [<InlineData("    ")>]
        let ``create - if only whitespace return error`` (s:string) =
            s
            |> Query.create
            |> Result.errorValue
            |> should equal (Some QueryIsOnlyWhitespace)

        [<Theory>]
        [<InlineData("a")>]
        [<InlineData("ab")>]
        [<InlineData("ab cd")>]
        [<InlineData("  abcd  ")>]
        let ``create - store text exactly as specified without trimming`` (s:string) =
            s
            |> Query.create
            |> Result.okValue
            |> should equal (Some (Query.CaseInsensitive s))

    module FormattedTextTests =

        let parseSpan (s:String) =
            if s.[0] = '!' then (s.Substring(1) |> Span.highlight)
            else s |> Span.regular

        let parseFormattedText (s:String) = 
            s.Split(',',System.StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map parseSpan

        [<Theory>]
        [<InlineData("a", "(a)+")>]
        [<InlineData("ab", "(ab)+")>]
        [<InlineData("abc", "(abc)+")>]
        [<InlineData("aa", "(a)+a")>]
        [<InlineData("aba", "(ab)+a")>] 
        [<InlineData("abcabc", "(abc)+abc")>]
        [<InlineData("aaa", "(aa)+a")>]
        let ``createRegexPatternFromQuery`` (query:string) (expected:string) =
            let result = query |> FormattedText.createRegexPatternFromQuery
            result
            |> should equal expected

        [<Fact>]
        let ``highlightMatches - when search in empty string find nothing``() =
            let query = Query.create "abc" |> Result.okValueOrThrow
            let highlighter = FormattedText.highlightMatches query
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
        let ``highlightMatches`` (comment:string) (source:string) (query:string) (expected:string) =
            let format (s:Span) = 
                match s.Format with
                | Highlight -> sprintf "!%s" s.Text
                | Regular -> s.Text
            let expected = 
                expected
                |> parseFormattedText
                |> Seq.map format
            let actual =
                query
                |> Query.create
                |> Result.map FormattedText.highlightMatches
                |> Result.map (fun h -> h source)
                |> Result.map (fun r -> r |> Seq.map format)
                |> Result.okValueOrThrow
            expected
            |> should equivalent actual
