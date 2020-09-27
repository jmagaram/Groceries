module Models.FormsTypes

type TextInput<'T, 'Error> = { Value: string; ValidationResult: Result<'T, 'Error> }

// when is this needed? why not just call or pass the function to change the input?
type TextInputMessage =
    | TypeText of string
    | GainFocus
    | LoseFocus

type ChooseZeroOrOne<'T> = { Choices: 'T list; Selected: 'T option }

type ChooseZeroOrOneMessage<'TKey> = 
    | ChooseByKey of 'TKey
    | ClearSelection
