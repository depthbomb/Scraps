$proj_dir = "$($PSScriptRoot)\Scraps"
$profiles = "linux-x64",
            "linux-arm",
            "linux-arm64",
            "win-x64"

# CD into the main assembly folder
iex "cd $($proj_dir)"

# Publish each profile
Foreach ($p in $profiles)
{
    iex "dotnet publish -c Release -p:PublishProfile=$($p)"
}

iex "cd $($PSScriptRoot)\Scraps.GUI"

iex "dotnet publish -c Release -p:PublishProfile=win10-x64"