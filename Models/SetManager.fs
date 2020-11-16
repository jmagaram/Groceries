module Models.SetManager

open CoreTypes

type BulkEdit = { Original: Set<string>; Proposed: TextBox }

type Summary =
    { Proposed: List<string>
      Unchanged: Set<string>
      Create: Set<string>
      MoveOrDelete: Map<string, string option> }

type Form =
    | BulkEditMode of BulkEdit
    | SummaryMode of Summary

type Message =
    | BulkTextBoxMessage of TextBoxMessage
    | GoToSummary
    | GoBackToBulkEdit
    | MoveRename of string * string
    | Delete of string

module BulkEdit =

    let create normalizer delimeter items =
        { Original = items |> Seq.map normalizer |> Set.ofSeq
          Proposed =
              items
              |> SetString.fromItems normalizer delimeter
              |> TextBox.create }

    let propose normalizer delimiter ps b =
        { b with
              BulkEdit.Proposed =
                  System.String.Join(delimiter, ps |> Seq.map normalizer)
                  |> TextBox.create }

    let update normalize splitOn delimiter msg b =
        let normalize = SetString.fromString normalize splitOn delimiter

        { b with
              BulkEdit.Proposed = b.Proposed |> TextBox.update normalize msg }

    let items normalize splitOn (b: BulkEdit) =
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

module Summary =

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
        BulkEdit.create normalizer delimeter (original s)
        |> BulkEdit.propose normalizer delimeter s.Proposed

module Form =

    let bulkEdit f =
        match f with
        | BulkEditMode b -> Some b
        | _ -> None

    let summary f =
        match f with
        | SummaryMode s -> Some s
        | _ -> None

    let update normalize splitOn tryParse delimiter msg f =
        match msg with
        | BulkTextBoxMessage msg ->
            f
            |> bulkEdit
            |> Option.get
            |> fun b ->
                b
                |> BulkEdit.update normalize splitOn delimiter msg
                |> BulkEditMode
        | GoToSummary ->
            f
            |> bulkEdit
            |> Option.get
            |> BulkEdit.summary normalize splitOn tryParse
            |> Option.get
            |> SummaryMode
        | GoBackToBulkEdit ->
            f
            |> summary
            |> Option.get
            |> Summary.bulkEdit normalize delimiter
            |> BulkEditMode
        | MoveRename (x, y) ->
            f
            |> summary
            |> Option.get
            |> Summary.move x y
            |> SummaryMode
        | Delete x ->
            f
            |> summary
            |> Option.get
            |> Summary.delete x
            |> SummaryMode
