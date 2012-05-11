module Storage.Load.Tests

open System
open System.Text
open NUnit.Framework
open FsUnit
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.StorageClient
open Microsoft.WindowsAzure.ServiceRuntime
open Fog.Core
open Fog.Storage.Queue
open Fog.Storage.Table
open Fog.Storage.Blob
open System.Data.Services.Common

[<DataServiceKey("PartitionKey", "RowKey")>]
type TestRecord() = 
    let mutable partitionKey = ""
    let mutable rowKey = ""
    let mutable name = ""
    member x.PartitionKey with get() = partitionKey and set v = partitionKey <- v
    member x.RowKey with get() = rowKey and set v = rowKey <- v
    member x.Name with get() = name and set v = name <- v

let RunBlobTest() =
    [1..1000]
    |> Seq.iter
           (fun n -> 
               let message = sprintf "This is a test %i" n
               UploadBlob "testcontainerload" "testblobload" message |> ignore
               DeleteBlob "testcontainerload" "testblobload"
          )

let RunTableTest() =
    [1..1000]
    |> Seq.iter
           (fun n -> 
               let testRecord = TestRecord( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
               CreateEntity "testtableload" testRecord |> ignore
               DeleteEntity "testtableload" testRecord
          )

let RunQueueTest() =
    [1..1000]
    |> Seq.iter
           (fun n -> 
               let message = sprintf "This is a test %i" n
               AddMessage "testqueue" message |> ignore
               let result = GetMessages "testqueue" 20 5
               for m in result do
                   DeleteMessage "testqueue" m
          )


