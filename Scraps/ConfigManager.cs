#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2021  Caprine Logic

/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.

/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
/// GNU General Public License for more details.

/// You should have received a copy of the GNU General Public License
/// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion License

using System;
using System.IO;
using System.Text.Json;

using Scraps.Models;
using Scraps.Constants;

namespace Scraps
{
    public class ConfigManager
    {
        private string _configFile = Files.ConfigFile;

        public Config Config;

        public void Create() => Save(new Config());

        public void Save(Config config, string filename = null)
        {
            string configFile = filename != null ? Path.Combine(Paths.StorePath, filename + ".json") : _configFile;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(config, options);

            File.WriteAllText(configFile, json);
        }

        public Config Read()
        {
            if (FileExists())
            {
                var options = new JsonSerializerOptions
                {
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true,
                };

                using (var sr = File.OpenText(_configFile))
                {
                    string json = sr.ReadToEnd();
                    var config = JsonSerializer.Deserialize<Config>(json, options);

                    Config = config;

                    return config;
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format("Config file not found at expected path: {0}", _configFile));
            }
        }

        public T Get<T>(string key) => (T)Config.GetType().GetProperty(key).GetValue(Config);

        public bool FileExists()
        {
            if (File.Exists(_configFile))
            {
                return true;
            }
            else
            {
                string assemblyRoot = AppDomain.CurrentDomain.BaseDirectory;
                string altLocation = Path.Combine(assemblyRoot, "Config.json");

                if (File.Exists(altLocation))
                {
                    _configFile = altLocation;

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
