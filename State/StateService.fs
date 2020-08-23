module StateService

open DomainTypes
open FSharp.Control.Reactive;

let create = 
    { StateService.state = 
        State.create()
        |> Subject.behavior 
      now = Utilities.nowUtc }