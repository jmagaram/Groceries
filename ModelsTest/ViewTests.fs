namespace ModelsTest

open System
open Xunit
open FsUnit
open FsUnit.Xunit
open Models.ValidationTypes
open FsCheck
open FsCheck.Xunit

module FormattedTextTests =
    
    open Models
    open Models.ViewTypes
    open Models.FormattedText

    let private parseSpan (s:String) =
        if s.[0] = '!' then (s.Substring(1) |> TextSpan.highlight)
        else s |> TextSpan.normal

    let private parseFormattedText (s:String) = 
        s.Split(',',System.StringSplitOptions.RemoveEmptyEntries)
        |> Seq.map parseSpan

    //[<Fact>]
    //let ``applyFilter - when search an empty string find nothing``() =
    //    let filter = TextFilter.create "abc" |> Result.okValueOrThrow
    //    let highlighter = HighlightedText.applyFilter filter
    //    let source = ""
    //    let actual = highlighter source
    //    actual |> Seq.isEmpty |> should equal true

    [<Fact>]
    let ``applyFilter - when search a whitespace string find just that``() =
        let searchTerm = SearchTerm.tryParse "abc" |> Result.okOrThrow
        let highlighter = FormattedText.applyFilter searchTerm
        let source = "     "
        let actual = highlighter source |> List.ofSeq
        actual 
        |> should equal [ TextSpan.normal source ]

    [<Theory>]
    [<InlineData("Once at beginning", "123xyz","123", "!123,xyz")>]
    [<InlineData("Once at end", "123xyz","xyz", "123,!xyz")>]
    [<InlineData("At beginning and end", "123xyz123","123", "!123,xyz,!123")>]
    [<InlineData("Case insensitive", "abcxyz","XYZ", "abc,!xyz")>]
    [<InlineData("Overlapping matches 1", "aaaaaa","aa", "!aaaaaa")>]
    [<InlineData("Overlapping matches 2", "ababaccaba","aba", "!ababa,cc,!aba")>]
    [<InlineData("Overlapping matches 3", "aaabaaaab","aa","!aaa,b,!aaaa,b")>]
    [<InlineData("Overlapping matches 4", "xxaabbbaabbbaaxx","aabbbaa", "xx,!aabbbaabbbaa,xx")>]
    [<InlineData("Overlapping matches 5", "xxabbbbabbbbaxx","abbbba", "xx,!abbbbabbbba,xx")>]
    [<InlineData("Overlapping matches 6", "baaaaacaaaccaa aaacaaa aac b","aa", "b,!aaaaa,c,!aaa,cc,!aa, ,!aaa,c,!aaa, ,!aa,c b")>]
    [<InlineData("Overlapping matches 7", "AaA","aA", "!AaA")>]
    [<InlineData("Overlapping matches 8", "AbaaaAAb aabcb", "aAA","Ab,!aaaAA,b aabcb")>]
    [<InlineData("Overlapping matches 9", "AAAa", "aaa","!AAAa")>]
    [<InlineData("Overlapping matches 10", "abaababa", "aba", "!abaababa")>]
    [<InlineData("Overlapping matches 11", "abababaaba", "aba", "!abababaaba")>]
    [<InlineData("Complex case 1", "apple pleasant plum","pl", "ap,!pl,e ,!pl,easant ,!pl,um")>]
    [<InlineData("Not found at all", "abc","x","abc")>]
    [<InlineData("Query is same as entire source", "abc","abc","!abc")>]
    [<InlineData("Search for regex characters", "abc^$()[]\/?.+*abc", "^$()[]\/?.+*", "abc,!^$()[]\/?.+*,abc")>]
    let ``applyFilter with specific examples`` (comment:string) (source:string) (searchTerm:string) (expected:string) =
        let formatSpan (s:TextSpan) = 
            match s.Format with
            | Highlight -> sprintf "!%s" s.Text
            | Normal -> s.Text
        let expected = 
            expected
            |> parseFormattedText
            |> Seq.map formatSpan
            |> List.ofSeq
        let actual =
            let filter = 
                SearchTerm.tryParse searchTerm
                |> Result.okOrThrow
            let result =
                source
                |> FormattedText.applyFilter filter
                |> Seq.map formatSpan
                |> List.ofSeq
            result
        actual
        |> should equal expected

    type ApplyFilterParameters =
        { Filter : SearchTerm
          Source : string }

    let manyChars =
        let knownCharacters = [
            'a';'b';'c';'d'; // space?
            'A';'B';
            '^';'$';'(';')';'*';'+';'?';'.';'\\';'/';'<';'>']
        Gen.frequency [
            (4, Gen.elements (knownCharacters |> Seq.takeAtMost 3));
            (1, Gen.elements (knownCharacters |> Seq.takeAtMost 5));
            (1, Gen.elements (knownCharacters |> Seq.takeAtMost 7));
            (1, Gen.elements (knownCharacters |> Seq.takeAtMost 100));
            (0, Arb.generate<char>);
        ]

    let applyFilterParameters charGen =
        gen {
            let! startFilterChar = charGen |> Gen.filter (fun c -> not (Char.IsWhiteSpace(c)))
            let! endFilterCharLen = Gen.frequency [ 
                (10, Gen.choose(0,2)); 
                ( 5, Gen.choose(3,5));
                ( 1, Gen.choose(6,8));
                ( 1, Gen.choose(9,11));
                ( 1, Gen.choose(12,20))]
            let! endChars = charGen |> Gen.listOfLength endFilterCharLen
            let filter = 
                startFilterChar :: endChars
                |> List.toArray
                |> fun s -> String(s)
                |> SearchTerm

            let! searchTextLen = Gen.frequency [ 
                (1, Gen.choose(0,10)); 
                (1, Gen.choose(11,500))]
            let! searchTextChars = Gen.arrayOfLength searchTextLen charGen
            let searchText = String(searchTextChars)

            return {
                Filter = filter
                Source = searchText
            }
        }

    type Generators =
        static member ApplyFilterParameters() = 
            applyFilterParameters manyChars
            |> Arb.fromGen

    [<Property(MaxTest=1000, Arbitrary=[| typeof<Generators> |] )>]
    let ``applyFilter - concatenated spans equal source`` (p:ApplyFilterParameters) =
        p.Source 
        |> FormattedText.applyFilter p.Filter
        |> Seq.fold (fun total i -> total + i.Text) ""
        |> fun x -> x = p.Source

    [<Property(MaxTest=1000, Arbitrary=[| typeof<TestDataGenerators> |] )>]
    let ``generator exploration`` (items:ListOfLength<char,Num5>) =
        items.Items |> List.length |> should equal 5
