module FindViewModel
open DomainTypes

type HighlightedItem =
    { Item : Item 
      TitleMatches : (string * bool) list }

let tryFilter filter (item:Item) =
    let titleMatches = 
        match item.Title with | Title t -> t
        |> String.find filter 
        |> Seq.toList
    let doesMatch =
        titleMatches
        |> Seq.exists (fun (s, isMatch) -> isMatch)
    match doesMatch with
    | false -> None
    | true ->
        { Item = item
          TitleMatches = titleMatches } |> Some

type ViewModel = 
    { Items : Item seq
      HighlightedItems : HighlightedItem seq
      Filter : string }

let initialize items = 
    let initialFilter = ""
    { Items = items
      HighlightedItems = 
        items 
        |> Seq.choose (tryFilter initialFilter)
        |> Seq.sortBy (fun i -> i.Item.Title)
      Filter = initialFilter }

let updateSearch model filter = 
    { model with
        Filter = filter
        HighlightedItems = 
            model.Items 
            |> Seq.choose (tryFilter filter) 
            |> Seq.sortBy (fun i -> i.Item.Title) }