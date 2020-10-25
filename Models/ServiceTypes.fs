module Models.ServiceTypes

open System.Threading.Tasks
open System.Threading

type ICosmosConnector =
    abstract CreateDatabaseAsync: unit -> Task
    abstract DeleteDatabaseAsync: unit -> Task
    abstract PullSinceAsync: lastSync:int -> token:CancellationToken -> Task<DtoTypes.Changes>
    abstract PullEverythingAsync: token:CancellationToken -> Task<DtoTypes.Changes>
    abstract PushAsync: DtoTypes.Changes -> token:CancellationToken -> Task

type SynchronizationStatus =
    | Synchronizing
    | NoChanges
    | HasChanges
