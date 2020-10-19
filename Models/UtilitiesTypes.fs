[<AutoOpen>]
module Models.UtilitiesTypes

open System

[<Measure>]
type days

type IKey<'TKey> =
    abstract Key: 'TKey

type Clock = unit -> DateTimeOffset
