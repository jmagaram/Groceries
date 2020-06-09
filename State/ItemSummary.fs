module ItemSummary
open System
open DomainTypes

type ConvertFromItem = Item -> ItemSummary
let convertFromItem : ConvertFromItem = fun i ->
    { Id = i.Id
      Title = i.Title |> Title.asString 
      Quantity = 
        i.Quantity 
        |> Option.map Quantity.asString
        |> Option.defaultValue ""
      Note = 
        i.Note 
        |> Option.map Note.asString
        |> Option.defaultValue ""
      Repeat = i.Repeat
      Status = i.Status }

// might go faster if filtering at the source, normalized data
type PostponedFilterPredicate = DateTime -> PostponedItemFilter -> ItemSummary -> bool
let matchesPostponedItemFilter : PostponedFilterPredicate = fun now filter item ->
    match item.Status, filter with
    | Status.Active, _ -> true
    | Status.Complete, _ -> true
    | _, ExcludePostponedItems -> false
    | _, AllPostponedItems -> true
    | Status.Postponed due, IncludeOverdueOnly -> due < now
    | Status.Postponed due, IncludeOverdueAndFutureItems ts -> due < now.Add(ts)
