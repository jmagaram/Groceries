[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.ItemEditForm

open Models
open Models.StateTypes
open Models.ValidationTypes

type ItemAvailability = { Store: Store; IsSold: bool }

type RelativeSchedule =
    | Once
    | Completed
    | Repeat of {| Interval: int<days>; PostponeDays: int<days> option |}

type ListItem<'T> =
    { Value : 'T 
      Key : string
      IsSelected : bool }

//type CategoryEditForm = 
//    { CategoryId : CategoryId
//      CategoryName : string 
//      CategoryNameError : StringError option
//    }

type T =
    { ItemId: ItemId
      ItemName: string
      ItemNameError: (StringError list) option
      Quantity: string
      QuantityError: (StringError list) option
      Schedule: RelativeSchedule
      RepeatIntervalChoices: int<days> list 
      //Category : Category option // figure out later if new or existing
      //NewCategory : string
      //NewCategoryError : string option

    }

let repeatIntervalNormalize d = d |> max (Repeat.rules.Max) |> min Repeat.rules.Min

let repeatIntervalAsText (d: int<days>) =
    let d = d |> int
    let monthsExactly = if d / 30 > 0 && d % 30 = 0 then Some (d / 30) else None
    let weeksExactly = if d / 7 > 0 && d % 7 = 0 then Some (d / 7) else None
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

let createNew =
    { ItemId = Id.create ItemId
      ItemName = ""
      ItemNameError = "" |> ItemName.tryParse |> Result.error
      Quantity = ""
      QuantityError = "" |> Quantity.tryParse |> Result.error
      Schedule = RelativeSchedule.Once
      RepeatIntervalChoices = Repeat.commonIntervals }

let itemNameEdit n form =
    { form with
          ItemName = n
          ItemNameError = n |> String.trim |> ItemName.tryParse |> Result.error }

let itemNameLoseFocus (form: T) = { form with ItemName = form.ItemName |> String.trim }

let quantityEdit n form =
    { form with
          Quantity = n
          QuantityError = n |> String.trim |> Quantity.tryParse |> Result.error }

let quantityLoseFocus (form: T) = { form with Quantity = form.Quantity |> String.trim }

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
    member this.RepeatIntervalAsText(d) = d |> repeatIntervalAsText
