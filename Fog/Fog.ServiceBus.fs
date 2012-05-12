module Fog.ServiceBus

open System
open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open Microsoft.WindowsAzure.ServiceRuntime

// TODO: Consider memoization
let CreateSecurityTokenWithKeys issuerConfigKey keyConfigKey = 
    let issuer = RoleEnvironment.GetConfigurationSettingValue issuerConfigKey 
    let key = RoleEnvironment.GetConfigurationSettingValue keyConfigKey
    TokenProvider.CreateSharedSecretTokenProvider(issuer, key)    

let CreateSecurityToken() =
    CreateSecurityTokenWithKeys "ServiceBusIssuer" "ServiceBusKey"

let GetServiceUri scheme nameSpace servicePath =
    ServiceBusEnvironment.CreateServiceUri(scheme, nameSpace, servicePath)

let GetServiceQueueManagerWithTokenProvider (tokenProvider:TokenProvider) scheme nameSpace servicePath =
    let uri = ServiceBusEnvironment.CreateServiceUri(scheme, nameSpace, servicePath)
    NamespaceManager(uri, tokenProvider)

let CreateQueueWithManager (serviceQueueManager:NamespaceManager) (queueName:string) =
    let qName = queueName.ToLower()
    match serviceQueueManager.QueueExists(qName) with
    | true -> serviceQueueManager.GetQueue(qName)
    | _ -> serviceQueueManager.CreateQueue(qName)

// TODO: Consider memoization
let GetMessagingFactory (uri:Uri) (tokenProvider:TokenProvider) =
    MessagingFactory.Create(uri, tokenProvider)   
    
let SendMessageWithManager (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) message = 
    CreateQueueWithManager serviceManager queueName |> ignore
    let factory = GetMessagingFactory serviceManager.Address tokenProvider 
    let messageSender = factory.CreateMessageSender(queueName.ToLower())
    use message = new BrokeredMessage(message)
    messageSender.Send(message)

let SendMessage (queueName:string) message = 
    let tokenProvider = CreateSecurityToken()
    let scheme = RoleEnvironment.GetConfigurationSettingValue "ServiceBusScheme" 
    let ns = RoleEnvironment.GetConfigurationSettingValue "ServiceBusNamespace" 
    let servicePath = RoleEnvironment.GetConfigurationSettingValue "ServiceBusServicePath" 
    let serviceManager = GetServiceQueueManagerWithTokenProvider tokenProvider scheme ns servicePath
    SendMessageWithManager serviceManager tokenProvider (queueName.ToLower()) message

let HandleMessagesWithManager (serviceManager:NamespaceManager) (tokenProvider:TokenProvider) (queueName:string) successHandler failureHandler = 
    CreateQueueWithManager serviceManager queueName |> ignore
    let factory = GetMessagingFactory serviceManager.Address tokenProvider 
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

let HandleMessages (queueName:string) successHandler failureHandler = 
    let tokenProvider = CreateSecurityToken()
    let scheme = RoleEnvironment.GetConfigurationSettingValue "ServiceBusScheme" 
    let ns = RoleEnvironment.GetConfigurationSettingValue "ServiceBusNamespace" 
    let servicePath = RoleEnvironment.GetConfigurationSettingValue "ServiceBusServicePath" 
    let serviceManager = GetServiceQueueManagerWithTokenProvider tokenProvider scheme ns servicePath
    HandleMessagesWithManager serviceManager tokenProvider (queueName.ToLower()) successHandler failureHandler