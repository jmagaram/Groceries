module TextBoxTests

open System
open Xunit
open FsUnit

open DomainTypes

let stringError = "wrong number of characters"

let isBetween1And5Characters s =
    match s |> String.length with
    | x when x >= 1 && x <= 1 -> None
    | _ -> Some stringError

let trimAndCapitalize s = 
    s 
    |> String.trim 
    |> fun s -> s.ToUpper()

let update = TextBox.update isBetween1And5Characters trimAndCapitalize

let initialize = TextBox.create isBetween1And5Characters trimAndCapitalize

[<Fact>]
let ``create - expect empty text`` () =
    initialize 
    |> TextBox.text 
    |> should equal ""

[<Fact>]
let ``create - expect not focused`` () =
    initialize 
    |> TextBox.hasFocus
    |> should equal false

[<Fact>]
let ``create - will validate and set error`` () =
    initialize 
    |> TextBox.error
    |> should equal (Some stringError)

[<Fact>]
let ``create - will calculate and store a normalized empty string`` () =
    TextBox.create (fun s -> None) (fun s -> if s = "" then "none" else s)
    |> TextBox.normalizedText
    |> should equal "none"

[<Fact>]
let ``setText - will set text to normalized if does not have focus`` () =
    initialize
    |> update LoseFocus
    |> update (SetText "  abc    ")
    |> TextBox.text
    |> should equal "ABC"

[<Fact>]
let ``setText - will not not normalize if has focus`` () =
    initialize
    |> update GetFocus
    |> update (SetText "  abc    ")
    |> TextBox.text
    |> should equal "  abc    "

[<Fact>]
let ``setText - when error and has focus will calculate it`` () =
    initialize
    |> update GetFocus
    |> update (SetText "much too long")
    |> TextBox.error
    |> should equal (Some stringError)

[<Fact>]
let ``setText - when error and not focused will calculate it`` () =
    initialize
    |> update LoseFocus
    |> update (SetText "much too long")
    |> TextBox.error
    |> should equal (Some stringError)

[<Fact>]
let ``lose focus - will normalize`` () =
    initialize
    |> update GetFocus
    |> update (SetText "  abc    ")
    |> update LoseFocus
    |> TextBox.text
    |> should equal "ABC"

//[<Theory>]
//[<InlineData(update)>]
//let ``selectItem - when item is in the choices select it`` (x:int) =
//    let result =
//        Selector.create
//        |> Selector.add 1
//        |> Selector.add 2
//        |> Selector.add 3
//        |> Selector.selectItem x
//    result.SelectedItem
//    |> should equal (Some x)

