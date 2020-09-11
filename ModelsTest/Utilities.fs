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
    abstract Value: int

type ListOfSize<'T, 'Count> =
    | ListOfSize of 'T list * 'Count
    member this.Items =
        match this with
        | ListOfSize (items, _) -> items

type ListInSizeRange<'T, 'MinLength, 'MaxLength> =
    | ListInSizeRange of 'T list * 'MinLength * 'MaxLength
    member this.Items =
        match this with
        | ListInSizeRange (items, _, _) -> items

type TestDataGenerators() =
    static let allCharacters = seq { Char.MinValue .. Char.MaxValue }

    static let filteredChar f = allCharacters |> Seq.filter f


    static member ListInSizeRange<'T,'Min,'Max 
                                    when 'Min :> IInteger 
                                    and 'Min: (new: unit -> 'Min) 
                                    and 'Max :> IInteger 
                                    and 'Max: (new: unit -> 'Max)>() =
        let min = new 'Min()
        let max = new 'Max()
        let nMin = (min :> IInteger).Value
        let nMax = (max :> IInteger).Value
        gen {
            let! n = Gen.choose (nMin, nMax)
            return! Arb.generate<'T> |> Gen.listOfLength n
        }
        |> Gen.map (fun i -> ListInSizeRange(i, min, max))
        |> Arb.fromGen

    static member ListOfLength<'T, 'Count when 'Count :> IInteger and 'Count: (new: unit -> 'Count)>() =
        let count = new 'Count()
        let n = (count :> IInteger).Value

        Arb.generate<'T>
        |> Gen.listOfLength n
        |> Gen.map (fun i -> ListOfSize(i, count))
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

type Int1() =
    interface IInteger with
        member _.Value = 1

type Int2() =
    interface IInteger with
        member _.Value = 2

type Int3() =
    interface IInteger with
        member _.Value = 3

type Int4() =
    interface IInteger with
        member _.Value = 4

type Int5() =
    interface IInteger with
        member _.Value = 5

type Int6() =
    interface IInteger with
        member _.Value = 6

type Int7() =
    interface IInteger with
        member _.Value = 7

type Int8() =
    interface IInteger with
        member _.Value = 8

type Int9() =
    interface IInteger with
        member _.Value = 9

type Int10() =
    interface IInteger with
        member _.Value = 10

type Int11() =
    interface IInteger with
        member _.Value = 11

type Int12() =
    interface IInteger with
        member _.Value = 12

type Int13() =
    interface IInteger with
        member _.Value = 13

type Int14() =
    interface IInteger with
        member _.Value = 14

type Int15() =
    interface IInteger with
        member _.Value = 15

type Int16() =
    interface IInteger with
        member _.Value = 16

type Int17() =
    interface IInteger with
        member _.Value = 17

type Int18() =
    interface IInteger with
        member _.Value = 18

type Int19() =
    interface IInteger with
        member _.Value = 19

type Int20() =
    interface IInteger with
        member _.Value = 20

type Int30() =
    interface IInteger with
        member _.Value = 30

type Int40() =
    interface IInteger with
        member _.Value = 40

type Int50() =
    interface IInteger with
        member _.Value = 50

type Int60() =
    interface IInteger with
        member _.Value = 60

type Int70() =
    interface IInteger with
        member _.Value = 70

type Int80() =
    interface IInteger with
        member _.Value = 80

type Int90() =
    interface IInteger with
        member _.Value = 90

type Int100() =
    interface IInteger with
        member _.Value = 100
