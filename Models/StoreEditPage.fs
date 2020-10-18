module Models.StoreEditPage

open StateTypes

type Message =
    | BeginEditStore of SerializedId
    | BeginCreateNewStore
    | StoreEditFormMessage of StoreEditForm.Message
    | SubmitStoreEditForm
    | CancelStoreEditForm
    | DeleteStore

let beginEditStore id s =
    { s with
          StoreEditPage = StoreEditForm.editExistingByGuid id s |> Some }

let beginCreateNewStore s =
    { s with
          StoreEditPage = StoreEditForm.createNew |> Some }

let private form (s: State) =
    s.StoreEditPage
    |> Option.asResult "There is no category editing form."

let cancel (s: State) = { s with StoreEditPage = None }

let submit (s: State) =
    result {
        let! form = s |> form

        let! form =
            form
            |> StoreEditForm.tryCommit
            |> Result.mapError (sprintf "%A")

        let s =
            match form with
            | StoreEditForm.FormResult.InsertStore c ->
                s
                |> StateUpdateCore.insertStore
                    { StoreName = c
                      StoreId = StoreId.create ()
                      Etag = None }
            | StoreEditForm.FormResult.EditStore c -> s |> StateUpdateCore.updateStore c

        return s |> cancel
    }

let delete (s: State) =
    result {
        let! form = s |> form

        let! id =
            form.StoreId
            |> Option.asResult "Can only delete an existing category, not one that is being created."

        return s |> StateUpdateCore.deleteStore id |> cancel
    }

let private handleFormMessage m s =
    result {
        let! form = s |> form

        return
            { s with
                  StoreEditPage = form |> StoreEditForm.handle m |> Some }
    }

let handle (msg: Message) (s: State) =
    match msg with
    | BeginEditStore id -> s |> beginEditStore id |> Ok
    | BeginCreateNewStore -> s |> beginCreateNewStore |> Ok
    | StoreEditFormMessage msg -> s |> handleFormMessage msg
    | SubmitStoreEditForm -> s |> submit 
    | CancelStoreEditForm -> s |> cancel |> Ok
    | DeleteStore -> s |> delete
