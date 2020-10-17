[<AutoOpen>]
module Models.Result

let okOrThrow r =
    match r with
    | Ok v -> v
    | Error e -> failwithf "Could not get the Ok value; this was the Error: %A" e

let asOption r =
    match r with
    | Ok v -> Some v
    | Error _ -> None

let isOk r =
    match r with
    | Ok _ -> true
    | Error _ -> false

let isError r = r |> isOk |> not

let error r =
    match r with
    | Ok v -> None
    | Error e -> Some e

type ResultBuilder() =
    member this.Return(x) = Ok x
    member this.Bind(x, f) = Result.bind f x
    member this.ReturnFrom r = r

let result = ResultBuilder()

let fromResults rs =
    rs
    |> Seq.scan (fun (vs, err) i ->
        match i with
        | Ok v -> (v :: vs, err)
        | Error e -> (vs, Some e)) ([], None)
    |> Seq.takeTo (fun (vs, err) -> err.IsSome)
    |> Seq.last
    |> fun (vs, err) ->
        match err with
        | None -> vs |> List.rev |> Ok
        | Some err -> Error err
