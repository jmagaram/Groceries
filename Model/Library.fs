namespace Model

module Char =

    let isWhitespace c = System.Char.IsWhiteSpace(c)

module String =

    let lengthIsAtLeast min s = (String.length s) >= min

    let lengthIsAtMost max s = (String.length s) <= max

    let startsWithOrEndsWithWhitespace s =
        match s |> String.length with
        | 0 -> false
        | len -> (s.[0] |> Char.isWhitespace) || (s.[len-1] |> Char.isWhitespace)

    let containsNewLine (s:string) = s.Contains(System.Environment.NewLine)

[<AutoOpen>]
module Types =

    [<Measure>]
    type chars

    type PlainTextRule =
        | MinimumLength of int<chars>
        | MaximumLength of int<chars>
        | NoLeadingOrTrailingWhitespace
        | SingleLineOnly

    type PlainText = PlainText of string

    type ValidatePlainTextRules = PlainTextRule seq -> string -> PlainTextRule seq

    type CreatePlainText = string -> PlainTextRule seq -> Result<PlainText, PlainTextRule>

    let plainTextRuleAsPredicate (r:PlainTextRule) =
        match r with
        | SingleLineOnly -> String.containsNewLine
        | MinimumLength x -> String.lengthIsAtLeast (x |> int)
        | MaximumLength x -> String.lengthIsAtMost (x |> int)
        | NoLeadingOrTrailingWhitespace -> String.startsWithOrEndsWithWhitespace >> not

    let validatePlainTextRules : ValidatePlainTextRules = fun rs s ->
        rs
        |> Seq.choose (fun r -> 
            match s |> (r |> plainTextRuleAsPredicate) with
            | false -> Some r
            | true -> None)

    let createPlainText : CreatePlainText = fun s rs ->
        s
        |> validatePlainTextRules rs
        |> Seq.tryHead
        |> Option.map Error
        |> Option.defaultValue(s |> PlainText |> Ok )

module PlainText =

    let ruleAsPredicate (r:PlainTextRule) =
        match r with
        | SingleLineOnly -> String.containsNewLine
        | MinimumLength x -> String.lengthIsAtLeast (x |> int)
        | MaximumLength x -> String.lengthIsAtMost (x |> int)
        | NoLeadingOrTrailingWhitespace -> String.startsWithOrEndsWithWhitespace >> not

    let violatedRules : ValidatePlainTextRules = fun rs s ->
        rs
        |> Seq.choose (fun r -> 
            match s |> (r |> ruleAsPredicate) with
            | false -> Some r
            | true -> None)

    let create : CreatePlainText = fun s rs ->
        s
        |> violatedRules rs
        |> Seq.tryHead
        |> Option.map Error
        |> Option.defaultValue(s |> PlainText |> Ok )

  
    
    //type CreateText = Text 

    // weird if no whitespace is permitted that can specify which chars are ok
    //let titleRequirements = 
    //    [ MinLength 1<chars>
    //      MaxLength 50<chars>
    //      NoLeadingOrTrailingWhitespaceIsPermitted
    //      PermittedWhitespace (Set.singleton Space)
    //    ]

    //type WhitespacePlacement =
    //    | WhitespaceIsDisallowedEverywhere
    //    | WhitespaceIsPermittedAnywhere
    //    | WhitespaceIsOkAnywherePermittedInternalOnly

    //type WhitespaceCharacter =
    //    | Space
    //    | CarriageReturnLineFeed
    //    | Other

    //type WhitespacePosition =
    //    { AllowLeading : bool 
    //      AllowTrailing : bool
    //      AllowInternal : bool }

    //type PlainTextRequirement =
    //    { MinLength : int<chars> 
    //      MaxLength : int<chars>
    //      Whitespace : WhitespacePlacement * WhitespaceCharacter
    //    }

    //// which requirement is invalid?

    //type TextError = 
    //    | TextIsTooLong
    //    | TextIsTooShort
    //    | ContainsLeadingOrTrailingWhitespace
    //    | ContainsInternalWhitespace
    //    | ContainsInvalidCharacters

    //type Title = Title of string

    //type TitleError = TitleError of TextError

    //type TitleRequirements = TitleRequirements of TextRequirement
    
    // guides the UI editor

    //type TitleError =
    //    | Too
    //    | TooLong
    //type CreateTitle = string -> Result<Title,TitleError>