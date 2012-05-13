Fog
=======

**Fog** brings the cloud down to earth and wraps it in something more easily used by F#. It provides a more functional approach to creating Windows Azure apps with F#.

Building Azure apps with Fog that need to interact with Azure Table Storage, Blob Storage, Queue Storage, and/or the Service Bus is very easy 
with the help of a few configuration settings that have specific names. The examples that follow use this approach. Fog also provides more fine-grained
interaction if that is desired. See the integration tests for examples.

Syntax
=======

**Blob Storage**

With Fog all you have to do to interact with Azure blob Storage is to add the connection string information in the config with a name of 
"BlobStorageConnectionString". Once that is done, you can use syntax like the following:

	UploadBlob "testcontainer" "testblob" "This is a test" |> ignore
	DeleteBlob "testcontainer" "testblob"

or

	UploadBlob "testcontainer" "testblob" testBytes |> ignore
	DownloadBlob<byte[]> "testcontainer" "testblob"

**Table Storage**

The simplest way to interact with Azure table Storage is to add the connection string information in the config with a name of 
"TableStorageConnectionString". Once that is done, you can use syntax like the following:

    [<DataServiceKey("PartitionKey", "RowKey")>]
	type TestClass() = 
		let mutable partitionKey = ""
		let mutable rowKey = ""
		let mutable name = ""
		member x.PartitionKey with get() = partitionKey and set v = partitionKey <- v
		member x.RowKey with get() = rowKey and set v = rowKey <- v
		member x.Name with get() = name and set v = name <- v

    let originalClass = TestClass( PartitionKey = "TestPart", RowKey = Guid.NewGuid().ToString(), Name = "test" )
    
	CreateEntity "testtable" originalClass |> ignore
    
	let newClass = originalClass
    newClass.Name <- "test2"
    UpdateEntity "testtable" newClass |> ignore
    
	DeleteEntity "testtable" newClass

**Queue Storage**

For queue storage, add the connection string configuration value with setting name "QueueStorageConnectionString".

    AddMessage "testqueue" "This is a test message" |> ignore
    let result = GetMessages "testqueue" 20 5
    for m in result do
        DeleteMessage "testqueue" m

**Service Bus**

There are a few service bus related config entries. Here's the list expected names: ServiceBusIssuer, ServiceBusKey, ServiceBusScheme, ServiceBusNamespace, ServiceBusServicePath

To send a message do this:

	type TestRecord = { Name : string }

	let testRecord = { Name = "test" } 

    SendMessage "testQueue" testRecord

To receive a message, pass the queue name, a function to handle successful message retrieval, and another function to handle errors.

    HandleMessages "testQueue"
        <| fun m -> m.GetBody<TestRecord>().Name
        <| fun ex m -> raise ex        

To use topics in a pub/sub type of scenario, use the something like the following to subscribe:

    Subscribe "topictest2" "AllTopics4"
        <| fun m ->
              printfn "%s" m.GetBody<TestRecord>().Name
        <| fun ex m -> raise ex        

Message publishing can be accomplished like this:
             
    Publish "topictest2" testRecord

A few other handy functions include Unsubscribe and DeleteTopic:

	Unsubscribe "topictest2" "AllTopics5"
	DeleteTopic "topictest2"

Releases
=======
0.1.0.0 - Is the initial release which provides support for Azure table, blob, and queue storage as well as Service Bus Queues and Topics. 

MIT License
=======

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.