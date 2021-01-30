namespace ModelsTest

open Models
open Xunit
open FsUnit
open FsUnit.Xunit
open FsCheck
open FsCheck.Xunit

module StateTests =

    [<Fact>]
    let ``can create sample data`` () =
        let x = Models.State.createSampleData FamilyId.anonymous UserId.anonymous
        true
