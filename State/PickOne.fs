module PickOne
open DomainTypes

let choices (p:PickOne<'T>) = p.Choices

let selectedItem (p:PickOne<'T>) = p.SelectedItem

let create i =
    let s = i |> Set.ofSeq
    let h = s |> Seq.head // may throw
    { PickOne.Choices = s
      SelectedItem = h }

let selectIf f p =
    match p |> choices |> Seq.filter f |> Seq.zeroOrOne with
    | None -> p
    | Some i -> { p with SelectedItem = i }

let select i p =
    match p |> choices |> Set.contains i with
    | false -> failwith "That item does not exist in the set of possible choices."
    | true -> { p with SelectedItem = i }

let update msg p =
    match msg with
    | PickOneByItem i -> p |> select i
    | PickOneByPredicate f -> p |> selectIf f