module Fog.Storage.Queue

open System
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open System.Data.Services.Client
open Fog.Core

let BuildQueueClientWithConnStr(connectionString) =
    memoize (fun conn -> 
                 let storageAccount = GetStorageAccount conn
                 storageAccount.CreateCloudQueueClient() ) connectionString 

let BuildQueueClient() = BuildQueueClientWithConnStr "QueueStorageConnectionString"

let GetQueueReference (client:CloudQueueClient) (queueName:string) =
    client.GetQueueReference <| queueName.ToLower()

let CreateQueueWithClient (client:CloudQueueClient) (queueName:string) =
    let queue = GetQueueReference client queueName
    queue.CreateIfNotExist() |> ignore
    queue

let DeleteQueueWithClient (client:CloudQueueClient) (queueName:string) =
    let queue = GetQueueReference client queueName
    if queue.Exists() then queue.Delete()

let AddMessageWithClient (client:CloudQueueClient) (queueName:string) content =
    let queue = CreateQueueWithClient client queueName
    match box content with
    | :? string as s -> queue.AddMessage(CloudQueueMessage(s))
    | :? (byte[]) as b -> queue.AddMessage(CloudQueueMessage(b))
    | _ -> failwith "The provided content is not of a support type (i.e. string or byte[]"
    queue

let DeleteMessageWithClient (client:CloudQueueClient) (queueName:string) (message:CloudQueueMessage) =
    let queue = CreateQueueWithClient client queueName
    queue.DeleteMessage(message)

let GetMessageWithClient (client:CloudQueueClient) (queueName:string) = 
    let queue = CreateQueueWithClient client queueName
    queue.GetMessage()

let GetMessagesWithClient (client:CloudQueueClient) (queueName:string) (messageCount:int) (ttlInMinutes:int) = 
    let queue = CreateQueueWithClient client queueName
    queue.GetMessages(messageCount, TimeSpan.FromMinutes(float ttlInMinutes))

let AddMessage (queueName:string) content  =
    let client = BuildQueueClient()
    AddMessageWithClient client queueName content

let GetMessage queueName =
    let client = BuildQueueClient()
    GetMessageWithClient client queueName

let GetMessages (queueName:string) (messageCount:int) (ttlInMinutes:int) = 
    let client = BuildQueueClient()
    GetMessagesWithClient client queueName messageCount ttlInMinutes

let DeleteMessage (queueName:string) (message:CloudQueueMessage) =
    let client = BuildQueueClient()
    DeleteMessageWithClient client queueName message
