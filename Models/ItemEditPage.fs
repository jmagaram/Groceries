[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemEditPage

open StateTypes

type Message =
    | BeginEditItem of SerializedId
    | BeginCreateNewItem
    | BeginCreateNewItemWithName of string
    | ItemEditFormMessage of ItemForm.Message
    | SubmitItemEditForm
    | CancelItemEditForm
    | DeleteItem

let beginEditItem id clock state =
    let form =
        state
        |> ItemForm.editItemFromSerializedId id clock 

    { state with ItemEditPage = Some form }

let beginCreateItem name state =
    let categories = state |> StateQuery.categories
    let stores = state |> StateQuery.stores
    let name = name |> Option.defaultValue ""
    let form =
        ItemForm.createNewItem name stores categories

    { state with ItemEditPage = Some form }

let private form state =
    state.ItemEditPage 
    |> Option.asResult "No form is being edited."

let updateForm msg state =
    result {
        let! form = state |> form
        let form = form |> ItemForm.handleMessage msg
        return { state with ItemEditPage = Some form }
    }

let cancel state = 
    { state with ItemEditPage = None }

let delete state =
    result {
        let! form = state |> form

        let! id =
            form.ItemId
            |> Option.asResult "Can not delete an item without an ID."

        let state =
            state
            |> StateUpdateCore.deleteItem id
            |> cancel

        return state
    }

let trySubmit now state = 
    result {
        let! form = state |> form
        let itemFormResult = form |> ItemForm.asItemFormResult now 
        let state = state |> ItemForm.processResult itemFormResult
        let state = state |> cancel
        return state
    }

// very inconsistent handling of Result; when to throw and when to handle a result
let reduce msg clock now state =
    match msg with
    | Message.BeginCreateNewItem -> state |> beginCreateItem None
    | Message.BeginCreateNewItemWithName n -> state |> beginCreateItem (Some n)
    | Message.BeginEditItem id -> state |> beginEditItem id clock
    | Message.CancelItemEditForm -> state |> cancel
    | Message.DeleteItem -> state |> delete |> Result.okOrThrow
    | Message.ItemEditFormMessage msg -> state |> updateForm msg |> Result.okOrThrow
    | Message.SubmitItemEditForm -> state |> trySubmit now |> Result.okOrThrow
