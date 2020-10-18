[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemForm

open System
open System.Runtime.CompilerServices
open StateTypes

// Make sure all these are pure messages; include current time and any necessary
// GUID values; don't create these on-the-fly inside various methods Maybe there
// should be a standard "ItemFormHost" message that includes delete, create,
// cancel, submit and then use this wherever the form is hosted rather than
// hardwiring it into the ItemPageMessage and ItemPopUpDilaogEditorMessage etc.
type Message =
    | ItemNameSet of string
    | ItemNameBlur
    | QuantitySet of string
    | QuantityBlur
    | NoteSet of string
    | NoteBlur
    | ScheduleOnce
    | ScheduleCompleted
    | ScheduleRepeat
    | FrequencySet of int<days>
    | PostponeSet of int<days>
    | PostponeClear
    | CategoryModeChooseExisting
    | CategoryModeCreateNew
    | ChooseCategoryUncategorized
    | ChooseCategory of Guid
    | NewCategoryNameSet of string
    | NewCategoryNameBlur
    | StoresSetAvailability of store: StoreId * isSold: bool
    | Purchased
    | Transaction of Message seq

let itemNameValidation (f:ItemForm) = f.ItemName.ValueTyping |> ItemName.tryParse
let itemNameChange s (f:ItemForm)  = { f with ItemName = f.ItemName |> TextBox.typeText s }

let itemNameBlur f =
    { f with
          ItemForm.ItemName = f.ItemName |> TextBox.loseFocus ItemName.normalizer }

let quantityValidation (f:ItemForm) =
    f.Quantity.ValueTyping |> String.tryParseOptional Quantity.tryParse

let quantityChange s (f:ItemForm) = { f with Quantity = f.Quantity |> TextBox.typeText s }

let quantityBlur f =
    { f with
          ItemForm.Quantity = f.Quantity |> TextBox.loseFocus Quantity.normalizer }

let noteChange s (f:ItemForm) = { f with Note = f.Note |> TextBox.typeText s }
let noteValidation (f:ItemForm) = f.Note.ValueTyping |> String.tryParseOptional Note.tryParse

let noteBlur f =
    { f with
          ItemForm.Note = f.Note |> TextBox.loseFocus Note.normalizer }

let categoryModeChooseExisting f = { f with CategoryMode = ChooseExisting }
let categoryModeCreateNew f = { f with CategoryMode = CreateNew }
let chooseCategoryUncategorized f = { f with CategoryChoice = None }

let categoryModeIsCreateNew f =
    match f.CategoryMode with
    | CreateNew -> true
    | _ -> false

let chooseCategory i f =
    { f with
          CategoryChoice =
              f.CategoryChoiceList
              |> List.find (fun j -> j.CategoryId = StateTypes.CategoryId i)
              |> Some }

let categoryNameValidation (f:ItemForm) =
    f.NewCategoryName.ValueTyping |>  CategoryName.tryParse

let categoryNameChange s (f:ItemForm) = { f with NewCategoryName = f.NewCategoryName |> TextBox.typeText s }

let categoryNameBlur (f: ItemForm) =
    let normalized = f.NewCategoryName |> TextBox.loseFocus CategoryName.normalizer

    let exists =
        f.CategoryChoiceList
        |> Seq.tryFind (fun i ->
            String.Equals
                (i.CategoryName |> CategoryName.asText, normalized.ValueCommitted, StringComparison.InvariantCultureIgnoreCase))

    match exists with
    | None -> { f with NewCategoryName = normalized }
    | Some c ->
        { f with
              CategoryMode = ChooseExisting
              CategoryChoice = Some c
              NewCategoryName = TextBox.create "" }

let scheduleOnce f = { f with ScheduleKind = Once }
let scheduleCompleted f = { f with ScheduleKind = Completed }
let scheduleRepeat f = { f with ScheduleKind = Repeat }

let frequencyCoerceIntoBounds d =
    Frequency.rules
    |> RangeValidation.forceIntoBounds d

let frequencySet v f =
    { f with
          ItemForm.Frequency =
              v
              |> frequencyCoerceIntoBounds
              |> Frequency.create
              |> Result.okOrThrow }

let frequencyChoices (f: ItemForm) =
    f.Frequency :: Frequency.commonFrequencyChoices
    |> Seq.distinct
    |> Seq.sort
    |> List.ofSeq

let frequencyAsText (d: StateTypes.Frequency) =
    let d = d |> Frequency.days |> int

    let monthsExactly =
        d
        |> divRem 30
        |> Option.filter (fun i -> i.Quotient >= 1 && i.Remainder = 0)
        |> Option.map (fun i -> if i.Quotient = 1 then "Monthly" else sprintf "Every %i months" i.Quotient)

    let weeksExactly =
        d
        |> divRem 7
        |> Option.filter (fun i -> i.Quotient >= 1 && i.Remainder = 0)
        |> Option.map (fun i -> if i.Quotient = 1 then "Weekly" else sprintf "Every %i weeks" i.Quotient)

    monthsExactly
    |> Option.orElse weeksExactly
    |> Option.defaultWith (fun () -> if d = 1 then "Daily" else sprintf "Every %i days" d)

let postponeSet v f = { f with Postpone = Some v }

let postponeUntilFrequency f =
    { f with
          Postpone = f.Frequency |> Frequency.days |> Some }

let postponeClear f = { f with Postpone = None }

let purchased f =
    match f.ScheduleKind with
    | Once -> f |> scheduleCompleted
    | Repeat -> f |> postponeUntilFrequency
    | Completed -> f

let postponeDurationAsText (d: int<days>) =
    let d = d |> int

    let monthsExactly =
        if d / 30 > 0 && d % 30 = 0 then Some(d / 30) else None

    let weeksExactly =
        if d / 7 > 0 && d % 7 = 0 then Some(d / 7) else None

    match monthsExactly with
    | Some m -> if m = 1 then "1 month" else sprintf "%i months" m
    | None ->
        match weeksExactly with
        | Some w -> if w = 1 then "1 week" else sprintf "%i weeks" w
        | None -> if d = 1 then "1 day" else sprintf "%i days" d

let postponeChoices (f: ItemForm) =
    f.Postpone
    :: (Schedule.commonPostponeChoices |> List.map Some)
    |> Seq.choose id
    |> Seq.map frequencyCoerceIntoBounds
    |> Seq.distinct
    |> Seq.sort
    |> List.ofSeq

let storesSetAvailability id isSold f =
    { f with
          ItemForm.Stores =
              f.Stores
              |> List.map (fun a -> if a.Store.StoreId = id then { a with IsSold = isSold } else a) }

let canDelete (f: ItemForm) = f.ItemId.IsSome

let createNewItem itemName stores cats =
    { ItemId = None
      ItemName = TextBox.create itemName
      Etag = None
      Quantity = TextBox.create ""
      Note = TextBox.create ""
      ScheduleKind = ScheduleKind.Once
      Frequency = Frequency.frequencyDefault
      Postpone = None
      CategoryMode = CategoryMode.ChooseExisting
      NewCategoryName = TextBox.create ""
      CategoryChoice = None
      CategoryChoiceList =
          cats
          |> Seq.sortBy (fun (i: StateTypes.Category) -> i.CategoryName)
          |> List.ofSeq
      Stores =
          stores
          |> Seq.map (fun i ->
              { ItemAvailability.Store = i
                ItemAvailability.IsSold = true })
          |> Seq.sortBy (fun i -> i.Store.StoreName)
          |> List.ofSeq }

let editItem (clock: Now) cats (i: QueryTypes.ItemQry) =
    { ItemId = Some i.ItemId
      ItemName = i.ItemName |> ItemName.asText |> TextBox.create
      Etag = i.Etag
      Quantity =
          i.Quantity
          |> Option.map Quantity.asText
          |> Option.defaultValue ""
          |> TextBox.create
      Note =
          i.Note
          |> Option.map Note.asText
          |> Option.defaultValue ""
          |> TextBox.create
      ScheduleKind =
          match i.Schedule with
          | StateTypes.Schedule.Completed -> Completed
          | StateTypes.Schedule.Once -> Once
          | StateTypes.Schedule.Repeat _ -> Repeat
      Frequency =
          match i.Schedule with
          | StateTypes.Schedule.Completed -> Frequency.frequencyDefault
          | StateTypes.Schedule.Once -> Frequency.frequencyDefault
          | StateTypes.Schedule.Repeat r -> r.Frequency
      Postpone = i.Schedule |> Schedule.postponedUntilDays (clock())
      CategoryMode = CategoryMode.ChooseExisting
      NewCategoryName = "" |> TextBox.create
      CategoryChoice = i.Category
      CategoryChoiceList =
          cats
          |> Seq.sortBy (fun (i: StateTypes.Category) -> i.CategoryName)
          |> List.ofSeq
      Stores =
          i.Availability
          |> Seq.sortBy (fun i -> i.Store.StoreName)
          |> List.ofSeq }

let editItemFromGuid (itemId: Guid) (clock: Now) (s: StateTypes.State) =
    let itemQry = s |> StateQuery.itemQryFromGuid itemId
    editItem clock (s |> StateQuery.categories) itemQry

let editItemFromSerializedId (itemId: string) (clock: Now) (s: StateTypes.State) =
    let itemId = ItemId.deserialize itemId |> Option.get

    let item =
        s
        |> StateQuery.itemsTable
        |> DataTable.findCurrent itemId

    let itemQry = StateQuery.itemQry item s
    let categories = s |> StateQuery.categories
    editItem clock categories itemQry

let hasErrors f =
    (f |> itemNameValidation |> Result.isError)
    || (f |> quantityValidation |> Result.isError)
    || (f |> noteValidation |> Result.isError)
    || ((f |> categoryModeIsCreateNew)
        && (f |> categoryNameValidation |> Result.isError))

let rec handleMessage msg (f: ItemForm) =
    match msg with
    | ItemNameSet s -> f |> itemNameChange s
    | ItemNameBlur -> f |> itemNameBlur
    | QuantitySet s -> f |> quantityChange s
    | QuantityBlur -> f |> quantityBlur
    | NoteSet s -> f |> noteChange s
    | NoteBlur -> f |> noteBlur
    | ScheduleOnce -> f |> scheduleOnce
    | ScheduleCompleted -> f |> scheduleCompleted
    | ScheduleRepeat -> f |> scheduleRepeat
    | FrequencySet v -> f |> frequencySet v
    | PostponeSet d -> f |> postponeSet d
    | PostponeClear -> f |> postponeClear
    | CategoryModeChooseExisting -> f |> categoryModeChooseExisting
    | CategoryModeCreateNew -> f |> categoryModeCreateNew
    | ChooseCategoryUncategorized -> f |> chooseCategoryUncategorized
    | ChooseCategory g -> f |> chooseCategory g
    | NewCategoryNameSet s -> f |> categoryNameChange s
    | NewCategoryNameBlur -> f |> categoryNameBlur
    | StoresSetAvailability (id: StateTypes.StoreId, isSold: bool) -> f |> storesSetAvailability id isSold
    | Purchased -> f |> purchased
    | Message.Transaction msgs -> msgs |> Seq.fold (fun f m -> handleMessage m f) f

type ItemFormResult =
    { Item: Item
      InsertCategory: Category option
      NotSold: StoreId list }

let asItemFormResult (now: DateTimeOffset) (f: ItemForm) =
    let insertCategory =
        match f.CategoryMode with
        | ChooseExisting -> None
        | CreateNew ->
            f
            |> categoryNameValidation
            |> Result.okOrThrow
            |> fun c ->
                Some
                    { StateTypes.Category.CategoryName = c
                      StateTypes.Category.CategoryId = CategoryId.create ()
                      StateTypes.Category.Etag = None }

    let item =
        { Item.ItemId =
              f.ItemId
              |> Option.defaultWith (fun () -> ItemId.create ())
          Item.ItemName = f |> itemNameValidation |> Result.okOrThrow
          Item.Etag = f.Etag
          Item.CategoryId =
              match f.CategoryMode with
              | ChooseExisting -> f.CategoryChoice
              | CreateNew -> insertCategory
              |> Option.map (fun i -> i.CategoryId)
          Item.Quantity = f |> quantityValidation |> Result.okOrThrow
          Item.Note = f |> noteValidation |> Result.okOrThrow
          Item.Schedule =
              match f.ScheduleKind with
              | Completed -> StateTypes.Schedule.Completed
              | Once -> StateTypes.Schedule.Once
              | Repeat ->
                  { StateTypes.Repeat.Frequency = f.Frequency
                    StateTypes.Repeat.PostponedUntil =
                        f.Postpone
                        |> Option.map (fun d -> now.AddDays(d |> float)) }
                  |> StateTypes.Repeat }

    let notSold =
        f.Stores
        |> Seq.choose (fun i -> if i.IsSold = false then Some i.Store.StoreId else None)
        |> List.ofSeq

    { Item = item
      InsertCategory = insertCategory
      NotSold = notSold }

// Needs a GUID for the category; don't bake it in
let processResult (r: ItemFormResult) (s: State) =
    let s =
        match r.InsertCategory with
        | None -> s
        | Some c -> s |> StateUpdateCore.insertCategory c

    let s = s |> StateUpdateCore.upsertItem r.Item

    let nsExpected = r.NotSold

    let nsCurrent =
        s
        |> StateQuery.notSoldItems
        |> Seq.choose (fun i -> if i.ItemId = r.Item.ItemId then Some i.StoreId else None)

    let nsToRemove = nsCurrent |> Seq.except nsExpected
    let nsToAdd = nsExpected |> Seq.except nsCurrent

    let s =
        nsToRemove
        |> Seq.map (fun s -> { StoreId = s; ItemId = r.Item.ItemId })
        |> Seq.fold (fun s i -> s |> StateUpdateCore.deleteNotSoldItem i) s

    let s =
        nsToAdd
        |> Seq.map (fun s -> { StoreId = s; ItemId = r.Item.ItemId })
        |> Seq.fold (fun s i -> s |> StateUpdateCore.insertNotSoldItem i) s

    s

[<Extension>]
type ItemFormExtensions =
    [<Extension>]
    static member ItemNameValidation(me: ItemForm) = me |> itemNameValidation

    [<Extension>]
    static member NoteValidation(me: ItemForm) = me |> noteValidation

    [<Extension>]
    static member QuantityValidation(me: ItemForm) = me |> quantityValidation

    [<Extension>]
    static member FrequencyChoices(me: ItemForm) = me |> frequencyChoices

    [<Extension>]
    static member PostponeChoices(me: ItemForm) = me |> postponeChoices

    [<Extension>]
    static member CategoryNameValidation(me: ItemForm) = me |> categoryNameValidation

    [<Extension>]
    static member HasErrors(me: ItemForm) = me |> hasErrors

    [<Extension>]
    static member CanDelete(me: ItemForm) = me |> canDelete

