module Models.FormsTypes

type ChooseOne<'T> when 'T : comparison =
    { Choices : 'T list 
      SelectedItem : 'T 
      Serialize : 'T -> string
      Deserialize : string -> 'T }

type ChooseOneItem<'T> = 
    { Value : 'T 
      IsSelected : bool
      Key : string }

