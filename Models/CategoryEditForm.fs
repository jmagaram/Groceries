[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.CategoryEditForm

open System.Runtime.CompilerServices
open StateTypes

type FormMode =
    | CreateNewCategoryMode
    | EditExistingCategoryMode

type FormResult=
    | InsertCategory of CategoryName
    | EditCategory of Category

let createNew =
    { CategoryId = None
      CategoryName = TextBox.create ""
      Etag = None }

let editExisting (c: Category) =
    { CategoryId = Some c.CategoryId
      CategoryName = c.CategoryName |> CategoryName.asText |> TextBox.create
      Etag = c.Etag }

let editExistingByGuid id s =
    let id = CategoryId.deserialize id |> Option.get

    match s
          |> StateQuery.categoriesTable
          |> DataTable.tryFindCurrent id with
    | None -> failwith "Could not find the category to edit."
    | Some c -> c |> editExisting

let typeCategoryName s f = { f with CategoryEditForm.CategoryName = f.CategoryName |> TextBox.typeText s }

let blurCategoryName f =
    { f with
          CategoryEditForm.CategoryName = f.CategoryName  |> (TextBox.loseFocus CategoryName.normalizer) }

let validateCategoryName f = f.CategoryName.ValueTyping |> CategoryName.tryParse

let hasErrors f = f |> validateCategoryName |> Result.isError

let mode f =
    match f.CategoryId with
    | None -> CreateNewCategoryMode
    | Some _ -> EditExistingCategoryMode

let tryCommit f =
    result {
        let! name = f |> validateCategoryName

        return
            match f.CategoryId with
            | None -> FormResult.InsertCategory name
            | Some id -> FormResult.EditCategory { CategoryId = id; Etag = f.Etag; CategoryName = name }
    }

let handle msg f =
    match msg with
    | CategoryNameMessage txt -> 
        match txt with
        | TextBoxMessage.LoseFocus -> f |> blurCategoryName
        | TextBoxMessage.TypeText s -> f |> typeCategoryName s

[<Extension>]
type CategoryEditFormExtensions =
    [<Extension>]
    static member HasErrors(me: CategoryEditForm) = me |> hasErrors

    [<Extension>]
    static member CategoryNameErrors(me: CategoryEditForm) = me |> validateCategoryName |> Result.error

    [<Extension>]
    static member Mode(me: CategoryEditForm) = me |> mode
