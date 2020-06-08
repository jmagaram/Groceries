module ItemSummary
open DomainTypes

type ConvertFromItem = Item -> ItemSummary
let convertFromItem : ConvertFromItem = fun i ->
    { Id = i.Id
      Title = i.Title |> Title.asString 
      Quantity = 
        i.Quantity 
        |> Option.map Quantity.asString
        |> Option.defaultValue ""
      Note = 
        i.Note 
        |> Option.map Note.asString
        |> Option.defaultValue ""
      Repeat = i.Repeat
      Status = i.Status }
