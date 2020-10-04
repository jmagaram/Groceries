module Models.StoreForm

open System
open StateTypes

type Form = { StoreId: StoreId option; StoreName: string; Etag : Etag option }

type FormMode =
    | CreateStoreMode
    | EditStoreMode

type StoreFormMessage =
    | StoreNameSet of string
    | StoreNameBlur

let storeNameSet s (f: Form) = { f with StoreName = s }

let storeNameBlur (f: Form) = { f with StoreName = f.StoreName |> StoreName.normalizer }

let storeNameValidate (f: Form) = f.StoreName |> StoreName.tryParse

let hasErrors (f: Form) = f |> storeNameValidate |> Result.isError

let formMode (f: Form) =
    match f.StoreId with
    | None -> CreateStoreMode
    | Some _ -> EditStoreMode

let createNew = { StoreId = None; StoreName = ""; Etag = None }

let editExisting (s: Store) =
    { StoreId = Some s.StoreId
      StoreName = s.StoreName |> StoreName.asText 
      Etag = s.Etag
    }

let editExistingFromGuid (id: Guid) (s: State) =
    match s
          |> State.storesTable
          |> DataTable.tryFindCurrent (StoreId id) with
    | None -> failwith "Could not find the store to edit."
    | Some store -> editExisting store

let storeFormResult (f: Form) =
    let storeName = f |> storeNameValidate |> Result.okOrThrow

    match f |> formMode with
    | CreateStoreMode -> storeName |> StoreFormMessage.InsertStore
    | EditStoreMode ->
        { Store.StoreId = f.StoreId |> Option.get
          Store.StoreName = storeName 
          Store.Etag = f.Etag }
        |> StoreFormMessage.UpdateStore

let handle msg (f: Form) =
    match msg with
    | StoreNameSet s -> f |> storeNameSet s
    | StoreNameBlur -> f |> storeNameBlur

type Form with
    member me.StoreNameValidation = me |> storeNameValidate
    member me.HasErrors = me |> hasErrors
    member me.Mode = me |> formMode
    member me.StoreFormResult () = me |> storeFormResult
