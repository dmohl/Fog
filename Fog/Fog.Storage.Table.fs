module Fog.Storage.Table

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open System.Data.Services.Client
open System.IO
open Fog.Core
open Microsoft.FSharp.Linq
open Microsoft.FSharp.Linq.Query

// This method returns a table client for the provided connection string 
let BuildTableClientWithConnStr(connectionString) =
    let storageAccount = GetStorageAccount connectionString
    storageAccount.CreateCloudTableClient()

// Convention based approach for return a table client with connection string named "BlobStorageConnectionString"
let BuildTableClient() = BuildTableClientWithConnStr "TableStorageConnectionString"

let CreateEntityWithClient (client:CloudTableClient) (tableName:string) entity = 
    let context = client.GetDataServiceContext()
    client.CreateTableIfNotExist <| tableName.ToLower() |> ignore
    context.AddObject(tableName, entity)
    context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate) |> ignore

let DeleteEntityWithDataContext (client:CloudTableClient) (tableName:string) entity =
    let context = client.GetDataServiceContext() 
    context.AttachTo(tableName, entity, "*")
    context.DeleteObject(entity)
    context.SaveChangesWithRetries() |> ignore

let UpdateEntityWithClient (client:CloudTableClient) (tableName:string) entity = 
    // TODO: Consider updating this to use ReplaceOnUpdate once the storage emulator supports it.
    DeleteEntityWithDataContext client tableName entity
    CreateEntityWithClient client tableName entity

let DeleteTableWithClient (client:CloudTableClient) (tableName:string) = 
    client.DeleteTableIfExist tableName |> ignore

let CreateTableWithClient (client:CloudTableClient) (tableName:string) = 
    client.CreateTableIfNotExist tableName |> ignore

let CreateEntity (tableName:string) entity = 
    let client = BuildTableClient()
    CreateEntityWithClient client tableName entity

let UpdateEntity (tableName:string) newEntity = 
    let client = BuildTableClient()
    UpdateEntityWithClient client tableName newEntity

