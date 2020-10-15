module Models.Experiment

open System

type Item<'T, 'U> =
    | ReadOnly of 'T
    | Edit of 'T * 'U

type ListView<'T, 'U> = ListView of Item<'T, 'U> list

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Item =

    let startEdit f x =
        match x with
        | ReadOnly t -> Edit(t, (f t))
        | Edit _ -> failwith "Can not start editing an item that is already being edited."

    let cancelEdit x =
        match x with
        | ReadOnly t -> failwith "Can not cancel editing an item that is not being edited."
        | Edit (t, _) -> ReadOnly t

    let tryCommit f x =
        match x with
        | ReadOnly _ -> failwith "Can not commit editing an item that is not being edited."
        | Edit (t, u) -> f u |> Option.map ReadOnly

    let isEditing x =
        match x with
        | Edit _ -> true
        | _ -> false

    let isReadOnly x =
        match x with
        | ReadOnly _ -> true
        | _ -> false

    let keyOf f x =
        match x with
        | ReadOnly t -> f t
        | Edit (t, _) -> f t

    let getReadOnly x =
        match x with
        | ReadOnly t -> t
        | _ -> null

    let getEdit x =
        match x with
        | ReadOnly _ -> null
        | Edit (_, x) -> x

    let mapEdit f x =
        match x with
        | ReadOnly _ -> x
        | Edit (i, j) -> Edit(i, f j)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ListView =

    let init items = items |> Seq.map ReadOnly |> List.ofSeq |> ListView

    let items lst =
        match lst with
        | ListView xs -> xs

    let fromItems i = ListView(i |> List.ofSeq)

    let isEditing lst = lst |> items |> Seq.exists Item.isEditing

    let startEdit f keyOf k lst =
        match lst |> isEditing with
        | true -> failwith "Can not start editing before a previous edit has been committed or canceled."
        | false ->
            let items =
                lst
                |> items
                |> Seq.map (fun i -> if (Item.keyOf keyOf i) = k then i |> (Item.startEdit f) else i)
                |> List.ofSeq

            let editCount = items |> Seq.filter Item.isEditing |> Seq.length

            match editCount with
            | 1 -> items |> fromItems
            | 0 -> failwith "Could not find an item with that key."
            | _ -> failwith "Attempting to edit more than one item at a time."

    let cancelEdit lst =
        lst
        |> items
        |> Seq.map (fun i -> if i |> Item.isEditing then i |> Item.cancelEdit else i)
        |> fromItems

    let tryCommit f lst =
        let result =
            lst
            |> items
            |> Seq.map (fun i -> if i |> Item.isEditing then i |> Item.tryCommit f else Some i)

        if result |> Seq.contains None then None else result |> Seq.map Option.get |> fromItems |> Some

    let insert i j v =
        match v |> isEditing with
        | true -> failwith "Can not insert a new item while editing another one."
        | false -> Edit(i, j) :: (v |> items) |> fromItems

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module GroceryItemsView =

    type GroceryItem = { Id: Guid; Name: string; Note: string; IsCompleted: bool }

    type GroceryItemEditing = { Id: Guid; Name: string; Note: string; IsCompleted: bool }

    type GroceryItemView = GroceryItemView of ListView<GroceryItem, GroceryItemEditing>

    let keyOf (g: GroceryItem) = g.Id

    let startEditCore (g: GroceryItem) =
        { GroceryItemEditing.Id = g.Id
          GroceryItemEditing.Name = g.Name
          GroceryItemEditing.Note = g.Note
          GroceryItemEditing.IsCompleted = g.IsCompleted }

    let tryCommitCore (g: GroceryItemEditing) =
        if g.Name |> String.isNullOrWhiteSpace then
            None
        else
            { GroceryItem.Id = g.Id
              GroceryItem.Name = g.Name
              GroceryItem.Note = g.Note
              GroceryItem.IsCompleted = g.IsCompleted }
            |> Some

    let txt (f: Item<GroceryItem, GroceryItemEditing>) =
        match f with
        | ReadOnly g -> g.Name
        | Edit (h, i) -> i.Name

    let init () =
        [ { GroceryItem.Name = "Apples"
            Note = "Prefer Envy"
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Bananas"
            Note = ""
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Oranges"
            Note = "Sumo are best!"
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Lettuce"
            Note = ""
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Endive"
            Note = ""
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Celery"
            Note = ""
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Tomatoes"
            Note = "Sumo are best!"
            IsCompleted = false
            GroceryItem.Id = newGuid () }
          { GroceryItem.Name = "Carrots"
            Note = ""
            IsCompleted = false
            GroceryItem.Id = newGuid () } ]
        |> ListView.init
        |> GroceryItemView

    let asListView (GroceryItemView x) = x

    let startEdit k x =
        x
        |> asListView
        |> ListView.startEdit startEditCore keyOf k
        |> GroceryItemView

    let tryCommit x =
        x
        |> asListView
        |> ListView.tryCommit tryCommitCore
        |> Option.map ListView.items
        |> Option.map (List.sortBy txt)
        |> Option.map (ListView.fromItems)
        |> Option.map GroceryItemView

    let cancelEdit x = x |> asListView |> ListView.cancelEdit |> GroceryItemView

    let insertNew x =
        match x |> tryCommit with
        | None -> failwith "Could not commit an existing edit!"
        | Some x ->
            let i =
                { GroceryItem.Name = ""
                  Note = ""
                  IsCompleted = false
                  GroceryItem.Id = newGuid () }

            let j = i |> startEditCore
            x |> asListView |> ListView.insert i j |> GroceryItemView

    let updateName t x =
        x
        |> asListView
        |> ListView.items
        |> List.map (Item.mapEdit (fun i -> { i with Name = t }))
        |> ListView.fromItems
        |> GroceryItemView

    let updateNote t x =
        x
        |> asListView
        |> ListView.items
        |> List.map (Item.mapEdit (fun i -> { i with Note = t }))
        |> ListView.fromItems
        |> GroceryItemView

    let toggleIsCompleted x =
        x
        |> asListView
        |> ListView.items
        |> List.map (Item.mapEdit (fun i -> { i with IsCompleted = not i.IsCompleted }))
        |> ListView.fromItems
        |> GroceryItemView

    type Message =
        | StartEdit of Guid
        | CancelEdit
        | CommitEdit
        | InsertNew
        | UpdateName of string
        | UpdateNote of string
        | ToggleCompleted

    let processMessage m v =
        match m with
        | StartEdit k -> v |> startEdit k
        | CancelEdit -> v |> cancelEdit
        | CommitEdit -> v |> tryCommit |> Option.get
        | InsertNew -> v |> insertNew
        | UpdateName t -> v |> updateName t
        | UpdateNote t -> v |> updateNote t
        | ToggleCompleted -> v |> toggleIsCompleted

    type GroceryItemView with
        member x.CanCommit = x |> tryCommit |> Option.isSome
        member x.Commit = x |> tryCommit |> Option.get
        member x.Items = x |> asListView |> ListView.items
        member x.IsEditing = x |> asListView |> ListView.isEditing
        member x.ProcessMessage(m) = x |> processMessage m
