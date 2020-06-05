module QuantityUpDown
open System
open System.Text.RegularExpressions

type UpDown = DomainTypes.UpDown<string>

type KnownUnit =
    { OneOf : string 
      ManyOf : string }

let private knownUnits = 
    [ 
        { OneOf = "jar"; ManyOf = "jars" }
        { OneOf = "can"; ManyOf = "cans" }
        { OneOf = "ounce"; ManyOf = "ounces" }
        { OneOf = "pound"; ManyOf = "pounds" }
        { OneOf = "gram"; ManyOf = "grams" }
        { OneOf = "head"; ManyOf = "heads" }
        { OneOf = "bunch"; ManyOf = "bunches" }
        { OneOf = "pack"; ManyOf = "packs" }
        { OneOf = "bag"; ManyOf = "bags" }
        { OneOf = "package"; ManyOf = "packages" }
        { OneOf = "box"; ManyOf = "boxes" }
        { OneOf = "pint"; ManyOf = "pints" }
        { OneOf = "gallon"; ManyOf = "gallons" }
        { OneOf = "container"; ManyOf = "containers" }
    ]

let private manyOf u = 
    knownUnits
    |> Seq.where(fun i -> i.OneOf = u || i.ManyOf = u)
    |> Seq.map (fun i -> i.ManyOf)
    |> Seq.tryHead
    |> Option.defaultValue u

let private oneOf u = 
    knownUnits
    |> Seq.where(fun i -> i.OneOf = u || i.ManyOf = u)
    |> Seq.map (fun i -> i.OneOf)
    |> Seq.tryHead
    |> Option.defaultValue u

let private grammar = new Regex("^\s*(\d+)\s*(.*)", RegexOptions.Compiled)

type private ParsedQuantity =
    { Quantity : int 
      Units : string }

let private format q =
    match q with
    | { Units = "" } -> sprintf "%i" q.Quantity
    | _ -> sprintf "%i %s" q.Quantity q.Units

let private parse s =
    let m = grammar.Match(s)
    match m.Success with
    | false -> None
    | true -> 
        let qty = System.Int32.Parse(m.Groups.[1].Value)
        let units = m.Groups.[2].Value
        Some { Quantity = qty; Units = units }

let private increase qty = 
    match isNullOrWhiteSpace qty with
    | true -> Some "2" 
    | false ->
        match parse qty with
        | None -> None
        | Some i ->
            let qty = i.Quantity + 1
            let result =
                { Quantity = qty 
                  Units = i.Units |> manyOf }
                |> format
            Some result

let private decrease qty = 
    match parse qty with
    | None -> None
    | Some i ->
        match i.Quantity with
        | x when x <= 1 -> None
        | _ ->
            let qty = i.Quantity - 1
            let result =
                { Quantity = qty
                  Units = 
                    match qty with
                    | 1 -> i.Units |> oneOf
                    | _ -> i.Units |> manyOf }
                |> format
            Some result

let instance =
    { UpDown.Increase = increase
      UpDown.Decrease = decrease }