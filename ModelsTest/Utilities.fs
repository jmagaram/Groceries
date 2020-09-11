[<AutoOpen>]
module ModelsTest.Utilities

open System
open FsCheck

type Letter = Letter of char
type LetterOrDigit = LetterOrDigit of char
type Digit = Digit of char
type Punctuation = Punctuation of char
type Symbol = Symbol of char
type Control = Control of char
type RegexEscape = RegexEscape of char
type WhiteSpace = WhiteSpace of char

type IInteger =
    abstract AsInteger: int

type Num5() =
    interface IInteger with
        member x.AsInteger = 5

type ListOfLength<'T, 'Count> =
    | ListOfLength of 'T list * 'Count
    member this.Items =
        match this with
        | ListOfLength (items, _) -> items

type TestDataGenerators() =
    static let allCharacters = seq { Char.MinValue .. Char.MaxValue }

    static let filteredChar f = allCharacters |> Seq.filter f

    static member ListOfLength<'T, 'Count when 'Count :> IInteger and 'Count: (new: unit -> 'Count)>() =
        let count = new 'Count()
        let n = (count :> IInteger).AsInteger

        Arb.generate<'T>
        |> Gen.listOfLength n
        |> Gen.map (fun i -> ListOfLength(i, count))
        |> Arb.fromGen

    static member WhiteSpace() =
        filteredChar (fun i -> Char.IsWhiteSpace(i))
        |> Seq.map WhiteSpace
        |> Gen.elements
        |> Arb.fromGen

    static member Letter() =
        filteredChar (fun i -> Char.IsLetter(i))
        |> Seq.map Letter
        |> Gen.elements
        |> Arb.fromGen

    static member Digit() =
        filteredChar (fun i -> Char.IsDigit(i))
        |> Seq.map Digit
        |> Gen.elements
        |> Arb.fromGen

    static member LetterOrDigit() =
        filteredChar (fun i -> Char.IsLetterOrDigit(i))
        |> Seq.map Digit
        |> Gen.elements
        |> Arb.fromGen

    static member Punctuation() =
        filteredChar (fun i -> Char.IsPunctuation(i))
        |> Seq.map Punctuation
        |> Gen.elements
        |> Arb.fromGen

    static member Symbol() =
        filteredChar (fun i -> Char.IsSymbol(i))
        |> Seq.map Symbol
        |> Gen.elements
        |> Arb.fromGen

    static member Control() =
        filteredChar (fun i -> Char.IsControl(i))
        |> Seq.map Control
        |> Gen.elements
        |> Arb.fromGen

    static member RegexEscapedCharacters() =
        @"^$()[]\/?.+*# ".ToCharArray()
        |> Array.toSeq
        |> Seq.map RegexEscape
        |> Gen.elements
        |> Arb.fromGen
