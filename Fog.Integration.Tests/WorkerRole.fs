namespace Fog.Integration.Tests

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
 
    let duration fn = 
        let timer = new System.Diagnostics.Stopwatch()
        timer.Start()
        let result = fn()
        Trace.WriteLine <| sprintf "Ellapsed Time: %i" timer.ElapsedMilliseconds
        result
 
    override wr.Run() =
        try
            log "Fog.Integration.Tests entry point called" "Information"
            Fog.Storage.Blob.Tests.RunAll()
            Fog.Storage.Table.Tests.RunAll()
            Fog.Storage.Queue.Tests.RunAll()
            // Replace the config information before uncommenting the call to run the ServiceBus tests
            //Fog.ServiceBus.Tests.RunAll()
            // Replace the app config cache relted information before uncommenting the call to run these tests
            // Fog.Caching.Tests.RunAll()
            log "Fog.Integration.Tests completed successfully" "Information"
            // The following load tests take a little while to run. Be aware of this before uncommenting.
            //duration Storage.Load.Tests.RunBlobTest
            //duration Storage.Load.Tests.RunTableTest
            //duration Storage.Load.Tests.RunQueueTest
        with
        | ex -> log ex.Message "ERROR"
    override wr.OnStart() = base.OnStart()
