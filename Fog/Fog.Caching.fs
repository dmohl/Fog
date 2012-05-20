module Fog.Caching

open System
open Fog.Core
open Microsoft.ApplicationServer.Caching

let GetDefaultCache () =
    memoize (fun () ->        
        let cacheFactory = new DataCacheFactory()
        cacheFactory.GetDefaultCache(), cacheFactory) ()

let PutWithCustomTimeout key value timeoutInMinutes =          
    let cache, _ = GetDefaultCache()
    cache.Put(key, value, TimeSpan.FromMinutes(float timeoutInMinutes))

let Put key value =
    let cache, _ = GetDefaultCache()
    cache.Put(key, value)

let Get<'a> key =
    let cache, _ = GetDefaultCache()
    match cache.Get key with
    | null -> None
    | r -> Some(r :?> 'a)

let DisposeCacheFactory() =
    let _, cachFactory = GetDefaultCache ()
    cachFactory.Dispose()
