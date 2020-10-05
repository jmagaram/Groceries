module Models.SynchronizationTypes

type DataRow<'T> =
    | Unchanged of 'T
    | Modified of {| Original: 'T; Current: 'T |}
    | Added of 'T
    | Deleted of 'T

type DataTable<'Key, 'T when 'Key: comparison> = DataTable of Map<'Key, DataRow<'T>>

type Change<'T, 'Key> =
    | Upsert of 'T
    | Delete of 'Key

type ResolveChanges<'T, 'Key when 'Key: comparison> =
    Change<'T, 'Key> list -> DataTable<'Key, 'T> -> DataTable<'Key, 'T>
