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
using System.Reflection;
using System.Collections.Generic;

using NLog;

using Scraps.Constants;

namespace Scraps
{
    public static class PluginManager
    {
        public static List<Assembly> Assemblies { get; private set; }

        /// <summary>
        /// Loads the assemblies located in the plugins folder
        /// </summary>
        /// <returns></returns>
        public static bool LoadPlugins()
        {
            var log = LogManager.GetCurrentClassLogger();

            if (Assemblies != null) return false;

            Assemblies = new();

            // Iterate through all DLL assemblies in our plugins folder. The plugins folder should already exist as it is checked (and created) in the
            // bootstrapper.
            foreach (string assemblyPath in Directory.EnumerateFiles(Paths.PluginsPath, "*.dll", SearchOption.AllDirectories))
            {
                Assembly assembly;

                try
                {
                    assembly = Assembly.LoadFrom(assemblyPath);

                    var types = assembly.GetTypes();

                    Assemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    log.Error("Failed to load plugin {Path}", assemblyPath);
                    log.Trace(ex);

                    continue;
                }
            }

            return Assemblies.Count > 0;
        }
    }
}
