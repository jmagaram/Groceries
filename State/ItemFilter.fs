module ItemFilter

open DomainTypes

type private IncludeAll = ItemFilter
let includeAll : IncludeAll =
    { PostponedItemFilter = PostponedItemFilter.AllPostponedItems }

