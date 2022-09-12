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

using Scraps.Next.Events;
using Scraps.Next.Models;
using Scraps.Next.Dialogs;

namespace Scraps.Next.Services;

public class UpdateService : IDisposable
{
    #region Events

    /// <summary>
    /// Raised when an update check has started
    /// </summary>
    public event EventHandler<UpdateServiceCheckingUpdatesArgs> OnCheckingUpdates; 

    /// <summary>
    /// Raised when an update is available
    /// </summary>
    public event EventHandler<UpdateServiceUpdateAvailableArgs> OnUpdateAvailable; 

    #endregion
    
    private bool                  _updateAvailable;
    private LatestReleaseResponse _latestRelease;

    private readonly HttpClient _http;
    private readonly Logger     _log = LogManager.GetCurrentClassLogger();

    public UpdateService()
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Add("user-agent", "Scraps - depthbomb/Scraps");
    }
    
    ~UpdateService() => Dispose();
    
    public void Dispose()
    {
        _http?.Dispose();
        _latestRelease = null;
    }

    public bool IsUpdateAvailable => _updateAvailable;

    public string GetUpdateBody() => _latestRelease?.Body;

    public async Task CheckForUpdatesAsync()
    {
        _log.Debug("Checking for updates");
        
        OnCheckingUpdates?.Invoke(this, new UpdateServiceCheckingUpdatesArgs());
        
        string json = await _http.GetStringAsync(GlobalShared.UpdateLatestReleaseEndpoint);
        _latestRelease = JsonSerializer.Deserialize<LatestReleaseResponse>(json);
        if (_latestRelease != null)
        {
            var latestVersion = new Version(_latestRelease.TagName.Replace("v", ""));
            var compare       = GlobalShared.VersionAsDotNet().CompareTo(latestVersion);
            if (compare < 0)
            {
                _updateAvailable = true;
                
                OnUpdateAvailable?.Invoke(this, new UpdateServiceUpdateAvailableArgs(_latestRelease));
                
                _log.Debug("Release {Version} is available", latestVersion);

                using (var updateDialog = new UpdateTaskDialog(_latestRelease))
                {
                    updateDialog.ShowDialog();
                }
            }
            else
            {
                _latestRelease = null;
                
                _log.Debug("No updates available");
            }
        }
    }

    public void CheckUntilUpdateAvailable()
    {
        Task.Run(async () =>
        {
            _log.Debug("Started update check loop");
            
            while (!_updateAvailable)
            {
                await CheckForUpdatesAsync();
                await Task.Delay(GlobalShared.UpdateAutoCheckInterval);
            }
        });
    }
}