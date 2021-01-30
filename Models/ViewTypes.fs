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

type SetBulkEditForm =
    { Original: Set<string>
      Proposed: TextBox }

type SetMapChangesForm =
    { Proposed: List<string>
      Unchanged: Set<string>
      Create: Set<string>
      MoveOrDelete: Map<string, string option> }

type SetEditWizard =
    | BulkEditMode of SetBulkEditForm
    | SetMapChangesMode of SetMapChangesForm

type SetEditWizardMessage =
    | BulkTextBoxMessage of TextBoxMessage
    | GoToSummary
    | GoBackToBulkEdit
    | MoveRename of string * string
    | Delete of string

//type EditFamilyFormMode =
//    | NotYetSubmitted
//    | Submitting
//    | SubmitErrorOf of string

type EditFamilyForm =
    { FamilyName: TextBox
      FamilyId: FamilyId
      Invitees: TextBox list
      FilledOutBy: EmailAddress
      Etag: Etag option }