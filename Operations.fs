[<RequireQualifiedAccess>]
module MigrondiFsx.Operations

open Migrondi.Migrations
open Migrondi.Types


let Init () =
    MigrondiRunner.RunInit(
        { path = System.Environment.CurrentDirectory
          json = false
          noColor = false }
    )
    |> ignore

let CreateMigrations() =
        let first =
            { name = "First"; json = false; noColor = false; }
        let second = { first with name = "second" }
        let third = { second with name = "third" }
        [first; second; third]
        |> List.iter (fun opts -> MigrondiRunner.RunNew(opts) |> ignore)

