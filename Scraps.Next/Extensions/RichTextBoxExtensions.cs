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

namespace Scraps.Next.Extensions;

public static class RichTextBoxExtensions
{
    public static void AppendLine(this RichTextBox rtb, string text, Color color)
    {
        rtb.AppendText(text, color);
        rtb.AppendText(Environment.NewLine);
    }

    public static void AppendText(this RichTextBox rtb, string text, Color color)
    {
        rtb.SelectionStart  = rtb.TextLength;
        rtb.SelectionLength = 0;
        rtb.SelectionColor  = color;
        rtb.AppendText(text);
        rtb.SelectionColor = rtb.ForeColor;
        
        if (rtb.Text.Length > rtb.MaxLength - 200)
        {
            rtb.Clear();
        }
    }
}