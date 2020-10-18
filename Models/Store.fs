namespace Models
open System
open StateTypes
open ValidationTypes
open StringValidation

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreId =

    let create () = newGuid () |> StoreId

    let serialize i =
        match i with
        | StoreId g -> g |> Guid.serialize

    let deserialize s = s |> Guid.tryDeserialize |> Option.map StoreId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreName =

    let rules = singleLine 3<chars> 30<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator StoreName List.head

    let asText (StoreName s) = s
