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
using System.Drawing;
using System.Windows.Forms;

using NLog;
using NLog.Targets;

using Scraps.GUI.Extensions;

namespace Scraps.GUI.Logging
{
    [Target("RTB")]
    public sealed class RtbTarget : TargetWithLayout
    {
        private readonly RichTextBox _rtb;

        public RtbTarget(RichTextBox rtb)
        {
            _rtb = rtb;
        }

        protected override void Write(LogEventInfo logEvent)
        {
            string level = logEvent.Level.Name;
            string prefix = DateTime.Now.ToString("HH:mm:ss");
            string message = RenderLogEvent(Layout, logEvent);
            _rtb.AppendText(prefix + " ", Color.DarkGray);
            _rtb.AppendLine(message, GetColor(level));
            _rtb.ScrollToCaret();
        }

        private Color GetColor(string level) => level switch
        {
            "Trace" => Color.DeepPink,
            "Info"  => Color.AliceBlue,
            "Warn"  => Color.Orange,
            "Error" => Color.Red,
            "Fatal" => Color.DarkRed,
            _       => Color.Gray,
        };
    }
}
