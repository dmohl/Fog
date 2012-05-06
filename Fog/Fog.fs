module Fog.Storage

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open System.IO

// This method returns a blob client for the provided connection string 
let BuildBlobClientWithConn connectionString =
    let storageAccount = RoleEnvironment.GetConfigurationSettingValue connectionString 
                         |> CloudStorageAccount.Parse
    storageAccount.CreateCloudBlobClient()

// Convention based approach for return a blob client with connection string named "BlobStorageConnectionString"
let BuildBlobClient() = BuildBlobClientWithConn "BlobStorageConnectionString"

let GetBlobContainer (client:CloudBlobClient) (containerName:string) = 
    let container = client.GetContainerReference <| containerName.ToLower()
    container.CreateIfNotExist() |> ignore
    container

let DeleteBlobContainer (blobContainer:CloudBlobContainer) = 
    blobContainer.Delete()

let GetBlobReference (container:CloudBlobContainer) (name:string) : CloudBlob = 
    container.GetBlobReference <| name.ToLower() 

let UploadToBlob<'a> (container:CloudBlobContainer) (blobName:string) (item:'a) = 
    let blob = GetBlobReference container blobName
    match box item with
    | :? Stream as s -> blob.UploadFromStream s
    | :? string as text -> blob.UploadText text
    | :? (byte[]) as b -> blob.UploadByteArray b
    | _ -> failwith "This type is not supported"

let DownloadStreamFromBlob (container:CloudBlobContainer) (blobName:string) (stream:#Stream) = 
    let blob = GetBlobReference container blobName
    blob.DownloadToStream stream
    stream.Seek(0L, SeekOrigin.Begin) |> ignore

let DownloadFromBlob<'a> (container:CloudBlobContainer) (blobName:string) = 
    let blob = GetBlobReference container blobName
    match typeof<'a> with
    | str when str = typeof<string> -> blob.DownloadText() |> box :?> 'a
    | b when b = typeof<byte[]> -> blob.DownloadByteArray() |> box :?> 'a
    | _ -> failwith "This type is not supported"

//let UploadFileToBlob (container:CloudBlobContainer) (blobName:string) (filePath:string) = 
//    UploadFromStreamToBlob container blobName <| File.OpenRead(filePath)
//
//let UploadStringToBlob (container:CloudBlobContainer) (blobName:string) (text:string) =
//    let blob = GetBlobReference container blobName
//    blob.UploadText text
//
//let UploadByteArrayToBlob (container:CloudBlobContainer) (blobName:string) (bytes:byte[]) =
//    let blob = GetBlobReference container blobName
//    blob.UploadByteArray bytes