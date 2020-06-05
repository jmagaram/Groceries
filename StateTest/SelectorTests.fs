module SelectorTests

open System
open Xunit
open FsUnit

[<Fact>]
let ``create - expect no selected item`` () =
    Selector.create.SelectedItem.IsNone
    |> should be True

[<Fact>]
let ``create - expect no choices`` () =
    Selector.create.Choices
    |> Set.count 
    |> should equal 0

[<Fact>]
let ``clearSelection - expect selected item to be none`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.add 2
    |> Selector.selectItem 1
    |> Selector.clearSelection
    |> fun i -> i.SelectedItem
    |> should equal None

[<Fact>]
let ``add - expect choices to have all added items`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.add 2
    |> Selector.add 3
    |> fun i -> i.Choices
    |> should equivalent [1;2;3]

[<Fact>]
let ``addItems - expect choices to have all added items`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.addMany [2;3;4]
    |> fun i -> i.Choices
    |> should equivalent [1;2;3;4]

[<Fact>]
let ``addItems - does not change current selected item`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.selectItem 1
    |> Selector.addMany [2;3;4]
    |> fun i -> i.SelectedItem
    |> should equal (Some 1)

[<Fact>]
let ``add - when duplicates expect choices to have unique items`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.add 1
    |> Selector.add 2
    |> Selector.add 2
    |> fun i -> i.Choices
    |> should equivalent [2;1]

[<Fact>]
let ``select - when nothing matches expect selected item is none`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.add 2
    |> Selector.select (fun i -> i = 3)
    |> fun i -> i.SelectedItem
    |> should equal None

[<Fact>]
let ``select - when a single item matches expect the selected item to be set`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.add 2
    |> Selector.select (fun i -> i = 2)
    |> fun i -> i.SelectedItem
    |> should equal (Some 2)

[<Fact>]
let ``select - when a single item matches overwrite existing selection`` () =
    Selector.create
    |> Selector.add 1
    |> Selector.add 2
    |> Selector.select (fun i -> i = 2)
    |> Selector.select (fun i -> i = 1)
    |> fun i -> i.SelectedItem
    |> should equal (Some 1)

[<Fact>]
let ``select - when multiple items match should throw`` () =
    let f () =
        Selector.create
        |> Selector.add 1
        |> Selector.add 2
        |> Selector.add 3
        |> Selector.select (fun i -> i > 1)
        |> ignore
    f
    |> should throw typeof<Exception>

[<Theory>]
[<InlineData(1)>]
[<InlineData(2)>]
[<InlineData(3)>]
let ``selectItem - when item is in the choices select it`` (x:int) =
    let result =
        Selector.create
        |> Selector.add 1
        |> Selector.add 2
        |> Selector.add 3
        |> Selector.selectItem x
    result.SelectedItem
    |> should equal (Some x)

[<Fact>]
let ``selectItem - when item is not in the choices throw`` () =
    let f () =
        Selector.create
        |> Selector.add 1
        |> Selector.add 2
        |> Selector.add 3
        |> Selector.selectItem 99
        |> ignore
    f
    |> should throw typeof<Exception>

