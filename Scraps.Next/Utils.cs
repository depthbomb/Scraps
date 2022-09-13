﻿#region License
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

using System.Diagnostics;

using Scraps.Next.Extensions;

namespace Scraps.Next;

public static class Utils
{
    public static DialogResult ShowInfo(IWin32Window owner, string title, string content, MessageBoxButtons buttons = MessageBoxButtons.OK)
        => MessageBox.Show(owner, content, title, buttons, MessageBoxIcon.Information);
    
    public static DialogResult ShowWarning(IWin32Window owner, string title, string content, MessageBoxButtons buttons = MessageBoxButtons.OK)
        => MessageBox.Show(owner, content, title, buttons, MessageBoxIcon.Warning);
    
    public static DialogResult ShowError(IWin32Window owner, string title, string content, MessageBoxButtons buttons = MessageBoxButtons.OK)
        => MessageBox.Show(owner, content, title, buttons, MessageBoxIcon.Error);

    public static Form GetMainForm()
        => GetFormByTitle(GlobalShared.WindowTitle);

    public static Form GetFormByTitle(string title)
        => Application.OpenForms.Cast<Form>().FirstOrDefault(form => form.Text == title);

    public static Process OpenUrl(string url)
        => Process.Start(new ProcessStartInfo("cmd", $"/c start {url}"));

    /// <summary>
    /// Returns `true` if the provided cookie value is valid.
    /// </summary>
    /// <param name="cookie">Cookie value string</param>
    public static bool IsValidCookie(string cookie)
        => !cookie.IsNullOrEmpty() && cookie.Length > 200;
}