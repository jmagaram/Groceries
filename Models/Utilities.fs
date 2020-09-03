namespace Models

module Map =

    let values m = m |> Map.toSeq |> Seq.map snd

    //let mapValues f m = m |> Map.map (fun _ v -> v |> f)


