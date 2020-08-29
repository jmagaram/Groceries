module TextBox
open DomainTypes

let fromField (f:FormField<string, _, _>) =
    { Text = f.Proposed 
      Error = f.ValidationResult |> Result.errorValue }