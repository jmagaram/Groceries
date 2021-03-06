﻿namespace Models

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
    static member ShoppingList(me: StateTypes.State, now: DateTimeOffset) = me |> ShoppingList.create (now)

    [<Extension>]
    static member LoggedInUserSettings(me:StateTypes.State) = me |> State.userSettingsForLoggedInUser

[<Extension>]
type CoreTypeExtensions =
    [<Extension>]
    static member AsText(me: ItemName) = me |> ItemName.asText

    [<Extension>]
    static member Serialize(me: ItemId) = me |> ItemId.serialize

    [<Extension>]
    static member AsText(me: StoreName) = me |> StoreName.asText

    [<Extension>]
    static member AsText(me: CategoryName) = me |> CategoryName.asText

    [<Extension>]
    static member QuantityText(me: Quantity) = me |> Quantity.asText

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

[<Extension>]
type ItemFormExtensions =
    [<Extension>]
    static member ItemNameValidation(me: ItemForm) = me |> ItemForm.itemNameValidation

    [<Extension>]
    static member NoteValidation(me: ItemForm) = me |> ItemForm.noteValidation

    [<Extension>]
    static member QuantityValidation(me: ItemForm) = me |> ItemForm.quantityValidation

    [<Extension>]
    static member PostponeChoices(me: ItemForm) = me |> ItemForm.postponeChoices

    [<Extension>]
    static member CategoryNameValidation(me: ItemForm) = me |> ItemForm.categoryNameValidation

    [<Extension>]
    static member HasErrors(me: ItemForm) = me |> ItemForm.hasErrors

    [<Extension>]
    static member CanDelete(me: ItemForm) = me |> ItemForm.canDelete

    [<Extension>]
    static member CategoryCommittedName(me: ItemForm) = me |> ItemForm.categoryCommittedName

