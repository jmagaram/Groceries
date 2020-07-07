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

type SpanError = | SpanHasNoCharacters

type HighlightedText = Span seq

type TextFilter = | CaseInsensitiveTextFilter of string

type TextFilterError = | FilterIsEmptyOrWhitespace

type ItemSummary =
    { Id : ItemId
      Title : HighlightedText
      Quantity : HighlightedText
      Note : HighlightedText
      Repeat : Repeat
      Status : Status }

type View =
    { TextFilterBox : TextBox<TextFilterError>
      Items : ItemSummary seq }

type ViewMessage =
    | ChangeTextFilter of TextBoxMessage

module Span = 

    let private create f s =
        match s |> String.length with
        | 0 -> Error SpanHasNoCharacters
        | _ -> Ok { Text = s; Format = f }

    let highlight s = s |> create Highlight

    let highlightUnsafe s = s |> highlight |> Result.okValueOrThrow
    
    let regular s = s |> create Regular

    let regularUnsafe s = s |> regular |> Result.okValueOrThrow

    let isHighlight s = 
        match s.Format with 
        | Highlight _ -> true 
        | _ -> false

    let asString s = s.Text

module TextFilter =

    type private Create = string -> Result<TextFilter, TextFilterError>
    let create : Create = fun s -> 
        match s |> isNullOrWhiteSpace with
        | true -> Error FilterIsEmptyOrWhitespace
        | false -> Ok (TextFilter.CaseInsensitiveTextFilter s)

    let toLowerRegexPattern (s:string) =
        let s = s.ToLowerInvariant()
        let len = s |> String.length
        let beginEqualsEndChars =
            [len/2..-1..1]
            |> Seq.filter (fun i -> s.Substring(0,i) = s.Substring(len-i))
            |> Seq.tryHead
        match beginEqualsEndChars with
        | None -> sprintf "(%s)+" s
        | Some x -> 
            let a = s.Substring(0,len-x)
            let b = s.Substring(len-x)
            sprintf "(%s)+%s" a b

module HighlightedText =

    type private ApplyTextFilter = TextFilter -> string -> HighlightedText
    let applyFilter : ApplyTextFilter = fun q s ->
        let (CaseInsensitiveTextFilter filter) = q
        let regex = 
            Regex.Escape(filter)
            |> TextFilter.toLowerRegexPattern
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
                        if (prevEnd < currStart) then
                            yield Span.regular (s.Substring(prevEnd, currStart - prevEnd))
                    yield Span.highlight (curr.Value)
                let lastEnd = ms.[ms.Count-1] |> endIndex
                if lastEnd < s.Length then
                    yield Span.regular (s.Substring(lastEnd, s.Length - lastEnd))
        }
        |> Seq.map Result.okValueOrThrow

    let empty = Seq.empty

    let regular s = 
        let result = s |> Span.regular
        match result with
        | Error (SpanError.SpanHasNoCharacters) -> empty
        | Ok s -> s |> Seq.singleton

    let hasHighlights h =
        h
        |> Seq.exists (fun i -> i |> Span.isHighlight)

module ItemSummary =

    type Create = TextFilter option -> DomainTypes.Item -> ItemSummary
    let create : Create = fun f i ->
        let applyFilter =
            match f with
            | Some f -> fun s -> s |> HighlightedText.applyFilter f
            | None -> fun s -> HighlightedText.regular s
        { Id = i.Id
          Title = 
            i.Title 
            |> Title.asString 
            |> applyFilter
          Quantity = 
                i.Quantity 
                |> Option.map Quantity.asString 
                |> Option.map applyFilter
                |> Option.defaultValue HighlightedText.empty
          Note = 
            i.Note
            |> Option.map Note.asString
            |> Option.map applyFilter
            |> Option.defaultValue HighlightedText.empty
          Status = i.Status
          Repeat = i.Repeat }

module FilterTextBox =

    let normalize = trim

    let validate t = 
        t 
        |> TextFilter.create
        |> Result.errorValue

    let create = TextBox.create validate normalize

    let update = TextBox.update validate normalize

module View =

    type Create = View
    let create : Create = 
        { View.Items = Seq.empty
          View.TextFilterBox = FilterTextBox.create }

    type Update = ViewMessage -> DomainTypes.Item seq -> View -> View
    let update : Update = fun msg items view ->
        let textBox' =
            match msg with
            | ChangeTextFilter msg -> view.TextFilterBox |> FilterTextBox.update msg
        let items =
            if (textBox'.NormalizedText = view.TextFilterBox.NormalizedText) // didn't really change anything
            then view.Items
            else 
                let filter = TextFilter.create textBox'.NormalizedText
                match filter with
                | Error FilterIsEmptyOrWhitespace -> Seq.empty
                | Ok filter ->
                    items
                    |> Seq.map (fun i -> (i.Title, i |> ItemSummary.create (Some filter)))
                    |> Seq.filter (fun (_,s) -> s.Title |> HighlightedText.hasHighlights )
                    |> Seq.sortBy (fun (a, _) -> a)
                    |> Seq.map (fun (_, b) -> b)
        { view with 
            TextFilterBox = textBox'
            Items = items }

module Tests = 

    open Xunit
    open FsUnit
    open FsCheck
    open FsCheck.Xunit

    module SpanTests = 

        [<Fact>]
        let ``create - if empty string return error`` () =
            "" |> Span.highlight |> Result.isError |> should equal true
            "" |> Span.regular |> Result.isError |> should equal true

        [<Fact>]
        let ``create - can include a bunch of whitespace`` () =
            let ws = "   "
            ws |> Span.highlightUnsafe |> Span.asString |> should equal ws
            ws |> Span.regularUnsafe |> Span.asString |> should equal ws

        [<Theory>]
        [<InlineData("abc")>]
        [<InlineData("abc")>]
        let ``create - can include many characters`` (s:string) =
            s |> Span.highlightUnsafe |> Span.asString |> should equal s
            s |> Span.regularUnsafe |> Span.asString |> should equal s

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
            |> should equal (Some (TextFilter.CaseInsensitiveTextFilter s))

        [<Theory>]
        [<InlineData("ABC", "(abc)+")>]
        [<InlineData("a", "(a)+")>]
        [<InlineData("ab", "(ab)+")>]
        [<InlineData("abc", "(abc)+")>]
        [<InlineData("aa", "(a)+a")>]
        //[<InlineData("aaa", "(aa)+a")>] don't know what this should be
        [<InlineData("aA", "(a)+a")>]
        [<InlineData("aba", "(ab)+a")>] 
        [<InlineData("abcabc", "(abc)+abc")>]
        [<InlineData("abbbbba", "(abbbbb)+a")>]
        [<InlineData("abxxxxxxxxxab", "(abxxxxxxxxx)+ab")>]
        let ``toLowerRegexPattern`` (filter:string) (expected:string) =
            filter
            |> TextFilter.toLowerRegexPattern
            |> should equal expected

    module HighlightedTextTests =

        let private parseSpan (s:String) =
            if s.[0] = '!' then (s.Substring(1) |> Span.highlightUnsafe)
            else s |> Span.regularUnsafe

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

        [<Fact>]
        let ``applyFilter - when search a whitespace string find just that``() =
            let filter = TextFilter.create "abc" |> Result.okValueOrThrow
            let highlighter = HighlightedText.applyFilter filter
            let source = "     "
            let actual = highlighter source
            actual 
            |> 
            should equivalent [ Span.regularUnsafe source ]

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
        [<InlineData("Complex case 1", "apple pleasant plum","pl", "ap,!pl,e ,!pl,easant ,!pl,um")>]
        [<InlineData("Not found at all", "abc","x","abc")>]
        [<InlineData("Query is same as entire source", "abc","abc","!abc")>]
        [<InlineData("Search for regex characters", "abc^$()[]\/?.+*abc", "^$()[]\/?.+*", "abc,!^$()[]\/?.+*,abc")>]
        let ``applyFilter with specific examples`` (comment:string) (source:string) (filter:string) (expected:string) =
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
            actual
            |> should equivalent expected

        type ApplyFilterParameters =
            { Filter : TextFilter
              Source : string }

        let chooseFromList xs = 
            gen { 
                let! i = Gen.choose (0, List.length xs-1) 
                return List.item i xs 
            }

        let validCharacters = chooseFromList ['a';'A';'b';'c';' '];
        
        let filter =
            gen {
                let! len = Gen.choose (1,10)
                let! chars = Gen.arrayOfLength len validCharacters
                let filter = String(chars).Trim() |> TextFilter.create
                return filter
            }
            |> Gen.filter Result.isOk
            |> Gen.map Result.okValueOrThrow

        let textToSearch =
            gen {
                let! len = Gen.choose (0,30)
                let! chars = Gen.arrayOfLength len validCharacters
                let s = String(chars)
                return s
            }

        let applyFilterParameters =
            gen {
                let! f = filter
                let! s = textToSearch
                let result = 
                    { Filter = f
                      Source = s }
                return result
            }

        type Generators =
            static member ApplyFilterParameters() = Arb.fromGen applyFilterParameters

        [<Property(Arbitrary=[| typeof<Generators> |] )>]
        let ``apply filter - concatenated spans equal source`` (p:ApplyFilterParameters) =
            p.Source 
            |> HighlightedText.applyFilter p.Filter
            |> Seq.fold (fun total i -> total + i.Text) ""
            |> should equal p.Source

        [<Property(Arbitrary=[| typeof<Generators> |] )>]
        let ``applyFilter - spans alternate between highlighted and regular`` (p:ApplyFilterParameters) =
            p.Source 
            |> HighlightedText.applyFilter p.Filter
            |> Seq.pairwise
            |> Seq.exists (fun (a,b) -> a.Format = b.Format)
            |> should equal false

        [<Property(MaxTest=10000, Arbitrary=[| typeof<Generators> |] )>]
        let ``applyFilter - find nothing when search regular results`` (p:ApplyFilterParameters) =
            p.Source
            |> HighlightedText.applyFilter p.Filter
            |> Seq.choose (fun i -> 
                match i.Format with
                | Format.Regular -> Some i.Text
                | _ -> None)
            |> Seq.forall (fun t -> 
                match t |> HighlightedText.applyFilter p.Filter |> Seq.tryExactlyOne with
                | Some r -> r.Format = Format.Regular
                | _ -> false)
            |> should equal true

        let filterLength f = match f with | CaseInsensitiveTextFilter f -> f.Length

        let endsWithMatch filter text  =
            text
            |> HighlightedText.applyFilter filter
            |> Seq.toList
            |> fun ss ->
                match ss with
                | [] -> false
                | [a] -> a.Format = Format.Highlight
                | [_;b] -> b.Format = Format.Highlight
                | _ -> false

        [<Property(MaxTest=10000,Arbitrary=[| typeof<Generators> |] )>]
        let ``applyFilter - if highlight is longer than filter, then overlapping or back to back filters in source`` (p:ApplyFilterParameters) =
            let filterLength = match p.Filter with | CaseInsensitiveTextFilter f -> f.Length
            p.Source
            |> HighlightedText.applyFilter p.Filter
            |> Seq.choose (fun i -> 
                match i.Format with
                | Format.Highlight when i.Text.Length > filterLength -> Some i.Text
                | _ -> None)
            |> Seq.forall (fun t -> 
                let excess = t.Length - filterLength
                [0..excess]
                |> Seq.forall (fun x -> 
                    let res = endsWithMatch p.Filter (t.Substring(x))
                    res))

        [<Property(Arbitrary=[| typeof<Generators> |] )>]
        let ``applyFilter - results do not depend on case of filter`` (p:ApplyFilterParameters) =
            let mapFilter mapping f = 
                match f with 
                | CaseInsensitiveTextFilter t -> mapping t |> TextFilter.create |> Result.okValueOrThrow 
            let upperFilter = p.Filter |> mapFilter (fun t -> t.ToUpper())
            let lowerFilter = p.Filter |> mapFilter (fun t -> t.ToLower())
            let withUpperFilter = p.Source |> HighlightedText.applyFilter upperFilter
            let withLowerFilter = p.Source |> HighlightedText.applyFilter lowerFilter
            withUpperFilter
            |> should equivalent withLowerFilter

        [<Property(Arbitrary=[| typeof<Generators> |] )>]
        let ``applyFilter - results do not depend on case of source text`` (p:ApplyFilterParameters) =
            let withUpperSource = 
                p.Source.ToUpper() 
                |> HighlightedText.applyFilter p.Filter
                |> Seq.map (fun t -> { t with Text = t.Text.ToLower() })
            let withLowerSource = 
                p.Source.ToLower() 
                |> HighlightedText.applyFilter p.Filter
                |> Seq.map (fun t -> { t with Text =t.Text.ToLower() })
            withUpperSource
            |> should equivalent withLowerSource