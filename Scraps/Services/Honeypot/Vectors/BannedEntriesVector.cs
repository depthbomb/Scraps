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

namespace Scraps.Services.Honeypot.Vectors;

public class BannedEntriesVector : IHoneypotVector
{
    public bool   Detected     { get; private set; }
    public string DetectReason { get; private set; }
    
    private readonly Regex _bannedUsersRegex = new("<img class='tiny-raffle-avatar\\s?' style='border-color:\\s?#CC1100;?' src='(.*)' loading=\"lazy\"\\s?\\/>", RegexOptions.Compiled);
    
    public void Check(string html)
    {
        var entries = _bannedUsersRegex.Matches(html);
        if (entries.Count > 1)
        {
            Detected     = true;
            DetectReason = $"{entries.Count} users who entered this raffle are now banned";
        }
    }
}
