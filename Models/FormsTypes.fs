module Models.FormsTypes

type TextInput<'T, 'Error> = { Value: string; ValidationResult: Result<'T, 'Error> }

// when is this needed? why not just call or pass the function to change the input?
type TextInputMessage =
    | LoseFocus
    | GainFocus
    | TypeText of string

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

//SelectedItem : 'T
//Serialize : 'T -> string
//Deserialize : string -> 'T }

// to display in HTML need to convert each item to a:
// display as
// key
// is selected or not?
//
// after selected, get the key
// or use an index, like selected index

//type ChooseOneItem<'T> = { Value: 'T; IsSelected: bool; Key: string }

// is there a nothing selected?
//type ChooseOne<'T when 'T: comparison> = { Choices: 'T list; Selected: 'T } // what if this is not in the list?

//type EditShoppingItemCommand =
//    | IncreaseQuantity
//    | DecreaseQuantity
//    | Delete

//module EditItemForm =
//    open StateTypes

//    type StoreAvailability =
//        | NotItem
//        | NotItemAndNotCategory
//        | SellsItem

//    type T =
//        { ItemName : TextInput<ItemName, ValidationTypes.StringError>
//          Category : TextInput<CategoryName, ValidationTypes.StringError>



// enabled, disabled, invisible

// can increase
