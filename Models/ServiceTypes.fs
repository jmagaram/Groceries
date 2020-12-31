module Models.ServiceTypes

open System.Threading.Tasks
open System.Threading

type PushResult<'T> =
    | Pushed of 'T * 'T
    | ConcurrencyConflict of 'T

type ICosmosConnector =
    abstract CreateDatabaseAsync: unit -> Task
    abstract DeleteDatabaseAsync: unit -> Task
    abstract PullSinceAsync: lastSync:int -> earlierThan: int option -> token:CancellationToken -> Task<DtoTypes.Changes>
    abstract PullEverythingAsync: token:CancellationToken -> Task<DtoTypes.Changes>
    abstract PushAsync : DtoTypes.Changes -> token:CancellationToken -> Task<DtoTypes.Changes>

type SynchronizationStatus =
    | Synchronizing
    | NoChanges
    | HasChanges
