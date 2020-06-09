module Status

open System

type private IsDueWithin = TimeSpan option -> DomainTypes.Status -> bool
let isDueWithin : IsDueWithin = fun t s ->
    match s with
    | DomainTypes.Status.Active -> true
    | DomainTypes.Status.Complete -> false
    | DomainTypes.Status.Postponed dt -> 
        match t with
        | None -> false
        | Some horizon -> 
            let now = nowUtc()
            (dt - now) <= horizon
