﻿#region License
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
using Scraps.Next.Properties;

namespace Scraps.Next.Services;

public class SettingsService : IDisposable
{
    public event EventHandler<SettingsServiceSavedArgs> OnSaved;
    public event EventHandler<SettingsServiceResetArgs> OnReset;

    public UserSettings Settings => _settings;

    private readonly UserSettings _settings;
    private readonly Logger       _log = LogManager.GetCurrentClassLogger();

    public SettingsService()
    {
        _settings = UserSettings.Default;
    }

    ~SettingsService() => Dispose();
    
    public void Dispose() => Save(false);

    /// <summary>
    /// Sets a property value. Does NOT save.
    /// </summary>
    /// <param name="key">The name of the property to set</param>
    /// <param name="value">The value to set the property to</param>
    /// <typeparam name="T">The type to cast the property value to</typeparam>
    /// <returns><see cref="SettingsService"/></returns>
    public SettingsService Set<T>(string key, T value)
    {
        if (_settings[key] != null)
        {
            _settings[key] = value;
            
            _log.Debug("Set {Key} to {Value}", key, value);
        }
        
        return this;
    }
    
    /// <summary>
    /// Retrieves a setting value if it exists.
    /// </summary>
    /// <param name="key">The key of the property to retrieve</param>
    /// <typeparam name="T">The type to cast the return value to</typeparam>
    /// <returns>The value if it exists, `null` otherwise</returns>
    public T Get<T>(string key)
    {
        return (T)_settings[key];
    }

    /// <summary>
    /// Returns `true` settings require an upgrade after a new installation.
    /// </summary>
    public bool RequireUpgrade() => _settings.UpgradeRequired;

    /// <inheritdoc cref="ApplicationSettingsBase.Upgrade()"/>
    public void Upgrade()
    {
        _settings.Upgrade();
        Set("UpgradeRequired", false);
        Save(false);
        
        _log.Debug("Upgraded settings from previous installation");
    }

    public void TryUpgrade()
    {
        if (RequireUpgrade())
        {
            Upgrade();
        }
    }

    /// <summary>
    /// Saves the current properties to persistent storage. See <see cref="ApplicationSettingsBase.Save()">ApplicationSettingsBase.Save()</see>.
    /// </summary>
    /// <param name="reload">Reloads the values on retrieval from storage. See <see cref="ApplicationSettingsBase.Reload()">ApplicationSettingsBase.Reload()</see>.</param>
    public void Save(bool reload = true)
    {
        _settings.Save();

        if (reload)
        {
            _settings.Reload();
        }
        
        OnSaved?.Invoke(this, new SettingsServiceSavedArgs(_settings));
        
        _log.Debug("Saved settings (reload={Reload})", reload);
    }

    /// <summary>
    /// Resets properties to their default values. See <see cref="ApplicationSettingsBase.Reset()">ApplicationSettingsBase.Reset()</see>.
    /// </summary>
    public void Reset()
    {
        _settings.Reset();
        
        OnReset?.Invoke(this, new SettingsServiceResetArgs(_settings));
        
        _log.Debug("Reset settings to defaults");
    }
}