module TextField
open DomainTypes

let loseFocus field = 
    { field with BindToTextBox = field.NormalizedText }

let typeText normalize validate =
    fun s field -> 
        let s = s |> normalize
        let res = s |> validate
        { field with
            NormalizedText = s 
            ValidationResult = res }

let init normalize validate str =
    let str = str |> normalize
    let res = str |> validate
    { BindToTextBox = str 
      NormalizedText = str 
      ValidationResult = res }


