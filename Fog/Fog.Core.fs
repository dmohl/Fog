module Fog.Core

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open Microsoft.FSharp.Reflection
open System.Reflection

let GetStorageAccount connectionString = 
    RoleEnvironment.GetConfigurationSettingValue connectionString 
    |> CloudStorageAccount.Parse
