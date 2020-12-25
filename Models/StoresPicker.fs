[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.StoresPicker

open CoreTypes
open SelectMany

let createFromAvailability availability =
    availability
    |> Seq.map (fun j -> j.Store)
    |> create
    |> withOriginalSelection (
        availability
        |> Seq.choose (fun j -> if j.IsSold then Some j.Store else None)
    )

let create itemId state =
    let itemName =
        state
        |> State.tryFindItem itemId
        |> Option.get
        |> fun i -> i.ItemName

    let model =
        state
        |> State.stores
        |> Seq.map
            (fun s ->
                { ItemAvailability.Store = s
                  IsSold = state |> State.storeSellsItemById itemId s.StoreId })
        |> createFromAvailability

    (itemName, model)

let asItemAvailability s =
    s
    |> SelectMany.selectionSummary
    |> Seq.map
        (fun i ->
            { ItemAvailability.Store = i.Item
              IsSold = i.IsSelected })

let asItemAvailabilityMessage itemId s =
    (itemId, s |> asItemAvailability)
    |> StateTypes.StateMessage.ItemAvailabilityMessage
