module Test
open System
open DomainTypes

let random = new Random()

let randomFrom items = 
    let array = items |> Seq.toArray
    fun () -> array.[random.Next(0, array.Length)]

let sampleItems =
    let itemsToParse = 
        [ 
            ("Chocolate bars", "5", "Very dark, Alter Eco preferred", Some Frequency.Every2Weeks)
            ("Dried cannellini or white beans", "Plenty", "", Some Frequency.Monthly)
            ("Dried chickpeas", "", "Check the bulk section if no cans available",None)
            ("Flour - White All Purpose", "2 large bags", "" ,None)
            ("Flour - White Bread", "2 large bags", "" ,None)
            ("Gluten (for baking)", "", "" ,None)
            ("Chocolate chips", "", "Prefer Ghiradelli brand", Some Frequency.Monthly)
            ("Peanut butter", "Several jars", "CB, Adams, or Santa Cruz", Some Frequency.Monthly)
            ("Coconut milk (unsweetened)", "4 cans", "", None)
            ("Olive oil (medium quality)", "1 jar", "", Some Frequency.Every2Months)
            ("Fish sauce", "2 bottles", "Prefer a specialty brand rather than Thai Kitchen", Some Frequency.Every3Months)
            ("Mamma's lil' peppers", "1 jar", "", None)
            ("Heritage Flakes cereal", "2 bags", "", Some Frequency.Every3Weeks)
            ("Limes", "4", "", Some Frequency.Weekly)
            ("Lemon", "3", "", Some Frequency.Weekly)
            ("Oranges", "6", "Cara Cara preferred, maybe Sumo", Some Frequency.Weekly)
            ("Apples", "8", "Like the semi-sweet varieties like Pink Lady and Envy. Avoid Granny Smith and Fuji.", Some Frequency.Weekly)
            ("Blueberries", "2 large tubs", "Organic preferred", Some Frequency.Weekly)
            ("Bananas", "1 bunch", "", Some Frequency.Weekly)
            ("Tomatoes (cherry)", "1 pint", "Like the on-the-vine kind", Some Frequency.Weekly)
            ("Avocados", "3 firm and 2 ripe", "", Some Frequency.Weekly)
            ("Broccoli or broccolini", "", "", Some Frequency.Weekly)
            ("Ginger", "", "", Some Frequency.Every2Weeks)
            ("Lemongrass", "", "", None)
            ("Onions (Yellow)", "3", "", Some Frequency.Weekly)
            ("Butter (salted)", "", "Lurpak brand only", Some Frequency.Monthly)
            ("Feta cheese", "", "",None)
            ("Siggi's yogurt", "6 or so assorted flavors", "", Some Frequency.Every2Weeks)
            ("Milk (low-fat)", "1 half gallon", "", Some Frequency.Every2Weeks)
            ("Kombucha", "4 jars", "Assorted flavors", Some Frequency.Every3Weeks)
            ("Nancy's yogurt", "2 tubs", "", Some Frequency.Every3Weeks)
            ("Eggs", "2 dozen", "Free-range and happy birds please!", Some Frequency.Every2Weeks)
            ("Mochi", "3 pints, at least one vegan", "", Some Frequency.Weekly)
            ("Deli meat (turkey or roast beef)", "Couple packages", "", None)
            ("Salmon filet", "32 oz", "Only if never previously frozen", None)
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
    let today = DateTime.Now
    let postponeUntilRandom fqy =
        let rangeMax = fqy |> Frequency.asDays
        let rangeMin = -10
        match shouldPostponeRandom() with
        | false -> None
        | true -> 
            let daysFromToday = rangeMin + random.Next(rangeMin, rangeMax + 20)
            today.AddDays(daysFromToday |> float) |> Some
    let createStatus freq =
        match freq with
        | None -> Status.Active
        | Some freq ->              
            let postpone = freq |> postponeUntilRandom
            match postpone with
            | Some dt -> Status.Postponed dt
            | None -> Status.Active
    let createItem title qty note freq = 
        { Item.Id = Guid.NewGuid() |> ItemId
          Title = title |> Title
          Quantity = qty |> parseQuantity
          Repeat = freq
          Status = freq |> createStatus
          Note = note |> parseNote }
    itemsToParse
    |> Seq.map (fun (title, qty, note, freq) -> createItem title qty note freq)
    |> Seq.map (fun i -> if random.Next(0,10) = 0 then { i with Status = Status.Complete } else i)
    |> Seq.toList

let initialState = 
    { State.Stores = Map.empty
      State.ItemIsUnavailableInStore = Set.empty
      State.Items = 
        sampleItems 
        |> Seq.map (fun i -> (i.Id, i))
        |> Map.ofSeq
    }
