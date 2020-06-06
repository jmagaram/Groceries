module Selector
open DomainTypes

let create = 
    { Choices = Set.empty
      SelectedItem = None 
      Error = None }

let add item selector = { selector with Selector.Choices = selector.Choices |> Set.add item }

let addMany items selector =
    items
    |> Seq.fold (fun s i -> s |> add i) selector

let clearSelection selector = { selector with Selector.SelectedItem = None }

let select predicate selector = 
    let item = 
        selector.Choices 
        |> Seq.filter predicate
        |> Seq.zeroOrOne
    { selector with SelectedItem = item }

let selectItem item selector =
    match selector.Choices |> Set.contains item with
    | true -> { selector with SelectedItem = Some item }
    | false -> failwith "The item is not in the list."

let hasError e selector = { selector with Selector.Error = e }

let clearError selector = selector |> hasError None

module Tests = 

    open System
    open Xunit
    open FsUnit

    [<Fact>]
    let ``create - expect no selected item`` () =
        create.SelectedItem.IsNone
        |> should be True

    [<Fact>]
    let ``create again - expect no selected item`` () =
        create.SelectedItem.IsNone
        |> should be True


    [<Fact>]
    let ``create - expect no choices`` () =
        create.Choices
        |> Set.count 
        |> should equal 0

    [<Fact>]
    let ``clearSelection - expect selected item to be none`` () =
        create
        |> add 1
        |> add 2
        |> selectItem 1
        |> clearSelection
        |> fun i -> i.SelectedItem
        |> should equal None

    [<Fact>]
    let ``add - expect choices to have all added items`` () =
        create
        |> add 1
        |> add 2
        |> add 3
        |> fun i -> i.Choices
        |> should equivalent [1;2;3]

    [<Fact>]
    let ``addItems - expect choices to have all added items`` () =
        create
        |> add 1
        |> addMany [2;3;4]
        |> fun i -> i.Choices
        |> should equivalent [1;2;3;4]

    [<Fact>]
    let ``addItems - does not change current selected item`` () =
        create
        |> add 1
        |> selectItem 1
        |> addMany [2;3;4]
        |> fun i -> i.SelectedItem
        |> should equal (Some 1)

    [<Fact>]
    let ``add - when duplicates expect choices to have unique items`` () =
        create
        |> add 1
        |> add 1
        |> add 2
        |> add 2
        |> fun i -> i.Choices
        |> should equivalent [2;1]

    [<Fact>]
    let ``select - when nothing matches expect selected item is none`` () =
        create
        |> add 1
        |> add 2
        |> select (fun i -> i = 3)
        |> fun i -> i.SelectedItem
        |> should equal None

    [<Fact>]
    let ``select - when a single item matches expect the selected item to be set`` () =
        create
        |> add 1
        |> add 2
        |> select (fun i -> i = 2)
        |> fun i -> i.SelectedItem
        |> should equal (Some 2)

    [<Fact>]
    let ``select - when a single item matches overwrite existing selection`` () =
        create
        |> add 1
        |> add 2
        |> select (fun i -> i = 2)
        |> select (fun i -> i = 1)
        |> fun i -> i.SelectedItem
        |> should equal (Some 1)

    [<Fact>]
    let ``select - when multiple items match should throw`` () =
        let f () =
            create
            |> add 1
            |> add 2
            |> add 3
            |> select (fun i -> i > 1)
            |> ignore
        f
        |> should throw typeof<Exception>

    [<Theory>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    let ``selectItem - when item is in the choices select it`` (x:int) =
        let result =
            create
            |> add 1
            |> add 2
            |> add 3
            |> selectItem x
        result.SelectedItem
        |> should equal (Some x)

    [<Fact>]
    let ``selectItem - when item is not in the choices throw`` () =
        let f () =
            create
            |> add 1
            |> add 2
            |> add 3
            |> selectItem 99
            |> ignore
        f
        |> should throw typeof<Exception>

