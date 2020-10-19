module Models.ChangeTrackerTypes

type DataRow<'T> =
    | Unchanged of 'T
    | Modified of {| Original: 'T; Current: 'T |}
    | Added of 'T
    | Deleted of 'T

type DataTable<'Key, 'T when 'Key: comparison> = DataTable of Map<'Key, DataRow<'T>>

type Change<'T, 'Key> =
    | Upsert of 'T
    | Delete of 'Key

type RowIsDeletedError = RowIsDeletedError
type DuplicateKeyError = DuplicateKeyError
type KeyNotFoundError = KeyNotFoundError

type UpdateError =
    | KeyNotFound of KeyNotFoundError
    | RowIsDeleted of RowIsDeletedError
