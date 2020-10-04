module Models.CategoryForm

open System
open StateTypes

type Form = { CategoryId: CategoryId option; CategoryName: string; Etag : Etag option }

type FormMode =
    | CreateCategoryMode
    | EditCategoryMode

type CategoryFormMessage =
    | CategoryNameSet of string
    | CategoryNameBlur

let catNameSet s (f: Form) = { f with CategoryName = s }

let catNameBlur (f: Form) = { f with CategoryName = f.CategoryName |> CategoryName.normalizer }

let catNameValidate (f: Form) = f.CategoryName |> CategoryName.tryParse

let hasErrors (f: Form) = f |> catNameValidate |> Result.isError

let formMode (f: Form) =
    match f.CategoryId with
    | None -> CreateCategoryMode
    | Some _ -> EditCategoryMode

let createNew = { CategoryId = None; CategoryName = ""; Etag = None }

let editExisting (s: Category) =
    { CategoryId = Some s.CategoryId
      CategoryName = s.CategoryName |> CategoryName.asText 
      Etag = s.Etag }

let editExistingFromGuid (id: Guid) (s: State) =
    match s
          |> State.categoriesTable
          |> DataTable.tryFindCurrent (CategoryId id) with
    | None -> failwith "Could not find the category to edit."
    | Some cat -> editExisting cat

let catFormResult (f: Form) =
    let catName = f |> catNameValidate |> Result.okOrThrow

    match f |> formMode with
    | CreateCategoryMode -> catName |> CategoryFormMessage.InsertCategory
    | EditCategoryMode ->
        { Category.CategoryId = f.CategoryId |> Option.get
          Category.CategoryName = catName 
          Category.Etag = f.Etag }
        |> CategoryFormMessage.UpdateCategory

let handle msg (f: Form) =
    match msg with
    | CategoryNameSet s -> f |> catNameSet s
    | CategoryNameBlur -> f |> catNameBlur

type Form with
    member me.CategoryNameValidation = me |> catNameValidate
    member me.HasErrors = me |> hasErrors
    member me.Mode = me |> formMode
    member me.CategoryFormResult () = me |> catFormResult
