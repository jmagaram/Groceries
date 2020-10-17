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

let setPostponedViewHorizon d s =
    let d = d |> min 365<days> |> max -365<days>
    { s with PostponedViewHorizon = d }
