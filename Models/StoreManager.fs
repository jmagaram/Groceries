module Models.StoreManager

open CoreTypes
open StateTypes
open SetManager

type StoreManagerForm = StoreManagerForm of Form

let form cm =
    match cm with
    | StoreManagerForm f -> f

let crlf = "\r\n"

let splitOn = [| crlf; "\r"; "\n"; ","; ";" |]

let normalizer = StoreName.normalizer

let tryParse = StoreName.tryParse >> Result.map StoreName.asText

let create stores =
    stores
    |> Seq.map StoreName.asText
    |> BulkEdit.create normalizer crlf
    |> BulkEditMode
    |> StoreManagerForm

type StoreManagerForm with
    static member FromState(state) =
        state
        |> State.stores
        |> Seq.map (fun i -> i.StoreName)
        |> create

    member me.Form = me |> form
    member me.BulkEdit = me |> form |> Form.bulkEdit
    member me.Summary = me |> form |> Form.summary

    member me.Update(msg) =
        me
        |> form
        |> Form.update normalizer splitOn tryParse crlf msg
        |> StoreManagerForm

    member me.Errors =
        me
        |> form
        |> Form.bulkEdit
        |> Option.map (fun b ->
            b
            |> BulkEdit.items normalizer splitOn
            |> BulkEdit.validationResults tryParse
            |> Seq.choose Result.asErrorOption)
        |> Option.defaultValue Seq.empty

    member me.HasChanges =
        me
        |> form
        |> Form.summary
        |> Option.get
        |> Summary.hasChanges

    member me.ReorganizeResult =
        me
        |> form
        |> Form.summary
        |> Option.get
        |> fun s ->
            { ReorganizeStoresMessage.Delete = s.MoveOrDelete |> Seq.map (fun i -> i.Key) |> List.ofSeq
              ReorganizeStoresMessage.Create = s.Create |> List.ofSeq
              Move =
                  s.MoveOrDelete
                  |> Map.toSeq
                  |> Seq.choose (fun (x, y) -> y |> Option.map (fun y -> (x, y)))
                  |> List.ofSeq }
