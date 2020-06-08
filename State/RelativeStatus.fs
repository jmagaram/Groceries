module RelativeStatus

open DomainTypes
open Xunit
open FsUnit

let active = RelativeStatus.Active

let complete = RelativeStatus.Complete

let postponedDays d = RelativeStatus.PostponedDays d

let serialize s =
    match s with
    | RelativeStatus.Active -> "active"
    | RelativeStatus.Complete -> "complete"
    | RelativeStatus.PostponedDays d  -> d.ToString()

let deserialize s =
    if s = "active"
    then RelativeStatus.Active |> Ok
    elif s = "complete"
    then RelativeStatus.Complete |> Ok
    else
        s
        |> tryParseInt
        |> Option.map RelativeStatus.PostponedDays
        |> Option.map Ok
        |> Option.defaultValue (Error (sprintf "Could not deserialize %s" s))

let orderBy r = 
    match r with
    | RelativeStatus.Active -> ("a",0)
    | RelativeStatus.PostponedDays d -> ("b",d)
    | RelativeStatus.Complete -> ("c",0)

let standardPostponeIntervals =
    [1; 7; 14; 21; 30; 60; 90; 180]
    |> Seq.map RelativeStatus.PostponedDays

type TimeUnit =
    | Days of int
    | Weeks of int
    | Months of int

let chunk d =
    if (d%30=0) then Months (d/30)
    elif (d%7=0) then Weeks (d/7)
    else Days d