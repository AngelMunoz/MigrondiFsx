// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open MigrondiFsx

[<EntryPoint>]
let main argv =
    Operations.Init() |> Operations.CreateMigrations
    0 // return an integer exit code
