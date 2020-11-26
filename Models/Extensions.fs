module Models.Extensions

open System
open System.Runtime.CompilerServices
open CoreTypes
open StateTypes
open ViewTypes

[<Extension>]
type StateExtensions =
    [<Extension>]
    static member ItemDetails(me: State, itemId) = me |> ShoppingList.createItemFromItemId itemId

    [<Extension>]
    static member Categories(me: State) =
        me
        |> State.categories
        |> Seq.sortBy (fun i -> i.CategoryName)

    [<Extension>]
    static member Stores(me: State) = me |> State.stores

[<Extension>]
type CoreTypeExtensions =
    [<Extension>]
    static member AsText(me: StoreName) = me |> StoreName.asText

    [<Extension>]
    static member AsText(me: CategoryName) = me |> CategoryName.asText

    [<Extension>]
    static member QuantityText(me: Quantity) = me |> Quantity.asText

    [<Extension>]
    static member AsRepeat(me: Schedule) = me |> Schedule.asRepeat

    [<Extension>]
    static member AsText(me: Frequency) = me |> ItemForm.frequencyAsText

    [<Extension>]
    static member Frequency(me: Schedule) =
        me
        |> Schedule.tryAsRepeat
        |> Option.map (fun i -> i.Frequency)

    [<Extension>]
    static member PostponedUntil(me: Schedule, now: DateTimeOffset) = me |> Schedule.postponedUntilDays now

[<Extension>]
type ItemDetailExtensions =
    [<Extension>]
    static member StoresNotSold(me: ShoppingList.Item) =
        me.Availability
        |> Seq.choose (fun i -> if i.IsSold then None else i.Store.StoreName |> StoreName.asText |> Some)
        |> Seq.sort

[<Extension>]
type GeneralExtensions =
    [<Extension>]
    static member AsString(me: FormattedText) = me |> FormattedText.asString
