module ItemListView
open System
open DomainTypes

type Create = Item seq -> ItemListView
let create : Create = fun items ->
    { Items = 
        items
        |> Seq.map ItemSummary.convertFromItem
        |> Seq.sortBy (fun i -> i.Title)
      Filter = ItemFilter.includeAll }

// recreating every time; not efficient
// should cache or something
type Update = Item seq -> NowUtc -> ItemListViewMessage -> ItemListView
let update : Update = fun items now msg ->
    let now = now()
    let (SetFilter filter) = msg
    { Items = 
        items
        |> Seq.map ItemSummary.convertFromItem 
        |> Seq.filter (ItemSummary.matchesPostponedItemFilter now filter.PostponedItemFilter)
        |> Seq.sortBy (fun i -> i.Title)
        |> Seq.toList // if this is required, why?
      Filter = filter }

// to fix
// smart sort?
// annoying serialize deserialize hassle
// no namespace
// too cumbersome to add files and too many files
// probably chunk a bunch of functions into modules

let filterSelector =
    let usefulDefault = PostponedItemFilter.IncludeOverdueAndFutureItems (TimeSpan.FromDays(7 |> float))
    [ PostponedItemFilter.AllPostponedItems
      ExcludePostponedItems
      IncludeOverdueAndFutureItems (TimeSpan.FromDays(1 |> float))
      usefulDefault
      IncludeOverdueAndFutureItems (TimeSpan.FromDays(14 |> float))
      IncludeOverdueOnly
      ExcludePostponedItems ]
    |> PickOne.create
    |> PickOne.select usefulDefault

let serialize (f:PostponedItemFilter) =
    match f with
    | ExcludePostponedItems -> "none"
    | AllPostponedItems -> "all"
    | IncludeOverdueOnly -> "overdue"
    | IncludeOverdueAndFutureItems ts -> sprintf "%s" (ts.ToString())

let deserialize s =
    if s = "none" then ExcludePostponedItems
    elif s = "all" then AllPostponedItems
    elif s = "overdue" then IncludeOverdueOnly // originally a spelling error
    else 
        s
        |> tryParseTimeSpan
        |> Option.map (fun t -> IncludeOverdueAndFutureItems t)
        |> Option.get

let description f =
    match f with
    | ExcludePostponedItems -> "Hide all"
    | AllPostponedItems -> "Show all"
    | IncludeOverdueOnly -> "Overdue"
    | IncludeOverdueAndFutureItems ts -> sprintf "Upcoming %s days" (ts.TotalDays.ToString())
