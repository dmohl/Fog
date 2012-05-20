module Fog.Caching.Tests

open FsUnit
open Option
open System
open System.Runtime.Serialization
open Microsoft.ApplicationServer.Caching
open Fog.Caching

[<DataContract>]
type TestRecord = 
    { [<DataMember>] mutable Id : Guid
      [<DataMember>] mutable Name : string }

let testRecord = { Id = Guid.NewGuid(); Name = "Dan" }

let ``It should put and get a cached value``() =
    let key = testRecord.Id.ToString()  
    Put key testRecord |> ignore
    let result = Get<TestRecord> key
    result |> isSome |> should be True
    (result |> Option.get).Name |> should equal "Dan"
    DisposeCacheFactory()

let ``It should put with custom timeout and get a cached value``() =
    let key = testRecord.Id.ToString()  
    PutWithCustomTimeout key testRecord 10 |> ignore
    let result = Get<TestRecord> key
    result |> isSome |> should be True
    (result |> Option.get).Name |> should equal "Dan"
    DisposeCacheFactory()

let RunAll () =     
    ``It should put and get a cached value``()
    ``It should put with custom timeout and get a cached value``()
