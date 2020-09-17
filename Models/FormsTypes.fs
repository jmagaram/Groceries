module Models.FormsTypes

type TextInput<'T, 'Error> =
    { Value : string
      ValidationResult : Result<'T,'Error> }

type TextInputMessage =
    | LoseFocus
    | GainFocus
    | TypeText of string

// most input can just be a value plus an error message
// but a combo box and choose one specific item and enable others
// but a list of things, select 3 and certain buttons are enabled or disabled
// category - is this a combo you can type in with auto-complete?
// can you add a new category just by typing the name in?

//type Item =
//    { ItemId: ItemId
//      ItemName: ItemName
//      Note: Note option
//      Quantity: Quantity option
//      CategoryId: CategoryId option
//      Schedule: Schedule }
//    interface IKey<ItemId> with
//        member this.Key = this.ItemId



//type FormField<'TInput, T 'Proposed, 'T> =
//    { Value : 'T 
//    }

type ChooseOne<'T> when 'T : comparison =
    { Choices : 'T list 
      SelectedItem : 'T 
      Serialize : 'T -> string
      Deserialize : string -> 'T }

type ChooseOneItem<'T> = 
    { Value : 'T 
      IsSelected : bool
      Key : string }