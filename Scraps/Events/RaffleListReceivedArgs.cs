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
using System.Collections.Generic;

namespace Scraps.Events
{
    /// <summary>
    /// An event that contains a list of available raffle IDs
    /// </summary>
    public class RaffleListReceivedArgs : EventArgs
    {
        public List<string> AvailableRaffles { get; set; }

        /// <summary>
        /// Raised when a list of available raffle IDs is received
        /// </summary>
        /// <param name="availableRaffles"></param>
        public RaffleListReceivedArgs(List<string> availableRaffles)
        {
            AvailableRaffles = availableRaffles;
        }
    }
}
