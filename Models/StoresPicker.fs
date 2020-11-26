[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.StoresPicker

open CoreTypes
open SelectMany

let storeAvailabilityPicker itemId state =
    state
    |> State.stores
    |> Seq.map
        (fun s ->
            { ItemAvailability.Store = s
              IsSold =
                  state
                  |> State.notSoldTable
                  |> DataTable.tryFindCurrent { NotSoldItem.ItemId = itemId; StoreId = s.StoreId }
                  |> Option.isNone })
    |> fun i ->
        i
        |> Seq.map (fun j -> j.Store)
        |> create
        |> withOriginalSelection
            (i
             |> Seq.choose (fun j -> if j.IsSold then Some j.Store else None))

let asItemAvailability s =
    s
    |> SelectMany.selectionSummary
    |> Seq.map (fun i -> { ItemAvailability.Store = i.Item; IsSold = i.IsSelected })

let asItemAvailabilityMessage itemId s =
    (itemId, s |> asItemAvailability)
    |> StateTypes.StateMessage.ItemAvailabilityMessage