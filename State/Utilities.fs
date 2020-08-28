[<AutoOpen>]
module Utilities

let newGuid () = System.Guid.NewGuid()

let nowUtc : DomainTypes.NowUtc = fun () -> System.DateTime.UtcNow

let memoize f = 
    let mutable cache = Map.empty
    let f' x =
        match cache |> Map.tryFind x with
        | None ->
            let result = f x
            cache <- cache |> Map.add x result
            result
        | Some result -> result
    f'
