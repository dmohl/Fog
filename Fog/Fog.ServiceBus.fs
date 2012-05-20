module Fog.ServiceBus

open System
open System.Diagnostics
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open Microsoft.WindowsAzure.ServiceRuntime
open Fog.Core

type MessageSender with
    member x.AsyncSend (message:BrokeredMessage) =
        Async.FromBeginEnd(message,
            (fun (message:BrokeredMessage, callback, state) -> x.BeginSend(message, callback, state)),
            x.EndSend)       

let CreateSecurityTokenWithKeys issuerConfigKey keyConfigKey = 
    memoize 
        (fun issuerKey keyKey -> 
               let issuer = RoleEnvironment.GetConfigurationSettingValue issuerKey 
               let key = RoleEnvironment.GetConfigurationSettingValue keyKey
               TokenProvider.CreateSharedSecretTokenProvider(issuer, key)
        ) issuerConfigKey keyConfigKey   
    
let CreateSecurityToken() =
    CreateSecurityTokenWithKeys "ServiceBusIssuer" "ServiceBusKey"

let GetServiceUri scheme nameSpace servicePath =
    ServiceBusEnvironment.CreateServiceUri(scheme, nameSpace, servicePath)

let GetServiceQueueManagerWithTokenProvider (tokenProvider:TokenProvider) scheme nameSpace servicePath =
    let uri = ServiceBusEnvironment.CreateServiceUri(scheme, nameSpace, servicePath)
    NamespaceManager(uri, tokenProvider)

let CreateQueueWithManager (serviceQueueManager:NamespaceManager) (queueName:string) =
    let qName = queueName
    try
        match serviceQueueManager.QueueExists(qName) with
        | true -> serviceQueueManager.GetQueue(qName), qName
        | _ -> serviceQueueManager.CreateQueue(qName), qName
    with
    | :? MessagingEntityAlreadyExistsException -> serviceQueueManager.GetQueue(qName), qName

let GetMessagingFactory (address:string) (tokenProvider:TokenProvider) =
    memoize
        (fun (addr:string) (tp:TokenProvider) -> 
            MessagingFactory.Create(addr, tp) ) address tokenProvider   

let private createMessageSender (manager:NamespaceManager) (tokenProvider:TokenProvider) (targetName:string) =
    memoize
        (fun (uri:string) (tProvider:TokenProvider) (name:string) -> 
             let factory = GetMessagingFactory uri tProvider 
             factory.CreateMessageSender(name) ) 
           manager.Address.AbsoluteUri tokenProvider targetName

let private sendMessageToQueueOrTopic (manager:NamespaceManager) (tokenProvider:TokenProvider) (targetName:string) message =
    let messageSender = createMessageSender manager tokenProvider targetName
    use message = new BrokeredMessage(message)
    messageSender.Send(message)
    messageSender.Close()

let SendMessageWithManager (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) message = 
    let qDesc, qName = CreateQueueWithManager serviceManager queueName 
    sendMessageToQueueOrTopic serviceManager tokenProvider qName message

let private getDefaultServiceManagerAndTokenProvider() = 
    let tokenProvider = CreateSecurityToken()
    let scheme = RoleEnvironment.GetConfigurationSettingValue "ServiceBusScheme" 
    let ns = RoleEnvironment.GetConfigurationSettingValue "ServiceBusNamespace" 
    let servicePath = RoleEnvironment.GetConfigurationSettingValue "ServiceBusServicePath" 
    let serviceManager = GetServiceQueueManagerWithTokenProvider tokenProvider scheme ns servicePath
    serviceManager, tokenProvider

let SendMessage (queueName:string) message = 
    let serviceManager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    SendMessageWithManager serviceManager tokenProvider (queueName) message

let private safeComplete (message:BrokeredMessage) = 
    try
        message.Complete()
    with
    | :? MessageLockLostException -> () // Ignore according to http://windowsazurecat.com/2011/09/best-practices-leveraging-windows-azure-service-bus-brokered-messaging-api/
    | :? MessagingException -> () // Ignore according to http://windowsazurecat.com/2011/09/best-practices-leveraging-windows-azure-service-bus-brokered-messaging-api/
    | ex -> raise ex

let private safeAbandon (message:BrokeredMessage) = 
    try
        message.Abandon()
        true
    with
    | :? MessageLockLostException -> false // Ignore according to http://windowsazurecat.com/2011/09/best-practices-leveraging-windows-azure-service-bus-brokered-messaging-api/
    | :? MessagingException -> false // Ignore according to http://windowsazurecat.com/2011/09/best-practices-leveraging-windows-azure-service-bus-brokered-messaging-api/

let private handleMessageFromQueueOrTopic (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) successHandler failureHandler = 
    let factory = GetMessagingFactory serviceManager.Address.AbsoluteUri tokenProvider 
    let messageReceiver = factory.CreateMessageReceiver(queueName)
    let rec handleMessage() = 
        async {
            do! Async.Sleep(1000)
            try   
                let message = messageReceiver.Receive()
                if message <> null then
                    try
                        message |> safeComplete
                        successHandler message                
                    with
                    | ex ->
                        if not (safeAbandon(message)) then
                            failureHandler ex message 
                handleMessage() 
            with
            | :? MessagingEntityNotFoundException as exn -> () // Ignore as this just means the queue/topic isn't yet available 
            | ex -> failureHandler ex <| new BrokeredMessage("")
        } |> Async.Start 
    
    handleMessage()

let HandleMessagesWithManager (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) successHandler failureHandler = 
    let _, name = CreateQueueWithManager serviceManager queueName
    handleMessageFromQueueOrTopic serviceManager tokenProvider name successHandler failureHandler

let HandleMessages (queueName:string) successHandler failureHandler = 
    let serviceManager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    HandleMessagesWithManager serviceManager tokenProvider (queueName) successHandler failureHandler

let CreateTopicWithManager (manager:NamespaceManager) (topicName:string) =
    let topic = topicName
    try
        match manager.TopicExists(topic) with
        | true -> manager.GetTopic topic, topic
        | false -> manager.CreateTopic topic, topic
    with
    | :? MessagingEntityAlreadyExistsException -> manager.GetTopic topic, topic

let CreateSubscriptionWithManager (manager:NamespaceManager) (topicName:string) (subscriptionName:string) =
    let topic = topicName
    match manager.SubscriptionExists(topic, subscriptionName) with
    | true -> manager.GetSubscription(topic, subscriptionName)
    | false -> manager.CreateSubscription(topic, subscriptionName)

let PublishWithManager (manager:NamespaceManager) (tokenProvider:TokenProvider) topicName message  =
    let qDesc, tName = CreateTopicWithManager manager topicName
    sendMessageToQueueOrTopic manager tokenProvider tName message

let SubscribeWithManager (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (topicName:string) (subscriptionName:string) successHandler failureHandler =
    let topicDesc, name = CreateTopicWithManager serviceManager topicName
    CreateSubscriptionWithManager serviceManager name subscriptionName |> ignore
    let path = sprintf "%s/subscriptions/%s" name subscriptionName
    handleMessageFromQueueOrTopic serviceManager tokenProvider path successHandler failureHandler

let UnsubscribeWithManager (manager:NamespaceManager) (tokenProvider:TokenProvider) (topicName:string) (subscriptionName:string) =
    let _, name = CreateTopicWithManager manager topicName
    CreateSubscriptionWithManager manager name subscriptionName |> ignore
    manager.DeleteSubscription(name, subscriptionName)

let DeleteTopicWithManager (manager:NamespaceManager) (tokenProvider:TokenProvider) (topicName:string) =
    let _, name = CreateTopicWithManager manager topicName
    manager.DeleteTopic(name)

let Publish topicName message =
    let serviceManager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    PublishWithManager serviceManager tokenProvider topicName message

let Subscribe (topicName:string) (subscriptionName:string) successHandler failureHandler =
    let manager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    SubscribeWithManager manager tokenProvider topicName subscriptionName successHandler failureHandler

let Unsubscribe topicName subscriptionName = 
    let manager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    UnsubscribeWithManager manager tokenProvider topicName subscriptionName

let DeleteTopic topicName =
    let manager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    DeleteTopicWithManager manager tokenProvider topicName


