[<AutoOpen>]
module Models.Option

type OptionBuilder() =
    member this.Return(x) = Some x
    member this.Bind(x, f) = Option.bind f x
    member this.ReturnFrom r = r

let option = OptionBuilder()

let asResult e o =
    o
    |> Option.map Ok
    |> Option.defaultValue (Error e)