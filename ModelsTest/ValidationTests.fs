namespace ModelsTest

open System
open Xunit
open FsUnit
open FsUnit.Xunit
open Models.ValidationTypes

module StringValidationTests =

    open Models.StringValidation
    open FsCheck

    [<Fact>]
    let ``onlyContains - when character list is empty throw`` () = 
        (fun () -> onlyContains [] |> ignore)
        |> shouldFail

    [<Fact>]
    let ``onlyContains - when source is empty string return true`` () =
        (onlyContains [ CharacterKind.Letter ]) "" |> should equal true
        (onlyContains [ CharacterKind.Mark ]) "" |> should equal true
        (onlyContains [ CharacterKind.Number ]) "" |> should equal true
        (onlyContains [ CharacterKind.Space ]) "" |> should equal true
        (onlyContains [ CharacterKind.LineFeed ]) "" |> should equal true
        (onlyContains [ CharacterKind.Punctuation ]) "" |> should equal true
        (onlyContains [ CharacterKind.Symbol ]) "" |> should equal true

    [<Theory>]
    [<InlineData("abc", true)>]
    [<InlineData("123", false)>]
    [<InlineData("123abc123", false)>]
    [<InlineData("abc123abc", false)>]
    let ``onlyContains - check for letters`` (source: string, expected: bool) =
        let onlyContainsLetters = onlyContains [ CharacterKind.Letter ]
        let actual = source |> onlyContainsLetters
        Assert.Equal(expected, actual)