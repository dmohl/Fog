module Fog.ServiceBus.Tests

open System
open NUnit
open FsUnit
open Fog.ServiceBus
open Microsoft.ServiceBus.Messaging

[<Serializable>]
type TestRecord = {
    Name : string
}

let testRecord = { Name = "test" } 

//let ``It should create a security token with provided config keys``() = 
//    CreateSecurityTokenWithKeys "TestServiceBusIssuer" "TestServiceBusKey"
//
//let ``It should create a security token``() = 
//    CreateSecurityToken()

let ``It should create a service queue manager``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    manager.Address.AbsoluteUri |> should equal "sb://dmohlfsmvc4azure.servicebus.windows.net/" 

let ``It should create a queue``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    let queue = CreateQueueWithManager manager "testQueue"
    queue.Path |> should equal "testqueue" 

let ``It should send a message``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    SendMessageWithManager manager tokenProvider "testQueue" testRecord

let ``It should handle a message``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    SendMessageWithManager manager tokenProvider "testQueue" testRecord
    HandleMessagesWithManager manager tokenProvider "testQueue"
        <| fun m -> m.GetBody<TestRecord>().Name |> should equal "test"
        <| fun ex m -> raise ex        

let ``It should send a message with friendly function``() =
    SendMessage "testQueue" testRecord

let ``It should handle a message with friendly function``() =
    SendMessage "testQueue" testRecord
    HandleMessages "testQueue"
        <| fun m -> m.GetBody<TestRecord>().Name |> should equal "test"
        <| fun ex m -> raise ex        
  
// TODO:
// 2. Add async options
// 3. Is there a better way to determine the default Uri?

let RunAll () = 
    //``It should create a security token with provided config keys``() 
    //``It should create a security token``()
    ``It should create a service queue manager``()
    ``It should create a queue``()
    ``It should send a message``()
    ``It should handle a message``()
    ``It should send a message with friendly function``()
    ``It should handle a message with friendly function``()