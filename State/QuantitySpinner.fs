module QuantitySpinner
open System
open System.Text.RegularExpressions

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

let increase qty = 
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

let decrease qty = 
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

module Tests = 

    open System
    open Xunit
    open FsUnit

    type Expectation =
        | CanIncrease of string
        | CanDecrease of string
        | CanNotIncrease
        | CanNotDecrease
        | CanNotIncreaseOrDecrease

    type QuantityAdjustmentTests () as this =
        inherit TheoryData<string, Expectation> ()
        do 
           this.Add ("", CanIncrease "2")

           this.Add ("1 jar", CanIncrease "2 jars")
           this.Add ("2 jar", CanIncrease "3 jars")
           this.Add ("3 jars", CanIncrease "4 jars")
           this.Add ("1", CanIncrease "2")
           this.Add ("2", CanIncrease "3")
           this.Add ("3", CanIncrease "4")

           this.Add("Several large", CanNotIncrease)
           this.Add("Several large", CanNotDecrease)
           this.Add("Lots", CanNotIncrease)
           this.Add("Lots", CanNotDecrease)

           this.Add ("1 jar", CanNotDecrease)
           this.Add ("", CanNotDecrease)
           this.Add ("2 jars", CanDecrease "1 jar")
           this.Add ("3 jars", CanDecrease "2 jars")
           this.Add ("1", CanNotDecrease)
           this.Add ("2", CanDecrease "1")
           this.Add ("3", CanDecrease "2")

           this.Add ("1 goolash", CanIncrease "2 goolash")
           this.Add ("2 goolash", CanDecrease "1 goolash")
           this.Add ("1 goolash", CanNotDecrease)

    [<Theory>]
    [<ClassData(typeof<QuantityAdjustmentTests>)>]
    let ``can adjust quantities`` (start:string) (expectedResult:Expectation) =
        let target = {| Increase = increase; Decrease = decrease |}  
        match expectedResult with
        | CanNotDecrease -> target.Decrease start |> should equal None
        | CanNotIncrease -> target.Increase start |> should equal None
        | CanNotIncreaseOrDecrease -> 
            target.Decrease start |> should equal None
            target.Increase start |> should equal None
        | CanIncrease result -> 
            start
            |> target.Increase
            |> should equal (Some result)
        | CanDecrease result -> 
            start 
            |> target.Decrease
            |> should equal (Some result)
