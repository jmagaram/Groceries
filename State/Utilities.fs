[<AutoOpen>]
module Utilities

let newGuid () = System.Guid.NewGuid()

let nowUtc : DomainTypes.NowUtc = fun () -> System.DateTime.UtcNow