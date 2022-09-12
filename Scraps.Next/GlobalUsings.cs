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

global using System;
global using System.IO;
global using System.Net;
global using System.Linq;
global using System.Net.Http;
global using System.Threading;
global using System.Text.Json;
global using System.Configuration;
global using System.Threading.Tasks;
global using System.Collections.Generic;
global using System.Text.RegularExpressions;
global using System.Text.Json.Serialization;

global using NLog;
global using NLog.Config;
global using NLog.Targets;

global using Windows.Win32;