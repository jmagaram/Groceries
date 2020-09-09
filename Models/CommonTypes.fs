[<AutoOpen>]
module Models.CommonTypes

type IKey<'TKey> =
    abstract Key: 'TKey
