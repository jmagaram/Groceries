module ItemEditModel
open DomainTypes

let hasErrors (m:ItemEditModel) = 
    (m.Title.ValidationResult |> Result.isError)
    || (m.Quantity.ValidationResult |> Result.isError)
    || (m.Note.ValidationResult |> Result.isError)
    || (m.Repeat.ValidationResult |> Result.isError)
    || (m.RelativeStatus.ValidationResult |> Result.isError)

let canSubmitIfNoErrors (m:ItemEditModel) =
    match m |> hasErrors, m.CanSubmit with
    | true, false -> m
    | false, true -> m
    | true, true -> { m with CanSubmit = false }
    | false, false -> { m with CanSubmit = true }

let createNew =
    let title = FormField.init (Some Title.normalize) Title.create ""
    let qty = FormField.init (Some Quantity.normalize) Quantity.create ""
    let qtyBigger = qty.ValidationResult |> Result.okValue |> Option.bind QuantitySpinner.increaseQty
    let qtySmaller = qty.ValidationResult |> Result.okValue |> Option.bind QuantitySpinner.decreaseQty
    let note = FormField.init (Some Note.normalize) Note.create ""
    let repeat = FormField.init None Ok Repeat.doesNotRepeat
    let relativeStatus = FormField.init None Ok RelativeStatus.active
    { ItemEditModel.Title = title 
      Quantity = qty
      QuantityBigger = qtyBigger
      QuantitySmaller = qtySmaller
      Note = note
      Repeat = repeat
      RelativeStatus = relativeStatus
      CanSubmit = true
      CanCancel = true
      CanDelete = false }
    |> canSubmitIfNoErrors

let titleMessageHandler = FormField.handleMessage (Some Title.normalize) Title.create
let qtyMessageHandler = FormField.handleMessage (Some Quantity.normalize) Quantity.create
let noteMessageHandler = FormField.handleMessage (Some Note.normalize) Note.create
let repeatMessageHandler = FormField.handleMessage None Ok 
let relativeStatusMessageHandler = FormField.handleMessage None Ok 

let rec update msg (model:ItemEditModel) =
    let model =
        match msg with
        | ItemEditMessage.TitleMessage m -> { model with Title = model.Title |> titleMessageHandler m }
        | ItemEditMessage.NoteMessage m -> { model with Note = model.Note |> noteMessageHandler m }
        | ItemEditMessage.QuantityMessage m -> { model with Quantity = model.Quantity |> qtyMessageHandler m }
        | ItemEditMessage.RepeatMessage m -> { model with Repeat = model.Repeat |> repeatMessageHandler m }
        | ItemEditMessage.SetRelativeStatus m -> { model with RelativeStatus = model.RelativeStatus |> relativeStatusMessageHandler m }
        | ItemEditMessage.InvokeCommand m -> 
            match m with
            | Submit -> if model |> hasErrors then failwith "Can not submit." else model
            | QuantityIncrease -> 
                match model.QuantityBigger with
                | None -> failwith "Can not increase the quantity!"
                | Some q -> 
                    let bigger = QuantitySpinner.increaseQty q
                    let smaller = QuantitySpinner.decreaseQty q
                    let qText = q |> Quantity.asString
                    let model = 
                        { model with 
                            QuantityBigger = bigger 
                            QuantitySmaller = smaller }
                    let msg = qText |> FormFieldMessage.Propose |> QuantityMessage
                    model |> update msg
            | QuantityDecrease -> 
                match model.QuantitySmaller with
                | None -> failwith "Can not decrease the quantity!"
                | Some q -> 
                    let bigger = QuantitySpinner.increaseQty q
                    let smaller = QuantitySpinner.decreaseQty q
                    let qText = q |> Quantity.asString
                    let model = 
                        { model with 
                            QuantityBigger = bigger 
                            QuantitySmaller = smaller }
                    let msg = qText |> FormFieldMessage.Propose |> QuantityMessage
                    model |> update msg
            | _ -> failwith "Do not know how to handle commands right now."
    model
    |> canSubmitIfNoErrors