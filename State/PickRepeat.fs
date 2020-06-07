module PickRepeat
open DomainTypes

let create =
    Repeat.standardRepeatIntervals
    |> Seq.append (Repeat.doesNotRepeat |> Seq.singleton)
    |> PickOne.create