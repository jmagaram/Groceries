[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Models.SelectZeroOrOne

type SelectZeroOrOne<'T when 'T: comparison> =
    { Choices: Set<'T>
      OriginalChoice: 'T option
      CurrentChoice: 'T option }

let create choice items =
    { Choices = items |> Set.ofSeq
      CurrentChoice = choice
      OriginalChoice = choice }

let select i z = 
    { z with CurrentChoice = Some i }

let selectNone z = 
    { z with CurrentChoice = None }

let hasChanges z = z.OriginalChoice <> z.CurrentChoice

type SelectZeroOrOne<'T when 'T:comparison> with
     member me.HasChanges = me |> hasChanges
     member me.SelectNone() = me |> selectNone
     member me.Select(item) = me |> select item


