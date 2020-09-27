module Models.FormsTypes

type TextInput<'T, 'Error> = { Value: string; ValidationResult: Result<'T, 'Error> }

// when is this needed? why not just call or pass the function to change the input?
type TextInputMessage =
    | TypeText of string
    | GainFocus
    | LoseFocus

type Modes2Tag =
    | Mode1Of2Tag
    | Mode2Of2Tag

type Modes2<'T1, 'T2> = { CurrentMode: Modes2Tag; Mode1: 'T1; Mode2: 'T2 }

type Modes3Tag =
    | Mode1Of3Tag
    | Mode2Of3Tag
    | Mode3Of3Tag

type Modes3<'T1, 'T2, 'T3> =
    { CurrentMode: Modes3Tag
      Mode1: 'T1
      Mode2: 'T2
      Mode3: 'T3 }

type ChooseZeroOrOne<'T> = { Choices: 'T list; Selected: 'T option }

type ChooseZeroOrOneMessage<'TKey> = 
    | ChooseByKey of 'TKey
    | ClearSelection
