[<AutoOpen>]
module Models.Seq

/// <summary>Returns a sequence that, when iterated, yields elements of the underlying sequence until and
/// including the first element where the given predicate returns True, and then returns no further
/// elements.</summary>
let takeTo<'T> predicate (source: 'T seq) =
    seq {
        use en = source.GetEnumerator()
        let mutable isDone = false

        while isDone = false && en.MoveNext() do
            yield en.Current
            isDone <- predicate en.Current
    }

let zeroOrOne s =
    let result = s |> Seq.truncate 2 |> Seq.toList

    match result with
    | [] -> None
    | [ x ] -> Some x
    | _ -> failwith "Too many items in the sequence. Expected zero or one."

let leftJoin xs ys f =
    xs
    |> Seq.map (fun x -> (x, ys |> Seq.filter (fun y -> f x y)))

let join xs ys f =
    leftJoin xs ys f
    |> Seq.collect (fun (x, ys) -> ys |> Seq.map (fun y -> (x, y)))

