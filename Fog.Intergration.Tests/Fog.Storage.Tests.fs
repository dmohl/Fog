module ``Fog Storage Tests``

open NUnit.Framework
open FsUnit
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.StorageClient
open System.Text
open Fog.Storage
open System.IO

let ``It should create a blob storage client with a convention based connectionString``() = 
    BuildBlobClient().BaseUri.AbsoluteUri |> should equal "http://127.0.0.1:10000/devstoreaccount1"

let ``It should create a blob storage client with a provided connectionString``() = 
    let client = (BuildBlobClientWithConn "TestBlobStorageConnectionString")
    client.BaseUri.AbsoluteUri |> should equal "http://127.0.0.1:10000/devstoreaccount1"

let ``It should create a blob storage container``() =
   let container = GetBlobContainer <| BuildBlobClient() <| "testcontainer"
   container.Name |> should equal "testcontainer"

let ``It should get an existing blob storage container``() =
   GetBlobContainer <| BuildBlobClient() <| "testcontainer" |> ignore
   let container = GetBlobContainer <| BuildBlobClient() <| "testcontainer"
   container.Name |> should equal "testcontainer"

let ``It should delete an existing blob storage container``() =
   GetBlobContainer <| BuildBlobClient() <| "testcontainer"
   |> DeleteBlobContainer

let ``It should upload specified text``() =
   let container = GetBlobContainer <| BuildBlobClient() <| "testcontainer"
   UploadToBlob container "testblob" "This is a test"  
   DownloadFromBlob<string> container "testblob" |> should equal "This is a test"
   // TODO: Also, this all feels clunky. Need to make this easier. A person has to know may more about this than they should.

let ``It should upload a specified byte array``() =
   let testBytes = Encoding.ASCII.GetBytes("This is a test")
   let container = GetBlobContainer <| BuildBlobClient() <| "testcontainer"
   UploadToBlob container "testblob" testBytes  
   DownloadFromBlob<byte[]> container "testblob" |> should equal testBytes

let ``It should upload a specified file by name``() =
   let container = GetBlobContainer <| BuildBlobClient() <| "testcontainer"
   using (File.OpenRead("test.xml")) <| fun f -> UploadToBlob container "testblob" f
   using (new MemoryStream()) 
       <| fun s -> (DownloadStreamFromBlob container "testblob" s
                    use sr = new StreamReader(s)
                    sr.ReadToEnd() |> should equal "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n<test/>\r\n")

let RunAll () = 
    ``It should create a blob storage client with a convention based connectionString``()
    ``It should create a blob storage client with a provided connectionString``()
    ``It should create a blob storage container``()
    ``It should get an existing blob storage container``()
    ``It should delete an existing blob storage container``()
    ``It should upload a specified byte array``()
    ``It should upload specified text``()    
    ``It should upload a specified file by name``()
