module String

let rec find (query:string) (source:string)  =
    seq {
        if source <> "" then
            let index = source.IndexOf(query)
            if (index = -1) then
                yield (source, false)
            else
                if (index = 0) then
                    yield (query, true)
                    let remain = source.Substring(query.Length)
                    yield! find query remain
                else
                    let beforeMatch = source.Substring(0, index)
                    let afterMatch = source.Substring(index + query.Length)
                    yield (beforeMatch, false)
                    yield (query, true)
                    yield! find query afterMatch 
    }