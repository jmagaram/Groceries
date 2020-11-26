namespace Models

type SelectMany<'T when 'T: comparison> =
    { Items: Set<'T>
      SelectedOriginal: Set<'T>
      Selected: Set<'T> }

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

open SelectMany

type SelectMany<'T when 'T: comparison> with
    member me.HasChanges = me |> hasChanges
    member me.SelectionSummary = me |> selectionSummary
    member me.Toggle(i) = me |> toggleSelected i
    member me.SelectAll() = me |> selectAll
    member me.AllSelected = me |> allSelected
    member me.RevertToOriginalSelection() = me |> revertToOriginalSelection