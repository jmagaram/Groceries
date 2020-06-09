module State
open DomainTypes

let private random = new System.Random()

let private randomFrom items = 
    let array = items |> Seq.toArray
    fun () -> array.[random.Next(0, array.Length)]

let private sampleItems =
    let itemsToParse = 
        [ 
            ("Chocolate bars", "5", "Very dark, Alter Eco preferred",  Repeat.DailyInterval 14)
            ("Dried cannellini or white beans", "Plenty", "", Repeat.DailyInterval 30)
            ("Dried chickpeas", "", "Check the bulk section if no cans available", DoesNotRepeat)
            ("Flour - White All Purpose", "2 large bags", "" ,DoesNotRepeat)
            ("Flour - White Bread", "2 large bags", "" ,DoesNotRepeat)
            ("Gluten (for baking)", "", "" ,DoesNotRepeat)
            ("Chocolate chips", "", "Prefer Ghiradelli brand", Repeat.DailyInterval 30)
            ("Peanut butter", "Several jars", "CB, Adams, or Santa Cruz", Repeat.DailyInterval 30)
            ("Coconut milk (unsweetened)", "4 cans", "", DoesNotRepeat)
            ("Olive oil (medium quality)", "1 jar", "", Repeat.DailyInterval 60)
            ("Fish sauce", "2 bottles", "Prefer a specialty brand rather than Thai Kitchen", Repeat.DailyInterval 90)
            ("Mamma's lil' peppers", "1 jar", "", DoesNotRepeat)
            ("Heritage Flakes cereal", "2 bags", "", Repeat.DailyInterval 21)
            ("Limes", "4", "", Repeat.DailyInterval 7)
            ("Lemon", "3", "", Repeat.DailyInterval 7)
            ("Oranges", "6", "Cara Cara preferred, maybe Sumo", Repeat.DailyInterval 7)
            ("Apples", "8", "Like the semi-sweet varieties like Pink Lady and Envy. Avoid Granny Smith and Fuji.", Repeat.DailyInterval 7)
            ("Blueberries", "2 large tubs", "Organic preferred", Repeat.DailyInterval 7)
            ("Bananas", "1 bunch", "", Repeat.DailyInterval 7)
            ("Tomatoes (cherry)", "1 pint", "Like the on-the-vine kind", Repeat.DailyInterval 7)
            ("Avocados", "3 firm and 2 ripe", "", Repeat.DailyInterval 7)
            ("Broccoli or broccolini", "", "", Repeat.DailyInterval 7)
            ("Ginger", "", "", Repeat.DailyInterval 14)
            ("Lemongrass", "", "", DoesNotRepeat)
            ("Onions (Yellow)", "3", "", Repeat.DailyInterval 7)
            ("Butter (salted)", "", "Lurpak brand only", Repeat.DailyInterval 30)
            ("Feta cheese", "", "",DoesNotRepeat)
            ("Siggi's yogurt", "6 or so assorted flavors", "", Repeat.DailyInterval 14)
            ("Milk (low-fat)", "1 half gallon", "", Repeat.DailyInterval 14)
            ("Kombucha", "4 jars", "Assorted flavors", Repeat.DailyInterval 21)
            ("Nancy's yogurt", "2 tubs", "", Repeat.DailyInterval 21)
            ("Eggs", "2 dozen", "Free-range and happy birds please!", Repeat.DailyInterval 14)
            ("Mochi", "3 pints, at least one vegan", "", Repeat.DailyInterval 7)
            ("Deli meat (turkey or roast beef)", "Couple packages", "", DoesNotRepeat)
            ("Salmon filet", "32 oz", "Only if never previously frozen", DoesNotRepeat)
        ]
    let parseQuantity qty = 
        match qty |> isNullOrWhiteSpace with
        | true -> None
        | false -> qty |> Quantity |> Some
    let parseNote note = 
        match note |> isNullOrWhiteSpace with
        | true -> None
        | false -> note |> Note |> Some
    let shouldPostponeRandom =
        "pppppp_"
        |> Seq.map (fun c -> c = 'p')
        |> randomFrom
    let today = System.DateTime.Now
    let postponeUntilRandom d =
        let rangeMax = d
        let rangeMin = -10
        match shouldPostponeRandom() with
        | false -> None
        | true -> 
            let daysFromToday = rangeMin + random.Next(rangeMin, rangeMax + 20)
            today.AddDays(daysFromToday |> float) |> Some
    let createStatus r =
        match r with
        | DoesNotRepeat -> Status.Active
        | DailyInterval i ->              
            let postpone = i |> postponeUntilRandom
            match postpone with
            | Some dt -> Status.Postponed dt
            | None -> Status.Active
    let createItem title qty note freq = 
        { Item.Id = Id.create ItemId
          Title = title |> Title
          Quantity = qty |> parseQuantity
          Repeat = freq
          Status = freq |> createStatus
          Note = note |> parseNote }
    itemsToParse
    |> Seq.map (fun (title, qty, note, freq) -> createItem title qty note freq)
    |> Seq.map (fun i -> if random.Next(0,10) = 0 then { i with Status = Status.Complete } else i)
    |> Seq.toList

type Create = unit -> State
let create : Create = fun () -> 
    { Stores = Map.empty
      ItemIsUnavailableInStore = Set.empty
      Items = 
        sampleItems 
        |> Seq.map (fun i -> (i.Id, i))
        |> Map.ofSeq 
      ItemListView = ItemListView.create Seq.empty None
    }

type Update = StateMessage -> State -> State
let update : Update = fun msg s ->
    match msg with
    | InsertItem i ->
        let ni = i |> ItemEditorModel.toNewItem System.DateTime.Now
        { s with Items = s.Items |> Map.add ni.Id ni }
    | ItemListViewMessage m -> s

let private addItem state (item:Item) = 
    { state with State.Items = state.Items |> Map.add item.Id item }

type AddSampleItems = State -> State
let addSampleItems : AddSampleItems = fun s ->
    let state = 
        sampleItems
        |> Seq.fold addItem s
    let view = 
        let filter = System.TimeSpan.FromDays(1 |> float) |> Some
        let items = state.Items |> Map.values
        ItemListView.create items filter
    { s with ItemListView = view }

let stateWithSampleItems = create() |> addSampleItems

module Tests =

    open Xunit
    open FsUnit

    [<Fact>]
    let ``Sample items has lots of stuff in it`` () =
        sampleItems
        |> List.length
        |> should greaterThan 10

    [<Fact>]
    let ``Sample items have many unique ids`` () =
        sampleItems
        |> Seq.map (fun i -> i.Id)
        |> Set.ofSeq
        |> Set.count
        |> should greaterThan 10

    [<Fact>]
    let ``Create with sample items - has stuff in it`` () =
        let actual = stateWithSampleItems
        actual.ItemListView.Items
        |> Seq.length
        |> should greaterThan 10
