[<RequireQualifiedAccess>]
module MigrondiFsx.Operations

open System
open System.IO
open Migrondi.Migrations
open Migrondi.Types

[<AutoOpen>]
module private RunnerOverrides =

    let tryCreateFsxFile path filename : Result<string, exn> =
        let timestamp =
            DateTimeOffset.Now.ToUnixTimeMilliseconds()

        let filename = $"{filename}_{timestamp}.fsx"
        let fullPath = Path.Combine(path, filename)

        let l2 =
            """#r "nuget: SqlHydra.Query, 0.200.1" """

        let l3 = "open SqlHydra.Query"

        let l4 =
            """let Up: SqlKata.Query list = [(* insert { into table<dbl.Record>; entity record } *)]"""

        let l5 =
            """let Down: SqlKata.Query list = [(* your things *)]"""

        let l6 =
            "/// ***NOTE***: Do not remove the following line"

        let l7 = """[Up; Down]"""

        let content =
            $"{l2}\n{l3}\n\n{l4}\n\n{l5}\n\n{l6}\n{l7}"

        try
            File.WriteAllText(fullPath, content)
            Path.GetFullPath(fullPath) |> Ok
        with
        | ex -> Error ex


let Init () =
    MigrondiRunner.RunInit(
        { path = System.IO.Path.Combine(System.Environment.CurrentDirectory, "migrations")
          json = false
          noColor = false }
    )
    |> ignore

let CreateMigrations () =
    let first =
        { name = "First"
          json = false
          noColor = false }

    let second = { first with name = "second" }
    let third = { second with name = "third" }

    [ first; second; third ]
    |> List.iter
        (fun opts ->
            MigrondiRunner.RunNew(opts, tryCreateNewMigrationFile = Func<_, _, _>(tryCreateFsxFile))
            |> printfn "%A")
