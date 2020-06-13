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

type FormattedString = Span list

module Span = 

    let private create f s =
        match s |> String.length with
        | 0 -> failwith "A span must have at least 1 character."
        | _ -> { Text = s; Format = f }

    let highlight s = s |> create Highlight
    
    let regular s = s |> create Regular 

    let formatOf (s:Span) = s.Format

    let textOf (s:Span) = s.Text

    let tryMerge s1 s2 =
        if (s1 |> formatOf) = (s2 |> formatOf) 
        then Some { s1 with Text = s1.Text + s2.Text }
        else None

// can a query have nothing in it and if so, what does that mean?
type Query = | CaseInsensitive of string

let createQuery s = CaseInsensitive s

// what happens if the source string is empty? should return empty
//type FindText = Query -> string -> Span seq

let createRegexPatternFromQuery s =
    let len = s |> String.length
    let div = len/2 + (len%2)
    let a = s.Substring(0, div)
    let b = s.Substring(div)
    if ((a+a+b).StartsWith(a+b)) 
    then sprintf "(%s)+%s" a b
    else sprintf "(%s)+" s

let findText (CaseInsensitive query) (s:string) =
    let regex = 
        Regex.Escape(query)
        |> createRegexPatternFromQuery
        |> fun pattern -> new Regex(pattern, RegexOptions.IgnoreCase)
    seq {
        let matches = regex.Matches(s)
        if matches.Count = 0
        then yield s |> Span.regular
        else
            for i = 0 to (matches.Count-1) do
                if i = 0 && matches.[0].Index > 0 then
                    yield (s.Substring(0, matches.[0].Index) |> Span.regular)
                if i <> 0 then
                    let previousMatchEnded = 
                        let m = matches.[i-1]
                        m.Index + m.Length
                    let currentMatchStarts = matches.[i].Index
                    let length = currentMatchStarts - previousMatchEnded
                    let nonMatch = s.Substring(previousMatchEnded, length)
                    yield nonMatch |> Span.regular
                yield (matches.[i].Value |> Span.highlight)
            let lastMatch = matches.[matches.Count-1]
            let lastMatchEnded = lastMatch.Index + lastMatch.Length
            if lastMatchEnded < s.Length then
                let r = s.Substring(lastMatchEnded, s.Length - lastMatchEnded)
                yield r |> Span.regular
    }

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

module Tests = 

    open Xunit
    open FsUnit

    [<Theory>]
    [<InlineData("")>]
    [<InlineData("    ")>]
    let ``CreateQuery - if only whitespace return none`` (s:string) =
        s
        |> createQuery
        |> should equal None

    [<Fact>]
    let ``CreateQuery - strips whitespace from beginning and end`` () =
        let (CaseInsensitive s) = "  abc " |> createQuery
        should equal "abc"

    let convertStringToSpan (s:String) =
        if s.[0] = '!' then (s.Substring(1) |> Span.highlight)
        else s |> Span.regular

    let parseSpans (s:String) = 
        s.Split(',',System.StringSplitOptions.RemoveEmptyEntries)
        |> Seq.map convertStringToSpan

    [<Theory>]
    [<InlineData("a", "(a)+")>]
    [<InlineData("ab", "(ab)+")>]
    [<InlineData("abc", "(abc)+")>]
    [<InlineData("aa", "(a)+a")>]
    [<InlineData("aba", "(ab)+a")>] 
    [<InlineData("abcabc", "(abc)+abc")>]
    [<InlineData("aaa", "(aa)+a")>]
    let ``createRegexPatternFromQuery`` (query:string) (expected:string) =
        let result = query |> createRegexPatternFromQuery
        result
        |> should equal expected

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
    let ``findText`` (comment:string) (source:string) (query:string) (expected:string) =
        let format (s:Span) = 
            match s.Format with
            | Highlight -> sprintf "!%s" s.Text
            | Regular -> s.Text
        let expected = 
            expected
            |> parseSpans
            |> Seq.map format
        let query = Query.CaseInsensitive query
        let actual = 
            source
            |> findText query
            |> Seq.map format
        actual
        |> should equivalent expected

// test that whitespace query can NOT be created
//[<InlineData("A whitespace query is sometimes found", "a  a  a  a"," ", "a,!  ,a,!  ,a,!  ,a")>]
// an empty query should be invalid
//[<InlineData("An empty query is never found", "xyz","", "xyz")>]

    //[<Theory>]
    //[<InlineData("abc")>]
    //[<InlineData("")>]
    //let ``When search for something in empty string return empty`` (query:string) =
    //    let expected = Seq.empty
    //    let actual = "" |> findTextCore (Query.CaseInsensitive query)
    //    actual |> should equivalent expected

