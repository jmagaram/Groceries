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

type ListItem<'T> = { Value: 'T; Key: string; IsSelected: bool }

type T =
    { ItemId: ItemId
      ItemName: TextInput<ItemName, StringError>
      Quantity: TextInput<Quantity, StringError>
      Schedule: RelativeSchedule
      RepeatIntervalChoices: int<days> list
      Stores: ItemAvailability list }

type StoreAvailabilitySummary =
    | SoldEverywhere
    | NotSoldAnywhere
    | SoldOnlyAt of Store
    | SoldEverywhereExcept of Store
    | VariedAvailability

let availabilitySummary (availList:ItemAvailability seq) = 
    let soldAt = availList |> Seq.choose (fun i -> if i.IsSold then Some i.Store else None) |> Set.ofSeq
    let notSoldAt = availList |> Seq.choose (fun i -> if i.IsSold = false then Some i.Store else None) |> Set.ofSeq
    match soldAt.Count, notSoldAt.Count with
    | x, 0 -> SoldEverywhere
    | 0, x when x >= 1 -> NotSoldAnywhere
    | 1, x when x >= 1 -> SoldOnlyAt (soldAt |> Seq.head)
    | x, 1 when x >= 1 -> SoldEverywhereExcept (notSoldAt |> Seq.head)
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

let createNew =
    { ItemId = Id.create ItemId
      ItemName = TextInput.init itemNameValidator ItemName.normalizer ""
      Quantity = TextInput.init quantityValidator Quantity.normalizer ""
      Schedule = RelativeSchedule.Once
      RepeatIntervalChoices = Repeat.commonIntervals
      Stores = stores }

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
    member this.ScheduleComplete() = this |> scheduleComplete
    member this.ScheduleOnlyOnce() = this |> scheduleOnlyOnce
    member this.ScheduleRepeat(d) = this |> scheduleRepeat d
    member this.SchedulePostpone(d) = this |> schedulePostpone d
    member this.SetStoreAvailability(s,b) = this |> setStoreAvailability s b
    member this.StoreSummary() = this.Stores |> availabilitySummary
    member this.RepeatIntervalAsText(d) = d |> repeatIntervalAsText
