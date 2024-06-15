let createInputDirectory (numOfDay: string) =
    let inputFolderPath = $"./Inputs/{numOfDay}/"
    System.IO.Directory.CreateDirectory(inputFolderPath)

let createSolutionDirectory (numOfDay: string) =
    let solutionFolderPath = $"./Solutions/{numOfDay}/"
    System.IO.Directory.CreateDirectory(solutionFolderPath)

let createInputFiles (directory: System.IO.DirectoryInfo) =
    System.IO.File.Create(directory.FullName + "example.txt") |> ignore
    System.IO.File.Create(directory.FullName + "input.txt") |> ignore

let createSolutionFiles (numOfDay: string) (directory: System.IO.DirectoryInfo) =
    let path = directory.FullName + $"Day{numOfDay}.cs"
    let contents = [
        $"using AdventOfCode2023.Common;";
        "";
        $"namespace AdventOfCode2023;";
        "";
        $"public class Day{numOfDay} : Solution";
        "{";
        "}"
    ]

    System.IO.File.WriteAllLines(path, contents) |> ignore


if fsi.CommandLineArgs.Length < 2 then
    printfn "Please provide a day number"
    System.Environment.Exit(1)

let numOfDay = fsi.CommandLineArgs.[1] |> int |> sprintf "%02i"

createInputDirectory numOfDay |> createInputFiles

createSolutionDirectory numOfDay |> createSolutionFiles numOfDay

