module Models.ViewTypes

open System
open Models.CoreTypes
open Models.ValidationTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = { Format: TextFormat; Text: string }

type FormattedText = FormattedText of TextSpan list

type TimeSpanEstimate =
    | Days of int
    | Weeks of int
    | Months of int

type SelectZeroOrOne<'T when 'T: comparison> =
    { Choices: Set<'T>
      OriginalChoice: 'T option
      CurrentChoice: 'T option }

type SelectMany<'T when 'T: comparison> =
    { Items: Set<'T>
      SelectedOriginal: Set<'T>
      Selected: Set<'T> }

type ItemQuickActionsView =
    { ItemId: ItemId
      ItemName: ItemName
      PostponeUntil: DateTimeOffset option
      PermitQuickNotSoldAt: Store option
      PermitStoresCustomization: bool }

