module Models.FormsTypes

type TextInput<'T, 'Error> = { Value: string; ValidationResult: Result<'T, 'Error> }

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

type ChooseOneItem<'T> = { Value: 'T; IsSelected: bool; Key: string }

// is there a nothing selected?
type ChooseOne<'T when 'T: comparison> =
    { Choices: 'T list
      Selected: 'T } // what if this is not in the list?

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
