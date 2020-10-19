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

type RowUpdateError =
    | RowIsDeleted 

type DataTableDeleteError =
    | RowToDeleteNotFound

type DataTableInsertError =
    | DuplicateKey

type DataTableUpdateError =
    | RowToUpdateNotFound 
    | RowIsDeletedInTable
