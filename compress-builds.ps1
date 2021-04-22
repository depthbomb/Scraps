$publish_dir = "$($PSScriptRoot)\Scraps\bin\Publish"
$assembly_name = "Scraps"
$runtimes = "linux-x64",
            "linux-arm",
            "linux-arm64",
            "win10-x64",
            "win10-x86",
            "win81-x64",
            "win81-x86"

Foreach ($r in $runtimes)
{
    $runtime_dir = "$($publish_dir)\$($r)"

    iex "cd $($runtime_dir)"

    del *.pdb

    if ($r.StartsWith("linux"))
    {
        # Windows 10 supports the tar command as of 17063, so just use that instead
        iex "tar -cvzf $($runtime_dir).tar.gz $($assembly_name)"
    }
    else
    {
        iex "7z a -t7z -m0=lzma2 -mx=9 -mfb=64 -md=1024m -ms=on $($runtime_dir).7z $($runtime_dir)\$($assembly_name).exe"
    }

    iex "cd $($publish_dir)"
}