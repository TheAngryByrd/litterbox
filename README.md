# Getting started

Make sure your VSCode instance is [set up to run development containers](https://code.visualstudio.com/docs/remote/containers).

Place a [dotnet dump](https://docs.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-dump) file in `./dumps`. 

Open the workspace via the command pallet option `> Open Workspace in Container`.

Run the FSI via `dotnet fsi fsi-dotnet-dump-analyzer.fsx`, with your own changes (consider using the name of your dump on line 70 instead of `dump.dump`).

Edit the script to your heart's content.

_Alternatively_, run `dotnet fsi` and run `#load "fsi-dotnet-dump-analyzer.fsx`, then proceed to play around in FSI with your references loaded.