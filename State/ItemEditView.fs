module ItemEditView
open System
open DomainTypes
open FSharp.Control.Reactive

let create (m:ItemEditModel) = 
    let title = m.Title |> TextBox.fromField
    let quantity = m.Quantity |> TextBox.fromField
    let note = m.Note |> TextBox.fromField
    let quantitySpinner = 
        { Spinner.CanIncrease = m.QuantityBigger.IsSome
          Spinner.CanDecrease = m.QuantitySmaller.IsSome }
    let repeat =
        let serialize = Repeat.serialize 
        let deserialize = Repeat.deserialize >> Result.okValueOrThrow
        let choices = 
            let repeaters = Repeat.standardRepeatIntervals
            let doesNotRepeat = Repeat.doesNotRepeat |> Seq.singleton
            let current = m.Repeat.Proposed |> Seq.singleton
            repeaters
            |> Seq.append doesNotRepeat
            |> Seq.append current
            |> Seq.distinct
            |> Seq.sortBy (fun i ->
                match i with
                | Repeat.DoesNotRepeat -> -1
                | Repeat.DailyInterval x -> x)
            |> List.ofSeq
        ChooseOne.init serialize deserialize choices
        |> ChooseOne.select m.Repeat.Proposed
    let relativeStatus =
        let serialize = RelativeStatus.serialize
        let deserialize = RelativeStatus.deserialize >> Result.okValueOrThrow
        let choices = 
            let standardPostpone = [0; 1; 7; 14; 21; 30; 60; 90] |> Seq.map RelativeStatus.postponedDays
            let active = RelativeStatus.active |> Seq.singleton
            let complete = RelativeStatus.complete |> Seq.singleton
            let current = m.RelativeStatus |> FormField.proposed |> Seq.singleton
            standardPostpone
            |> Seq.append active
            |> Seq.append complete
            |> Seq.append current
            |> Seq.distinct
            |> Seq.sortBy (fun i -> 
                match i with
                | RelativeStatus.Active -> -1
                | RelativeStatus.Complete -> System.Int32.MaxValue
                | RelativeStatus.PostponedDays x -> x)
            |> List.ofSeq
        ChooseOne.init serialize deserialize choices
        |> ChooseOne.select m.RelativeStatus.Proposed
    let commands = if m.CanSubmit then ItemEditCommand.Submit |> Set.singleton else Set.empty 
    { Title = title
      Quantity = quantity
      Note = note
      Repeat = repeat
      RelativeStatus = relativeStatus
      QuantitySpinner = quantitySpinner
      Commands = commands }

let fromObservable (m:IObservable<ItemEditModel>) = 
    m
    |> Observable.map create