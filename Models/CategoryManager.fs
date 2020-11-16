module Models.CategoryManager

open CoreTypes
open StateTypes
open SetManager

type CategoryManagerForm = CategoryManagerForm of Form

let form cm =
    match cm with
    | CategoryManagerForm f -> f

let crlf = "\r\n"

let splitOn = [| crlf; "\r"; "\n"; ","; ";" |]

let normalizer = CategoryName.normalizer

let tryParse = CategoryName.tryParse >> Result.map CategoryName.asText

let create categories =
    categories
    |> Seq.map CategoryName.asText
    |> BulkEdit.create normalizer crlf
    |> BulkEditMode
    |> CategoryManagerForm

type CategoryManagerForm with
    static member FromState(state) =
        state
        |> State.categories
        |> Seq.map (fun i -> i.CategoryName)
        |> create

    member me.Form = me |> form
    member me.BulkEdit = me |> form |> Form.bulkEdit
    member me.Summary = me |> form |> Form.summary

    member me.Update(msg) =
        me
        |> form
        |> Form.update normalizer splitOn tryParse crlf msg
        |> CategoryManagerForm

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
            { ReorganizeCategoriesMessage.Delete =
                  s.MoveOrDelete
                  |> Seq.map (fun i -> i.Key)
                  |> List.ofSeq
              ReorganizeCategoriesMessage.Create = s.Create |> List.ofSeq
              Move =
                  s.MoveOrDelete
                  |> Map.toSeq
                  |> Seq.choose (fun (x, y) -> y |> Option.map (fun y -> (x, y)))
                  |> List.ofSeq
            }
