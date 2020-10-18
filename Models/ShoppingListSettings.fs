[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ShoppingListSettings

open StateTypes

let create =
    { ShoppingListSettings.StoreFilter = None
      PostponedViewHorizon = 7<days>
      HideCompletedItems = true
      ItemTextFilter = None }

let setStoreFilter f s = { s with StoreFilter = f }

let clearStoreFilterIf k s =
    if s.StoreFilter = Some k then s |> setStoreFilter None else s

let setItemFilter f s = { s with ItemTextFilter = f |> SearchTerm.tryCoerce }

let clearItemFilter s = { s with ItemTextFilter = None }

let hideCompletedItems b s = { s with HideCompletedItems = b }

let setPostponedViewHorizon d s =
    let d = d |> min 365<days> |> max -365<days>
    { s with PostponedViewHorizon = d }

type Message =
    | ClearStoreFilter
    | SetStoreFilterTo of StoreId
    | SetPostponedViewHorizon of int<days>
    | HideCompletedItems of bool
    | SetItemFilter of string
    | ClearItemFilter

let map f s = { s with ShoppingListSettings = s.ShoppingListSettings |> DataRow.mapCurrent f }

let reduce msg (s:State) =
    match msg with
    | ClearStoreFilter -> s |> map (setStoreFilter None)
    | SetStoreFilterTo id -> s |> map (setStoreFilter (Some id))
    | SetPostponedViewHorizon d -> s |> map (setPostponedViewHorizon d)
    | HideCompletedItems b -> s |> map (hideCompletedItems b)
    | ClearItemFilter -> s |> map clearItemFilter
    | SetItemFilter f -> s |> map (setItemFilter f)
