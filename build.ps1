iex "cls"
iex "cd .\Scraps"

$profiles = "linux-arm64","linux-arm","win81-x64","win81-x86","win10-x64","win10-x86"

Foreach ($p in $profiles)
{
    $host.ui.RawUI.WindowTitle = "Building for $($p)..."
    iex "dotnet publish -c Release -p:PublishProfile=$($p)"
}