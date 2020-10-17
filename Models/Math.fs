[<AutoOpen>]
module Models.Math

let divRem divisor dividend =
    if divisor = 0 then
        None
    else
        Some
            {| Quotient = dividend / divisor
               Remainder = dividend % divisor |}
