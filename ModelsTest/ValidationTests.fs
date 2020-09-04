namespace ModelsTest

open System
open Xunit

module StringValidationTests =

    open Models.StringValidation

    let onlyContainsLetters = onlyContains [ CharacterKind.Letter ]

    [<Fact>]
    let ``onlyContains - when character list is empty throw`` () = true

    [<Fact>]
    let ``onlyContains - when source is empty string return true`` () =
        Assert.True(onlyContains [ CharacterKind.Letter ] "")
        Assert.True(onlyContains [ CharacterKind.Mark ] "")
        Assert.True(onlyContains [ CharacterKind.Number ] "")
        Assert.True(onlyContains [ CharacterKind.Space ] "")
        Assert.True(onlyContains [ CharacterKind.Space ] "")
        Assert.True(onlyContains [ CharacterKind.Space ] "")

    [<Theory>]
    [<InlineData("abc", true)>]
    [<InlineData("123", false)>]
    [<InlineData("123abc123", false)>]
    [<InlineData("", true)>]
    let ``onlyContains - check for letters`` (source: string, expected: bool) =
        let actual = source |> onlyContainsLetters
        Assert.Equal(expected, actual)
