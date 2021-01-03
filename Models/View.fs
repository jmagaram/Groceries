namespace Models

open System
open System.Text.RegularExpressions
open ViewTypes
open CoreTypes

module TextSpan =

    let private create f s =
        if s = "" then failwith "The text span is empty." else { Format = f; Text = s }

    let normal = create Normal

    let highlight = create Highlight

    let concatenate (s1: TextSpan) (s2: TextSpan) =
        if s1.Format = s2.Format then
            { TextSpan.Format = s1.Format
              TextSpan.Text = $"{s1.Text}{s2.Text}" }
            |> Some
        else
            None

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module FormattedText =

    let spans (FormattedText ft) = ft

    let private consolidate (source: TextSpan seq) =
        seq {
            use en = source.GetEnumerator()
            let mutable total = None

            while en.MoveNext() do
                match total with
                | None -> total <- Some en.Current
                | Some s ->
                    match TextSpan.concatenate s en.Current with
                    | None ->
                        yield s
                        total <- en.Current |> Some
                    | Some s -> total <- s |> Some

            match total with
            | Some s -> yield s
            | None -> ()
        }

    let fromSpans spans =
        spans
        |> consolidate
        |> List.ofSeq
        |> FormattedText

    let normal s =
        s
        |> TextSpan.normal
        |> List.singleton
        |> fromSpans

    let hasHighlight ft =
        ft
        |> spans
        |> Seq.exists
            (fun i ->
                match i.Format with
                | TextFormat.Highlight -> true
                | _ -> false)

    let asString s =
        String.Join("", s |> spans |> Seq.map (fun i -> i.Text))

module Highlighter =

    let private fromRegex (regex: Regex) s =
        match s |> String.length with
        | 0 -> [] |> FormattedText
        | _ ->
            seq {
                let ms = regex.Matches(s)

                if ms.Count = 0 then yield (TextSpan.normal s)
                elif ms.[0].Index > 0 then yield TextSpan.normal (s.Substring(0, ms.[0].Index))

                for i in 0 .. ms.Count - 1 do
                    yield TextSpan.highlight ms.[i].Value
                    let regStart = ms.[i].Index + ms.[i].Length

                    if i < ms.Count - 1 then
                        let len = ms.[i + 1].Index - regStart

                        if len > 0
                        then yield TextSpan.normal (s.Substring(regStart, len))
                    elif regStart < s.Length then
                        yield TextSpan.normal (s.Substring(regStart))

            }
            |> List.ofSeq
            |> FormattedText.fromSpans

    let find searchTerm =
        searchTerm |> SearchTerm.toRegex |> fromRegex


    let findAny searchTerms =
        let highlighters =
            searchTerms
            |> Seq.map SearchTerm.toRegexComponents
            |> Seq.map (fun (r, o) -> new Regex(r, o) |> fromRegex)
            |> List.ofSeq

        let highlightedIndexes (f: FormattedText) =
            f
            |> FormattedText.spans
            |> Seq.collect (fun s -> Seq.replicate s.Text.Length s.Format)
            |> Seq.indexed
            |> Seq.choose (fun (i, j) -> if j = TextFormat.Highlight then Some i else None)
            |> Set.ofSeq

        fun (s: String) ->
            let highlightedCharIndexes =
                highlighters
                |> Seq.map (fun findOneTerm -> findOneTerm s)
                |> Seq.map highlightedIndexes
                |> Seq.fold Set.union Set.empty

            s
            |> Seq.indexed
            |> Seq.map
                (fun (i, j) ->
                    match highlightedCharIndexes |> Set.contains i with
                    | true -> TextSpan.highlight (j.ToString())
                    | false -> TextSpan.normal (j.ToString()))
            |> FormattedText.fromSpans

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetString =

    let private normalizeItems normalize items =
        items
        |> Seq.filter (String.isNullOrWhiteSpace >> not)
        |> Seq.map normalize
        |> Seq.distinctBy (fun (i: string) -> i.ToLowerInvariant())

    let fromItems normalize delimeter items =
        items
        |> normalizeItems normalize
        |> fun items -> String.Join(delimeter, items)

    let fromString normalize (splitOn: string []) delimeter (s: string) =
        s.Split(separator = splitOn, options = StringSplitOptions.RemoveEmptyEntries)
        |> fromItems normalize delimeter

    let toItems normalize (splitOn: string []) (s: String) =
        s.Split(splitOn, StringSplitOptions.RemoveEmptyEntries)
        |> normalizeItems normalize

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module TimeSpanEstimate =

    let fromTimeSpan (ts: TimeSpan) =
        let (ts, neg) =
            if ts < TimeSpan.Zero then (-ts, -1.0) else (ts, 1.0)

        let cutoff = 0.8
        let days = ts.TotalDays
        let months = ts.TotalDays / 30.0
        let weeks = ts.TotalDays / 7.0

        if months > cutoff
        then Convert.ToInt32(months * neg) |> int |> Months
        elif (weeks > cutoff)
        then Convert.ToInt32(weeks * neg) |> int |> Weeks
        else Convert.ToInt32(days * neg) |> int |> Days

    let fromDays d = TimeSpan.FromDays(d |> float) |> fromTimeSpan

    let between (a: DateTimeOffset) (b: DateTimeOffset) = (b - a) |> fromTimeSpan

    let toTimeSpan (ts: TimeSpanEstimate) =
        match ts with
        | Days d -> TimeSpan.FromDays(float d)
        | Weeks w -> TimeSpan.FromDays((float w) * 7.0)
        | Months m -> TimeSpan.FromDays((float m) * 30.0)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SelectZeroOrOne =

    let create choice items =
        { Choices = items |> Set.ofSeq
          CurrentChoice = choice
          OriginalChoice = choice }

    let select i z = 
        { z with CurrentChoice = Some i }

    let selectNone z = 
        { z with CurrentChoice = None }

    let hasChanges z = z.OriginalChoice <> z.CurrentChoice

module SelectZeroOrOneCategory =

    let createFromPickList current choices =
        SelectZeroOrOne.create current choices

    let create current state =
        createFromPickList current (state |> State.categories)

    let asStateMessage item (s:SelectZeroOrOne<CoreTypes.Category>) =
        match s.CurrentChoice with
        | None -> StateTypes.ModifyItem (item, Item.Message.ClearCategory)
        | Some c -> StateTypes.ModifyItem (item, Item.Message.UpdateCategory c.CategoryId)
        |> StateTypes.ItemMessage

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SelectMany =

    let create items =
        { Items = items |> Set.ofSeq
          SelectedOriginal = Set.empty
          Selected = Set.empty }

    let assertItemInSet i s =
        if s.Items |> Set.contains i |> not then failwith "The set does not contain that item."

    let withOriginalSelection items s =
        items
        |> Seq.fold
            (fun t i ->
                assertItemInSet i t

                { t with
                      SelectedOriginal = t.SelectedOriginal |> Set.add i
                      Selected = t.Selected |> Set.add i })
            s

    let select item s =
        s |> assertItemInSet item
        { s with Selected = s.Selected |> Set.add item }

    let selectMany items s = items |> Seq.fold (fun t i -> t |> select i) s

    let selectAll s = s |> selectMany s.Items

    let deselect item s =
        s |> assertItemInSet item
        { s with Selected = s.Selected |> Set.remove item }

    let deselectAll s = { s with Selected = Set.empty }

    let isSelected i s =
        s |> assertItemInSet i
        s.Selected |> Set.contains i

    let toggleSelected i s =
        match s |> isSelected i with
        | true -> s |> deselect i
        | false -> s |> select i

    let added s = Set.difference s.Selected s.SelectedOriginal

    let removed s = Set.difference s.SelectedOriginal s.Selected

    let hasChanges s =
        (s |> added |> Set.isEmpty |> not)
        || (s |> removed |> Set.isEmpty |> not)

    let selectionSummary s =
        s.Items
        |> Seq.map (fun i -> {| Item = i; IsSelected = s.Selected |> Set.contains i |})

    let allSelected s = s.Selected.Count = s.Items.Count

    let revertToOriginalSelection s = { s with Selected = s.SelectedOriginal }

module SelectManyStores =

    open CoreTypes
    open SelectMany

    let createFromAvailability availability =
        availability
        |> Seq.map (fun j -> j.Store)
        |> create
        |> withOriginalSelection (
            availability
            |> Seq.choose (fun j -> if j.IsSold then Some j.Store else None)
        )

    let create itemId state =
        let itemName =
            state
            |> State.tryFindItem itemId
            |> Option.get
            |> fun i -> i.ItemName

        let model =
            state
            |> State.stores
            |> Seq.map
                (fun s ->
                    { ItemAvailability.Store = s
                      IsSold = state |> State.storeSellsItemById itemId s.StoreId })
            |> createFromAvailability

        (itemName, model)

    let asItemAvailability s =
        s
        |> SelectMany.selectionSummary
        |> Seq.map
            (fun i ->
                { ItemAvailability.Store = i.Item
                  IsSold = i.IsSelected })

    let asItemAvailabilityMessage itemId s =
        (itemId, s |> asItemAvailability)
        |> StateTypes.StateMessage.ItemAvailabilityMessage

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ItemQuickActionsView =

    let private create itemId activeStore state =
        let item =
            state
            |> State.itemsTable
            |> DataTable.findCurrent itemId

        let shoppingAtAndSellsItem =
            activeStore
            |> Option.bind (fun s -> state |> State.tryFindStore s)
            |> Option.filter (fun s -> state |> State.storeSellsItemById itemId s.StoreId)

        let storesExist =
            state |> State.stores |> Seq.isEmpty |> not

        { ItemId = item.ItemId
          ItemName = item.ItemName
          PostponeUntil = item.PostponeUntil
          PermitQuickNotSoldAt = shoppingAtAndSellsItem
          PermitStoresCustomization = storesExist }

    let createNoActiveStore itemId state = create itemId None state
    let createWithActiveStore itemId storeId state = create itemId (Some storeId) state

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetBulkEditForm =

    let create normalizer delimeter items =
        { Original = items |> Seq.map normalizer |> Set.ofSeq
          Proposed =
              items
              |> SetString.fromItems normalizer delimeter
              |> TextBox.create }

    let propose normalizer delimiter ps b =
        { b with
              SetBulkEditForm.Proposed =
                  System.String.Join(delimiter, ps |> Seq.map normalizer)
                  |> TextBox.create }

    let update normalize splitOn delimiter msg b =
        let normalize = SetString.fromString normalize splitOn delimiter

        { b with
              SetBulkEditForm.Proposed = b.Proposed |> TextBox.update normalize msg }

    let items normalize splitOn (b: SetBulkEditForm) =
        b.Proposed.ValueTyping
        |> SetString.toItems normalize splitOn

    let validationResults tryParse items =
        items
        |> Seq.map (fun i ->
            i
            |> tryParse
            |> Result.mapError (fun e -> {| Proposed = i; Error = e |}))

    let tryFindIgnoreCase s items =
        items
        |> Seq.tryFind (fun i -> i |> String.equalsInvariantCultureIgnoreCase s)

    let summary normalize splitOn tryParse b =
        let items = b |> items normalize splitOn
        let validationResults = items |> validationResults tryParse |> List.ofSeq

        match validationResults |> Seq.forall Result.isOk with
        | false -> None
        | true ->
            let goal = validationResults |> Seq.choose Result.asOption |> Set.ofSeq
            let original = b.Original |> Set.ofSeq
            let unchanged = Set.intersect original goal
            let deleted = original - goal
            let created = goal - original

            let moveOrDelete =
                deleted
                |> Seq.fold (fun total source ->
                    let target = goal |> tryFindIgnoreCase source
                    total |> Map.add source target) Map.empty

            { Create = created
              Unchanged = unchanged
              MoveOrDelete = moveOrDelete
              Proposed = items |> List.ofSeq }
            |> Some

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetMapChangesForm =

    let targets s = s.Unchanged |> Seq.append s.Create |> Seq.sort

    let original s =
        s.Unchanged
        |> Seq.append (s.MoveOrDelete |> Map.keys)
        |> Seq.sort

    let private edit x y s =
        if s.MoveOrDelete |> Map.containsKey x |> not then failwith "Can not move or delete that item."

        if y
           |> Option.map (fun y -> s |> targets |> Seq.contains y |> not)
           |> Option.defaultValue false then
            failwith "That is not a valid target for moving an item."

        { s with MoveOrDelete = s.MoveOrDelete.Add(x, y) }

    let delete x s = s |> edit x None

    let move x y s = s |> edit x (Some y)

    let hasChanges s =
        (s.Create |> Seq.isEmpty |> not)
        || (s.MoveOrDelete |> Map.isEmpty |> not)

    let bulkEdit normalizer delimeter s =
        SetBulkEditForm.create normalizer delimeter (original s)
        |> SetBulkEditForm.propose normalizer delimeter s.Proposed

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module SetEditWizardForm =

    let bulkEdit f =
        match f with
        | BulkEditMode b -> Some b
        | _ -> None

    let summary f =
        match f with
        | SetMapChangesMode s -> Some s
        | _ -> None

    let update normalize splitOn tryParse delimiter msg f =
        match msg with
        | BulkTextBoxMessage msg ->
            f
            |> bulkEdit
            |> Option.get
            |> fun b ->
                b
                |> SetBulkEditForm.update normalize splitOn delimiter msg
                |> BulkEditMode
        | GoToSummary ->
            f
            |> bulkEdit
            |> Option.get
            |> SetBulkEditForm.summary normalize splitOn tryParse
            |> Option.get
            |> SetMapChangesMode
        | GoBackToBulkEdit ->
            f
            |> summary
            |> Option.get
            |> SetMapChangesForm.bulkEdit normalize delimiter
            |> BulkEditMode
        | MoveRename (x, y) ->
            f
            |> summary
            |> Option.get
            |> SetMapChangesForm.move x y
            |> SetMapChangesMode
        | Delete x ->
            f
            |> summary
            |> Option.get
            |> SetMapChangesForm.delete x
            |> SetMapChangesMode


