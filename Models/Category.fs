namespace Models

open System
open StateTypes
open ValidationTypes
open StringValidation

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CategoryId =

    let create () = newGuid () |> CategoryId

    let serialize i =
        match i with
        | CategoryId g -> g |> Guid.serialize

    let deserialize s =
        s
        |> Guid.tryDeserialize
        |> Option.map CategoryId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CategoryName =

    let rules = singleLine 3<chars> 30<chars>
    let normalizer = String.trim

    let validator = rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator CategoryName List.head

    let asText (CategoryName s) = s
