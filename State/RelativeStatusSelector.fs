module RelativeStatusSelector
open DomainTypes

let create =
    RelativeStatus.standardPostponeIntervals
    |> Seq.append (RelativeStatus.active |> Seq.singleton)
    |> Seq.append (RelativeStatus.complete |> Seq.singleton)
    |> Seq.append (RelativeStatus.postponedDays 37 |> Seq.singleton)
    |> Seq.append (RelativeStatus.postponedDays 1 |> Seq.singleton)
    |> Seq.append (RelativeStatus.postponedDays 0 |> Seq.singleton)
    |> Seq.append (RelativeStatus.postponedDays -3 |> Seq.singleton)
    |> PickOne.create