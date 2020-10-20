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

type DataRowIsDeletedError = | DataRowIsDeletedError

type RowToDeleteNotFound = | RowToDeleteNotFound

type DuplicateKey = | DuplicateKey

type DataTableUpdateError =
    | RowToUpdateNotFound
    | RowIsDeletedInTable
