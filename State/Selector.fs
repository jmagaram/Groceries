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