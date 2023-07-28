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

namespace Scraps;

public static class Args
{
    private record Arg
    {
        public string Name      { get; init; }
        public string ShortName { get; init; }
        public bool   Active    { get; set; }
    }

    private static readonly List<Arg> RegisteredArgs = new();
    private static readonly Regex     ArgPrefixRegex = new("(/|--?)", RegexOptions.Compiled);

    /// <summary>
    /// Parses program args and sets registered args active if applicable.
    /// </summary>
    /// <param name="args">Args array from program entry</param>
    public static void Parse(IEnumerable<string> args)
    {
        foreach (var a in args)
        {
            var arg = GetArg(a);
            if (arg != null && !arg.Active)
            {
                arg.Active = true;
            }
        }
    }

    /// <summary>
    /// Registers an arg that the program accepts as valid.
    /// </summary>
    /// <param name="name">The full name of the arg</param>
    /// <param name="shortName">The short name or alias of the arg</param>
    /// <exception cref="Exception">Arg is already registered</exception>
    public static void Register(string name, string shortName)
    {
        if (RegisteredArgs.Any(f => f.Name == name || f.ShortName == shortName))
        {
            throw new Exception($"Arg {name}|{shortName} is already registered");
        }

        RegisteredArgs.Add(new Arg
        {
            Name      = name.ToLower(),
            ShortName = shortName.ToLower(),
        });
    }

    /// <summary>
    /// Returns true if an arg from the program args is found and active in the arg registry.
    /// </summary>
    /// <param name="arg">The arg name or short name to check the registry for</param>
    /// <returns>`true` if the arg is found in the registry and is active</returns>
    public static bool Has(string arg)
    {
        var registryArg = GetArg(arg);

        return registryArg != null && registryArg.Active;
    }

    private static Arg GetArg(string input)
    {
        var arg = ArgPrefixRegex.Replace(input.ToLower(), "");
        
        return RegisteredArgs.FirstOrDefault(f => f.Name == arg || f.ShortName == arg);
    }
}
