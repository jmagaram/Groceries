[<AutoOpen>]
module Models.Common

open System

let keyOf<'T, 'TKey when 'T :> IKey<'TKey>> i = (i :> IKey<'TKey>).Key

let clock: Clock = fun () -> DateTimeOffset.Now

let refEquals<'T when 'T: not struct> x y = Object.ReferenceEquals(x, y)

let newGuid () = System.Guid.NewGuid()

let memoize<'Cache, 'X, 'Y when 'Y: equality> empty tryFind (add: 'X -> 'Y -> 'Cache -> 'Cache) f =
    let mutable cache = empty

    let f' x =
        match cache |> tryFind x with
        | Some y -> y
        | None ->
            let y = f x
            cache <- add x y cache
            y

    f'

let memoizeAll f =
    let empty = Map.empty
    let tryFind = Map.tryFind
    let add = Map.add
    memoize empty tryFind add f

let memoizeLast (f, isEqual) =
    let empty = None

    let tryFind x' cache =
        cache
        |> Option.filter (fun (x, _) -> isEqual x x')
        |> Option.map (fun (_, y) -> y)

    let add x y cache = Some(x, y)
    memoize empty tryFind add f
