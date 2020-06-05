[<AutoOpen>]
module Seq

let takeAtMost n s = 
    s
    |> Seq.mapi (fun index item -> if index<n then Some item else None)
    |> Seq.takeWhile (fun i -> i.IsSome)
    |> Seq.choose id

let zeroOrOne s =
    let result =
        s
        |> takeAtMost 2
        |> Seq.toList
    match result with
    | [] -> None
    | [x] -> Some x
    | _ -> failwith "Too many items in the sequence. Expected zero or one."