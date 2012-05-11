module Fog.Core

open Microsoft.WindowsAzure
open Microsoft.WindowsAzure.ServiceRuntime
open Microsoft.WindowsAzure.StorageClient
open Microsoft.FSharp.Reflection
open System.Reflection

let GetStorageAccount connectionString = 
    RoleEnvironment.GetConfigurationSettingValue connectionString 
    |> CloudStorageAccount.Parse

// From http://blogs.msdn.com/b/dsyme/archive/2007/05/31/a-sample-of-the-memoization-pattern-in-f.aspx
let memoize f =
    let cache = ref Map.empty
    fun x ->
        match (!cache).TryFind(x) with
        | Some res -> res
        | None ->
             let res = f x
             cache := (!cache).Add(x,res)
             res