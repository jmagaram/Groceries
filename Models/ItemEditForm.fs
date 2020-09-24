[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemEditForm

open Models
open Models.StateTypes
open Models.ValidationTypes
open Models.FormsTypes

type ItemAvailability = { Store: Store; IsSold: bool }

type RelativeSchedule =
    | Once
    | Completed
    | Repeat of {| Interval: int<days>; PostponeDays: int<days> option |}

type CategoryPickerMode =
    | NoCategory
    | ChooseExistingCategory
    | CreateNewCategory

type CategoryPicker =
    { CategoryPickerMode: CategoryPickerMode
      CreateCategory: TextInput<CategoryName, StringError>
      ExistingCategory: Category option
      ExistingCategoryChoices: Category list }

module CategoryPicker =

    let categoryNameValidator = CategoryName.tryParse >> Result.mapError List.head

    let init cats =
        { CategoryPickerMode = NoCategory
          CreateCategory = TextInput.init categoryNameValidator CategoryName.normalizer ""
          ExistingCategory = None
          ExistingCategoryChoices = cats }

    let setMode mode cp = { cp with CategoryPickerMode = mode }

    let newCategoryNameEdit n cp =
        { cp with
              CreateCategory =
                  cp.CreateCategory
                  |> TextInput.setText categoryNameValidator n
              CategoryPickerMode = CreateNewCategory }

    let newCategoryNameLoseFocus cp =
        { cp with
              CreateCategory =
                  cp.CreateCategory
                  |> TextInput.loseFocus CategoryName.normalizer }

    let chooseExisting id cp =
        let cat =
            cp.ExistingCategoryChoices
            |> List.tryFind (fun i -> i.CategoryId = id)

        { cp with
              ExistingCategory = cat
              CategoryPickerMode = ChooseExistingCategory }

type CategoryPicker with
    member this.ModeNoCategory() = this |> CategoryPicker.setMode NoCategory
    member this.ModeCreateNew() = this |> CategoryPicker.setMode CreateNewCategory
    member this.ModeChooseExisting() = this |> CategoryPicker.setMode ChooseExistingCategory
    member this.NewCategoryEdit(n) = this |> CategoryPicker.newCategoryNameEdit n
    member this.NewCategoryLoseFocus() = this |> CategoryPicker.newCategoryNameLoseFocus
    member this.ChooseExisting(id) = this |> CategoryPicker.chooseExisting (CategoryId id)

type T =
    { ItemId: ItemId
      ItemName: TextInput<ItemName, StringError>
      Quantity: TextInput<Quantity, StringError>
      Note: TextInput<Note, StringError>
      Schedule: RelativeSchedule
      RepeatIntervalChoices: int<days> list
      Stores: ItemAvailability list
      Category: CategoryPicker }

type StoreAvailabilitySummary =
    | SoldEverywhere
    | NotSoldAnywhere
    | SoldOnlyAt of Store
    | SoldEverywhereExcept of Store
    | VariedAvailability

let availabilitySummary (availList: ItemAvailability seq) =
    let soldAt =
        availList
        |> Seq.choose (fun i -> if i.IsSold then Some i.Store else None)
        |> Set.ofSeq

    let notSoldAt =
        availList
        |> Seq.choose (fun i -> if i.IsSold = false then Some i.Store else None)
        |> Set.ofSeq

    match soldAt.Count, notSoldAt.Count with
    | x, 0 -> SoldEverywhere
    | 0, x when x >= 1 -> NotSoldAnywhere
    | 1, x when x >= 1 -> SoldOnlyAt(soldAt |> Seq.head)
    | x, 1 when x >= 1 -> SoldEverywhereExcept(notSoldAt |> Seq.head)
    | _, _ -> VariedAvailability

// stores
// list all stores and availability of each

let repeatIntervalNormalize d = d |> max (Repeat.rules.Max) |> min Repeat.rules.Min

let repeatIntervalAsText (d: int<days>) =
    let d = d |> int
    let monthsExactly = if d / 30 > 0 && d % 30 = 0 then Some(d / 30) else None
    let weeksExactly = if d / 7 > 0 && d % 7 = 0 then Some(d / 7) else None

    match monthsExactly with
    | Some m -> if m = 1 then "Monthly" else sprintf "Every %i months" m
    | None ->
        match weeksExactly with
        | Some w -> if w = 1 then "Weekly" else sprintf "Every %i weeks" w
        | None -> if d = 1 then "Daily" else sprintf "Every %i days" d

let repeatIntervalSerialize d =
    match d with
    | Some d -> d.ToString()
    | None -> "doesNotRepeat"

let repeatIntervalDeserialize s =
    if s = "doesNotRepeat" then
        None
    else
        match s |> String.tryParseInt with
        | None -> Some 7<days>
        | Some d -> Some(d * 1<days> |> repeatIntervalNormalize)

let itemNameValidator = ItemName.tryParse >> Result.mapError List.head
let quantityValidator = Quantity.tryParse >> Result.mapError List.head
let noteValidator = Note.tryParse >> Result.mapError List.head

let stores =
    [ ("QFC", true)
      ("Whole Foods", false)
      ("Trader Joe's", true)
      ("Cosco", true) ]
    |> List.map (fun (name, isAvailable) ->
        { Store =
              { StoreId = Id.create StoreId
                StoreName = StoreName.tryParse name |> Result.okOrThrow }
          IsSold = isAvailable })

let cats =
    [ "Food"; "Frozen"; "Dry"; "Dairy" ]
    |> List.map (fun i ->
        { CategoryId = Id.create CategoryId
          CategoryName = i |> CategoryName.tryParse |> Result.okOrThrow })

let createNew =
    { ItemId = Id.create ItemId
      ItemName = TextInput.init itemNameValidator ItemName.normalizer ""
      Quantity = TextInput.init quantityValidator Quantity.normalizer ""
      Note = TextInput.init noteValidator Note.normalizer ""
      Schedule = RelativeSchedule.Once
      RepeatIntervalChoices = Repeat.commonIntervals
      Stores = stores 
      Category = CategoryPicker.init cats 
    }

let setStoreAvailability s b (form: T) =
    { form with
          Stores =
              form.Stores
              |> List.map (fun i -> if i.Store.StoreId = s then { i with IsSold = b } else i) }

let itemNameEdit n (form: T) =
    { form with
          ItemName = form.ItemName |> TextInput.setText itemNameValidator n }

let itemNameLoseFocus (form: T) =
    { form with
          ItemName = form.ItemName |> TextInput.loseFocus ItemName.normalizer }

let quantityEdit n (form: T) =
    { form with
          Quantity = form.Quantity |> TextInput.setText quantityValidator n }

let quantityLoseFocus (form: T) =
    { form with
          Quantity = form.Quantity |> TextInput.loseFocus Quantity.normalizer }

let noteEdit n (form: T) =
    { form with
          Note = form.Note |> TextInput.setText noteValidator n }

let noteLoseFocus (form: T) =
    { form with
          Note = form.Note |> TextInput.loseFocus Note.normalizer }

let scheduleComplete (form: T) = { form with Schedule = RelativeSchedule.Completed }

let scheduleOnlyOnce (form: T) = { form with Schedule = RelativeSchedule.Once }

let scheduleRepeat (d: int<days>) (form: T) =
    let d = d |> repeatIntervalNormalize

    let schedule =
        match form.Schedule with
        | Once -> Repeat {| Interval = d; PostponeDays = None |}
        | Completed -> Repeat {| Interval = d; PostponeDays = None |}
        | Repeat r -> Repeat {| r with Interval = d |}

    { form with
          Schedule = schedule
          RepeatIntervalChoices = d :: Repeat.commonIntervals |> List.distinct |> List.sort }

let schedulePostpone (d: int<days>) (form: T) =
    let schedule =
        match form.Schedule with
        | Once -> Repeat {| Interval = 7<days>; PostponeDays = Some d |}
        | Completed -> Repeat {| Interval = 7<days>; PostponeDays = Some d |}
        | Repeat r -> Repeat {| r with PostponeDays = Some d |}

    { form with Schedule = schedule }

type T with
    member this.ItemNameEdit(n) = this |> itemNameEdit n
    member this.ItemNameLoseFocus() = this |> itemNameLoseFocus
    member this.QuantityEdit(n) = this |> quantityEdit n
    member this.QuantityLoseFocus() = this |> quantityLoseFocus
    member this.NoteEdit(n) = this |> noteEdit n
    member this.NoteLoseFocus() = this |> noteLoseFocus
    member this.ScheduleComplete() = this |> scheduleComplete
    member this.ScheduleOnlyOnce() = this |> scheduleOnlyOnce
    member this.ScheduleRepeat(d) = this |> scheduleRepeat d
    member this.SchedulePostpone(d) = this |> schedulePostpone d
    member this.SetStoreAvailability(s, b) = this |> setStoreAvailability s b
    member this.StoreSummary() = this.Stores |> availabilitySummary
    member this.RepeatIntervalAsText(d) = d |> repeatIntervalAsText
    member this.SetCategory(c) = { this with Category = c }

