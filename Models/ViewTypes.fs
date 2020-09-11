module Models.ViewTypes

type TextFormat =
    | Highlight
    | Normal

type TextSpan = 
    { Format : TextFormat
      Text : string }

type SearchTerm = | SearchTerm of string

type FormattedText = FormattedText of TextSpan list

//type TextFilterError = | FilterIsEmptyOrWhitespace
//type TextFilter = | CaseInsensitiveTextFilter of string

// finding matches is different than displaying them

//type Span =
//    { Format : Format 
//      Text : string }


//type Item =
//    { ItemId: StateTypes.ItemId
//      Name: StateTypes.ItemName
//      Note: StateTypes.Note option
//      Quantity: StateTypes.Quantity option
//      Category: StateTypes.Category option
//      NotSold : StateTypes.Store list
//      Schedule: StateTypes.Schedule }

