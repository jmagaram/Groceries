module Models.ServiceTypes

open System.Threading.Tasks

type PushResult<'T> =
    | Pushed of 'T * 'T
    | ConcurrencyConflict of 'T

type ICosmosConnector =
    abstract PullSinceAsync: lastSync:int -> earlierThan: int option -> Task<DtoTypes.Changes>
    abstract PullEverythingAsync: unit -> Task<DtoTypes.Changes>
    abstract PushAsync : DtoTypes.Changes -> Task<DtoTypes.Changes>

type SynchronizationStatus =
    | Synchronizing
    | NoChanges
    | HasChanges
