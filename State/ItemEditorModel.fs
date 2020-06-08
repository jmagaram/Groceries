module ItemEditorModel
open DomainTypes

let create = 
    let initialQuantity = ""
    { Title = TitleTextBox.create |> TitleTextBox.update (TextBoxMessage.SetText "")
      Repeat = RepeatSelector.create |> (PickOne.selectIf Repeat.isDoesNotRepeat)
      Quantity = QuantityTextBox.create |> QuantityTextBox.update (TextBoxMessage.SetText initialQuantity)
      QuantitySpinner = QuantitySpinner.create initialQuantity
      Note = NoteTextBox.create |> NoteTextBox.update (TextBoxMessage.SetText "")
      Status = RelativeStatusSelector.create |> PickOne.select RelativeStatus.active }

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
    | RelativeStatusSelectorMessage msg -> 
        { model with Status = model.Status |> PickOne.update (PickOneMessage.PickOneByItem msg) }

let hasErrors (m:ItemEditorModel) =
    m.Note.Error.IsSome || m.Title.Error.IsSome || m.Quantity.Error.IsSome

let toNewItem (now:System.DateTime) (m:ItemEditorModel) =
    { Id = ItemId newGuid
      Title = Title m.Title.NormalizedText
      Note = Note m.Note.NormalizedText |> Some
      Quantity = Quantity m.Quantity.NormalizedText |> Some
      Repeat = m.Repeat.SelectedItem
      Status = 
        match m.Status.SelectedItem with
        | DomainTypes.RelativeStatus.Active -> DomainTypes.Status.Active
        | DomainTypes.RelativeStatus.Complete -> DomainTypes.Status.Complete
        | DomainTypes.RelativeStatus.PostponedDays d -> DomainTypes.Status.Postponed (now.AddDays(d |> float)) }
