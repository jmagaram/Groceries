namespace Models
open DomainTypes

[<AutoOpen>]
module General = 

    let keyOf<'T, 'TKey when 'T :> IKey<'TKey>> i = (i :> IKey<'TKey>).Key

[<AutoOpen>]
module Map =

    let values m = m |> Map.toSeq |> Seq.map snd

//[<AutoOpen>]
//module Seq =

//    let doesNotExist q = q |> Seq.

