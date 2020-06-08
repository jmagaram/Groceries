module Quantity
open DomainTypes

let normalize : NormalizeString = trim

let maxLength = 10

type private Create = string -> Result<Quantity, QuantityError>
let create : Create = fun q ->
    let t = q |> normalize
    if t.Length > maxLength then Error (QuantityError.QuantityIsOverMaxLength maxLength)
    else Ok (Quantity t)

type private AsString = Quantity -> string
let asString : AsString = fun q -> 
    match q with | Quantity q -> q