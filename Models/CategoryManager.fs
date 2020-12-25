module Models.CategoryManager

open CoreTypes
open StateTypes
open SetManager

type CategoryManagerWizard = CategoryManagerWizard of SetEditWizard

let getWizard cm =
    match cm with
    | CategoryManagerWizard f -> f

let crlf = "\r\n"

let splitOn = [| crlf; "\r"; "\n"; ","; ";" |]

let normalizer = CategoryName.normalizer

let tryParse =
    CategoryName.tryParse
    >> Result.map CategoryName.asText

let create categories =
    categories
    |> Seq.map CategoryName.asText
    |> SetBulkEditForm.create normalizer crlf
    |> BulkEditMode
    |> CategoryManagerWizard

let fromState state =
    state
    |> State.categories
    |> Seq.map (fun i -> i.CategoryName)
    |> create

let update msg (f: CategoryManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.update normalizer splitOn tryParse crlf msg
    |> CategoryManagerWizard

let errors (f: CategoryManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.bulkEdit
    |> Option.map
        (fun b ->
            b
            |> SetBulkEditForm.items normalizer splitOn
            |> SetBulkEditForm.validationResults tryParse
            |> Seq.choose Result.asErrorOption)
    |> Option.defaultValue Seq.empty

let hasChanges (f: CategoryManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.summary
    |> Option.get
    |> SetMapChangesForm.hasChanges

let reorganizeResult (f: CategoryManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.summary
    |> Option.get
    |> fun s ->
        { ReorganizeCategoriesMessage.Delete =
              s.MoveOrDelete
              |> Seq.map (fun i -> i.Key)
              |> List.ofSeq
          ReorganizeCategoriesMessage.Create = s.Create |> List.ofSeq
          Move =
              s.MoveOrDelete
              |> Map.toSeq
              |> Seq.choose (fun (x, y) -> y |> Option.map (fun y -> (x, y)))
              |> List.ofSeq }

let bulkEdit (f: CategoryManagerWizard) =
    f |> getWizard |> SetEditWizardForm.bulkEdit

let summary (f: CategoryManagerWizard) =
    f |> getWizard |> SetEditWizardForm.summary

type CategoryManagerWizard with
    static member FromState(state) = state |> fromState
    member me.BulkEdit = me |> bulkEdit
    member me.Summary = me |> summary
    member me.Update(msg) = me |> update msg
    member me.Errors = me |> errors
    member me.HasChanges = me |> hasChanges
    member me.ReorganizeResult = me |> reorganizeResult
