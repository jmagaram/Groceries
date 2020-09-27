[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemEditForm

open System
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

let cats =
    [ "Food"; "Frozen"; "Dry"; "Dairy" ]
    |> List.map (fun i ->
        { CategoryId = Id.create CategoryId
          CategoryName = i |> CategoryName.tryParse |> Result.okOrThrow })

type CategoryMode =
    | CreateNewMode
    | ExistingOrUncategorizedMode

type CategoryPicker =
    { Mode: CategoryMode
      CreateNewCategory: TextBox<CategoryName, StringError>
      ExistingOrUncategorized: ChooseZeroOrOne<Category> }

type CategoryMessage =
    | NewCategoryMessage of TextBoxMessage
    | SelectorMessage of ChooseZeroOrOneMessage<Guid>
    | SetMode of CategoryMode

type T =
    { ItemId: ItemId
      ItemName: TextBox<ItemName, StringError>
      Quantity: TextBox<Quantity option, StringError>
      Note: TextBox<Note option, StringError>
      Schedule: RelativeSchedule
      RepeatIntervalChoices: int<days> list
      Stores: ItemAvailability list
      CategoryPicker: CategoryPicker }

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

let newCategoryPicker =
    let defaultMode = CategoryMode.ExistingOrUncategorizedMode
    let choices = ChooseZeroOrOne.init cats |> ChooseZeroOrOne.selectNothing

    let createNew =
        TextBox.init CategoryName.tryParse CategoryName.normalizer ""

    { Mode = defaultMode
      ExistingOrUncategorized = choices
      CreateNewCategory = createNew }

let createNew =
    { ItemId = Id.create ItemId
      ItemName = TextBox.init ItemName.tryParse ItemName.normalizer ""
      Quantity = TextBox.init quantityParser Quantity.normalizer ""
      Note = TextBox.init noteParser Note.normalizer ""
      Schedule = RelativeSchedule.Once
      RepeatIntervalChoices = Repeat.commonIntervals
      Stores = stores
      CategoryPicker = newCategoryPicker }

let setStoreAvailability s b (form: T) =
    { form with
          Stores =
              form.Stores
              |> List.map (fun i -> if i.Store.StoreId = s then { i with IsSold = b } else i) }

let processItemNameMessage msg (f: T) =
    let handler = TextBox.handleMessage ItemName.tryParse ItemName.normalizer
    { f with ItemName = f.ItemName |> handler msg }

let processQuantityMessage msg (f: T) =
    let handler = TextBox.handleMessage quantityParser Quantity.normalizer
    { f with Quantity = f.Quantity |> handler msg }

let processNoteMessage msg (f: T) =
    let handler = TextBox.handleMessage noteParser Note.normalizer
    { f with Note = f.Note |> handler msg }

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

let processCategoryPickerMessage msg f =
    let rec go msg cp =
        let processCatMessage =
            TextBox.handleMessage CategoryName.tryParse CategoryName.normalizer

        let processCatExisting =
            ChooseZeroOrOne.handleMessage (fun (c: Category) ->
                match c.CategoryId with
                | CategoryId id -> id)

        match msg with
        | NewCategoryMessage msg ->
            { cp with
                  CreateNewCategory = cp.CreateNewCategory |> processCatMessage msg }
            |> go (SetMode CategoryMode.CreateNewMode)
        | SelectorMessage msg ->
            { cp with
                  ExistingOrUncategorized = cp.ExistingOrUncategorized |> processCatExisting msg }
            |> go (SetMode CategoryMode.ExistingOrUncategorizedMode)
        | SetMode mode -> { cp with Mode = mode }

    let cp' = go msg f.CategoryPicker
    { f with CategoryPicker = cp' }

type Message =
    | ItemNameMessage of TextBoxMessage
    | QuantityMessage of TextBoxMessage
    | NoteMessage of TextBoxMessage
    | CategoryMessage of CategoryMessage

let processMessage (m: Message) (f: T) =
    match m with
    | ItemNameMessage m -> f |> processItemNameMessage m
    | QuantityMessage m -> f |> processQuantityMessage m
    | NoteMessage m -> f |> processNoteMessage m
    | CategoryMessage m -> f |> processCategoryPickerMessage m

type T with
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
