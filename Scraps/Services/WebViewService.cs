#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2022 Caprine Logic
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion

using System.Diagnostics;
using System.ComponentModel;

using Microsoft.Web.WebView2.Core;

namespace Scraps.Services;

public class WebViewService
{
    private readonly Logger _log = LogManager.GetCurrentClassLogger();

    public async Task TryInstallRuntimeAsync()
    {
        if (!IsRuntimeInstalled())
        {
            var res = Utils.ShowInfo(null, "WebView2 Runtime Required", "Scraps requires the WebView2 runtime to be installed.\n\nWould you like to install it now?", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                await InstallRuntimeAsync();
            }
            else
            {
                _log.Debug("User refused WebView2 runtime installation prompt, exiting");
                Environment.Exit(0);
            }
        }
    }

    public async Task InstallRuntimeAsync()
    {
        string downloadUrl =  GlobalShared.WebView2InstallerUrl;
        using (var http = new HttpClient())
        {
            _log.Debug("Downloading WebView2 runtime from {Url}", downloadUrl);
            
            string downloadPath = Path.GetTempFileName();
            var    res          = await http.GetAsync(downloadUrl);
            if (res.IsSuccessStatusCode)
            {
                try
                {
                    _log.Debug("Writing download response to {Path}", downloadPath);
                    
                    byte[] bytes = await res.Content.ReadAsByteArrayAsync();
                    await using (var fs = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await fs.WriteAsync(bytes);
                    }
                    
                    _log.Debug("Running installer");

                    var proc = Process.Start(downloadPath);
                    if (proc != null)
                    {
                        await proc.WaitForExitAsync();
                        
                        _log.Debug("Installer exited");
                    }
                }
                catch (HttpRequestException ex)
                {
                    _log.Error("Failed to write runtime installer");
                    _log.Error(ex);
                    throw;
                }
                catch (Win32Exception ex)
                {
                    _log.Error("Failed to start runtime installer {Path}", downloadPath);
                    _log.Error(ex);
                    throw;
                }
            }
            else
            {
                _log.Error("Unable to download WebView2 runtime: {Reason}", res.ReasonPhrase);
                
                throw new Exception("Failed to download WebView2 runtime");
            }
        }
    }

    /// <summary>
    /// Returns `true` if the WebView2 runtime is installed on the system.
    /// </summary>
    public bool IsRuntimeInstalled()
    {
        try
        {
            CoreWebView2Environment.GetAvailableBrowserVersionString();

            _log.Debug("WebView2 runtime is installed");
            
            return true;
        }
        catch
        {
            _log.Debug("WebView2 runtime is not installed");
            
            return false;
        }
    }
}