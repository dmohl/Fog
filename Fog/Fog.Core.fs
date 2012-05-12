module Fog.Core

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open Microsoft.FSharp.Reflection
open System.Reflection

let GetStorageAccount connectionString = 
    RoleEnvironment.GetConfigurationSettingValue connectionString 
    |> CloudStorageAccount.Parse

let memoize fn =
    let cache = ref Map.empty
    fun key ->
        let cacheMap = !cache 
        match cacheMap.TryFind key with
        | Some result -> result
        | None ->
            let result = fn key
            cache := Map.add key result cacheMap
            result