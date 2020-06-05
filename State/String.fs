[<AutoOpen>]
module String

let isNullOrWhiteSpace s = System.String.IsNullOrWhiteSpace(s)

let trim (s:string) = 
    match s |> isNullOrWhiteSpace with
    | true -> ""
    | false -> s.Trim()

let rec find (query:string) (source:string)  =
    seq {
        if source <> "" then
            let index = source.IndexOf(query, System.StringComparison.CurrentCultureIgnoreCase)
            if (index = -1) || query = "" then
                yield (source, false)
            else
                if (index = 0) then
                    let matchAtBeginning = source.Substring(0, query.Length)
                    yield (matchAtBeginning, true)
                    let remain = source.Substring(query.Length)
                    yield! find query remain
                else
                    let beforeMatch = source.Substring(0, index)
                    let matchInMiddle = source.Substring(index, query.Length)
                    let afterMatch = source.Substring(index + query.Length)
                    yield (beforeMatch, false)
                    yield (matchInMiddle, true)
                    yield! find query afterMatch 
    }

let tryParseWith (tryParseFunc: string -> bool * _) = tryParseFunc >> function
    | true, v    -> Some v
    | false, _   -> None

let tryParseDate = tryParseWith System.DateTime.TryParse

let tryParseInt = tryParseWith System.Int32.TryParse

let tryParseGuid = tryParseWith System.Guid.TryParse