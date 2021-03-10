#r "nuget: Microsoft.Diagnostics.Runtime"

open System
open System.Collections.Generic
open Microsoft.Diagnostics.Runtime
open System.Linq

type MemoryDump =
    {
        target : DataTarget
        runtime : ClrRuntime
        runtimeHeapObjects : Lazy<ClrObject array>

    } interface IDisposable with
        member x.Dispose () =
            x.target.Dispose()
      static member Create target runtime =
        {
            target = target
            runtime = runtime
            runtimeHeapObjects = lazy(runtime.Heap.EnumerateObjects() |> Seq.toArray)
        }
type TypeHeapStats = {
    clrType : ClrType
    count : int64
    totalSize : uint64
}
    with
        member x.AddObject size = {
            x with
                totalSize = x.totalSize + size
                count = x.count + 1L
        }
let loadDump (filePath) =
    
    let target = DataTarget.LoadDump filePath
    let runtime = target.ClrVersions.[0].CreateRuntime()
    MemoryDump.Create target runtime

let computeHeapStatistics (dump : MemoryDump) =
    printfn "starting computeHeapStatistics"
    let source = dump.runtimeHeapObjects.Value
    let length = source.Length
    let init = Dictionary<string, TypeHeapStats>()
    let mutable count = 0
    let init =
        (init,dump.runtime.Heap.EnumerateObjects())
        ||> Seq.fold(fun state next ->
            if count % 1000 = 0 then
                let percentage = ((float count) / (float length)) * 100.
                printfn "Progess at %.2f%%" percentage
            let name =
                if (next.Type |> isNull) || (next.Type.Name |> isNull) then
                    "null"
                else
                    next.Type.Name

            let value =
                match state.TryGetValue(name) with
                | (true, v) ->
                    v.AddObject next.Size
                | _ ->
                    { clrType = next.Type; count = 1L; totalSize = next.Size }
            state.[name] <- value
            count <- count + 1
            state
        )
    init.Values.ToList()

let mydump = loadDump (IO.Path.Join(__SOURCE_FILE__,"..", "dumps", "mydump.dump"))

let heap = computeHeapStatistics mydump
heap
|> Seq.sortByDescending(fun d -> d.totalSize)
|> Seq.take 10
