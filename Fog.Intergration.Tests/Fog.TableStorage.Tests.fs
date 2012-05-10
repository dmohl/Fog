module Fog.Storage.Table.Tests

open System
open NUnit.Framework
open FsUnit
open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.StorageClient
open Microsoft.WindowsAzure.ServiceRuntime
open System.Data.Services.Common
open System.Text
open Fog.Core
open Fog.Storage.Table
open System.IO
open Microsoft.FSharp.Linq.Query

[<DataServiceKey("PartitionKey", "RowKey")>]
type TestRecord() = 
    let mutable partitionKey = ""
    let mutable rowKey = ""
    let mutable name = ""
    member x.PartitionKey with get() = partitionKey and set v = partitionKey <- v
    member x.RowKey with get() = rowKey and set v = rowKey <- v
    member x.Name with get() = name and set v = name <- v

let ``It should create a table storage client with a convention based connectionString``() = 
    BuildTableClient().BaseUri.AbsoluteUri |> should equal "http://127.0.0.1:10002/devstoreaccount1"

let ``It should create a table storage client with a provided connectionString``() = 
    let client = (BuildTableClientWithConnStr "TestTableStorageConnectionString")
    client.BaseUri.AbsoluteUri |> should equal "http://127.0.0.1:10002/devstoreaccount1"

let ``It should add a record to a specified table``() = 
    let client = BuildTableClient()
    let testRecord = TestRecord( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
    CreateEntityWithClient client "testtable" testRecord
    let context = client.GetDataServiceContext()
    let result =
        query <@ seq { for e in context.CreateQuery<TestRecord>("testtable") do
                         if e.PartitionKey = testRecord.PartitionKey && e.RowKey = testRecord.RowKey then
                           yield e } @> |> Seq.head    
    result.Name |> should equal "test"

let ``It should allow updating a record``() = 
    let client = BuildTableClient()
    let context = client.GetDataServiceContext()
    let testRecord = TestRecord( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
    CreateEntityWithClient client "testtable" testRecord
    let resultRecord =
        query <@ seq { for e in context.CreateQuery<TestRecord>("testtable") do
                         if e.PartitionKey = testRecord.PartitionKey && e.RowKey = testRecord.RowKey then
                           yield e } @> |> Seq.head    
    
    let updateRecord = testRecord 
    updateRecord.Name <- "test2"
    UpdateEntityWithClient client "testtable" updateRecord
    let result =
        query <@ seq { for e in context.CreateQuery<TestRecord>("testtable") do
                         if e.PartitionKey = testRecord.PartitionKey && e.RowKey = testRecord.RowKey then
                           yield e } @> |> Seq.head    
    result.Name |> should equal "test2"

let ``It should allow deleting a record``() = 
    let client = BuildTableClient()
    let testRecord = TestRecord( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
    CreateEntityWithClient client "testtable" testRecord
    let context = client.GetDataServiceContext()
    let resultRecord =
        query <@ seq { for e in context.CreateQuery<TestRecord>("testtable") do
                         if e.PartitionKey = testRecord.PartitionKey && e.RowKey = testRecord.RowKey then
                           yield e } @> |> Seq.head    
    DeleteEntityWithDataContext client "testtable" resultRecord

let ``It should allow creating and deleting a table``() = 
    let client = BuildTableClient()
    client.DoesTableExist "testtable2" |> should equal false
    CreateTableWithClient client "testtable2"
    client.DoesTableExist "testtable2" |> should equal true
    DeleteTableWithClient client "testtable2"
    client.DoesTableExist "testtable2" |> should equal false

let ``It should allow easy creation of a record``() =
    let testRecord = TestRecord( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
    CreateEntity "testtable" testRecord |> ignore
    let client = BuildTableClient()
    let context = client.GetDataServiceContext()
    let result = query <@ seq { for e in context.CreateQuery<TestRecord>("testtable") do
                                  if e.PartitionKey = testRecord.PartitionKey && e.RowKey = testRecord.RowKey then
                                    yield e } @> |> Seq.head    
    result.Name |> should equal "test"

let ``It should allow easy update of a record``() =
    let originalRecord = TestRecord( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
    CreateEntity "testtable" originalRecord |> ignore
    let newRecord = originalRecord
    newRecord.Name <- "test2"
    UpdateEntity "testtable" newRecord |> ignore
    let client = BuildTableClient()
    let context = client.GetDataServiceContext()
    let result = 
        query <@ seq { for e in context.CreateQuery<TestRecord>("testtable") do
                         if e.PartitionKey = originalRecord.PartitionKey && e.RowKey = originalRecord.RowKey then
                           yield e } @> |> Seq.head    
    result.Name |> should equal "test2"

// TODO:
// 1 Memoise the client and queue? Stress Test
// 2. Need to add ability to use a different connection string easily. 
// 3. Add async version of the most important functions (i.e. download/upload) -> Might wait until VS11 support is added. These will likely be provided OOTB.
// 4. Make all downloads and uploads run in parallel? -> Might wait until VS11 support is added.
// 5. This gets improved once we can use F# 3.0. OData Type Provider, OOTB Query syntax, etc. make this even easier. Need to build some of that in when VS11 support is added.

let RunAll () = 
    ``It should create a table storage client with a convention based connectionString``()
    ``It should create a table storage client with a provided connectionString``()
    ``It should add a record to a specified table``()
    ``It should allow deleting a record``()
    ``It should allow updating a record``()
    ``It should allow creating and deleting a table``()
    ``It should allow easy creation of a record``()
    ``It should allow easy update of a record``()