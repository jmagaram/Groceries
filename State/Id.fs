module Id
open DomainTypes

let create f = newGuid () |> f