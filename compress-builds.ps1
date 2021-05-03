$publish_dir = "$($PSScriptRoot)\Scraps\bin\Publish"
$setup_dir = "$($PSScriptRoot)\setup"
$assembly_name = "Scraps"
$runtimes = "linux-x64",
            "linux-arm",
            "linux-arm64"

Foreach ($r in $runtimes)
{
    $runtime_dir = "$($publish_dir)\$($r)"

    iex "cd $($runtime_dir)"

    del *.pdb

    # Windows 10 supports the tar command as of 17063, so just use that instead
    iex "tar -cvzf $($runtime_dir).tar.gz $($assembly_name)"

    iex "cd $($publish_dir)"
}

iex "cd $($setup_dir)"

Start-Process -NoNewWindow -FilePath "C:\Program Files (x86)\Inno Setup 6\iscc.exe" -ArgumentList "scraps.iss"