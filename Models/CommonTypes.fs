[<AutoOpen>]
module Models.CommonTypes

type IKey<'TKey> =
    abstract Key: 'TKey

type Clock = unit -> System.DateTimeOffset