module ItemListView
open System
open DomainTypes

type Create = Item seq -> TimeSpan option -> ItemListView
let create : Create = fun items future ->
    { Items = 
        items
        |> Seq.filter (fun i -> i.Status |> Status.isDueWithin future)
        |> Seq.map ItemSummary.convertFromItem
        |> Seq.sortBy (fun i -> i.Title)
      FutureHorizon = future }