module Models.StoreManager

open CoreTypes
open StateTypes
open ViewTypes

type StoreManagerWizard = StoreManagerWizard of SetEditWizard

let getWizard cm =
    match cm with
    | StoreManagerWizard f -> f

let crlf = "\r\n"

let splitOn = [| crlf; "\r"; "\n"; ","; ";" |]

let normalizer = StoreName.normalizer

let tryParse =
    StoreName.tryParse >> Result.map StoreName.asText

let create stores =
    stores
    |> Seq.map StoreName.asText
    |> SetBulkEditForm.create normalizer crlf
    |> BulkEditMode
    |> StoreManagerWizard

let fromState state =
    state
    |> State.stores
    |> Seq.map (fun i -> i.StoreName)
    |> create

let update msg (f: StoreManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.update normalizer splitOn tryParse crlf msg
    |> StoreManagerWizard

let errors (f: StoreManagerWizard) =
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

let hasChanges (f: StoreManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.summary
    |> Option.get
    |> SetMapChangesForm.hasChanges

let reorganizeResult (f: StoreManagerWizard) =
    f
    |> getWizard
    |> SetEditWizardForm.summary
    |> Option.get
    |> fun s ->
        { ReorganizeStoresMessage.Delete =
              s.MoveOrDelete
              |> Seq.map (fun i -> i.Key)
              |> List.ofSeq
          ReorganizeStoresMessage.Create = s.Create |> List.ofSeq
          Move =
              s.MoveOrDelete
              |> Map.toSeq
              |> Seq.choose (fun (x, y) -> y |> Option.map (fun y -> (x, y)))
              |> List.ofSeq }

let bulkEdit (f: StoreManagerWizard) =
    f |> getWizard |> SetEditWizardForm.bulkEdit

let summary (f: StoreManagerWizard) =
    f |> getWizard |> SetEditWizardForm.summary

type StoreManagerWizard with
    static member FromState(state) = state |> fromState
    member me.BulkEdit = me |> bulkEdit
    member me.Summary = me |> summary
    member me.Update(msg) = me |> update msg
    member me.Errors = me |> errors
    member me.HasChanges = me |> hasChanges
    member me.ReorganizeResult = me |> reorganizeResult
