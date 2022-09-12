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

namespace Scraps.Next.Services;

internal record Flag
{
    public string Name      { get; init; }
    public string ShortName { get; init; }
    public bool   Active    { get; set; }
}

public class FlagsService
{
    private readonly IList<Flag>               _flags;
    private readonly IDictionary<string, Flag> _flagCache;
    private readonly Regex                     _flagPrefixPattern;

    public FlagsService()
    {
        _flags             = new List<Flag>();
        _flagCache         = new Dictionary<string, Flag>();
        _flagPrefixPattern = new Regex("(/|-+)", RegexOptions.Compiled);
    }

    /// <summary>
    /// Parses program args and sets registered flags active if applicable.
    /// </summary>
    /// <param name="args">Args array from program entry</param>
    public void ParseFlags(IEnumerable<string> args)
    {
        foreach (string arg in args)
        {
            var flag = GetFlag(arg);
            if (flag != null && !flag.Active)
            {
                flag.Active = true;
            }
        }
    }

    /// <summary>
    /// Registers a flag that the program accepts as valid.
    /// </summary>
    /// <param name="name">The full name of the flag</param>
    /// <param name="shortName">The short name or alias of the flag</param>
    /// <exception cref="Exception">Flag is already registered</exception>
    public FlagsService RegisterFlag(string name, string shortName)
    {
        if (_flags.Any(f => f.Name == name || f.ShortName == shortName))
        {
            throw new Exception($"Flag {name}|{shortName} is already registered");
        }

        _flags.Add(new Flag
        {
            Name      = name.ToLower(),
            ShortName = shortName.ToLower(),
        });

        return this;
    }

    /// <summary>
    /// Returns true if a flag from the program args is found and active in the flag registry.
    /// </summary>
    /// <param name="flag">The flag name or short name to check the registry for</param>
    /// <returns>`true` if the flag is found in the registry and is active</returns>
    public bool HasFlag(string flag)
    {
        var registryFlag = GetFlag(flag);

        return registryFlag != null && registryFlag.Active;
    }

    private Flag GetFlag(string input)
    {
        if (_flagCache.ContainsKey(input))
        {
            return _flagCache[input];
        }
        
        string flag           = _flagPrefixPattern.Replace(input.ToLower(), "");
        var    registeredFlag = _flags.FirstOrDefault(f => f.Name == flag || f.ShortName == flag);

        _flagCache[input] = registeredFlag;
        
        return registeredFlag;
    }
}