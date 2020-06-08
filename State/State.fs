module State
open DomainTypes

let create = 
    { Stores = Map.empty
      ItemIsUnavailableInStore = Set.empty
      Items = 
        Test.sampleItems 
        |> Seq.map (fun i -> (i.Id, i))
        |> Map.ofSeq }


// problem with Now

let update msg s =
    match msg with
    | InsertItem i ->
        let ni = i |> ItemEditorModel.toNewItem System.DateTime.Now
        { s with Items = s.Items |> Map.add ni.Id ni }


