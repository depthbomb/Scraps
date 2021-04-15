$proj_dir = "$($PSScriptRoot)\Scraps"
$profiles = "linux-arm64",
            "linux-arm",
            "win10-x64",
            "win10-x86",
            "win81-x64",
            "win81-x86",
			"win10-x64-scd",
            "win10-x86-scd",
            "win81-x64-scd",
            "win81-x86-scd"

iex "cd $($proj_dir)"

Foreach ($p in $profiles)
{
    iex "dotnet publish -c Release -p:PublishProfile=$($p)"
}