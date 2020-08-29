module FormField
open DomainTypes

let lostFocus (f:FormField<_,_,_>) = 
    match f.Proposed <> f.Normalized with
    | true -> { f with Proposed = f.Normalized }
    | false -> f

let proposed (f:FormField<_,_,_>) = f.Proposed

let gainedFocus f = f

let propose normalize validate =
    fun p f ->
        let n = 
            match normalize with
            | None -> p
            | Some normalize -> p |> normalize
        let r = n |> validate
        { f with
            Proposed = p
            Normalized = n
            ValidationResult = r }

let init normalize validate proposed =
    let n = 
        match normalize with
        | None -> proposed
        | Some normalize -> proposed |> normalize
    let r = n |> validate
    { InitialValue = n 
      Proposed = n 
      Normalized = n 
      ValidationResult = r }

let handleMessage normalize validate =
    let propose = propose normalize validate
    let lostFocus = lostFocus
    let gainedFocus = gainedFocus
    fun msg f ->
        match msg with
        | LostFocus -> f |> lostFocus
        | GainedFocus -> f |> gainedFocus
        | Propose p -> f |> propose p