﻿#region License
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

namespace Scraps.Events
{
    /// <summary>
    /// An event that contains basic statistics throughout the bot's operation
    /// </summary>
    public class StatusUpdateArgs : EventArgs
    {
        /// <summary>
        /// Current number of raffles joined
        /// </summary>
        public int RafflesJoined { get; set; }

        /// <summary>
        /// Message to display
        /// </summary>
        public string Status { get; set; }

        public StatusUpdateArgs(int rafflesJoined, string status)
        {
            RafflesJoined = rafflesJoined;
            Status = status;
        }
    }
}
