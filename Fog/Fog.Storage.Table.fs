module Fog.Storage.Table

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open System.Data.Services.Client
open System.IO
open Fog.Core
open Microsoft.FSharp.Linq
open Microsoft.FSharp.Linq.Query

// This method returns a blob client for the provided connection string 
let BuildTableClientWithConnStr(connectionString) =
    let storageAccount = GetStorageAccount connectionString
    storageAccount.CreateCloudTableClient()

// Convention based approach for return a blob client with connection string named "BlobStorageConnectionString"
let BuildTableClient() = BuildTableClientWithConnStr "TableStorageConnectionString"

let CreateInTableWithDataContext (context:TableServiceContext) (client:CloudTableClient) (tableName:string) entity = 
    client.CreateTableIfNotExist <| tableName.ToLower() |> ignore
    context.AddObject(tableName, entity)
    context.SaveChangesWithRetries(SaveChangesOptions.ReplaceOnUpdate) |> ignore

let DeleteFromTableWithDataContext (context:TableServiceContext) (tableName:string) entity = 
    context.DeleteObject(entity)
    context.SaveChangesWithRetries() |> ignore

let UpdateInTableWithDataContext (context:TableServiceContext) (client:CloudTableClient) (tableName:string) oldEntity newEntity = 
    // TODO: Consider updating this to use AttachTo once the storage emulator supports it.
    DeleteFromTableWithDataContext context tableName oldEntity
    CreateInTableWithDataContext context client tableName newEntity

