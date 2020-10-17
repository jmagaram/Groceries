[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.StoreEditForm

open System.Runtime.CompilerServices
open StateTypes

let createNew =
    { StoreId = None
      StoreName = TextBox.create ""
      Etag = None }

let editExisting (c: Store) =
    { StoreId = Some c.StoreId
      StoreName = c.StoreName |> StoreName.asText |> TextBox.create
      Etag = c.Etag }

let editExistingByGuid id s =
    let id = StoreId.deserialize id |> Option.get

    match s
          |> StateQuery.storesTable
          |> DataTable.tryFindCurrent id with
    | None -> failwith "Could not find the store to edit."
    | Some c -> c |> editExisting

let typeStoreName s f = { f with StoreEditForm.StoreName = f.StoreName |> TextBox.typeText s }

let blurStoreName f =
    { f with
          StoreEditForm.StoreName = f.StoreName  |> (TextBox.loseFocus StoreName.normalizer) }

let validateStoreName f = f.StoreName.ValueTyping |> StoreName.tryParse

let hasErrors f = f |> validateStoreName |> Result.isError

type FormMode =
    | CreateNewStoreMode
    | EditExistingStoreMode

let mode f =
    match f.StoreId with
    | None -> CreateNewStoreMode
    | Some _ -> EditExistingStoreMode

let tryCommit f =
    result {
        let! name = f |> validateStoreName

        return
            match f.StoreId with
            | None -> StoreEditFormResult.InsertStore name
            | Some id -> StoreEditFormResult.EditStore { StoreId = id; Etag = f.Etag; StoreName = name }
    }

let handle msg f =
    match msg with
    | StoreNameMessage txt -> 
        match txt with
        | TextBoxMessage.LoseFocus -> f |> blurStoreName
        | TextBoxMessage.TypeText s -> f |> typeStoreName s

[<Extension>]
type StoreEditFormExtensions =
    [<Extension>]
    static member HasErrors(me: StoreEditForm) = me |> hasErrors

    [<Extension>]
    static member StoreNameErrors(me: StoreEditForm) = me |> validateStoreName |> Result.error

    [<Extension>]
    static member Mode(me: StoreEditForm) = me |> mode
