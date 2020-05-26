module QuantityUpDownTests

open System
open Xunit
open FsUnit
open QuantityUpDown

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
    let target = instance  
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
