module Fog.Storage.Blob

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open System.IO
open Fog.Core

let BuildBlobClientWithConnStr(connectionString) =
    memoize (fun conn -> 
               let storageAccount = GetStorageAccount conn
               storageAccount.CreateCloudBlobClient() ) connectionString

let BuildBlobClient() = BuildBlobClientWithConnStr "BlobStorageConnectionString"

let GetBlobContainer (client:CloudBlobClient) (containerName:string) = 
    let container = client.GetContainerReference <| containerName.ToLower()
    container.CreateIfNotExist() |> ignore
    container

let DeleteBlobContainer (blobContainer:CloudBlobContainer) = 
    blobContainer.Delete()

let GetBlobReferenceInContainer (container:CloudBlobContainer) (name:string) : CloudBlob = 
    container.GetBlobReference <| name.ToLower() 

let GetBlobReference (containerName:string) name : CloudBlob = 
    let container = GetBlobContainer <| BuildBlobClient() <| containerName
    GetBlobReferenceInContainer container name

let UploadBlobToContainer<'a> (container:CloudBlobContainer) (blobName:string) (item:'a) = 
    let blob = GetBlobReferenceInContainer container blobName
    match box item with
    | :? Stream as s -> blob.UploadFromStream s
    | :? string as text -> blob.UploadText text
    | :? (byte[]) as b -> blob.UploadByteArray b
    | _ -> failwith "This type is not supported"
    blob

let UploadBlob<'a> (containerName:string) (blobName:string) (item:'a) = 
    let container = GetBlobContainer <| BuildBlobClient() <| containerName
    UploadBlobToContainer<'a> container blobName item

let DownloadBlobStreamFromContainer (container:CloudBlobContainer) (blobName:string) (stream:#Stream) = 
    let blob = GetBlobReferenceInContainer container blobName
    blob.DownloadToStream stream
    stream.Seek(0L, SeekOrigin.Begin) |> ignore

let DownloadBlobStream containerName blobName (stream:#Stream) = 
    let container = GetBlobContainer <| BuildBlobClient() <| containerName
    DownloadBlobStreamFromContainer container blobName stream

let DownloadBlobFromContainer<'a> (container:CloudBlobContainer) (blobName:string) = 
    let blob = GetBlobReferenceInContainer container blobName
    match typeof<'a> with
    | str when str = typeof<string> -> blob.DownloadText() |> box :?> 'a
    | b when b = typeof<byte[]> -> blob.DownloadByteArray() |> box :?> 'a
    | _ -> failwith "This type is not supported"

let DownloadBlob<'a> (containerName:string) (blobName:string) = 
    let container = GetBlobContainer <| BuildBlobClient() <| containerName
    DownloadBlobFromContainer<'a> container blobName

let DeleteBlobFromContainer (container:CloudBlobContainer) (blobName:string) = 
    let blob = GetBlobReferenceInContainer container blobName
    blob.Delete()

let DeleteBlob (containerName) (blobName:string) = 
    let container = GetBlobContainer <| BuildBlobClient() <| containerName
    DeleteBlobFromContainer container blobName

let GetBlobMetadata (blobReference:CloudBlob) = 
    blobReference.FetchAttributes()
    blobReference.Metadata

let SetBlobMetadata (metadata:list<string*string>) (blobReference:CloudBlob) = 
    metadata |> Seq.iter(fun (k,v) -> blobReference.Metadata.Add(k, v))
    blobReference.SetMetadata()
