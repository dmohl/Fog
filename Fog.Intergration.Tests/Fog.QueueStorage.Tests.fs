module Fog.Storage.Queue.Tests

open System
open System.Text
open NUnit.Framework
open FsUnit
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.StorageClient
open Microsoft.WindowsAzure.ServiceRuntime
open Fog.Core
open Fog.Storage.Queue

let ``It should create a queue storage client with a convention based connectionString``() = 
    BuildQueueClient().BaseUri.AbsoluteUri |> should equal "http://127.0.0.1:10001/devstoreaccount1"

let ``It should create a queue storage client with a provided connectionString``() = 
    let client = (BuildQueueClientWithConnStr "TestTableStorageConnectionString")
    client.BaseUri.AbsoluteUri |> should equal "http://127.0.0.1:10001/devstoreaccount1"

let client = BuildQueueClient()
let queue = GetQueueReference client "testQueue"
DeleteQueueWithClient client "testQueue"

let ``It should allow creation of a queue``() = 
    let queue = CreateQueueWithClient client "testQueue"
    queue.Name |> should equal "testqueue"

let ``It should allow adding a string message to a queue``() =
    let result = AddMessageWithClient client "testqueue" "This is a test message"
    let message = result.GetMessage()
    message.AsString |> should equal "This is a test message"
    DeleteMessageWithClient client "testqueue" message

let ``It should allow adding a byte array message to a queue``() =    
    DeleteQueueWithClient client "testqueuebytes"
    let testBytes = Encoding.ASCII.GetBytes("This is a test")
    let result = AddMessageWithClient client "testqueuebytes" testBytes
    let message = result.GetMessage()
    message.AsBytes |> should equal testBytes 
    DeleteMessageWithClient client "testqueuebytes" message

let ``It should allow a queue to be deleted``() =
    CreateQueueWithClient client "testQueue2" |> ignore
    DeleteQueueWithClient client "testQueue2"

let ``It should allow retrieval of a single message``() =
    AddMessageWithClient client "testqueue" "This is a test message" |> ignore
    let result = GetMessageWithClient client "testqueue"
    result.AsString |> should equal "This is a test message"

let ``It should allow retrieval of multiple messages``() =
    AddMessageWithClient client "testqueue" "This is a test message" |> ignore
    AddMessageWithClient client "testqueue" "This is a test message" |> ignore
    let result = GetMessagesWithClient client "testqueue" 20 5
    for m in result do
        DeleteMessageWithClient client "testqueue" m

let ``It should add a message with an easy function``() =
    AddMessage "testqueue" "This is a test message" |> ignore

let ``It should get a message with an easy function``() =
    AddMessage "testqueue" "This is a test message" |> ignore
    let result = GetMessage "testqueue" 
    result.AsString |> should equal "This is a test message"

let ``It should delete a message with an easy function``() =
    AddMessage "testqueue" "This is a test message" |> ignore
    let result = GetMessages "testqueue" 20 5
    for m in result do
        DeleteMessage "testqueue" m

// TODO:
// 3. Add async version of the most important functions (i.e. download/upload) -> Might wait until VS11 support is added. These will likely be provided OOTB.
// 4. Make all downloads and uploads run in parallel? -> Might wait until VS11 support is added.
// 5. This gets improved once we can use F# 3.0. OData Type Provider, OOTB Query syntax, etc. make this even easier. Need to build some of that in when VS11 support is added.

let RunAll () = 
    ``It should create a queue storage client with a convention based connectionString``()
    ``It should create a queue storage client with a provided connectionString``()
    ``It should allow creation of a queue``()
    ``It should allow adding a string message to a queue``()
    ``It should allow adding a byte array message to a queue``()
    ``It should allow a queue to be deleted``()
    ``It should allow retrieval of a single message``()
    ``It should allow retrieval of multiple messages``()
    ``It should add a message with an easy function``()
    ``It should get a message with an easy function``()
    ``It should delete a message with an easy function``()