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

using Scraps.Services.Honeypot.Vectors;

namespace Scraps.Services.Honeypot;

public class HoneypotService : IDisposable
{
    public bool   IsHoneypot { get; private set; }
    public string Reason     { get; private set; }
    
    private readonly List<IHoneypotVector> _vectors;

    public HoneypotService()
    {
        _vectors = new List<IHoneypotVector>
        {
            new BannedEntriesVector()
        };
    }

    ~HoneypotService() => Dispose();
    
    public void Dispose() => _vectors.Clear();
    
    public void Check(string html)
    {
        foreach (var vector in _vectors)
        {
            vector.Check(html);
            if (vector.Detected)
            {
                IsHoneypot = true;
                Reason     = vector.DetectReason;
                break;
            }
        }
    }
}
