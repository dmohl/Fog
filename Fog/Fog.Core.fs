module Fog.Core

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient

let GetStorageAccount connectionString = 
    RoleEnvironment.GetConfigurationSettingValue connectionString 
    |> CloudStorageAccount.Parse


