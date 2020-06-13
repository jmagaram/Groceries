module FindViewModel
open DomainTypes

type HighlightedItem =
    { Item : Item 
      TitleMatches : (string * bool) list }

let tryFilter filter (item:Item) =
    let highlighter = 
        filter
        |> FindView.Query.create
        |> Result.map FindView.FormattedText.highlightMatches
        |> Result.okValueOrThrow
    let (Title t) = item.Title
    let titleMatches = highlighter t
    let doesMatch = 
        titleMatches 
        |> Seq.exists FindView.Span.isHighlight
    match doesMatch with
    | false -> None
    | true ->
        { Item = item
          TitleMatches = 
            titleMatches 
            |> Seq.map (fun i -> (i.Text, i |> FindView.Span.isHighlight)) 
            |> List.ofSeq
        } |> Some

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