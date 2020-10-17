module Models.RangeValidation
open ValidationTypes

let createValidator (r: Range<_>) v =
    if v < r.Min then RangeError.TooSmall |> Some
    elif v > r.Max then RangeError.TooBig |> Some
    else None

let forceIntoBounds v (r: Range<_>) =
    if v < r.Min then r.Min
    elif v > r.Max then r.Max
    else v

let toResult normalizer validator onSuccess onError v = 
    match v |> normalizer |> validator with
    | None -> v |> onSuccess |> Ok
    | Some error -> error |> onError |> Error
