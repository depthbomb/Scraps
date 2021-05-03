$proj_dir = "$($PSScriptRoot)\Scraps"
$profiles = "linux-x64",
            "linux-arm",
            "linux-arm64",
            "win-x64"

# Build solution first (for plugins)
iex "dotnet build Scraps.sln -c Release"

# CD into the main assembly folder
iex "cd $($proj_dir)"

# Publish each profile
Foreach ($p in $profiles)
{
    iex "dotnet publish -c Release -p:PublishProfile=$($p)"
}