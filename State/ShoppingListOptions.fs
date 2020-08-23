module ShoppingListOptions
open DomainTypes

let createDefault =
    { ShoppingListOptions.Filter = ItemFilter.includeAll }
