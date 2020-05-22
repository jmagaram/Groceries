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
            ("Chocolate bars", "5", "Very dark, Alter Eco preferred", Some Duration.W2)
            ("Dried cannellini or white beans", "Plenty", "", Some Duration.M1)
            ("Dried chickpeas", "", "Check the bulk section if no cans available",None)
            ("Flour - White All Purpose", "2 large bags", "" ,None)
            ("Flour - White Bread", "2 large bags", "" ,None)
            ("Gluten (for baking)", "", "" ,None)
            ("Chocolate chips", "", "Prefer Ghiradelli brand", Some Duration.M1)
            ("Peanut butter", "Several jars", "CB, Adams, or Santa Cruz", Some Duration.M1)
            ("Coconut milk (unsweetened)", "4 cans", "", None)
            ("Olive oil (medium quality)", "1 jar", "", Some Duration.M2)
            ("Fish sauce", "2 bottles", "Prefer a specialty brand rather than Thai Kitchen", Some Duration.M3)
            ("Mamma's lil' peppers", "1 jar", "", None)
            ("Heritage Flakes cereal", "2 bags", "", Some Duration.W3)
            ("Limes", "4", "", Some Duration.D3)
            ("Lemon", "3", "", Some Duration.D3)
            ("Oranges", "6", "Cara Cara preferred, maybe Sumo", Some Duration.D3)
            ("Apples", "8", "Like the semi-sweet varieties like Pink Lady and Envy. Avoid Granny Smith and Fuji.", Some Duration.D3)
            ("Blueberries", "2 large tubs", "Organic preferred", Some Duration.D3)
            ("Bananas", "1 bunch", "", Some Duration.D3)
            ("Tomatoes (cherry)", "1 pint", "Like the on-the-vine kind", Some Duration.D3)
            ("Avocados", "3 firm and 2 ripe", "", Some Duration.D3)
            ("Broccoli or broccolini", "", "", Some Duration.D3)
            ("Ginger", "", "", Some Duration.W2)
            ("Lemongrass", "", "", None)
            ("Onions (Yellow)", "3", "", Some Duration.D3)
            ("Butter (salted)", "", "Lurpak brand only", Some Duration.M1)
            ("Feta cheese", "", "",None)
            ("Siggi's yogurt", "6 or so assorted flavors", "", Some Duration.W2)
            ("Milk (low-fat)", "1 half gallon", "", Some Duration.W2)
            ("Kombucha", "4 jars", "Assorted flavors", Some Duration.W3)
            ("Nancy's yogurt", "2 tubs", "", Some Duration.W3)
            ("Eggs", "2 dozen", "Free-range and happy birds please!", Some Duration.W2)
            ("Mochi", "3 pints, at least one vegan", "", Some Duration.D3)
            ("Deli meat (turkey or roast beef)", "Couple packages", "", None)
            ("Salmon filet", "32 oz", "Only if never previously frozen", None)
        ]
    let parseQuantity qty = 
        match String.IsNullOrWhiteSpace(qty) with
        | true -> None
        | false -> qty |> Quantity |> Some
    let parseNote note = 
        match String.IsNullOrWhiteSpace(note) with
        | true -> None
        | false -> note |> Note |> Some
    let shouldPostponeRandom =
        "pppppp_"
        |> Seq.map (fun c -> c = 'p')
        |> randomFrom
    let today = DateTime.Now
    let postponeUntilRandom fqy =
        let rangeMax = fqy |> Duration.asDays
        let rangeMin = -10
        match shouldPostponeRandom() with
        | false -> None
        | true -> 
            let daysFromToday = rangeMin + random.Next(rangeMin, rangeMax + 20)
            today.AddDays(daysFromToday |> float) |> Some
    let createSchedule freq =
        match freq with
        | None -> Schedule.Incomplete
        | Some fq ->              
            { Repeat.Frequency = fq
              PostponedUntil = fq |> postponeUntilRandom }
            |> Schedule.Repeat
    let createItem title qty note freq = 
        { Item.Id = Guid.NewGuid() |> ItemId
          Title = title |> Title
          Quantity = qty |> parseQuantity
          Note = note |> parseNote
          Schedule = freq |> createSchedule }
    itemsToParse
    |> Seq.map (fun (title, qty, note, freq) -> createItem title qty note freq)
    |> Seq.toList
