#region License
// Scraps - Scrap.TF Raffle Bot
// Copyright(C) 2021 Caprine Logic
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

using Microsoft.Win32;
using System.Reflection;

using Scraps.Events;

namespace Scraps.Services;

public class SettingsService
{
    public event EventHandler<SettingsServiceSavedArgs> OnSaved;
    public event EventHandler<SettingsServiceResetArgs> OnReset;

    public const string FullSettingsKey = "Software\\Caprine Logic\\Scraps\\Settings";

    // Default settings values
    private const string Cookie                = "";
    private const int    ScanDelay             = 5000;
    private const int    PaginateDelay         = 500;
    private const int    JoinDelay             = 4000;
    private const int    JoinJitter            = 0;
    private const int    ScanJitter            = 0;
    private const bool   IncrementScanDelay    = true;
    private const bool   SortByNew             = true;
    private const bool   Paranoid              = false;
    private const bool   AlwaysOnTop           = false;
    private const bool   CheckUpdates          = true;
    private const bool   FetchAnnouncements    = true;
    private const bool   SeenWarningDisclaimer = false;

    private readonly Logger _log = LogManager.GetCurrentClassLogger();

    public SettingsService Set(string settingKey, object settingValue)
    {
        using (var key = Registry.CurrentUser.OpenSubKey(FullSettingsKey, true))
        {
            if (settingValue is int)
            {
                key.SetValue(settingKey, settingValue, RegistryValueKind.String);
            }
            else
            {
                key.SetValue(settingKey, settingValue);
            }
            
            _log.Debug("Set {Key} to {Value}", settingKey, settingValue);
        }
        
        OnSaved?.Invoke(this, new SettingsServiceSavedArgs());

        return this;
    }

    public int GetInt(string settingKey) => Convert.ToInt32(Get(settingKey));

    public bool GetBool(string settingKey) => Convert.ToBoolean(Get(settingKey));

    public string GetString(string settingKey) => (string)Get(settingKey);

    public object Get(string settingKey)
    {
        using (var key = Registry.CurrentUser.OpenSubKey(FullSettingsKey, false))
        {
            return (string)key.GetValue(settingKey);
        }
    }
    
    public void TryUpgrade(bool reset = false)
    {
        CreateSubKey();
        
        var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        foreach (var field in fields)
        {
            if (field.IsLiteral && !field.IsInitOnly)
            {
                using (var key = Registry.CurrentUser.OpenSubKey(FullSettingsKey, true))
                {
                    var fieldName  = field.Name;
                    var fieldValue = field.GetValue(null);
                    if (key.GetValue(fieldName) == null || reset)
                    {
                        if (fieldValue is int)
                        {
                            key.SetValue(fieldName, fieldValue, RegistryValueKind.String);
                        }
                        else
                        {
                            key.SetValue(fieldName, fieldValue);
                        }
                        
                        _log.Debug("Set {Key} to {Value}", fieldName, fieldValue);
                    }
                }
            }
        }

        if (reset)
        {
            OnReset?.Invoke(this, new SettingsServiceResetArgs());
            
            _log.Debug("Settings reset to default values");
        }
    }

    private void CreateSubKey()
    {
        if (!HasSubKey())
        {
            Registry.CurrentUser.CreateSubKey(FullSettingsKey);
            
            _log.Debug("Created settings registry subkey at {KeyPath}", FullSettingsKey);
        }
    }

    private bool HasSubKey()
    {
        using (var key = Registry.CurrentUser.OpenSubKey(FullSettingsKey, false))
        {
            return key != null;
        }
    }
}
