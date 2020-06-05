[<AutoOpen>]
module Result

let okValue r =
    match r with
    | Result.Ok v -> Some v
    | Result.Error _ -> None

let errorValue r =
    match r with
    | Result.Ok _ -> None
    | Result.Error e -> e

let isOk r = r |> okValue |> Option.isSome

let isError r = r |> errorValue |> Option.isSome
