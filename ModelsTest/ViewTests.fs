namespace ModelsTest

open System
open Xunit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit
open Models
open Models.CoreTypes
open Models.ValidationTypes

module ReactiveTests =

    open System.Reactive.Linq
    open FSharp.Control.Reactive

    [<Fact>]
    let ``can create sample data`` () =
        let x = Models.State.createSampleData
        true

    [<Fact>]
    let ``distinctUntilChanged uses F# equality`` () =
        let x = [ 1; 2; 3; 4 ]
        let y = [ 1; 2; 3; 4 ]
        let bs = x |> Subject.behavior
        let obs = bs |> Observable.distinctUntilChanged
        let mutable count = 0
        use sub = obs |> Observable.subscribe (fun i -> count <- count + 1)
        bs.OnNext(y)
        bs.OnNext(x)
        bs.OnNext(y)
        count |> should equal 1

module HighlighterTests =

    open Models
    open Models.ViewTypes
    open Generators

    [<Fact>]
    let ``highlighter - when search an empty string find nothing`` () =
        let searchTerm = "abc" |> SearchTerm.tryParse |> Result.okOrThrow
        let highlighter = searchTerm |> Highlighter.create
        let source = ""

        source
        |> highlighter
        |> FormattedText.spans
        |> List.length
        |> should equal 0

    [<Fact>]
    let ``highlighter - when search a whitespace string find exactly that`` () =
        let searchTerm = "abc" |> SearchTerm.tryParse |> Result.okOrThrow
        let highlighter = searchTerm |> Highlighter.create
        let source = "     "
        let actual = source |> highlighter |> FormattedText.spans
        let expected = [ TextSpan.normal source ]
        actual |> should equal expected

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
    let ``highlighter - specific examples`` (comment: string) source searchTerm expected =
        let parseSpan (s: String) =
            if s.[0] = '!' then (s.Substring(1) |> TextSpan.highlight) else s |> TextSpan.normal

        let parseFormattedText (s: String) =
            s.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map parseSpan

        let formatSpan (s: TextSpan) =
            match s.Format with
            | Highlight -> sprintf "!%s" s.Text
            | Normal -> s.Text

        let expected =
            expected
            |> parseFormattedText
            |> Seq.map formatSpan
            |> List.ofSeq

        let highlighter =
            searchTerm
            |> SearchTerm.tryParse
            |> Result.map Highlighter.create
            |> Result.okOrThrow

        let actual =
            source
            |> highlighter
            |> FormattedText.spans
            |> List.map formatSpan

        actual |> should equal expected

    type HighlighterTest = { SearchTerm: SearchTerm; Source: string }

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
        let std = letterOrDigit.Select(fun (LetterOrDigit c) -> c)
        let rgx = regexEscape.Select(fun (RegexEscape c) -> c)
        let punc = punctuation.Select(fun (Punctuation c) -> c)
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

    let highlighterTestGen (charGens: Gen<char> []) =
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

            let! searchTerm =
                termChars
                |> Gen.listOfLength searchTermLen
                |> Gen.map (fun cs -> cs |> Array.ofList |> String)
                |> Gen.map SearchTerm.tryParse
                |> Gen.filter (fun r -> r |> Result.isOk)
                |> Gen.map (fun r -> r |> Result.okOrThrow)

            let sourceChars = Gen.frequency [ (20, termChars); (1, cr); (1, lf) ]

            let! source =
                sourceChars
                |> Gen.listOfLength sourceLen
                |> Gen.map (fun cs -> cs |> Array.ofList |> String)

            return { SearchTerm = searchTerm; Source = source }
        }

    type Comprehensive = Comprehensive of HighlighterTest
    type CommonEnglish = CommonEnglish of HighlighterTest
    type VeryLimited = VeryLimited of HighlighterTest

    type Generators =
        static member Comprehensive() =
            comprehensiveCharGens
            |> highlighterTestGen
            |> Gen.map Comprehensive
            |> Arb.fromGen

        static member CommonEnglish() =
            regularEnglishCharGens
            |> highlighterTestGen
            |> Gen.map CommonEnglish
            |> Arb.fromGen

        static member VeryLimited() =
            veryLimitedCharGens
            |> highlighterTestGen
            |> Gen.map VeryLimited
            |> Arb.fromGen

    [<Property(MaxTest = 1000, Arbitrary = [| typeof<Generators> |])>]
    let ``concatenated spans equal source`` (Comprehensive p) =
        p.Source
        |> Highlighter.create p.SearchTerm
        |> FormattedText.spans
        |> Seq.fold (fun total i -> total + i.Text) ""
        |> fun x -> x = p.Source

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``when search in regular spans will find nothing`` (Comprehensive p) =
        p.Source
        |> Highlighter.create p.SearchTerm
        |> FormattedText.spans
        |> Seq.choose (fun i ->
            match i.Format with
            | TextFormat.Normal -> Some i.Text
            | _ -> None)
        |> Seq.forall (fun t ->
            match t
                  |> Highlighter.create p.SearchTerm
                  |> FormattedText.spans
                  |> Seq.tryExactlyOne with
            | Some r -> r.Format = TextFormat.Normal
            | None -> false)

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``if highlight is longer than search term, then overlapping or back to back terms in source`` (VeryLimited p) =
        let highlighter = p.SearchTerm |> Highlighter.create

        let chooseLongHighlight searchTerm span =
            match span.Format with
            | Normal -> None
            | Highlight ->
                let termLen = searchTerm |> SearchTerm.value |> String.length
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

        let searchLen = p.SearchTerm |> SearchTerm.value |> String.length

        p.Source
        |> highlighter
        |> FormattedText.spans
        |> Seq.choose (chooseLongHighlight p.SearchTerm)
        |> Seq.map (rightmostSubstrings searchLen)
        |> Seq.concat
        |> Seq.forall endsWithHighlight

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``results do not depend on case of search term`` (Comprehensive p) =
        let mapSearchTerm mapping st =
            st
            |> SearchTerm.value
            |> mapping
            |> SearchTerm.tryParse
            |> Result.okOrThrow

        let upper = p.SearchTerm |> mapSearchTerm (fun t -> t.ToUpper())

        let lower = p.SearchTerm |> mapSearchTerm (fun t -> t.ToLower())

        let withUpperFilter =
            p.Source
            |> Highlighter.create upper
            |> FormattedText.spans
            |> List.ofSeq

        let withLowerFilter =
            p.Source
            |> Highlighter.create lower
            |> FormattedText.spans
            |> List.ofSeq

        withUpperFilter = withLowerFilter

    [<Property(MaxTest = 10000, Arbitrary = [| typeof<Generators> |])>]
    let ``results do not depend on case of source text`` (CommonEnglish p) =
        let highlighter = p.SearchTerm |> Highlighter.create

        let spanToLower span = { span with TextSpan.Text = span.Text.ToLowerInvariant() }

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

        let result = SetString.fromItems String.trim "|" items

        result |> should equal expected

    [<Theory>]
    [<MemberData(nameof (testData))>]
    let ``createFromString - split on delimeters, normalize each, remove duplicates by case, insert separator`` (source: string)
                                                                                                                (expected: string)
                                                                                                                =
        let splitOn = [| "|"; ","; ";" |]
        let delimeter = "|"

        let result = SetString.fromString String.trim splitOn delimeter source

        result |> should equal expected

    [<Theory>]
    [<MemberData(nameof (testData))>]
    let ``toItems - split on delimeters, normalize each, remove duplicates by case`` (source: string) (expected: string) =
        let splitOn = [| "|"; ","; ";" |]
        let result = SetString.toItems String.trim splitOn source |> List.ofSeq

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
            let target = if parts.[1] = "_" then None else Some parts.[1]
            (source, target)

        let result =
            BulkEdit.create normalizer delimiter (original |> asSet)
            |> BulkEdit.update normalizer splitOn delimiter (TextBoxMessage.TypeText proposed)
            |> BulkEdit.update normalizer splitOn delimiter TextBoxMessage.LoseFocus
            |> BulkEdit.summary normalizer splitOn parseNeverFail
            |> Option.get

        result.Create |> should equal (created |> asSet)
        result.Unchanged |> should equal (unchanged |> asSet)

        result.MoveOrDelete
        |> should equal (moved |> asSet |> Seq.map parseMove |> Map.ofSeq)
