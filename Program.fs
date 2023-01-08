open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection

open Giraffe
open Giraffe.EndpointRouting

(* Web App Configuration *)
// F# / Giraffe - endpoint routing docs: https://github.com/giraffe-fsharp/Giraffe/blob/master/DOCUMENTATION.md#endpoint-routing

let endpointsList = [
    GET [
        route "/" (text "/: hello world")
        routef "/%s" (fun (aString : string) ->
            text ($"/aString: hit with val: {aString}"))
    ]
    subRoute "/subroute/" [
        GET [
            route "one" (text "subroute/one: hit")
        ]
    ]
]

(* Infrastructure Configuration *)

let configureApp (app : IApplicationBuilder) =
    app
        .UseRouting()
        .UseEndpoints(fun e -> e.MapGiraffeEndpoints(endpointsList))
    |> ignore

let configureServices (services : IServiceCollection) =
    // Add Giraffe dependencies
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(
            fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    |> ignore)
        .Build()
        .Run()
    0