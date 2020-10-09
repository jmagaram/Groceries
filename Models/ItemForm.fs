module Models.ItemForm

open System

type ScheduleKind =
    | Once
    | Completed
    | Repeat

type CategoryMode =
    | ChooseExisting
    | CreateNew

type Form =
    { ItemId: StateTypes.ItemId option
      ItemNameProposed: string
      ItemName: string
      Etag: StateTypes.Etag option
      Quantity: string
      QuantityProposed: string
      Note: string
      NoteProposed: string
      ScheduleKind: ScheduleKind
      Frequency: StateTypes.Frequency
      Postpone: int<days> option
      CategoryMode: CategoryMode
      NewCategoryName: string
      NewCategoryNameProposed: string
      CategoryChoice: StateTypes.Category option
      CategoryChoiceList: StateTypes.Category list
      Stores: QueryTypes.ItemAvailability list }

type ItemFormResult =
    { Item: StateTypes.Item
      InsertCategory: StateTypes.Category option
      NotSold: StateTypes.StoreId list }

type ItemFormMessage =
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
    | StoresSetAvailability of store: StateTypes.StoreId * isSold: bool
    | Purchased
    | Transaction of ItemFormMessage seq

let itemNameValidation f = f.ItemNameProposed |> ItemName.tryParse
let itemNameChange s f = { f with ItemNameProposed = s }

let itemNameBlur f =
    { f with
          ItemName = f.ItemNameProposed |> ItemName.normalizer }

let quantityValidation f = f.QuantityProposed |> Quantity.tryParseOptional
let quantityChange s f = { f with QuantityProposed = s }

let quantityBlur f =
    { f with
          Quantity = f.QuantityProposed |> Quantity.normalizer }

let noteChange s f = { f with NoteProposed = s }
let noteValidation f = f.NoteProposed |> Note.tryParseOptional

let noteBlur f =
    { f with
          Note = f.NoteProposed |> Note.normalizer }

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

let categoryNameValidation f =
    f.NewCategoryNameProposed |> CategoryName.tryParse

let categoryNameChange s f = { f with NewCategoryNameProposed = s }

let categoryNameBlur (f: Form) =
    let normalized =
        f.NewCategoryNameProposed |> CategoryName.normalizer

    let exists =
        f.CategoryChoiceList
        |> Seq.tryFind (fun i ->
            String.Equals
                (i.CategoryName |> CategoryName.asText, normalized, StringComparison.InvariantCultureIgnoreCase))

    match exists with
    | None -> { f with NewCategoryName = normalized }
    | Some c ->
        { f with
              CategoryMode = ChooseExisting
              CategoryChoice = Some c
              NewCategoryName = "" }

let scheduleOnce f = { f with ScheduleKind = Once }
let scheduleCompleted f = { f with ScheduleKind = Completed }
let scheduleRepeat f = { f with ScheduleKind = Repeat }

let frequencyCoerceIntoBounds d =
    Frequency.rules
    |> RangeValidation.forceIntoBounds d

let frequencySet v f =
    { f with
          Frequency =
              v
              |> frequencyCoerceIntoBounds
              |> Frequency.create
              |> Result.okOrThrow }

let frequencyChoices (f: Form) =
    f.Frequency :: Frequency.common
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
let postponeDefault = None

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

let postponeChoices (f: Form) =
    f.Postpone
    :: (Repeat.commonPostponeDays |> List.map Some)
    |> Seq.choose id
    |> Seq.map frequencyCoerceIntoBounds
    |> Seq.distinct
    |> Seq.sort
    |> List.ofSeq

let storesSetAvailability id isSold f =
    { f with
          Stores =
              f.Stores
              |> List.map (fun a -> if a.Store.StoreId = id then { a with IsSold = isSold } else a) }

let canDelete (f: Form) = f.ItemId.IsSome

let createNewItem itemName stores cats =
    { ItemId = None
      ItemName = itemName
      ItemNameProposed = itemName
      Etag = None
      Quantity = ""
      QuantityProposed = ""
      Note = ""
      NoteProposed = ""
      ScheduleKind = ScheduleKind.Once
      Frequency = Frequency.goodDefault
      Postpone = postponeDefault
      CategoryMode = CategoryMode.ChooseExisting
      NewCategoryName = ""
      NewCategoryNameProposed = ""
      CategoryChoice = None
      CategoryChoiceList =
          cats
          |> Seq.sortBy (fun (i: StateTypes.Category) -> i.CategoryName)
          |> List.ofSeq
      Stores =
          stores
          |> Seq.map (fun i ->
              { QueryTypes.ItemAvailability.Store = i
                QueryTypes.ItemAvailability.IsSold = true })
          |> Seq.sortBy (fun i -> i.Store.StoreName)
          |> List.ofSeq }

let editItem (clock: Clock) cats (i: QueryTypes.ItemQry) =
    { ItemId = Some i.ItemId
      ItemName = i.ItemName |> ItemName.asText
      ItemNameProposed = i.ItemName |> ItemName.asText
      Etag = i.Etag
      Quantity =
          i.Quantity
          |> Option.map Quantity.asText
          |> Option.defaultValue ""
      QuantityProposed =
          i.Quantity
          |> Option.map Quantity.asText
          |> Option.defaultValue ""
      Note =
          i.Note
          |> Option.map Note.asText
          |> Option.defaultValue ""
      NoteProposed =
          i.Note
          |> Option.map Note.asText
          |> Option.defaultValue ""
      ScheduleKind =
          match i.Schedule with
          | StateTypes.Schedule.Completed -> Completed
          | StateTypes.Schedule.Once -> Once
          | StateTypes.Schedule.Repeat _ -> Repeat
      Frequency =
          match i.Schedule with
          | StateTypes.Schedule.Completed -> Frequency.goodDefault
          | StateTypes.Schedule.Once -> Frequency.goodDefault
          | StateTypes.Schedule.Repeat r -> r.Frequency
      Postpone =
          match i.Schedule with
          | StateTypes.Schedule.Completed -> postponeDefault
          | StateTypes.Schedule.Once -> postponeDefault
          | StateTypes.Schedule.Repeat r -> r |> Repeat.due (clock ())
      CategoryMode = CategoryMode.ChooseExisting
      NewCategoryName = ""
      NewCategoryNameProposed = ""
      CategoryChoice = i.Category
      CategoryChoiceList =
          cats
          |> Seq.sortBy (fun (i: StateTypes.Category) -> i.CategoryName)
          |> List.ofSeq
      Stores =
          i.Availability
          |> Seq.sortBy (fun i -> i.Store.StoreName)
          |> List.ofSeq }

let editItemFromGuid (itemId: Guid) (clock: Clock) (s: StateTypes.State) =
    let itemQry = s |> Query.itemQryFromGuid itemId
    editItem clock (s |> State.categories) itemQry

let hasErrors f =
    (f |> itemNameValidation |> Result.isError)
    || (f |> quantityValidation |> Result.isError)
    || (f |> noteValidation |> Result.isError)
    || ((f |> categoryModeIsCreateNew)
        && (f |> categoryNameValidation |> Result.isError))

let rec handleMessage msg (f: Form) =
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
    | Transaction msgs -> msgs |> Seq.fold (fun f m -> handleMessage m f) f

let asItemFormResult (now: DateTimeOffset) (f: Form) =
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
        { StateTypes.Item.ItemId =
              f.ItemId
              |> Option.defaultWith (fun () -> ItemId.create ())
          StateTypes.Item.ItemName = f |> itemNameValidation |> Result.okOrThrow
          StateTypes.Item.Etag = f.Etag
          StateTypes.Item.CategoryId =
              match f.CategoryMode with
              | ChooseExisting -> f.CategoryChoice
              | CreateNew -> insertCategory
              |> Option.map (fun i -> i.CategoryId)
          StateTypes.Item.Quantity = f |> quantityValidation |> Result.okOrThrow
          StateTypes.Item.Note = f |> noteValidation |> Result.okOrThrow
          StateTypes.Item.Schedule =
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

type Form with
    member me.ItemNameValidation = me |> itemNameValidation
    member me.NoteValidation = me |> noteValidation
    member me.QuantityValidation = me |> quantityValidation
    member me.FrequencyChoices = me |> frequencyChoices
    member me.PostponeChoices = me |> postponeChoices
    member me.CategoryNameValidation = me |> categoryNameValidation
    member me.HasErrors = me |> hasErrors
    member me.ItemFormResult(now) = me |> asItemFormResult now
    member me.CanDelete = me |> canDelete

open StateTypes

let itemFormResultAsTransaction (r: ItemFormResult) (s: State) =
    seq {
        yield!
            r.InsertCategory
            |> Option.map (InsertCategory >> CategoryMessage)
            |> Option.toList

        yield r.Item |> (UpsertItem >> ItemMessage)

        let nsExpected = r.NotSold

        let nsCurrent =
            s
            |> State.notSoldItems
            |> Seq.choose (fun i -> if i.ItemId = r.Item.ItemId then Some i.StoreId else None)

        let nsToRemove = nsCurrent |> Seq.except nsExpected
        let nsToAdd = nsExpected |> Seq.except nsCurrent

        yield!
            nsToAdd
            |> Seq.map (fun s ->
                { StateTypes.NotSoldItem.StoreId = s
                  StateTypes.NotSoldItem.ItemId = r.Item.ItemId }
                |> (InsertNotSoldItem >> NotSoldItemMessage))

        yield!
            nsToRemove
            |> Seq.map (fun s ->
                { StateTypes.NotSoldItem.StoreId = s
                  StateTypes.NotSoldItem.ItemId = r.Item.ItemId }
                |> (DeleteNotSoldItem >> NotSoldItemMessage))
    }
    |> StateMessage.Transaction
