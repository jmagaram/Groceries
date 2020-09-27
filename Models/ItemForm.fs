namespace Models

open System

module ItemFormTypes =
    open StateTypes

    type ScheduleKind =
        | Once
        | Completed
        | Repeat

    type CategoryMode =
        | ChooseExisting
        | CreateNew

    type Form =
        { ItemId: ItemId option
          ItemName: string
          Quantity: string
          Note: string
          ScheduleKind: ScheduleKind
          Frequency: int<days>
          Postpone: int<days> option
          CategoryMode: CategoryMode
          NewCategoryName: string
          CategoryChoice: Category option
          CategoryChoiceList: Category list
          Stores: QueryTypes.ItemAvailability list }

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
        | CategoryModeIsChooseExisting
        | CategoryModeIsCreateNew
        | ChooseCategoryUncategorized
        | ChooseCategory of Guid
        | NewCategoryNameSet of string
        | NewCategoryNameBlur
        | StoresSetAvailability of store: StoreId * isSold: bool

module ItemForm =
    open ItemFormTypes

    let itemNameValidation f = f.ItemName |> ItemName.tryParse
    let itemNameChange s f = { f with ItemName = s }
    let itemNameBlur f = { f with ItemName = f.ItemName |> ItemName.normalizer }
    let quantityValidation f = f.Quantity |> Quantity.tryParseOptional
    let quantityChange s f = { f with Quantity = s }
    let quantityBlur f = { f with Quantity = f.Quantity |> Quantity.normalizer }
    let noteChange s f = { f with Note = s }
    let noteValidation f = f.Note |> Note.tryParseOptional
    let noteBlur f = { f with Note = f.Note |> Note.normalizer }

    let categoryNameValidation f =
        if f.CategoryMode = CategoryMode.ChooseExisting then
            None |> Ok
        else
            f.NewCategoryName |> CategoryName.tryParseOptional

    let categoryNameChange s f = { f with NewCategoryName = s }

    let categoryNameBlur f =
        { f with
              NewCategoryName = f.NewCategoryName |> CategoryName.normalizer }

    let scheduleOnce f = { f with ScheduleKind = Once }
    let scheduleCompleted f = { f with ScheduleKind = Completed }
    let scheduleRepeat f = { f with ScheduleKind = Repeat }

    let frequencyCoerceIntoBounds d = Repeat.frequencyRules |> RangeValidation.forceIntoBounds d
    let frequencySet v f = { f with Frequency = v |> frequencyCoerceIntoBounds }
    let frequencyDefault = 7<StateTypes.days> |> frequencyCoerceIntoBounds

    let frequencyChoices (f: Form) =
        f.Frequency
        :: Repeat.commonFrequencies
        |> Seq.map frequencyCoerceIntoBounds
        |> Seq.sort
        |> List.ofSeq

    let frequencyAsText (d: int<StateTypes.days>) =
        let d = d |> int
        let monthsExactly = if d / 30 > 0 && d % 30 = 0 then Some(d / 30) else None
        let weeksExactly = if d / 7 > 0 && d % 7 = 0 then Some(d / 7) else None

        match monthsExactly with
        | Some m -> if m = 1 then "Monthly" else sprintf "Every %i months" m
        | None ->
            match weeksExactly with
            | Some w -> if w = 1 then "Weekly" else sprintf "Every %i weeks" w
            | None -> if d = 1 then "Daily" else sprintf "Every %i days" d

    let postponeSet v f = { f with Postpone = Some v }
    let postponeClear f = { f with Postpone = None }
    let postponeDefault = None
    let categoryModeIsChooseExisting f = { f with CategoryMode = ChooseExisting }
    let categoryModeIsCreateNew f = { f with CategoryMode = CreateNew }

    let chooseCategoryUncategorized f = { f with CategoryChoice = None }

    let chooseCategory i f =
        { f with
              CategoryChoice =
                  f.CategoryChoiceList
                  |> List.find (fun j -> j.CategoryId = StateTypes.CategoryId i)
                  |> Some }

    let storesSetAvailability id isSold f =
        { f with
              Stores =
                  f.Stores
                  |> List.map (fun a -> if a.Store.StoreId = id then { a with IsSold = isSold } else a) }

    let createNewItem stores cats =
        { ItemId = None
          ItemName = ""
          Quantity = ""
          Note = ""
          ScheduleKind = ScheduleKind.Once
          Frequency = frequencyDefault
          Postpone = postponeDefault
          CategoryMode = CategoryMode.ChooseExisting
          NewCategoryName = ""
          CategoryChoice = None
          CategoryChoiceList = cats |> List.sortBy (fun i -> i.CategoryName)
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
          Quantity =
              i.Quantity
              |> Option.map Quantity.asText
              |> Option.defaultValue ""
          Note = i.Note |> Option.map Note.asText |> Option.defaultValue ""
          ScheduleKind =
              match i.Schedule with
              | StateTypes.Schedule.Completed -> Completed
              | StateTypes.Schedule.Once -> Once
              | StateTypes.Schedule.Repeat _ -> Repeat
          Frequency =
              match i.Schedule with
              | StateTypes.Schedule.Completed -> frequencyDefault
              | StateTypes.Schedule.Once -> frequencyDefault
              | StateTypes.Schedule.Repeat r -> r.Frequency
          Postpone =
              match i.Schedule with
              | StateTypes.Schedule.Completed -> postponeDefault
              | StateTypes.Schedule.Once -> postponeDefault
              | StateTypes.Schedule.Repeat r -> r.PostponedUntil |> Repeat.postponeRelative clock
          CategoryMode = CategoryMode.ChooseExisting
          NewCategoryName = ""
          CategoryChoice = i.Category
          CategoryChoiceList = cats |> List.sortBy (fun i -> i.CategoryName)
          Stores =
              i.Availability
              |> Seq.sortBy (fun i -> i.Store.StoreName)
              |> List.ofSeq }

    let hasErrors f =
        (f |> itemNameValidation |> Result.isOk)
        && (f |> quantityValidation |> Result.isOk)
        && (f |> noteValidation |> Result.isOk)
        && (f |> categoryNameValidation |> Result.isOk)

    let handleMessage msg (f: Form) =
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
        | CategoryModeIsChooseExisting -> f |> categoryModeIsChooseExisting
        | CategoryModeIsCreateNew -> f |> categoryModeIsCreateNew
        | ChooseCategoryUncategorized -> f |> chooseCategoryUncategorized
        | ChooseCategory g -> f |> chooseCategory g
        | NewCategoryNameSet s -> f |> categoryNameChange s
        | NewCategoryNameBlur -> f |> categoryNameBlur
        | StoresSetAvailability (id: StateTypes.StoreId, isSold: bool) -> f |> storesSetAvailability id isSold

    type Form with
        member me.ItemNameValidation = me |> itemNameValidation
        member me.NoteValidation = me |> noteValidation
        member me.QuantityValidation = me |> quantityValidation
        member me.FrequencyChoices = me |> frequencyChoices
        member me.CategoryNameValidation = me |> categoryNameValidation
        member me.HasErrors = me |> hasErrors
