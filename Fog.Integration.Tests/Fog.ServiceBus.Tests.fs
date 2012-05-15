module Fog.ServiceBus.Tests

open System
open NUnit
open FsUnit
open Fog.ServiceBus
open Microsoft.ServiceBus.Messaging

type TestRecord = { Name : string }

let testRecord = { Name = "test" } 

let ``It should create a service queue manager``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    manager.Address.AbsoluteUri |> should equal "sb://dmohlfsmvc4azure.servicebus.windows.net/" 

let ``It should create a queue``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    let queue, _ = CreateQueueWithManager manager "testQueue"
    queue.Path |> should equal "testQueue" 

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

let ``It should publish a message``() =
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    PublishWithManager manager tokenProvider "topictest" testRecord

let ``It should subscribe unsubscribe and delete a topic``() =
    let sub1 = ref ""
    let sub2 = ref ""
    let tokenProvider = CreateSecurityToken()
    let manager = GetServiceQueueManagerWithTokenProvider tokenProvider "sb" "dmohlfsmvc4azure" String.Empty
    SubscribeWithManager manager tokenProvider "topictest" "AllTopics1"
        <| fun m ->
              sub1 := "complete" 
              m.GetBody<TestRecord>().Name |> should equal "test"
        <| fun ex m -> raise ex        

    SubscribeWithManager manager tokenProvider "topictest" "AllTopics2"
        <| fun m -> 
              sub2 := "complete" 
              m.GetBody<TestRecord>().Name |> should equal "test"
        <| fun ex m -> raise ex        
    PublishWithManager manager tokenProvider "topictest" testRecord
    let rec loop() = async {
        match !sub1, !sub2 with
        | "", _ -> loop()
        | _, "" -> loop()
        | _, _ -> 
            UnsubscribeWithManager manager tokenProvider "topictest" "AllTopics1"
            UnsubscribeWithManager manager tokenProvider "topictest" "AllTopics2"
            DeleteTopicWithManager manager tokenProvider "topictest" } |> Async.Start
    loop()

let ``It should subscribe unsubscribe and delete a topic with friendly functions``() =
    let sub1 = ref ""
    let sub2 = ref ""
    Subscribe "topictest2" "AllTopics4"
        <| fun m ->
              sub1 := "complete" 
              m.GetBody<TestRecord>().Name |> should equal "test"
        <| fun ex m -> raise ex        

    Subscribe "topictest2" "AllTopics5"
        <| fun m -> 
              sub2 := "complete" 
              m.GetBody<TestRecord>().Name |> should equal "test"
        <| fun ex m -> raise ex   
             
    Publish "topictest2" testRecord
    let rec loop() = async {
        match !sub1, !sub2 with
        | "", _ -> loop()
        | _, "" -> loop()
        | _, _ -> 
            Unsubscribe "topictest2" "AllTopics4"
            Unsubscribe "topictest2" "AllTopics5"
            DeleteTopic "topictest2" } |> Async.Start
    loop()

// TODO:
// 2. Add async options
// 3. Is there a better way to determine the default Uri?

let RunAll () = 
    ``It should create a service queue manager``()
    ``It should create a queue``()
    ``It should send a message``()
    ``It should handle a message``()
    ``It should send a message with friendly function``()
    ``It should handle a message with friendly function``()
    ``It should publish a message``()
    ``It should subscribe unsubscribe and delete a topic``()
    ``It should subscribe unsubscribe and delete a topic with friendly functions``()