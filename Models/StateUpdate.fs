[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.StateUpdate

open System
open StateTypes
open StateUpdateCore

type StateMessage =
    | ItemEditPageMessage of ItemEditPageMessage
    | CategoryEditPageMessage of CategoryEditPage.Message
    | StoreEditPageMessage of StoreEditPage.Message
    | ItemMessage of ItemMessage
    | ShoppingListSettingsMessage of ShoppingListSettingsMessage
    | Import of ImportChanges
    | AcceptAllChanges
    | ResetToSampleData

let updateSettingsStoreFilter k s =
    let isStoreReferenceValid =
        k
        |> Option.map (fun k -> s.Stores |> DataTable.tryFindCurrent k |> Option.isSome)
        |> Option.defaultValue true

    match isStoreReferenceValid with
    | false -> failwith "A store is referenced that does not exist."
    | true -> mapSettings (ShoppingListSettings.setStoreFilter k) s

let hideCompletedItems b s =
    s
    |> mapSettings (fun i -> { i with HideCompletedItems = b })

let createDefault =
    { Categories = DataTable.empty
      Items = DataTable.empty
      Stores = DataTable.empty
      NotSoldItems = DataTable.empty
      ShoppingListSettings = DataRow.unchanged ShoppingListSettings.create
      LastCosmosTimestamp = None
      CategoryEditPage = None
      StoreEditPage = None
      ItemEditPage = None }

let createSampleData () =

    let newCategory n s =
        s
        |> insertCategory
            { Category.CategoryId = CategoryId.create ()
              CategoryName = n |> CategoryName.tryParse |> Result.okOrThrow
              Etag = None }

    let newStore n s =
        s
        |> insertStore
            { Store.StoreId = StoreId.create ()
              StoreName = n |> StoreName.tryParse |> Result.okOrThrow
              Etag = None }

    let findCategory n (s: State) =
        let n = CategoryName.tryParse n |> Result.okOrThrow

        s.Categories
        |> DataTable.current
        |> Seq.find (fun i -> i.CategoryName = n)

    let findItem n (s: State) =
        let n = ItemName.tryParse n |> Result.okOrThrow

        s.Items
        |> DataTable.current
        |> Seq.find (fun i -> i.ItemName = n)

    let findStore n (s: State) =
        let n = StoreName.tryParse n |> Result.okOrThrow

        s.Stores
        |> DataTable.current
        |> Seq.find (fun i -> i.StoreName = n)

    let newItem name cat qty note s =
        s
        |> insertItem
            { Item.ItemId = ItemId.create ()
              ItemName = name |> ItemName.tryParse |> Result.okOrThrow
              Etag = None
              Quantity = if qty = "" then None else qty |> Quantity.tryParse |> Result.okOrThrow |> Some
              Note = if note = "" then None else note |> Note.tryParse |> Result.okOrThrow |> Some
              Item.Schedule = Schedule.Once
              Item.CategoryId = if cat = "" then None else Some (findCategory cat s).CategoryId }

    let now = System.DateTimeOffset.Now

    let markComplete n (s: State) =
        let item =
            s
            |> findItem n
            |> fun i -> { i with Schedule = Schedule.Completed }

        s |> mapItems (DataTable.update item)

    let makeRepeat n freq postpone (s: State) =
        let freq = Frequency.create freq |> Result.okOrThrow

        let postpone = postpone |> Option.map (fun d -> now.AddDays(d |> float))

        let repeat = Repeat.create freq postpone

        let item =
            s
            |> findItem n
            |> fun i -> { i with Schedule = Schedule.Repeat repeat }

        s |> mapItems (DataTable.update item)

    let doesNotSellItem store item (s: State) =
        let ns =
            { NotSoldItem.ItemId = (findItem item s).ItemId
              StoreId = (findStore store s).StoreId }

        s |> mapNotSoldItems (DataTable.insert ns)

    createDefault
    |> newCategory "Meat and Seafood"
    |> newCategory "Dairy"
    |> newCategory "Frozen"
    |> newCategory "Produce"
    |> newCategory "Dry"
    |> newItem "Bananas" "Produce" "1 bunch" ""
    |> newItem "Frozen mango chunks" "Frozen" "1 bag" ""
    |> newItem "Apples" "Produce" "6 large" ""
    |> newItem "Chocolate bars" "Dry" "Assorted; many" "Prefer Eco brand"
    |> newItem "Peanut butter" "Dry" "Several jars" "Like Santa Cruz brand"
    |> newItem "Nancy's lowfat yogurt" "Dairy" "1 tub" "Check the date"
    |> newItem "Ice cream" "Frozen" "2 pints" ""
    |> newItem "Dried flax seeds" "Dry" "1 bag" ""
    |> makeRepeat "Bananas" 7<days> None
    |> makeRepeat "Peanut butter" 14<days> (Some 3<days>)
    |> makeRepeat "Apples" 14<days> (Some -3<days>)
    |> markComplete "Ice cream"
    |> newStore "QFC"
    |> newStore "Whole Foods"
    |> newStore "Trader Joe's"
    |> newStore "Costco"
    |> newStore "Walgreens"
    |> doesNotSellItem "QFC" "Dried flax seeds"
    |> doesNotSellItem "Costco" "Chocolate bars"

let markItemComplete now id s =
    let item =
        s
        |> StateQuery.itemsTable
        |> DataTable.findCurrent id
        |> Item.markComplete now

    s |> mapItems (DataTable.update item)

let removePostpone id s =
    let item =
        s
        |> StateQuery.itemsTable
        |> DataTable.findCurrent id
        |> Item.removePostpone

    s |> mapItems (DataTable.update item)

let postponeItem now id d s =
    let item =
        s
        |> StateQuery.itemsTable
        |> DataTable.findCurrent id
        |> Item.postpone now d

    s |> mapItems (DataTable.update item)

let buyAgain id s =
    let item =
        s
        |> StateQuery.itemsTable
        |> DataTable.findCurrent id
        |> Item.buyAgain

    s |> mapItems (DataTable.update item)

let rec update msg s =
    let now = clock()

    match msg with
    | CategoryEditPageMessage msg -> s |> CategoryEditPage.handle msg |> Result.okOrThrow
    | StoreEditPageMessage msg -> s |> StoreEditPage.handle msg |> Result.okOrThrow
    | ItemEditPageMessage msg -> s |> ItemEditPage.reduce msg clock now
    | AcceptAllChanges -> s |> acceptAllChanges
    | Import c -> s |> importChanges c
    | ItemMessage msg ->
        match msg with
        | MarkComplete i -> s |> markItemComplete now i
        | RemovePostpone i -> s |> removePostpone i
        | Postpone (id, d) -> s |> postponeItem now id d
        | BuyAgain i -> s |> buyAgain i
        | InsertItem i -> s |> insertItem i
        | UpdateItem i -> s |> updateItem i
        | UpsertItem i -> s |> upsertItem i
        | DeleteItem k -> s |> deleteItem k
    | ShoppingListSettingsMessage msg ->
        match msg with
        | ClearStoreFilter -> s |> updateSettingsStoreFilter None
        | SetStoreFilterTo id -> s |> updateSettingsStoreFilter (Some id)
        | SetPostponedViewHorizon d -> s |> mapSettings (ShoppingListSettings.setPostponedViewHorizon d)
        | HideCompletedItems b -> s |> hideCompletedItems b
        | ClearItemFilter -> s |> mapSettings (ShoppingListSettings.clearItemFilter)
        | SetItemFilter f -> s |> mapSettings (ShoppingListSettings.setItemFilter f)
    | ResetToSampleData -> createSampleData ()
