namespace Fog.Intergration.Tests

open System
open System.Collections.Generic
open System.Diagnostics
open System.Linq
open System.Net
open System.Threading
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.Diagnostics
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient

type WorkerRole() =
    inherit RoleEntryPoint() 

    let log message kind = Trace.WriteLine(message, kind)

    override wr.Run() =
        try
            log "Fog.Intergration.Tests entry point called" "Information"
            Fog.Storage.Blob.Tests.RunAll()
            Fog.Storage.Table.Tests.RunAll()
            Fog.Storage.Queue.Tests.RunAll()
            log "Fog.Intergration.Tests completed successfully" "Information"
        with
        | ex -> log ex.Message "ERROR"
    override wr.OnStart() = base.OnStart()
