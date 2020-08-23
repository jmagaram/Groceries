module PlainText
open DomainTypes

let private ruleAsPredicate (r:PlainTextRule) =
    match r with
    | SingleLineOnly -> String.containsNewLine >> not
    | MinimumLength x -> String.lengthIsAtLeast (x |> int)
    | MaximumLength x -> String.lengthIsAtMost (x |> int)
    | NoLeadingOrTrailingWhitespace -> String.startsWithOrEndsWithWhitespace >> not

type private ViolatedRules = PlainTextRule seq

type private Validate = PlainTextRule seq -> string -> ViolatedRules
let private validate : Validate = fun rs s ->
    rs
    |> Seq.choose (fun r -> 
        match s |> (r |> ruleAsPredicate) with
        | false -> Some r
        | true -> None)

type private Create = string -> PlainTextRule seq -> Result<PlainText, PlainTextRule>
let create : Create = fun s rs ->
    s
    |> validate rs
    |> Seq.tryHead
    |> Option.map Error
    |> Option.defaultValue(s |> PlainText |> Ok )

module Tests = 

    open System
    open Xunit
    open FsUnit

    [<Fact>]
    let ``finds first error`` () =
        let minLength = MinimumLength 2<chars>
        let maxLength = MaximumLength 5<chars>

        // tricky; must define rules in order
        let requirements = 
            [ NoLeadingOrTrailingWhitespace
              SingleLineOnly
              minLength
              maxLength ]

        Assert.Equal(PlainText "ab" |> Ok, requirements |> create "ab")
        Assert.Equal(minLength |> Error, requirements |> create "a")

        Assert.Equal(PlainText "abcde" |> Ok, requirements |> create "abcde")
        Assert.Equal(maxLength |> Error, requirements |> create "abcdef")

        Assert.Equal(NoLeadingOrTrailingWhitespace |> Error, requirements |> create " abc")
        Assert.Equal(NoLeadingOrTrailingWhitespace |> Error, requirements |> create "abc ")
        Assert.Equal(NoLeadingOrTrailingWhitespace |> Error, requirements |> create "  abc ")
       
        Assert.Equal(SingleLineOnly |> Error, requirements |> create ("abc" + System.Environment.NewLine + "abc"))
