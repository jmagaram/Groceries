[<AutoOpen>]
module Models.Map

let values m = m |> Map.toSeq |> Seq.map snd
