namespace Models
open System
open System.Reactive.Linq
open System.Text.RegularExpressions
open FSharp.Control.Reactive
open ViewTypes

module SearchTerm =
    open ValidationTypes
    open StringValidation

    let rules =
        { MinLength = 1<chars>
          MaxLength = 20<chars>
          StringRules.OnlyContains = [ Letter; Mark; Number; Punctuation; Space; Symbol ] }

    let value (SearchTerm s) = s

    let tryParse = parser SearchTerm rules

    let toRegex (SearchTerm s) =
        let isRepeating s =
            let len = s |> String.length

            [ 1 .. len ]
            |> Seq.choose (fun i ->
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
            |> Seq.choose (fun i ->
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

        let options = RegexOptions.IgnoreCase ||| RegexOptions.CultureInvariant
        new Regex(pattern, options)

module TextSpan =

    let private create f s =
        if s = "" then failwith "The text span is empty." else { Format = f; Text = s }

    let normal = create Normal

    let highlight = create Highlight

module FormattedText =

    let spans (FormattedText ft) = ft

    let fromList spans = FormattedText spans

    let normal s = s |> TextSpan.normal |> List.singleton |> fromList

module Highlighter =

    let create: Highlighter =
        fun searchTerm ->
            let regex = searchTerm |> SearchTerm.toRegex

            fun s ->
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

                            if i < ms.Count - 1
                            then yield TextSpan.normal (s.Substring(regStart, ms.[i + 1].Index - regStart))
                            elif regStart < s.Length
                            then yield TextSpan.normal (s.Substring(regStart))








                    }
                    |> List.ofSeq
                    |> FormattedText.fromList

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let createItemCategory (c: StateTypes.CategoryId) (s: StateTypes.State) =
        let c = s.Categories |> DataTable.findCurrent c

        { CategoryId = c.CategoryId
          CategoryName =
              c.CategoryName
              |> CategoryName.asText
              |> FormattedText.normal }

    let fromItem (s: StateTypes.State) (item: StateTypes.Item) =
        { ItemId = item.ItemId
          ItemName = item.ItemName |> ItemName.asText |> FormattedText.normal
          Note =
              item.Note
              |> Option.map (fun n -> n |> Note.asText |> FormattedText.normal)
          Quantity =
              item.Quantity
              |> Option.map (fun q -> q |> Quantity.asText |> FormattedText.normal)
          Category =
              item.CategoryId
              |> Option.map (fun categoryId -> s.Categories |> DataTable.findCurrent categoryId)
              |> Option.map (fun c -> createItemCategory c.CategoryId s)
          Schedule = item.Schedule
          NotSoldAt =
              s.NotSoldItems
              |> DataTable.current
              |> Seq.choose (fun i ->
                  if i.ItemId = item.ItemId then s.Stores |> DataTable.findCurrent i.StoreId |> Some else None)
              |> Seq.map (fun i ->
                  { StoreId = i.StoreId
                    StoreName = i.StoreName |> StoreName.asText |> FormattedText.normal })
              |> List.ofSeq }

    let tryCreate (itemId: StateTypes.ItemId) (s: StateTypes.State) =
        s
        |> State.items
        |> DataTable.tryFindCurrent itemId
        |> Option.map (fromItem s)

    let create itemId s = tryCreate itemId s |> Option.get

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Store =

    let private storeItem (itemId: StateTypes.ItemId) (s: StateTypes.State) =
        let item = s.Items |> DataTable.findCurrent itemId

        { ItemId = item.ItemId
          ItemName = item.ItemName |> ItemName.asText |> FormattedText.normal
          Note =
              item.Note
              |> Option.map (fun n -> n |> Note.asText |> FormattedText.normal)
          Quantity =
              item.Quantity
              |> Option.map (fun q -> q |> Quantity.asText |> FormattedText.normal)
          Category =
              item.CategoryId
              |> Option.map (fun c -> Item.createItemCategory c s)
          Schedule = item.Schedule }

    let fromStore (s: StateTypes.State) (store: StateTypes.Store) =
        { StoreId = store.StoreId
          StoreName = store.StoreName |> StoreName.asText |> FormattedText.normal
          NotSoldItems =
              s.NotSoldItems
              |> DataTable.current
              |> Seq.choose (fun ns -> if ns.StoreId <> store.StoreId then None else Some(storeItem ns.ItemId s))
              |> List.ofSeq }

    let tryCreate (id: StateTypes.StoreId) (s: StateTypes.State) =
        s.Stores
        |> DataTable.tryFindCurrent id
        |> Option.map (fromStore s)

    let create (id: StateTypes.StoreId) (s: StateTypes.State) = tryCreate id s |> Option.get

    let allFromObservable (s: IObservable<Models.StateTypes.State>) =
        s
        |> Observable.map (fun s ->
            s.Stores
            |> DataTable.current
            |> Seq.map (fun store -> fromStore s store)
            |> List.ofSeq)

module Category =

    let create catId s =
        let cat = s |> State.categories |> DataTable.findCurrent catId

        { CategoryId = cat.CategoryId
          CategoryName =
              cat.CategoryName
              |> CategoryName.asText
              |> FormattedText.normal
          Items =
              s.Items
              |> DataTable.current
              |> Seq.choose (fun itm ->
                  if itm.CategoryId <> (Some catId) then
                      None
                  else
                      Some
                          { ItemId = itm.ItemId
                            ItemName = itm.ItemName |> ItemName.asText |> FormattedText.normal
                            Note =
                                itm.Note
                                |> Option.map (fun n -> n |> Note.asText |> FormattedText.normal)
                            Quantity =
                                itm.Quantity
                                |> Option.map (fun q -> q |> Quantity.asText |> FormattedText.normal)
                            Schedule = itm.Schedule
                            NotSoldAt =
                                s.NotSoldItems
                                |> DataTable.current
                                |> Seq.choose (fun ns ->
                                    if ns.ItemId = itm.ItemId then
                                        s.Stores |> DataTable.findCurrent ns.StoreId |> Some
                                    else
                                        None)
                                |> Seq.map (fun st ->
                                    { StoreId = st.StoreId
                                      StoreName = st.StoreName |> StoreName.asText |> FormattedText.normal })
                                |> List.ofSeq })
              |> List.ofSeq }



// not sure this attribute is needed
// https://github.com/fsharp/fslang-design/blob/master/FSharp-4.1/FS-1019-implicitly-add-the-module-suffix.md
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ShoppingList =

    let create (s: StateTypes.State) =
        { ShoppingList.Items =
              s.Items
              |> DataTable.current
              |> Seq.map (fun i -> s |> Item.create i.ItemId)
              |> Seq.sortBy (fun i -> (i.Category, i.ItemName))
              |> Seq.filter (fun i ->
                  s.ShoppingListViewOptions.StoreFilter
                  |> Option.map (fun storeId -> i.NotSoldAt |> Seq.forall (fun ns -> ns.StoreId <> storeId))
                  |> Option.defaultValue (true))
              |> List.ofSeq }

    // Could make this smarter to only recalculate when specific parts of the
    // State change. This logic should be in this assembly not the client.
    let fromObservable (s: IObservable<Models.StateTypes.State>) =
        s
        |> Observable.map create
        |> Observable.distinctUntilChanged
