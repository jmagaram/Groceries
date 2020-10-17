module Models.CategoryEditPage

open StateTypes

let beginEditCategory id s =
    { s with
          CategoryEditPage = CategoryEditForm.editExistingByGuid id s |> Some }

let beginCreateNewCategory s =
    { s with
          CategoryEditPage = CategoryEditForm.createNew |> Some }

let private form (s: State) =
    s.CategoryEditPage
    |> Option.asResult "There is no category editing form."

let cancel (s: State) = { s with CategoryEditPage = None }

let submit (s: State) =
    result {
        let! form = s |> form

        let! form =
            form
            |> CategoryEditForm.tryCommit
            |> Result.mapError (sprintf "%A")

        let s =
            match form with
            | CategoryEditForm.FormResult.InsertCategory c ->
                s
                |> StateUpdateCore.insertCategory
                    { CategoryName = c
                      CategoryId = CategoryId.create ()
                      Etag = None }
            | CategoryEditForm.FormResult.EditCategory c -> s |> StateUpdateCore.updateCategory c

        return s |> cancel
    }

let delete (s: State) =
    result {
        let! form = s |> form

        let! id =
            form.CategoryId
            |> Option.asResult "Can only delete an existing category, not one that is being created."

        return s |> StateUpdateCore.deleteCategory id |> cancel
    }

let private handleFormMessage m s =
    result {
        let! form = s |> form

        return
            { s with
                  CategoryEditPage = form |> CategoryEditForm.handle m |> Some }
    }

let handle (msg: CategoryEditPageMessage) (s: State) =
    match msg with
    | BeginEditCategory id -> s |> beginEditCategory id |> Ok
    | BeginCreateNewCategory -> s |> beginCreateNewCategory |> Ok
    | CategoryEditFormMessage msg -> s |> handleFormMessage msg
    | SubmitCategoryEditForm -> s |> submit 
    | CancelCategoryEditForm -> s |> cancel |> Ok
    | CategoryEditPageMessage.DeleteCategory -> s |> delete
