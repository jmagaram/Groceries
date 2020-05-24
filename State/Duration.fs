module Duration
open DomainTypes

let asDays (d:DomainTypes.Duration) = 
    match d with
    | D3 -> 3
    | W1 -> 7*1
    | W2 -> 7*2
    | W3 -> 7*3
    | M1 -> 30*1
    | M2 -> 30*2
    | M3 -> 30*3
    | M4 -> 30*4
    | M6 -> 30*6
    | M9 -> 30*9