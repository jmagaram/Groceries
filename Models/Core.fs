namespace Models

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices
open CoreTypes
open StringValidation
open ValidationTypes

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Etag =

    let tag (Etag e) = e

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemId =

    let create () = newGuid () |> ItemId

    let serialize i =
        match i with
        | ItemId g -> g |> Guid.serialize

    let deserialize s =
        s |> Guid.tryDeserialize |> Option.map ItemId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module UserId =

    let create () = newGuid () |> UserId

    let serialize i =
        match i with
        | UserId g -> g |> Guid.serialize

    let deserialize s =
        s |> Guid.tryDeserialize |> Option.map UserId

    let anonymous = Guid.Empty |> UserId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FamilyId =

    let create () = newGuid () |> FamilyId

    let serialize i =
        match i with
        | FamilyId g -> g |> Guid.serialize

    let deserialize s =
        s |> Guid.tryDeserialize |> Option.map FamilyId

    let anonymous = Guid.Empty |> FamilyId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FamilyName =

    let rules = singleLine 3<chars> 50<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator FamilyName List.head

    let asText (FamilyName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module EmailAddress =

    let normalizer (s: string) = s.Trim().ToLowerInvariant()

    let asText (EmailAddress s) = s

    // Based on, but not complete version of https://bit.ly/39OQGZC
    let tryParse email =
        match email with
        | null -> None
        | email ->
            let email = email |> normalizer

            try
                let pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
                let options = RegexOptions.IgnoreCase
                let timeout = TimeSpan.FromMilliseconds(250.0)

                match Regex.IsMatch(email, pattern, options, timeout) with
                | true -> Some(EmailAddress email)
                | false -> None
            with :? RegexMatchTimeoutException -> None

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemName =

    let rules = singleLine 3<chars> 50<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator ItemName List.head

    let asText (ItemName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Note =

    let rules = multipleLine 1<chars> 600<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Note List.head

    let asText (Note s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Quantity =

    let rules = singleLine 1<chars> 30<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator Quantity List.head

    let asText (Quantity s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let estimatePurchaseFrequency (purchases: DateTimeOffset seq) =
        let purchases =
            purchases
            |> Seq.distinctBy (fun i -> i.Date)
            |> Seq.sort
            |> Seq.takeAtMost 12
            |> List.ofSeq

        match purchases |> List.ofSeq with
        | [] -> TimeSpan.FromDays(14.0)
        | [ _ ] -> TimeSpan.FromDays(14.0)
        | xs ->

            let count = xs |> List.length
            let oldest = xs |> List.head
            let mostRecent = xs |> List.last
            let timePassed = mostRecent - oldest
            TimeSpan.FromDays(timePassed.TotalDays / (float (count - 1)))

    let removePostpone (i: Item) = { i with PostponeUntil = None }

    let postpone (now: DateTimeOffset) days (i: Item) =
        { i with
              PostponeUntil = now.AddDays(days |> float) |> Some }

    let postponeUsingPurchaseHistory (now: DateTimeOffset) purchaseHistory (i: Item) =
        { i with
              PostponeUntil =
                  now.Add(purchaseHistory |> estimatePurchaseFrequency)
                  |> Some }

    let postponeDaysAway (now: DateTimeOffset) (dt: DateTimeOffset) = 1<days> * ((dt - now).TotalDays |> int)

    let postponeDaysAwayOptional (now: DateTimeOffset) (postponedUntil: DateTimeOffset option) =
        postponedUntil
        |> Option.map (fun postponedUntil -> postponeDaysAway now postponedUntil)

    let commonPostponeChoices =
        [ 7; 14; 21; 30; 60; 90; 180 ]
        |> List.map (fun i -> i * 1<days>)

    type Message =
        | RemovePostpone
        | Postpone of int<days>
        | PostponeUsingPurchaseHistory of DateTimeOffset * DateTimeOffset seq
        | UpdateCategory of CategoryId
        | ClearCategory

    let update now msg i =
        match msg with
        | RemovePostpone -> i |> removePostpone
        | Postpone d -> i |> postpone now d
        | PostponeUsingPurchaseHistory (now, history) -> i |> postponeUsingPurchaseHistory now history
        | UpdateCategory id -> { i with CategoryId = Some id }
        | ClearCategory -> { i with CategoryId = None }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CategoryId =

    let create () = newGuid () |> CategoryId

    let serialize i =
        match i with
        | CategoryId g -> g |> Guid.serialize

    let deserialize s =
        s |> Guid.tryDeserialize |> Option.map CategoryId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module CategoryName =

    let rules = singleLine 3<chars> 30<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator CategoryName List.head

    let asText (CategoryName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreId =

    let create () = newGuid () |> StoreId

    let serialize i =
        match i with
        | StoreId g -> g |> Guid.serialize

    let deserialize s =
        s |> Guid.tryDeserialize |> Option.map StoreId

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module StoreName =

    let rules = singleLine 3<chars> 30<chars>
    let normalizer = String.trim

    let validator =
        rules |> StringValidation.createValidator

    let tryParse =
        StringValidation.createParser normalizer validator StoreName List.head

    let asText (StoreName s) = s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module NotSoldItem =

    let private separator = '|'

    let serialize (ns: NotSoldItem) =
        let storeId = ns.StoreId |> StoreId.serialize

        let itemId = ns.ItemId |> ItemId.serialize

        sprintf "%s%c%s" storeId separator itemId

    let deserialize (s: string) =
        result {
            if s |> String.isNullOrWhiteSpace then
                return!
                    "Could not deserialize an empty or null string to a NotSoldItem"
                    |> Error
            else
                let parts = s.Split(separator)

                match parts.Length with
                | 2 ->
                    let! storeId =
                        parts.[0]
                        |> StoreId.deserialize
                        |> Option.map Ok
                        |> Option.defaultValue (
                            sprintf "Could not deserialize the store ID in a NotSoldItem: %s" s
                            |> Error
                        )

                    let! itemId =
                        parts.[1]
                        |> ItemId.deserialize
                        |> Option.map Ok
                        |> Option.defaultValue (
                            sprintf "Could not deserialize the item ID in a NotSoldItem: %s" s
                            |> Error
                        )

                    return
                        { NotSoldItem.StoreId = storeId
                          NotSoldItem.ItemId = itemId }
                | _ ->
                    return!
                        s
                        |> sprintf "Attempting to deserialize a NotSoldItem that does not have exactly two parts: %s"
                        |> Error
        }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Purchase =

    let private separator = '|'

    let serialize (p: Purchase) =
        let itemId = p.ItemId |> ItemId.serialize

        let purchasedOn =
            p.PurchasedOn |> DateTimeOffset.serialize

        sprintf "%s%c%s" itemId separator purchasedOn

    let deserialize (s: string) =
        result {
            if s |> String.isNullOrWhiteSpace then
                return!
                    "Could not deserialize an empty or null string to a Purchase"
                    |> Error
            else
                let parts = s.Split(separator)

                match parts.Length with
                | 2 ->
                    let! itemId =
                        parts.[0]
                        |> ItemId.deserialize
                        |> Option.map Ok
                        |> Option.defaultValue (
                            sprintf "Could not deserialize the item ID in a Purchase: %s" s
                            |> Error
                        )

                    let! purchasedOn =
                        parts.[1]
                        |> DateTimeOffset.deserialize
                        |> Option.map Ok
                        |> Option.defaultValue (
                            sprintf "Could not deserialize the purchased on date in a Purchase: %s" s
                            |> Error
                        )

                    return
                        { Purchase.ItemId = itemId
                          Purchase.PurchasedOn = purchasedOn }
                | _ ->
                    return!
                        s
                        |> sprintf "Attempting to deserialize a Purchase that does not have exactly two parts: %s"
                        |> Error
        }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module TextBox =

    let create s = { ValueTyping = s; ValueCommitted = s }

    let typeText s t = { t with ValueTyping = s }

    let loseFocus normalize t =
        { t with
              ValueCommitted = normalize t.ValueTyping }

    let update normalize msg t =
        match msg with
        | TypeText s -> t |> typeText s
        | LoseFocus -> t |> loseFocus normalize

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SearchTerm =

    let rules =
        { MinLength = 1<chars>
          MaxLength = 20<chars>
          StringRules.OnlyContains =
              [ Letter
                Mark
                Number
                Punctuation
                Space
                Symbol ] }

    let normalizer = String.trim

    let length (SearchTerm t) = t.Length

    let validator =
        rules |> StringValidation.createValidator

    let tryParse s =
        s
        |> normalizer
        |> fun s ->
            match validator s |> Seq.toList with
            | [] -> Ok s
            | errors -> Error errors
        |> Result.mapError List.head
        |> Result.map SearchTerm

    let rec tryCoerce s =
        if s |> isNullOrWhiteSpace then
            None
        else
            match s |> normalizer |> tryParse with
            | Error IsRequired -> None
            | Error TooShort -> None
            | Ok t -> Some t
            | Error TooLong -> tryCoerce (s.Substring(0, rules.MaxLength |> int))
            | Error InvalidCharacters -> None // better to strip invalid chars

    let englishWordsToIgnore =
        [ "of"
          "to"
          "in"
          "as"
          "at"
          "by"
          "or"
          "on"
          "an"
          "the"
          "jar"
          "box"
          "can"
          "bag"
          "bunch"
          "pound"
          "ounce"
          "gram"
          "lots"
          "many"
          "whole" ]
        |> Set.ofSeq

    let splitOnSpace minWordLength wordsToIgnore (termsAsTyped: string) =
        let space = ' '

        if (termsAsTyped.Contains(space)) then
            termsAsTyped.Split(space, StringSplitOptions.RemoveEmptyEntries)
            |> Seq.map normalizer
            |> Seq.filter
                (fun s ->
                    s.Length >= minWordLength
                    && wordsToIgnore |> Set.contains s |> not)
            |> Seq.map tryCoerce
            |> Seq.choose id
        else
            match termsAsTyped |> tryCoerce with
            | None -> Seq.empty
            | Some t -> Seq.singleton t

    let value (SearchTerm s) = s

    let toRegexComponents (SearchTerm s) =
        let isRepeating s =
            let len = s |> String.length

            [ 1 .. len ]
            |> Seq.choose
                (fun i ->
                    let endsWith = s.Substring(len - i)
                    let n = len / i

                    match (endsWith |> String.replicate n) = s with
                    | true -> Some(endsWith, n)
                    | false -> None)
            |> Seq.filter (fun (_, i) -> i > 1)
            |> Seq.tryHead

        let edgeMiddleEdge s =
            let len = String.length s
            let maxEdgeLength = (len - 1) / 2

            seq { maxEdgeLength .. (-1) .. 1 }
            |> Seq.choose
                (fun i ->
                    let starts = s.Substring(0, i)
                    let ends = s.Substring(len - i)

                    if starts = ends then
                        let middle = s.Substring(i, len - i * 2)
                        Some {| Edge = starts; Middle = middle |}
                    else
                        None)
            |> Seq.tryHead

        let pattern =
            let s = s.ToLowerInvariant()
            let escape s = Regex.Escape(s)

            match s |> isRepeating with
            | Some (x, n) -> sprintf "(%s){%d,}" (escape x) n
            | None ->
                match s |> edgeMiddleEdge with
                | Some i -> sprintf "((%s)+(%s)*)+" (escape s) (escape (i.Middle + i.Edge))
                | None -> sprintf "(%s)+" (escape s)

        let options =
            RegexOptions.IgnoreCase
            ||| RegexOptions.CultureInvariant

        (pattern, options)

    let toRegex s =
        let (pattern, options) = s |> toRegexComponents
        new Regex(pattern, options)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ShoppingListSettings =

    let create =
        { ShoppingListSettings.StoreFilter = None
          PostponedViewHorizon = Some 5<days>
          TextFilter = TextBox.create ""
          IsTextFilterVisible = false }

    let storeFilterIsValid stores (s: ShoppingListSettings) =
        s.StoreFilter
        |> Option.map (fun f -> stores |> Seq.contains f)
        |> Option.defaultValue true

    let clearStoreFilter s = { s with StoreFilter = None }

    let setStoreFilter k s = { s with StoreFilter = Some k }

    let clearStoreFilterIf k s =
        if s.StoreFilter = Some k then
            s |> clearStoreFilter
        else
            s

    let private coerceSearchText s =
        s
        |> SearchTerm.tryCoerce
        |> Option.map SearchTerm.value
        |> Option.defaultValue ""

    let updateTextFilter msg s =
        { s with
              TextFilter =
                  s.TextFilter
                  |> TextBox.update coerceSearchText msg }

    let clearItemFilter s =
        s
        |> updateTextFilter (TextBoxMessage.TypeText "")
        |> updateTextFilter TextBoxMessage.LoseFocus

    let hidePostponedItems s = { s with PostponedViewHorizon = None }

    let setPostponedViewHorizon d s =
        let d = d |> min 365<days> |> max -365<days>
        { s with PostponedViewHorizon = Some d }

    let startSearch (s: ShoppingListSettings) =
        { s with
              IsTextFilterVisible = true
              TextFilter = TextBox.create "" }

    let endSearch (s: ShoppingListSettings) =
        { s with
              IsTextFilterVisible = false
              TextFilter = TextBox.create "" }

    type Message =
        | StartSearch
        | EndSearch
        | ClearStoreFilter
        | SetStoreFilterTo of StoreId
        | SetPostponedViewHorizon of int<days>
        | HidePostponedItems
        | TextFilter of TextBoxMessage
        | ClearItemFilter
        | Transaction of Message seq

    let rec update (msg: Message) s =
        match msg with
        | StartSearch -> s |> startSearch
        | EndSearch -> s |> endSearch
        | ClearStoreFilter -> s |> clearStoreFilter
        | SetStoreFilterTo k -> s |> setStoreFilter k
        | SetPostponedViewHorizon d -> s |> setPostponedViewHorizon d
        | HidePostponedItems -> s |> hidePostponedItems
        | ClearItemFilter -> s |> clearItemFilter
        | TextFilter msg -> s |> updateTextFilter msg
        | Transaction msgs -> msgs |> Seq.fold (fun res i -> res |> update i) s

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module UserSettings =

    type Message =
        | SetFontSize of FontSize
        | ShoppingListSettingsMessage of ShoppingListSettings.Message

    let create userId =
        { UserSettings.UserId = userId
          ShoppingListSettings = ShoppingListSettings.create
          FontSize = FontSize.NormalFontSize }

    let update (msg: Message) (s: UserSettings) =
        match msg with
        | SetFontSize f -> { s with FontSize = f }
        | ShoppingListSettingsMessage m ->
            s.ShoppingListSettings
            |> ShoppingListSettings.update m
            |> fun settings ->
                { s with
                      ShoppingListSettings = settings }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemForm =

    let itemNameValidation (f: ItemForm) =
        f.ItemName.ValueTyping |> ItemName.tryParse

    let updateItemName msg (f: ItemForm) =
        f.ItemName
        |> TextBox.update ItemName.normalizer msg
        |> fun n -> { f with ItemName = n }

    let quantityValidation (f: ItemForm) =
        f.Quantity.ValueTyping
        |> String.tryParseOptional Quantity.tryParse

    let updateQuantity msg (f: ItemForm) =
        f.Quantity
        |> TextBox.update Quantity.normalizer msg
        |> fun q -> { f with Quantity = q }

    let updateNote msg (f: ItemForm) =
        f.Note
        |> TextBox.update Note.normalizer msg
        |> fun n -> { f with Note = n }

    let noteValidation (f: ItemForm) =
        f.Note.ValueTyping
        |> String.tryParseOptional Note.tryParse

    let categoryModeChooseExisting f =
        { f with CategoryMode = ChooseExisting }

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
                  |> List.find (fun j -> j.CategoryId = CategoryId i)
                  |> Some }

    let categoryNameValidation (f: ItemForm) =
        f.NewCategoryName.ValueTyping
        |> CategoryName.tryParse

    let categoryNameChange s (f: ItemForm) =
        { f with
              NewCategoryName = f.NewCategoryName |> TextBox.typeText s }

    let categoryNameBlur (f: ItemForm) =
        let normalized =
            f.NewCategoryName
            |> TextBox.loseFocus CategoryName.normalizer

        let exists =
            f.CategoryChoiceList
            |> Seq.tryFind
                (fun i ->
                    String.Equals(
                        i.CategoryName |> CategoryName.asText,
                        normalized.ValueCommitted,
                        StringComparison.InvariantCultureIgnoreCase
                    ))

        match exists with
        | None -> { f with NewCategoryName = normalized }
        | Some c ->
            { f with
                  CategoryMode = ChooseExisting
                  CategoryChoice = Some c
                  NewCategoryName = TextBox.create "" }

    let categoryCommittedName (f: ItemForm) =
        match f.CategoryMode with
        | CategoryMode.ChooseExisting ->
            f.CategoryChoice
            |> Option.map (fun c -> c.CategoryName |> CategoryName.asText)
            |> Option.defaultValue ""
        | CategoryMode.CreateNew -> f.NewCategoryName.ValueCommitted

    let postponeSet v f = { f with Postpone = Some v }

    let postponeClear f = { f with Postpone = None }

    let toggleComplete (f: ItemForm) =
        match f.IsComplete with
        | false ->
            { f with
                  Postpone = Some 7<days>
                  IsComplete = true }
        | true ->
            { f with
                  Postpone = None
                  IsComplete = false }

    let postponeChoices (f: ItemForm) =
        f.Postpone
        :: (Item.commonPostponeChoices |> List.map Some)
        |> Seq.choose id
        |> Seq.distinct // coerce into bounds?
        |> Seq.sort
        |> List.ofSeq

    let storesSetAvailability id isSold f =
        { f with
              ItemForm.Stores =
                  f.Stores
                  |> List.map
                      (fun a ->
                          if a.Store.StoreId = id then
                              { a with IsSold = isSold }
                          else
                              a) }

    let storesSetAllAvailability isSoldAt (f: ItemForm) =
        f.Stores
        |> Seq.map
            (fun v ->
                { v with
                      IsSold = isSoldAt |> Set.contains (v.Store) })
        |> List.ofSeq
        |> fun availability -> { f with Stores = availability }

    let canDelete (f: ItemForm) = f.ItemId.IsSome

    let createNewItem itemName stores cats =
        { ItemId = None
          ItemName = TextBox.create itemName
          Etag = None
          Quantity = TextBox.create ""
          Note = TextBox.create ""
          IsComplete = false
          Postpone = None
          CategoryMode = CategoryMode.ChooseExisting
          NewCategoryName = TextBox.create ""
          CategoryChoice = None
          CategoryChoiceList =
              cats
              |> Seq.sortBy (fun (i: Category) -> i.CategoryName)
              |> List.ofSeq
          Stores =
              stores
              |> Seq.map
                  (fun i ->
                      { ItemAvailability.Store = i
                        ItemAvailability.IsSold = true })
              |> Seq.sortBy (fun i -> i.Store.StoreName)
              |> List.ofSeq }

    let editItem (now: DateTimeOffset) cats (i: ItemDenormalized) =
        { ItemId = Some i.ItemId
          ItemName = i.ItemName |> ItemName.asText |> TextBox.create
          Etag = i.Etag
          Quantity =
              i.Quantity
              |> Option.map Quantity.asText
              |> Option.defaultValue ""
              |> TextBox.create
          Note =
              i.Note
              |> Option.map Note.asText
              |> Option.defaultValue ""
              |> TextBox.create
          IsComplete = false
          Postpone =
              i.PostponeUntil
              |> Option.map (fun p -> Item.postponeDaysAway now p)
          CategoryMode = CategoryMode.ChooseExisting
          NewCategoryName = "" |> TextBox.create
          CategoryChoice = i.Category
          CategoryChoiceList =
              cats
              |> Seq.sortBy (fun (i: Category) -> i.CategoryName)
              |> List.ofSeq
          Stores =
              i.Availability
              |> Seq.sortBy (fun i -> i.Store.StoreName)
              |> List.ofSeq }

    let hasErrors f =
        (f |> itemNameValidation |> Result.isError)
        || (f |> quantityValidation |> Result.isError)
        || (f |> noteValidation |> Result.isError)
        || ((f |> categoryModeIsCreateNew)
            && (f |> categoryNameValidation |> Result.isError))

    type ItemFormResult =
        { Item: Item
          InsertCategory: Category option
          RecordPurchase: bool
          NotSold: StoreId list }

    let asItemFormResult (now: DateTimeOffset) (f: ItemForm) =
        let insertCategory =
            match f.CategoryMode with
            | ChooseExisting -> None
            | CreateNew ->
                f
                |> categoryNameValidation
                |> Result.okOrThrow
                |> fun c ->
                    Some
                        { Category.CategoryName = c
                          Category.CategoryId = CategoryId.create ()
                          Category.Etag = None }

        let item =
            { Item.ItemId =
                  f.ItemId
                  |> Option.defaultWith (fun () -> ItemId.create ())
              Item.ItemName = f |> itemNameValidation |> Result.okOrThrow
              Item.Etag = f.Etag
              Item.CategoryId =
                  match f.CategoryMode with
                  | ChooseExisting -> f.CategoryChoice
                  | CreateNew -> insertCategory
                  |> Option.map (fun i -> i.CategoryId)
              Item.Quantity = f |> quantityValidation |> Result.okOrThrow
              Item.Note = f |> noteValidation |> Result.okOrThrow
              Item.PostponeUntil =
                  f.Postpone
                  |> Option.map (fun d -> now.AddDays(d |> float)) }

        let notSold =
            f.Stores
            |> Seq.choose
                (fun i ->
                    if i.IsSold = false then
                        Some i.Store.StoreId
                    else
                        None)
            |> List.ofSeq

        { Item = item
          InsertCategory = insertCategory
          NotSold = notSold
          RecordPurchase = f.IsComplete }

    type Message =
        | ItemName of TextBoxMessage
        | Quantity of TextBoxMessage
        | Note of TextBoxMessage
        | PostponeSet of int<days>
        | PostponeClear
        | CategoryModeChooseExisting
        | CategoryModeCreateNew
        | ChooseCategoryUncategorized
        | ChooseCategory of Guid
        | NewCategoryName of TextBoxMessage
        | StoresSetAvailability of store: StoreId * isSold: bool
        | StoresSetAllAvailability of Set<Store>
        | ToggleComplete
        | Transaction of Message seq

    let rec update msg (f: ItemForm) =
        match msg with
        | ItemName m -> f |> updateItemName m
        | Quantity m -> f |> updateQuantity m
        | Note m -> f |> updateNote m
        | PostponeSet d -> f |> postponeSet d
        | PostponeClear -> f |> postponeClear
        | CategoryModeChooseExisting -> f |> categoryModeChooseExisting
        | CategoryModeCreateNew -> f |> categoryModeCreateNew
        | ChooseCategoryUncategorized -> f |> chooseCategoryUncategorized
        | ChooseCategory g -> f |> chooseCategory g
        | NewCategoryName (TextBoxMessage.TypeText s) -> f |> categoryNameChange s
        | NewCategoryName (TextBoxMessage.LoseFocus) -> f |> categoryNameBlur
        | StoresSetAvailability (id: StoreId, isSold: bool) -> f |> storesSetAvailability id isSold
        | StoresSetAllAvailability isSoldAt -> f |> storesSetAllAvailability isSoldAt
        | ToggleComplete -> f |> toggleComplete
        | Message.Transaction msgs -> msgs |> Seq.fold (fun f m -> update m f) f
