[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.State

open System
open ChangeTrackerTypes
open CoreTypes
open StateTypes

// Basic Queries

let categoriesTable (s: State) = s.Categories
let storesTable (s: State) = s.Stores
let itemsTable (s: State) = s.Items
let notSoldTable (s: State) = s.NotSoldItems
let userSettingsTable (s: State) = s.UserSettings
let purchasesTable (s: State) = s.Purchases

let userSettingsForLoggedInUser (s: State) =
    s
    |> userSettingsTable
    |> DataTable.tryFindCurrent (s.LoggedInUser)
    |> Option.defaultValue (UserSettings.create (s.LoggedInUser))

let categories = categoriesTable >> DataTable.current
let stores = storesTable >> DataTable.current
let items = itemsTable >> DataTable.current
let notSold = notSoldTable >> DataTable.current
let userSettings = userSettingsTable >> DataTable.current
let purchases = purchasesTable >> DataTable.current

let tryFindItem id s =
    s |> itemsTable |> DataTable.tryFindCurrent id

let tryFindStore id s =
    s |> storesTable |> DataTable.tryFindCurrent id

let tryFindCategory id s =
    s
    |> categoriesTable
    |> DataTable.tryFindCurrent id

let purchasesOf itemId s =
    s
    |> purchases
    |> Seq.filter (fun p -> p.ItemId = itemId)
    |> Seq.map (fun p -> p.PurchasedOn)

let tryFindMostRecentPurchase itemId s =
    s
    |> purchasesOf itemId
    |> Seq.sortByDescending id
    |> Seq.tryHead

let itemHasInvalidCategory (i: Item) s =
    i.CategoryId
    |> Option.map (fun c -> s |> tryFindCategory c |> Option.isNone)
    |> Option.defaultValue false

let notSoldHasInvalidItem (ns: NotSoldItem) s =
    s
    |> itemsTable
    |> DataTable.tryFindCurrent ns.ItemId
    |> Option.isNone

let notSoldHasInvalidStore (ns: NotSoldItem) s =
    s
    |> storesTable
    |> DataTable.tryFindCurrent ns.StoreId
    |> Option.isNone

let storeSellsItemById itemId storeId state =
    state
    |> notSoldTable
    |> DataTable.tryFindCurrent { StoreId = storeId; ItemId = itemId }
    |> Option.isNone

let storeSellsItem (i: Item) (s: Store) state =
    state |> storeSellsItemById i.ItemId s.StoreId

let hasChanges s =
    (s |> itemsTable |> DataTable.hasChanges)
    || (s |> categoriesTable |> DataTable.hasChanges)
    || (s |> storesTable |> DataTable.hasChanges)
    || (s |> notSoldTable |> DataTable.hasChanges)

// Update

let mapCategories f s = { s with Categories = f s.Categories }
let mapStores f s = { s with State.Stores = f s.Stores }
let mapItems f s = { s with Items = f s.Items }

let mapUserSettings f s =
    { s with
          UserSettings = f s.UserSettings }

let mapNotSoldItems f s =
    { s with
          NotSoldItems = f s.NotSoldItems }

let mapPurchases f s = { s with Purchases = f s.Purchases }

let fixItemForeignKeys s =
    s
    |> items
    |> Seq.filter (fun i -> s |> itemHasInvalidCategory i)
    |> Seq.map (fun i -> { i with CategoryId = None })
    |> Seq.fold (fun s i -> s |> mapItems (DataTable.update i)) s

let fixNotSoldForeignKeys s =
    s
    |> notSold
    |> Seq.filter
        (fun n ->
            let isItemInvalid = s |> notSoldHasInvalidItem n
            let isStoreInvalid = s |> notSoldHasInvalidStore n
            isItemInvalid || isStoreInvalid)
    |> Seq.fold (fun s i -> s |> mapNotSoldItems (DataTable.delete i)) s

let fixPurchaseForeignKeys s =
    s
    |> purchases
    |> Seq.filter (fun p -> s |> tryFindItem p.ItemId |> Option.isNone)
    |> Seq.fold (fun s i -> s |> mapPurchases (DataTable.delete i)) s

let fixUserSettingsForeignKeys s =
    let stores =
        s |> storesTable |> DataTable.keys |> Set.ofSeq

    s
    |> userSettings
    |> Seq.choose
        (fun u ->
            match u.ShoppingListSettings
                  |> ShoppingListSettings.storeFilterIsValid stores with
            | true -> None
            | false ->
                u.ShoppingListSettings
                |> ShoppingListSettings.clearStoreFilter
                |> fun i -> { u with ShoppingListSettings = i } |> Some)
    |> Seq.fold (fun state i -> state |> mapUserSettings (DataTable.upsert i)) s

let fixForeignKeys s =
    s
    |> fixItemForeignKeys
    |> fixNotSoldForeignKeys
    |> fixUserSettingsForeignKeys
    |> fixPurchaseForeignKeys

let (insertCategory, updateCategory, upsertCategory) =
    let go f (c: Category) s = s |> mapCategories (f c)
    (go DataTable.insert, go DataTable.update, go DataTable.upsert)

let (insertStore, updateStore, upsertStore) =
    let go f (s: Store) = mapStores (f s)
    (go DataTable.insert, go DataTable.update, go DataTable.upsert)

let (insertItem, updateItem, upsertItem) =
    let go f (i: Item) s =
        match s |> itemHasInvalidCategory i with
        | true -> failwith "The item has an invalid category foreign key."
        | false -> s |> mapItems (f i)

    (go DataTable.insert, go DataTable.update, go DataTable.upsert)

let insertNotSoldItem (n: NotSoldItem) s =
    if s |> notSoldHasInvalidItem n
    then failwith "The notSoldItem has an invalid item foreign key."
    elif s |> notSoldHasInvalidStore n
    then failwith "The notSoldItem has an invalid store foreign key."
    else s |> mapNotSoldItems (DataTable.insert n)

let insertPurchase (p: Purchase) s =
    match s |> tryFindItem p.ItemId with
    | None -> failwith "The purchase has an invalid item foreign key."
    | Some _ ->
        let areDatesClose (x: DateTimeOffset) (y: DateTimeOffset) =
            (x - y)
            |> TimeSpan.abs
            |> fun t -> t < TimeSpan.FromDays(1.0)

        match s
              |> purchases
              |> Seq.exists
                  (fun i ->
                      i.ItemId = p.ItemId
                      && areDatesClose i.PurchasedOn p.PurchasedOn) with
        | true -> s
        | false -> s |> mapPurchases (DataTable.insert p)

let deleteStore k s =
    s
    |> mapStores (DataTable.delete k)
    |> fixForeignKeys

let deleteItem k s =
    s
    |> mapItems (DataTable.delete k)
    |> fixForeignKeys

let deleteNotSoldItem k s =
    s |> mapNotSoldItems (DataTable.delete k)

let deleteCategory k s =
    s
    |> mapCategories (DataTable.delete k)
    |> fixForeignKeys

let importChanges (i: ImportChanges) (s: StateTypes.State) =
    { s with
          Items =
              i.ItemChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Items
          Categories =
              i.CategoryChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Categories
          Stores =
              i.StoreChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Stores
          NotSoldItems =
              i.NotSoldItemChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.NotSoldItems
          Purchases =
              i.PurchaseChanges
              |> Seq.fold (fun dt i -> dt |> DataTable.acceptChange i) s.Purchases
          LastCosmosTimestamp = i.LatestTimestamp }
    |> fixForeignKeys

let acceptAllChanges s =
    s
    |> mapItems DataTable.acceptChanges
    |> mapCategories DataTable.acceptChanges
    |> mapStores DataTable.acceptChanges
    |> mapNotSoldItems DataTable.acceptChanges
    |> mapPurchases DataTable.acceptChanges
    |> fixForeignKeys

let createDefault loggedInUser =
    { Categories = DataTable.empty
      Items = DataTable.empty
      Stores = DataTable.empty
      NotSoldItems = DataTable.empty
      Purchases = DataTable.empty
      UserSettings = DataTable.empty
      LoggedInUser = loggedInUser
      LastCosmosTimestamp = None
      ItemEditPage = None }

let createSampleData loggedInUser =

    let newCategory n =
        insertCategory
            { Category.CategoryId = CategoryId.create ()
              CategoryName = n |> CategoryName.tryParse |> Result.okOrThrow
              Etag = None }

    let newStore n =
        insertStore
            { Store.StoreId = StoreId.create ()
              StoreName = n |> StoreName.tryParse |> Result.okOrThrow
              Etag = None }

    let findCategory n (s: State) =
        s.Categories
        |> DataTable.current
        |> Seq.find (fun i -> i.CategoryName = (CategoryName.tryParse n |> Result.okOrThrow))

    let findItem n (s: State) =
        s.Items
        |> DataTable.current
        |> Seq.find (fun i -> i.ItemName = (ItemName.tryParse n |> Result.okOrThrow))

    let findStore n (s: State) =
        s.Stores
        |> DataTable.current
        |> Seq.find (fun i -> i.StoreName = (StoreName.tryParse n |> Result.okOrThrow))

    let newItem name cat qty note s =
        s
        |> insertItem
            { Item.ItemId = ItemId.create ()
              ItemName = name |> ItemName.tryParse |> Result.okOrThrow
              Etag = None
              Quantity =
                  qty
                  |> tryParseOptional Quantity.tryParse
                  |> Result.okOrThrow
              Note =
                  note
                  |> tryParseOptional Note.tryParse
                  |> Result.okOrThrow
              Item.PostponeUntil = None
              Item.CategoryId = if cat = "" then None else Some (findCategory cat s).CategoryId }

    let now = System.DateTimeOffset.Now

    let markComplete n (now: DateTimeOffset) s =
        let item =
            s
            |> findItem n
            |> fun i -> i |> Item.postpone now 7<days>

        s |> mapItems (DataTable.update item)

    let doesNotSellItem store item (s: State) =
        let ns =
            { NotSoldItem.ItemId = (findItem item s).ItemId
              StoreId = (findStore store s).StoreId }

        s |> mapNotSoldItems (DataTable.insert ns)

    let purchaseHistory item days (s: State) =
        let item = s |> findItem item

        days
        |> Seq.fold
            (fun total d ->
                let purchase =
                    { Purchase.ItemId = item.ItemId
                      PurchasedOn = now.AddDays(-d) }

                total |> insertPurchase purchase)
            s

    createDefault loggedInUser
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
    |> markComplete "Ice cream" now
    |> newStore "QFC"
    |> newStore "Whole Foods"
    |> newStore "Trader Joe's"
    |> newStore "Costco"
    |> newStore "Walgreens"
    |> doesNotSellItem "QFC" "Dried flax seeds"
    |> doesNotSellItem "Costco" "Chocolate bars"
    |> purchaseHistory "Ice cream" [ 7.0; 14.0 ]
    |> purchaseHistory "Bananas" [ 3.0; 17.0; 21.0 ]

let handleItemMessage now msg (s: State) =
    match msg with
    | ModifyItem (k, msg) ->
        let item =
            s.Items
            |> DataTable.findCurrent k
            |> Item.update now msg

        s |> updateItem item
    | DeleteItem k -> s |> deleteItem k

let handleItemEditPageMessage (now: DateTimeOffset) (msg: ItemEditPageMessage) (s: State) =
    let form state =
        state.ItemEditPage
        |> Option.asResult "No form is being edited."

    let cancel state = { state with ItemEditPage = None }

    let beginCreateItem name state =
        let categories = state |> categories
        let stores = state |> stores
        let name = name |> Option.defaultValue ""

        let form =
            ItemForm.createNewItem name stores categories

        { state with ItemEditPage = Some form }

    let itemDenormalized (i: Item) state =
        { ItemDenormalized.ItemId = i.ItemId
          ItemName = i.ItemName
          Etag = i.Etag
          Note = i.Note
          Quantity = i.Quantity
          PostponeUntil = i.PostponeUntil
          Category =
              i.CategoryId
              |> Option.bind (fun id -> state |> tryFindCategory id)
          Availability =
              state
              |> stores
              |> Seq.map
                  (fun s ->
                      { ItemAvailability.Store = s
                        IsSold = state |> storeSellsItem i s }) }

    let processResult (now: DateTimeOffset) (r: ItemForm.ItemFormResult) (s: State) =
        let s =
            match r.InsertCategory with
            | None -> s
            | Some c -> s |> insertCategory c

        let s = s |> upsertItem r.Item

        let nsExpected = r.NotSold

        let nsCurrent =
            s
            |> notSold
            |> Seq.choose (fun i -> if i.ItemId = r.Item.ItemId then Some i.StoreId else None)

        let nsToRemove = nsCurrent |> Seq.except nsExpected
        let nsToAdd = nsExpected |> Seq.except nsCurrent

        let s =
            nsToRemove
            |> Seq.map (fun s -> { StoreId = s; ItemId = r.Item.ItemId })
            |> Seq.fold (fun s i -> s |> deleteNotSoldItem i) s

        let s =
            nsToAdd
            |> Seq.map (fun s -> { StoreId = s; ItemId = r.Item.ItemId })
            |> Seq.fold (fun s i -> s |> insertNotSoldItem i) s

        let s =
            match r.RecordPurchase with
            | false -> s
            | true ->
                s
                |> insertPurchase
                    { Purchase.ItemId = r.Item.ItemId
                      PurchasedOn = now }

        s

    match msg with
    | BeginEditItem id ->
        let k = ItemId.deserialize id |> Option.get

        let item =
            s |> itemsTable |> DataTable.findCurrent k

        let item = s |> itemDenormalized item
        let cats = s |> categories
        let form = ItemForm.editItem now cats item
        { s with ItemEditPage = Some form }

    | BeginCreateNewItem -> s |> beginCreateItem None
    | BeginCreateNewItemWithName txt -> s |> beginCreateItem (Some txt)
    | ItemEditFormMessage msg ->
        { s with
              ItemEditPage = s.ItemEditPage |> Option.map (ItemForm.update msg) }
    | SubmitItemEditForm ->

        result {
            let! form = s |> form
            let itemFormResult = form |> ItemForm.asItemFormResult now
            let s = s |> processResult now itemFormResult
            let s = s |> cancel
            return s
        }
        |> Result.okOrThrow

    | CancelItemEditForm -> s |> cancel
    | ItemEditPageMessage.DeleteItem ->
        result {
            let! form = s |> form

            let! id =
                form.ItemId
                |> Option.asResult "Can not delete an item without an ID."

            let state = s |> deleteItem id |> cancel

            return state
        }
        |> Result.okOrThrow

let reorganizeCategories (msg: ReorganizeCategoriesMessage) (s: State) =
    let categoryName n =
        CategoryName.tryParse n |> Result.okOrThrow

    let findCat n s =
        let n = n |> CategoryName

        s
        |> categories
        |> Seq.find (fun i -> i.CategoryName = n)

    let createCats ns s =
        ns
        |> Seq.fold
            (fun total n ->
                let cat =
                    { Category.CategoryName = n |> categoryName
                      CategoryId = CategoryId.create ()
                      Etag = None }

                total |> insertCategory cat)
            s

    let deleteCats ns s =
        ns
        |> Seq.fold
            (fun total n ->
                let cat = total |> findCat n
                total |> deleteCategory cat.CategoryId)
            s

    let setItemCat x y s =
        let xCat = s |> findCat x
        let yCat = s |> findCat y

        s
        |> items
        |> Seq.filter (fun i -> i.CategoryId = Some xCat.CategoryId)
        |> Seq.fold
            (fun total i ->
                let i =
                    { i with
                          CategoryId = Some yCat.CategoryId }

                total |> updateItem i)
            s

    let setItemCats xys s =
        xys
        |> Seq.fold (fun total (x, y) -> setItemCat x y total) s

    s
    |> createCats msg.Create
    |> setItemCats msg.Move
    |> deleteCats msg.Delete

let reorganizeStores (msg: ReorganizeStoresMessage) (s: State) =
    let findStore n s =
        let n = n |> StoreName
        s |> stores |> Seq.find (fun i -> i.StoreName = n)

    let createStores ns s =
        ns
        |> Seq.fold
            (fun total n ->
                let cat =
                    { Store.StoreName = n |> StoreName
                      StoreId = StoreId.create ()
                      Etag = None }

                total |> insertStore cat)
            s

    let deleteStores ns s =
        ns
        |> Seq.fold
            (fun total n ->
                let cat = total |> findStore n
                total |> deleteStore cat.StoreId)
            s

    let setInventory x y s =
        let x = s |> findStore x
        let y = s |> findStore y

        s
        |> notSold
        |> Seq.filter (fun i -> i.StoreId = x.StoreId)
        |> Seq.fold
            (fun total i ->
                total
                |> deleteNotSoldItem i
                |> insertNotSoldItem { i with StoreId = y.StoreId })
            s

    let setInventories xys s =
        xys
        |> Seq.fold (fun total (x, y) -> setInventory x y total) s

    s
    |> createStores msg.Create
    |> setInventories msg.Move
    |> deleteStores msg.Delete
    |> fixForeignKeys

let updateItemAvailability itemId (ia: ItemAvailability) s =
    let ns =
        { NotSoldItem.ItemId = itemId
          NotSoldItem.StoreId = ia.Store.StoreId }

    match ia.IsSold, s |> notSoldTable |> DataTable.tryFindCurrent ns with
    | true, Some _ -> s |> deleteNotSoldItem ns
    | false, None -> s |> insertNotSoldItem ns
    | _, _ -> s

let handleItemAvailabilityMessage itemId (msg: ItemAvailability seq) s =
    msg
    |> Seq.fold (fun s i -> s |> updateItemAvailability itemId i) s

let handleItemOnlySoldAt itemId onlySoldAt s =
    let onlySoldAt = onlySoldAt |> Set.ofSeq

    s
    |> stores
    |> Seq.map
        (fun s ->
            { ItemAvailability.Store = s
              IsSold = onlySoldAt |> Set.contains (s.StoreId) })
    |> Seq.fold (fun state ia -> state |> updateItemAvailability itemId ia) s

let handleItemNotSoldAt itemId storeId s =
    s
    |> mapNotSoldItems (DataTable.upsert { ItemId = itemId; StoreId = storeId })

let handleUserSettingsMessage (msg: UserSettings.Message) (s: State) =
    let settings =
        s
        |> userSettingsForLoggedInUser
        |> UserSettings.update msg

    s |> mapUserSettings (DataTable.upsert settings)

let handleRecordPurchase (now: DateTimeOffset) (id: ItemId) (s: State) =
    let purchases =
        s
        |> purchases
        |> Seq.choose (fun p -> if p.ItemId = id then Some p.PurchasedOn else None)

    s
    |> insertPurchase { ItemId = id; PurchasedOn = now }
    |> handleItemMessage now (ModifyItem(id, Item.PostponeUsingPurchaseHistory(now, purchases)))

let update: Update =
    fun clock msg s ->
        let now = clock ()

        let rec go msg s =
            //(sprintf "Update: %A" msg)
            //|> String.ellipsize 100
            //|> dprintln

            match msg with
            | RecordPurchase msg -> s |> handleRecordPurchase now msg
            | ItemMessage msg -> s |> handleItemMessage now msg
            | UserSettingsMessage msg -> s |> handleUserSettingsMessage msg
            | ReorganizeCategoriesMessage msg -> s |> reorganizeCategories msg
            | ReorganizeStoresMessage msg -> s |> reorganizeStores msg
            | AcceptAllChanges -> s |> acceptAllChanges
            | Import c -> s |> importChanges c
            | ResetToSampleData -> createSampleData s.LoggedInUser
            | ItemEditPageMessage msg -> s |> handleItemEditPageMessage now msg
            | ItemNotSoldAt (itemId, storeId) -> s |> handleItemNotSoldAt itemId storeId
            | ItemOnlySoldAt (itemId, stores) -> s |> handleItemOnlySoldAt itemId stores
            | ItemAvailabilityMessage (itemId, availability) ->
                s
                |> handleItemAvailabilityMessage itemId availability
            | Transaction msg -> msg |> Seq.fold (fun res i -> go i res) s

        go msg s

let updateUsingStandardClock msg s = s |> update Miscellaneous.clock msg