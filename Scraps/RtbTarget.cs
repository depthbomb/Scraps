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

using Scraps.Extensions;

namespace Scraps;

[Target("RTB")]
public sealed class RtbTarget : TargetWithLayout
{
    private readonly RichTextBox _rtb;
    private readonly Color       _prefixColor = ColorTranslator.FromHtml("#475569");
    private readonly Color       _traceColor  = ColorTranslator.FromHtml("#cbd5e1");
    private readonly Color       _debugColor  = ColorTranslator.FromHtml("#64748b");
    private readonly Color       _infoColor   = ColorTranslator.FromHtml("#0ea5e9");
    private readonly Color       _warnColor   = ColorTranslator.FromHtml("#f97316");
    private readonly Color       _errorColor  = ColorTranslator.FromHtml("#dc2626");
    private readonly Color       _fatalColor  = ColorTranslator.FromHtml("#be123c");

    public RtbTarget(RichTextBox rtb)
    {
        _rtb = rtb;
    }

    protected override void Write(LogEventInfo logEvent)
    {
        string levelName = logEvent.Level.Name;
        string prefix    = $"[{DateTime.Now:HH:mm:ss}]";
        string message   = RenderLogEvent(Layout, logEvent);
        _rtb.AppendText($"{prefix} ", _prefixColor);
        _rtb.AppendLine(message, GetLevelColor(levelName));
        _rtb.ScrollToCaret();
    }

    private Color GetLevelColor(string level) => level switch
        {
            "Trace" => _traceColor,
            "Info"  => _infoColor,
            "Warn"  => _warnColor,
            "Error" => _errorColor,
            "Fatal" => _fatalColor,
            _       => _debugColor,
        };
}