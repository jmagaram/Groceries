module TextBox
open DomainTypes

let hasFocus tb = tb.HasFocus

let text tb = tb.Text

let error tb = tb.Error

let normalizedText tb = tb.NormalizedText

let create v n =
    let nt = "" |> n
    { Text = nt
      NormalizedText = nt
      Error = nt |> v 
      HasFocus = false }

let setText v n s tb =
    let nt = s |> n
    { tb with 
        Text = 
            match tb |> hasFocus with 
            | true -> s
            | false -> nt
        NormalizedText = nt
        Error = nt |> v }

let getFocus tb = 
    { tb with 
        HasFocus = true }

let loseFocus tb =
    { tb with 
        HasFocus = false; 
        Text = tb.NormalizedText }

let update v n msg tb = 
    match msg with
    | GetFocus -> tb |> getFocus
    | LoseFocus -> tb |> loseFocus
    | SetText t -> tb |> (setText v n t)

module Tests = 

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

    let update = update isBetween1And5Characters trimAndCapitalize

    let initialize = create isBetween1And5Characters trimAndCapitalize

    [<Fact>]
    let ``create - expect empty text`` () =
        initialize 
        |> text 
        |> should equal ""

    [<Fact>]
    let ``create - expect not focused`` () =
        initialize 
        |> hasFocus
        |> should equal false

    [<Fact>]
    let ``create - will validate and set error`` () =
        initialize 
        |> error
        |> should equal (Some stringError)

    [<Fact>]
    let ``create - will calculate and store a normalized empty string`` () =
        create (fun s -> None) (fun s -> if s = "" then "none" else s)
        |> normalizedText
        |> should equal "none"

    [<Fact>]
    let ``setText - will set text to normalized if does not have focus`` () =
        initialize
        |> update LoseFocus
        |> update (SetText "  abc    ")
        |> text
        |> should equal "ABC"

    [<Fact>]
    let ``setText - will not not normalize if has focus`` () =
        initialize
        |> update GetFocus
        |> update (SetText "  abc    ")
        |> text
        |> should equal "  abc    "

    [<Fact>]
    let ``setText - when error and has focus will calculate it`` () =
        initialize
        |> update GetFocus
        |> update (SetText "much too long")
        |> error
        |> should equal (Some stringError)

    [<Fact>]
    let ``setText - when error and not focused will calculate it`` () =
        initialize
        |> update LoseFocus
        |> update (SetText "much too long")
        |> error
        |> should equal (Some stringError)

    [<Fact>]
    let ``lose focus - will normalize`` () =
        initialize
        |> update GetFocus
        |> update (SetText "  abc    ")
        |> update LoseFocus
        |> text
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

