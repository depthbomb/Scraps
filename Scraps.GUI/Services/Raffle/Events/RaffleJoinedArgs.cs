#region License

/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2022 Caprine Logic

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

namespace Scraps.GUI.Services.Raffle;

public class RaffleJoinedArgs : EventArgs
{
    /// <summary>
    /// Number of queued raffles entered at this point
    /// </summary>
    public int Entered { get; }

    /// <summary>
    /// Total number of raffles in queue
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// ID of the raffle that was entered
    /// </summary>
    public string RaffleId { get; }

    /// <summary>
    /// The HTML of the raffle page
    /// </summary>
    public string PageHtml { get; }

    public RaffleJoinedArgs(string raffleId, string pageHtml, int entered, int total)
    {
        RaffleId = raffleId;
        PageHtml = pageHtml;
        Entered  = entered;
        Total    = total;
    }
}