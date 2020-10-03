module Models.SynchronizationTypes

type DataRow<'T> = 
    | Unchanged of 'T
    | Modified of {| Original: 'T; Current: 'T |}
    | Added of 'T
    | Deleted of 'T

type DataTable<'Key, 'T when 'Key: comparison> = DataTable of Map<'Key, DataRow<'T>>
