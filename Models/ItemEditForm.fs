[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemEditForm

open Models
open Models.StateTypes
open Models.ValidationTypes
open Models.FormsTypes

type ItemAvailability = { Store: Store; IsSold: bool }

type RelativeRepeat = { Interval: int<days>; PostponeDays: int<days> option }

type RelativeSchedule =
    | Once
    | Completed
    | Repeat of RelativeRepeat

// Does the parser AND normalizer both normalize? If yes, why does TextInput.init need both?
// Radio
// Combo?

// choose from list...
// if list is empty,

type CategorySelectorMode =
    | CreateNewCategory
    | ChooseCategoryOrUncategorized

type CategorySelector =
    { CategorySelectorMode: CategorySelectorMode
      CreateNewCategory: TextInput<CategoryName, StringError>
      ChooseExistingCategory: ChooseZeroOrOne<Category> }


type T =
    { ItemId: ItemId
      ItemName: TextInput<ItemName, StringError>
      Quantity: TextInput<Quantity option, StringError>
      Note: TextInput<Note option, StringError>
      Schedule: RelativeSchedule
      RepeatIntervalChoices: int<days> list
      Stores: ItemAvailability list
      CategorySelector: CategorySelector }

type StoreAvailabilitySummary =
    | SoldEverywhere
    | NotSoldAnywhere
    | SoldOnlyAt of Store
    | SoldEverywhereExcept of Store
    | VariedAvailability

// rename TextInput.init validate to "parser"

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

let repeatIntervalNormalize d = Repeat.rules |> RangeValidation.forceIntoBounds d

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

let (noteParser, quantityParser) =
    let tryParseOptional normalizer tryParse s =
        Some(normalizer s)
        |> Option.filter String.isNotEmpty
        |> Option.map tryParse
        |> Option.map (Result.map Some)
        |> Option.defaultValue (Ok None)

    let noteParser = tryParseOptional Note.normalizer Note.tryParse
    let quantityParser = tryParseOptional Quantity.normalizer Quantity.tryParse
    (noteParser, quantityParser)

let createNew =
    { ItemId = Id.create ItemId
      ItemName = TextInput.init ItemName.tryParse ItemName.normalizer ""
      Quantity = TextInput.init quantityParser Quantity.normalizer ""
      Note = TextInput.init noteParser Note.normalizer ""
      Schedule = RelativeSchedule.Once
      RepeatIntervalChoices = Repeat.commonIntervals
      Stores = stores
      CategorySelector =
          let chooseCategory = ChooseZeroOrOne.init cats |> ChooseZeroOrOne.selectNothing

          let createCategory =
              TextInput.init CategoryName.tryParse CategoryName.normalizer ""

          { CategorySelector.CategorySelectorMode = ChooseCategoryOrUncategorized
            CategorySelector.ChooseExistingCategory = chooseCategory
            CategorySelector.CreateNewCategory = createCategory } }

let setStoreAvailability s b (form: T) =
    { form with
          Stores =
              form.Stores
              |> List.map (fun i -> if i.Store.StoreId = s then { i with IsSold = b } else i) }

let itemNameEdit n (form: T) =
    { form with
          ItemName = form.ItemName |> TextInput.setText ItemName.tryParse n }

let itemNameLoseFocus (form: T) =
    { form with
          ItemName = form.ItemName |> TextInput.loseFocus ItemName.normalizer }

let quantityEdit n (form: T) =
    { form with
          Quantity = form.Quantity |> TextInput.setText quantityParser n }

let quantityLoseFocus (form: T) =
    { form with
          Quantity = form.Quantity |> TextInput.loseFocus Quantity.normalizer }

let noteEdit n (form: T) =
    { form with
          Note = form.Note |> TextInput.setText noteParser n }

let noteLoseFocus (form: T) =
    { form with
          Note = form.Note |> TextInput.loseFocus Note.normalizer }

let scheduleComplete (form: T) = { form with Schedule = RelativeSchedule.Completed }

let scheduleOnlyOnce (form: T) = { form with Schedule = RelativeSchedule.Once }

let scheduleRepeat (d: int<days>) (form: T) =
    let d = d |> repeatIntervalNormalize

    let schedule =
        match form.Schedule with
        | Once -> Repeat { Interval = d; PostponeDays = None }
        | Completed -> Repeat { Interval = d; PostponeDays = None }
        | Repeat r -> Repeat { r with Interval = d }

    { form with
          Schedule = schedule
          RepeatIntervalChoices = d :: Repeat.commonIntervals |> List.distinct |> List.sort }

let schedulePostpone (d: int<days>) (form: T) =
    let schedule =
        match form.Schedule with
        | Once -> Repeat { Interval = 7<days>; PostponeDays = Some d }
        | Completed -> Repeat { Interval = 7<days>; PostponeDays = Some d }
        | Repeat r -> Repeat { r with PostponeDays = Some d }

    { form with Schedule = schedule }

let removePostpone (form: T) =
    match form.Schedule with
    | Once -> form
    | Completed -> form
    | Repeat r ->
        { form with
              Schedule = Repeat { r with PostponeDays = None } }

let postponeChoices (form: T) =
    let p =
        match form.Schedule with
        | Repeat r -> r.PostponeDays
        | _ -> None

    [ 1; 3; 7; 14; 30; 60 ]
    |> Seq.map (fun i -> i * 1<days> |> Some)
    |> Seq.append [ p ]
    |> Seq.choose id
    |> Seq.distinct
    |> Seq.sort

let durationAsText (d: int<days>) =
    let d = d |> int
    let monthsExactly = if d / 30 > 0 && d % 30 = 0 then Some(d / 30) else None
    let weeksExactly = if d / 7 > 0 && d % 7 = 0 then Some(d / 7) else None

    match monthsExactly with
    | Some m -> if m = 1 then "1 month" else sprintf "%i months" m
    | None ->
        match weeksExactly with
        | Some w -> if w = 1 then "1 week" else sprintf "%i weeks" w
        | None -> if d = 1 then "1 day" else sprintf "%i days" d

type Message =
    | StartCreatingNewCategory
    | NewCategoryMessage of TextInputMessage
    | ExistingCategoryMessage of ChooseZeroOrOneMessage<System.Guid>

let mapCategory f (form: T) = { form with CategorySelector = f form.CategorySelector }

let processMessage (m: Message) (f: T) =
    match m with
    | StartCreatingNewCategory ->
        { f with
              CategorySelector =
                  { f.CategorySelector with
                        CategorySelectorMode = CreateNewCategory } }
    | ExistingCategoryMessage msg ->
        let handle =
            ChooseZeroOrOne.handleMessage (fun (i: Category) ->
                match i.CategoryId with
                | CategoryId x -> x)

        let choose = f.CategorySelector.ChooseExistingCategory |> handle msg

        { f with
              CategorySelector =
                  { f.CategorySelector with
                        ChooseExistingCategory = choose
                        CategorySelectorMode = ChooseCategoryOrUncategorized } }
    | NewCategoryMessage msg ->
        let handle =
            TextInput.handleMessage CategoryName.tryParse CategoryName.normalizer

        let ti = f.CategorySelector.CreateNewCategory |> handle msg

        { f with
              CategorySelector =
                  { f.CategorySelector with
                        CreateNewCategory = ti
                        CategorySelectorMode = CreateNewCategory } }

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
    member this.RemovePostpone() = this |> removePostpone
    member this.SetStoreAvailability(s, b) = this |> setStoreAvailability s b
    member this.StoreSummary() = this.Stores |> availabilitySummary
    member this.RepeatIntervalAsText(d) = d |> repeatIntervalAsText
    member this.PostponeChoices() = this |> postponeChoices // should this be a property? safer binding?
    member this.PostponeDurationAsText(d) = d |> durationAsText

    member this.PostponeDuration =
        match this.Schedule with
        | Repeat r -> r.PostponeDays
        | _ -> None
