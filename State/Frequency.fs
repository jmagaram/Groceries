module Frequency
open DomainTypes

let asDays (d:DomainTypes.Frequency) = 
    match d with
    | Daily -> 1
    | Weekly -> 7*1
    | Every2Weeks -> 7*2
    | Every3Weeks -> 7*3
    | Monthly -> 30*1
    | Every2Months -> 30*2
    | Every3Months -> 30*3

let description f =
    match f with
    | Daily -> "Daily"
    | Weekly -> "Weekly"
    | Every2Weeks -> "Every 2 weeks"
    | Every3Weeks -> "Every 3 weeks"
    | Monthly -> "Monthly"
    | Every2Months -> "Every 2 months"
    | Every3Months -> "Every 3 months"

let asSeq = 
    seq {
        yield Daily
        yield Weekly
        yield Every2Weeks
        yield Every3Weeks
        yield Monthly
        yield Every2Months
        yield Every3Months }