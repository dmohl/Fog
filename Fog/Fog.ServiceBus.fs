module Fog.ServiceBus

open System
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open Microsoft.WindowsAzure.ServiceRuntime
open Fog.Core

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
    match serviceQueueManager.QueueExists(qName) with
    | true -> serviceQueueManager.GetQueue(qName), qName
    | _ -> serviceQueueManager.CreateQueue(qName), qName

let GetMessagingFactory (address:string) (tokenProvider:TokenProvider) =
    memoize
        (fun (addr:string) (tp:TokenProvider) -> 
            MessagingFactory.Create(addr, tp) ) address tokenProvider   

let private sendMessageToQueueOrTopic (manager:NamespaceManager) (tokenProvider:TokenProvider) (targetName:string) message =
    let factory = GetMessagingFactory manager.Address.AbsoluteUri tokenProvider 
    let messageSender = factory.CreateMessageSender(targetName.ToLower())
    use message = new BrokeredMessage(message)
    messageSender.Send(message)

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
    SendMessageWithManager serviceManager tokenProvider (queueName.ToLower()) message

let private handleMessageFromQueueOrTopic (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) successHandler failureHandler = 
    let factory = GetMessagingFactory serviceManager.Address.AbsoluteUri tokenProvider 
    let messageReceiver = factory.CreateMessageReceiver(queueName.ToLower())
    let rec handleMessage() = async {
        let message = messageReceiver.Receive()
        if message <> null then
            try
                successHandler message
                message.Complete()
            with
            | ex ->
                failureHandler ex message 
                message.Abandon() } |> Async.Start
    handleMessage()

let HandleMessagesWithManager (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) successHandler failureHandler = 
    let _, name = CreateQueueWithManager serviceManager queueName
    handleMessageFromQueueOrTopic serviceManager tokenProvider name successHandler failureHandler

let HandleMessages (queueName:string) successHandler failureHandler = 
    let serviceManager, tokenProvider = getDefaultServiceManagerAndTokenProvider()
    HandleMessagesWithManager serviceManager tokenProvider (queueName.ToLower()) successHandler failureHandler

let CreateTopicWithManager (manager:NamespaceManager) (topicName:string) =
    let topic = topicName
    match manager.TopicExists(topic) with
    | true -> manager.GetTopic topic, topic
    | false -> manager.CreateTopic topic, topic

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