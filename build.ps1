iex "cls"
iex "cd .\Scraps"

$profiles = "win7-x64","win7-x86","win81-x64","win81-x86","win10-x64","win10-x86"

Foreach ($p in $profiles)
{
    $host.ui.RawUI.WindowTitle = "Building for $($p)..."
    iex "dotnet publish -c Release -p:PublishProfile=$($p)"
}