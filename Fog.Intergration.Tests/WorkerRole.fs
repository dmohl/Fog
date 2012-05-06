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
        log "Fog.Intergration.Tests entry point called" "Information"
        ``Fog Storage Tests``.RunAll()
        log "Fog.Intergration.Tests completed successfully" "Information"

    override wr.OnStart() = base.OnStart()
