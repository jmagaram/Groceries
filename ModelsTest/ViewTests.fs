namespace ModelsTest

open System
open Xunit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit
open Models
open Models.CoreTypes
open Models.ValidationTypes

module HighlighterTests =

    open Models
    open Models.ViewTypes
    open Generators

    [<Fact>]
    let ``find - when search an empty string find nothing`` () =
        let searchTerm =
            "abc" |> SearchTerm.tryParse |> Result.okOrThrow

        let highlighter = searchTerm |> Highlighter.find
        let source = ""

        source
        |> highlighter
        |> FormattedText.spans
        |> List.length
        |> should equal 0

    [<Fact>]
    let ``find - when search a whitespace string find exactly that`` () =
        let searchTerm =
            "abc" |> SearchTerm.tryParse |> Result.okOrThrow

        let highlighter = searchTerm |> Highlighter.find
        let source = "     "

        let actual =
            source |> highlighter |> FormattedText.spans

        let expected = [ TextSpan.normal source ]
        actual |> should equal expected

    let parseSpan (s: String) =
        if s.[0] = '!' then (s.Substring(1) |> TextSpan.highlight) else s |> TextSpan.normal

    let parseFormattedText (s: String) =
        s.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
        |> Seq.map parseSpan

    let formatSpan (s: TextSpan) =
        match s.Format with
        | Highlight -> sprintf "!%s" s.Text
        | Normal -> s.Text

    let expectedSpans expected =
        expected
        |> parseFormattedText
        |> Seq.map formatSpan
        |> List.ofSeq

    [<Theory>]
    [<InlineData("Once at beginning", "123xyz", "123", "!123,xyz")>]
    [<InlineData("Once at end", "123xyz", "xyz", "123,!xyz")>]
    [<InlineData("At beginning and end", "123xyz123", "123", "!123,xyz,!123")>]
    [<InlineData("Case insensitive", "abcxyz", "XYZ", "abc,!xyz")>]
    [<InlineData("Overlapping matches 1", "aaaaaa", "aa", "!aaaaaa")>]
    [<InlineData("Overlapping matches 2", "ababaccaba", "aba", "!ababa,cc,!aba")>]
    [<InlineData("Overlapping matches 3", "aaabaaaab", "aa", "!aaa,b,!aaaa,b")>]
    [<InlineData("Overlapping matches 4", "xxaabbbaabbbaaxx", "aabbbaa", "xx,!aabbbaabbbaa,xx")>]
    [<InlineData("Overlapping matches 5", "xxabbbbabbbbaxx", "abbbba", "xx,!abbbbabbbba,xx")>]
    [<InlineData("Overlapping matches 6",
                 "baaaaacaaaccaa aaacaaa aac b",
                 "aa",
                 "b,!aaaaa,c,!aaa,cc,!aa, ,!aaa,c,!aaa, ,!aa,c b")>]
    [<InlineData("Overlapping matches 7", "AaA", "aA", "!AaA")>]
    [<InlineData("Overlapping matches 8", "AbaaaAAb aabcb", "aAA", "Ab,!aaaAA,b aabcb")>]
    [<InlineData("Overlapping matches 9", "AAAa", "aaa", "!AAAa")>]
    [<InlineData("Overlapping matches 10", "abaababa", "aba", "!abaababa")>]
    [<InlineData("Overlapping matches 11", "abababaaba", "aba", "!abababaaba")>]
    [<InlineData("Complex case 1", "apple pleasant plum", "pl", "ap,!pl,e ,!pl,easant ,!pl,um")>]
    [<InlineData("Not found at all", "abc", "x", "abc")>]
    [<InlineData("Query is same as entire source", "abc", "abc", "!abc")>]
    [<InlineData("Search for regex characters", "abc^$()[]\/?.+*abc", "^$()[]\/?.+*", "abc,!^$()[]\/?.+*,abc")>]
    let ``find - specific examples`` (comment: string) source searchTerm expected =
        let highlighter =
            searchTerm
            |> SearchTerm.tryParse
            |> Result.map Highlighter.find
            |> Result.okOrThrow

        let actualSpans =
            source
            |> highlighter
            |> FormattedText.spans
            |> List.map formatSpan

        let expectedSpans = expected |> expectedSpans

        actualSpans |> should equal expectedSpans

    [<Theory>]
    [<InlineData("One terms", "abcabcabc", "b", "a,!b,ca,!b,ca,!b,c")>]
    [<InlineData("Two terms", "ab cd ef ab cd ef ab cd ef", "ab|ef", "!ab, cd ,!ef, ,!ab, cd ,!ef, ,!ab, cd ,!ef")>]
    [<InlineData("Three terms", "abcdeabcde", "a|c|e", "!a,b,!c,d,!ea,b,!c,d,!e")>]
    [<InlineData("One term prefixes another, prefix first", "abcabcabc", "a|ab", "!ab,c,!ab,c,!ab,c")>]
    [<InlineData("One term prefixes another, prefix last", "abcabcabc", "ab|a", "!ab,c,!ab,c,!ab,c")>]
    [<InlineData("One term contained in another, container first", "abcabcabc", "abc|b", "!abcabcabc")>]
    [<InlineData("One term contained in another, contained first", "abcabcabc", "b|abc", "!abcabcabc")>]
    [<InlineData("Found by fscheck","daeddaaebaba","ae|ed","d,!aed,da,!ae,baba")>]
    let ``findAny - specific examples`` (comment: string) source (searchTerm: string) expected =
        let highlighter =
            searchTerm.Split('|', StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map SearchTerm.tryParse
            |> Result.fromResults
            |> Result.map Highlighter.findAny
            |> Result.okOrThrow

        let actualSpans =
            source
            |> highlighter
            |> FormattedText.spans
            |> List.map formatSpan

        let expectedSpans = expected |> expectedSpans

        actualSpans |> should equal expectedSpans

    type SingleTermHighlighterTest =
        { SearchTerm: SearchTerm
          Source: string }

    type HighlighterTest =
        { SearchTerms: SearchTerm list
          Source: string }

    let toSingleTermHighlighterTest (ht: HighlighterTest) =
        { SingleTermHighlighterTest.SearchTerm = ht.SearchTerms.Head
          Source = ht.Source }

    let veryLimitedCharGens = [| Gen.elements "abcde" |]

    let regularEnglishCharGens =
        "abcde"
        |> Seq.append "ABCDE"
        |> Seq.append "012345"
        |> Seq.append " *-/()#;.[]"
        |> List.ofSeq
        |> Gen.elements
        |> fun g -> [| g |]

    let comprehensiveCharGens =
        let abcd = Gen.elements [ 'a'; 'b'; 'c'; 'd' ]

        let std =
            letterOrDigit.Select(fun (LetterOrDigit c) -> c)

        let rgx =
            regexEscape.Select(fun (RegexEscape c) -> c)

        let punc =
            punctuation.Select(fun (Punctuation c) -> c)

        let sym = symbol.Select(fun (Symbol c) -> c)
        let space = Gen.constant ' '

        [ (3, abcd)
          (1, Gen.frequency [ (10, std); (1, space) ])
          (1, rgx)
          (1, Gen.oneof [ rgx; punc; sym; space ])
          (1, Gen.oneof [ std; rgx; punc; sym; space ]) ]
        |> Seq.map (fun (n, g) -> Seq.replicate n g)
        |> Seq.concat
        |> Array.ofSeq

    let highlighterTestGen termCount (charGens: Gen<char> []) =
        gen {
            let cr = Gen.constant '\r'
            let lf = Gen.constant '\n'
            let! charGenIndex = Gen.choose (0, charGens.Length - 1)
            let termChars = charGens.[charGenIndex]
            let minLen = SearchTerm.rules.MinLength |> int
            let maxLen = SearchTerm.rules.MaxLength |> int

            let! searchTermLen =
                Gen.frequency [ (1, Gen.choose (minLen, minLen + 5))
                                (1, Gen.choose (minLen, maxLen)) ]

            let! sourceLen =
                Gen.frequency [ (1, Gen.choose (1, 10))
                                (1, Gen.choose (11, 200)) ]

            let! searchTerms =
                termChars
                |> Gen.listOfLength searchTermLen
                |> Gen.map (fun cs -> cs |> Array.ofList |> String)
                |> Gen.map SearchTerm.tryParse
                |> Gen.filter (fun r -> r |> Result.isOk)
                |> Gen.map (fun r -> r |> Result.okOrThrow)
                |> Gen.listOfLength termCount


            let sourceChars =
                Gen.frequency [ (20, termChars)
                                (1, cr)
                                (1, lf) ]

            let! source =
                sourceChars
                |> Gen.listOfLength sourceLen
                |> Gen.map (fun cs -> cs |> Array.ofList |> String)

            return
                { SearchTerms = searchTerms
                  Source = source }
        }

    type ComprehensiveOneTerm = ComprehensiveOneTerm of SingleTermHighlighterTest
    type CommonEnglishOneTerm = CommonEnglishOneTerm of SingleTermHighlighterTest
    type VeryLimitedOneTerm = VeryLimitedOneTerm of SingleTermHighlighterTest
    type VeryLimitedThreeTerm = VeryLimitedThreeTerm of HighlighterTest

    type Generators =
        static member ComprehensiveOneTerm =
            comprehensiveCharGens
            |> highlighterTestGen 1
            |> Gen.map toSingleTermHighlighterTest
            |> Gen.map ComprehensiveOneTerm
            |> Arb.fromGen

        static member CommonEnglishOneTerm =
            regularEnglishCharGens
            |> highlighterTestGen 1
            |> Gen.map toSingleTermHighlighterTest
            |> Gen.map CommonEnglishOneTerm
            |> Arb.fromGen

        static member VeryLimitedOneTerm =
            veryLimitedCharGens
            |> highlighterTestGen 1
            |> Gen.map toSingleTermHighlighterTest
            |> Gen.map VeryLimitedOneTerm
            |> Arb.fromGen

        static member VeryLimitedThreeTerm =
            veryLimitedCharGens
            |> highlighterTestGen 3
            |> Gen.map VeryLimitedThreeTerm
            |> Arb.fromGen

    [<Property(MaxTest = 1000, Arbitrary = [| typeof<Generators> |])>]
    let ``find - concatenated spans equal source`` (ComprehensiveOneTerm p) =
        p.Source
        |> Highlighter.find p.SearchTerm
        |> FormattedText.spans
        |> Seq.fold (fun total i -> total + i.Text) ""
        |> fun x -> x = p.Source

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``find - when search in regular spans will find nothing`` (ComprehensiveOneTerm p) =
        p.Source
        |> Highlighter.find p.SearchTerm
        |> FormattedText.spans
        |> Seq.choose
            (fun i ->
                match i.Format with
                | TextFormat.Normal -> Some i.Text
                | _ -> None)
        |> Seq.forall
            (fun t ->
                match t
                      |> Highlighter.find p.SearchTerm
                      |> FormattedText.spans
                      |> Seq.tryExactlyOne with
                | Some r -> r.Format = TextFormat.Normal
                | None -> false)

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``find - if highlight is longer than search term, then overlapping or back to back terms in source`` (VeryLimitedOneTerm p)
                                                                                                             =
        let highlighter = p.SearchTerm |> Highlighter.find

        let chooseLongHighlight searchTerm span =
            match span.Format with
            | Normal -> None
            | Highlight ->
                let termLen =
                    searchTerm |> SearchTerm.value |> String.length

                let spanLen = span.Text.Length
                if spanLen > termLen then Some span.Text else None

        let endsWithHighlight s =
            s
            |> highlighter
            |> FormattedText.spans
            |> List.tryLast
            |> Option.map (fun i -> i.Format = TextFormat.Highlight)
            |> Option.defaultValue false

        let rightmostSubstrings minLen s =
            { 0 .. String.length (s) - minLen }
            |> Seq.map (fun i -> s.Substring(i))
            |> List.ofSeq

        let searchLen =
            p.SearchTerm |> SearchTerm.value |> String.length

        p.Source
        |> highlighter
        |> FormattedText.spans
        |> Seq.choose (chooseLongHighlight p.SearchTerm)
        |> Seq.map (rightmostSubstrings searchLen)
        |> Seq.concat
        |> Seq.forall endsWithHighlight

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``find - results do not depend on case of search term`` (ComprehensiveOneTerm p) =
        let mapSearchTerm mapping st =
            st
            |> SearchTerm.value
            |> mapping
            |> SearchTerm.tryParse
            |> Result.okOrThrow

        let upper =
            p.SearchTerm
            |> mapSearchTerm (fun t -> t.ToUpper())

        let lower =
            p.SearchTerm
            |> mapSearchTerm (fun t -> t.ToLower())

        let withUpperFilter =
            p.Source
            |> Highlighter.find upper
            |> FormattedText.spans
            |> List.ofSeq

        let withLowerFilter =
            p.Source
            |> Highlighter.find lower
            |> FormattedText.spans
            |> List.ofSeq

        withUpperFilter = withLowerFilter

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``find - results do not depend on case of source text`` (CommonEnglishOneTerm p) =
        let highlighter = p.SearchTerm |> Highlighter.find

        let spanToLower span =
            { span with
                  TextSpan.Text = span.Text.ToLowerInvariant() }

        let withUpperSource =
            p.Source.ToUpper()
            |> highlighter
            |> FormattedText.spans
            |> Seq.map spanToLower
            |> List.ofSeq

        let withLowerSource =
            p.Source.ToLower()
            |> highlighter
            |> FormattedText.spans
            |> Seq.map spanToLower
            |> List.ofSeq

        withUpperSource = withLowerSource

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``findAny - highlighted chars with all terms is set union of each term`` (VeryLimitedThreeTerm p) =
        let highlightedCharIndexes (f: FormattedText) =
            f
            |> FormattedText.spans
            |> Seq.collect (fun s -> Seq.replicate s.Text.Length s.Format)
            |> Seq.indexed
            |> Seq.choose (fun (i, j) -> if j = TextFormat.Highlight then Some i else None)
            |> Set.ofSeq

        let findAny = p.SearchTerms |> Highlighter.findAny

        let highlightedAllTerms =
            p.Source |> findAny |> highlightedCharIndexes

        let highlightedUnion =
            p.SearchTerms
            |> Seq.map
                (fun t ->
                    let findOne = Highlighter.find t
                    let result = findOne p.Source
                    result |> highlightedCharIndexes)
            |> Seq.fold (fun t i -> Set.union t i) Set.empty

        highlightedAllTerms = highlightedUnion

module SetStringTests =

    let testData =
        [ ("a,b;c|d", "a|b|c|d")
          ("", "")
          (",,,,", "")
          ("a|||b", "a|b")
          ("a|   |     |b", "a|b")
          ("a", "a")
          (" a    ", "a")
          ("a|b|c", "a|b|c")
          (" a  |  b|c    ", "a|b|c")
          ("a|b|b|c|c|a", "a|b|c")
          ("a|B|b|c|C|a", "a|B|c") ]
        |> Seq.map (fun i -> [| i |> fst; i |> snd |])

    [<Theory>]
    [<MemberData(nameof (testData))>]
    let ``fromItems - normalize each, remove empty and duplicates by case, insert separator`` (source: string)
                                                                                              (expected: string)
                                                                                              =
        let items = source.Split('|', ';', ',')

        let result =
            SetString.fromItems String.trim "|" items

        result |> should equal expected

    [<Theory>]
    [<MemberData(nameof (testData))>]
    let ``createFromString - split on delimeters, normalize each, remove duplicates by case, insert separator`` (source: string)
                                                                                                                (expected: string)
                                                                                                                =
        let splitOn = [| "|"; ","; ";" |]
        let delimeter = "|"

        let result =
            SetString.fromString String.trim splitOn delimeter source

        result |> should equal expected

    [<Theory>]
    [<MemberData(nameof (testData))>]
    let ``toItems - split on delimeters, normalize each, remove duplicates by case`` (source: string)
                                                                                     (expected: string)
                                                                                     =
        let splitOn = [| "|"; ","; ";" |]

        let result =
            SetString.toItems String.trim splitOn source
            |> List.ofSeq

        let expected =
            expected.Split('|', StringSplitOptions.RemoveEmptyEntries)
            |> List.ofSeq

        result |> should equal expected

module SetManagerTests =

    open Models.SetManager
    open CoreTypes

    let normalizer = String.trim
    let delimiter = ","
    let splitOn = [| ","; ";" |]
    let parseNeverFail s = Ok s

    [<Theory>]
    [<InlineData("unchanged", "a,b,c", "a,b,c", "a,b,c", "", "")>]
    [<InlineData("empty", "", "", "", "", "")>]
    [<InlineData("case renames", "a,b,c", "A,B,C", "", "A,B,C", "a:A,b:B,c:C")>]
    [<InlineData("identify new items", "a,b,c", "a,b,c,d,e,f", "a,b,c", "d,e,f", "")>]
    [<InlineData("identify deletions", "a,b,c", "c,d,e", "c", "d,e", "a:_,b:_")>]
    [<InlineData("delete everything", "a,b,c", "", "", "", "a:_,b:_,c:_")>]
    [<InlineData("mix", "a,b,c", "B,c,d,e", "c", "B,d,e", "b:B,a:_")>]
    let summaryFromBulkEdit comment original proposed unchanged created moved =
        let asSet (s: string) =
            if (s |> String.isNullOrWhiteSpace) then
                Set.empty
            else
                s.Split(',', StringSplitOptions.RemoveEmptyEntries)
                |> Set.ofSeq

        let parseMove (s: string) =
            let parts = s.Split(':')
            let source = parts.[0]

            let target =
                if parts.[1] = "_" then None else Some parts.[1]

            (source, target)

        let result =
            SetBulkEditForm.create normalizer delimiter (original |> asSet)
            |> SetBulkEditForm.update normalizer splitOn delimiter (TextBoxMessage.TypeText proposed)
            |> SetBulkEditForm.update normalizer splitOn delimiter TextBoxMessage.LoseFocus
            |> SetBulkEditForm.summary normalizer splitOn parseNeverFail
            |> Option.get

        result.Create |> should equal (created |> asSet)

        result.Unchanged
        |> should equal (unchanged |> asSet)

        result.MoveOrDelete
        |> should equal (moved |> asSet |> Seq.map parseMove |> Map.ofSeq)
