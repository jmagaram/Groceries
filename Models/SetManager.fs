module Models.SetManager

open CoreTypes

type SetBulkEditForm = { Original: Set<string>; Proposed: TextBox }

type SetMapChangesForm =
    { Proposed: List<string>
      Unchanged: Set<string>
      Create: Set<string>
      MoveOrDelete: Map<string, string option> }

type SetEditWizard =
    | BulkEditMode of SetBulkEditForm
    | SetMapChangesMode of SetMapChangesForm

type SetEditWizardMessage =
    | BulkTextBoxMessage of TextBoxMessage
    | GoToSummary
    | GoBackToBulkEdit
    | MoveRename of string * string
    | Delete of string

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetBulkEditForm =

    let create normalizer delimeter items =
        { Original = items |> Seq.map normalizer |> Set.ofSeq
          Proposed =
              items
              |> SetString.fromItems normalizer delimeter
              |> TextBox.create }

    let propose normalizer delimiter ps b =
        { b with
              SetBulkEditForm.Proposed =
                  System.String.Join(delimiter, ps |> Seq.map normalizer)
                  |> TextBox.create }

    let update normalize splitOn delimiter msg b =
        let normalize = SetString.fromString normalize splitOn delimiter

        { b with
              SetBulkEditForm.Proposed = b.Proposed |> TextBox.update normalize msg }

    let items normalize splitOn (b: SetBulkEditForm) =
        b.Proposed.ValueTyping
        |> SetString.toItems normalize splitOn

    let validationResults tryParse items =
        items
        |> Seq.map (fun i ->
            i
            |> tryParse
            |> Result.mapError (fun e -> {| Proposed = i; Error = e |}))

    let tryFindIgnoreCase s items =
        items
        |> Seq.tryFind (fun i -> i |> String.equalsInvariantCultureIgnoreCase s)

    let summary normalize splitOn tryParse b =
        let items = b |> items normalize splitOn
        let validationResults = items |> validationResults tryParse |> List.ofSeq

        match validationResults |> Seq.forall Result.isOk with
        | false -> None
        | true ->
            let goal = validationResults |> Seq.choose Result.asOption |> Set.ofSeq
            let original = b.Original |> Set.ofSeq
            let unchanged = Set.intersect original goal
            let deleted = original - goal
            let created = goal - original

            let moveOrDelete =
                deleted
                |> Seq.fold (fun total source ->
                    let target = goal |> tryFindIgnoreCase source
                    total |> Map.add source target) Map.empty

            { Create = created
              Unchanged = unchanged
              MoveOrDelete = moveOrDelete
              Proposed = items |> List.ofSeq }
            |> Some

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetMapChangesForm =

    let targets s = s.Unchanged |> Seq.append s.Create |> Seq.sort

    let original s =
        s.Unchanged
        |> Seq.append (s.MoveOrDelete |> Map.keys)
        |> Seq.sort

    let private edit x y s =
        if s.MoveOrDelete |> Map.containsKey x |> not then failwith "Can not move or delete that item."

        if y
           |> Option.map (fun y -> s |> targets |> Seq.contains y |> not)
           |> Option.defaultValue false then
            failwith "That is not a valid target for moving an item."

        { s with MoveOrDelete = s.MoveOrDelete.Add(x, y) }

    let delete x s = s |> edit x None

    let move x y s = s |> edit x (Some y)

    let hasChanges s =
        (s.Create |> Seq.isEmpty |> not)
        || (s.MoveOrDelete |> Map.isEmpty |> not)

    let bulkEdit normalizer delimeter s =
        SetBulkEditForm.create normalizer delimeter (original s)
        |> SetBulkEditForm.propose normalizer delimeter s.Proposed

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetEditWizardForm =

    let bulkEdit f =
        match f with
        | BulkEditMode b -> Some b
        | _ -> None

    let summary f =
        match f with
        | SetMapChangesMode s -> Some s
        | _ -> None

    let update normalize splitOn tryParse delimiter msg f =
        match msg with
        | BulkTextBoxMessage msg ->
            f
            |> bulkEdit
            |> Option.get
            |> fun b ->
                b
                |> SetBulkEditForm.update normalize splitOn delimiter msg
                |> BulkEditMode
        | GoToSummary ->
            f
            |> bulkEdit
            |> Option.get
            |> SetBulkEditForm.summary normalize splitOn tryParse
            |> Option.get
            |> SetMapChangesMode
        | GoBackToBulkEdit ->
            f
            |> summary
            |> Option.get
            |> SetMapChangesForm.bulkEdit normalize delimiter
            |> BulkEditMode
        | MoveRename (x, y) ->
            f
            |> summary
            |> Option.get
            |> SetMapChangesForm.move x y
            |> SetMapChangesMode
        | Delete x ->
            f
            |> summary
            |> Option.get
            |> SetMapChangesForm.delete x
            |> SetMapChangesMode
