module ItemEditorModel
open DomainTypes

let create = 
    { Title = TitleTextBox.create
      Repeat = PickRepeat.create 
      Quantity = QuantityTextBox.create 
      QuantitySpinner = { CanIncrease = true; CanDecrease = true }
      Note = NoteTextBox.create
    }

let update msg model =
    match msg with
    | TitleMessage t -> { model with ItemEditorModel.Title = model.Title |> TitleTextBox.update t } 
    | NoteMessage t -> { model with Note = model.Note |> NoteTextBox.update t }
    | QuantityMessage t -> 
        let txtBox = model.Quantity |> QuantityTextBox.update t
        let spin = 
            { model.QuantitySpinner with 
                CanIncrease = txtBox.NormalizedText |> QuantitySpinner.increase |> Option.isSome
                CanDecrease = txtBox.NormalizedText |> QuantitySpinner.decrease |> Option.isSome }
        { model with
            Quantity = txtBox
            QuantitySpinner = spin }
    | RepeatMessage r -> { model with ItemEditorModel.Repeat = model.Repeat |> PickOne.update (PickOneMessage.PickOneByItem r) }
    | QuantitySpinner spin ->
        let qty =
            match spin with
            | Increase -> QuantitySpinner.increase model.Quantity.NormalizedText
            | Decrease -> QuantitySpinner.decrease model.Quantity.NormalizedText
        match qty with
        | None -> model
        | Some qty -> 
            let txtBox = model.Quantity |> QuantityTextBox.update (TextBoxMessage.SetText qty)
            let spin = 
                { model.QuantitySpinner with
                    CanIncrease = qty |> QuantitySpinner.increase |> Option.isSome 
                    CanDecrease = qty |> QuantitySpinner.decrease |> Option.isSome}
            { model with
                Quantity = txtBox
                QuantitySpinner = spin }
