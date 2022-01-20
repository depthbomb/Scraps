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

using System;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Scraps.GUI.Forms
{
    public partial class DebugForm : Form
    {
        public DebugForm()
        {
            InitializeComponent();

            _CreateToastButton.Click += CreateToastButton_Click;
            _ClearToastHistoryButton.Click += (_, _) => ToastNotificationManagerCompat.History.Clear();
            _CheckWebView2RuntimeButton.Click += (_, _) =>
            {
                try
                {
                    CoreWebView2Environment.GetAvailableBrowserVersionString();

                    Utils.ShowInfo("Debug", "Installed");
                }
                catch
                {
                    Utils.ShowError("Debug", "Installed");
                }
            };
        }

        private void CreateToastButton_Click(object sender, EventArgs e)
        {
            var actionButton = new ToastButton();
                actionButton.AddArgument("action", "buttonAction");
                actionButton.SetContent("Action Button");

                var dismissButton = new ToastButton();
                    dismissButton.SetContent("Dismiss Button");
                    dismissButton.SetDismissActivation();

                var toast = new ToastContentBuilder();
                    toast.AddAttributionText("Attribute Text");
                    toast.AddText("Title");
                    toast.AddText("Text 1");
                    toast.AddText("Text 2");
                    toast.AddButton(actionButton);
                    toast.AddButton(dismissButton);
                    toast.Show(toast =>
                    {
                        toast.ExpirationTime = DateTime.Now.AddSeconds(5);
                        toast.Priority = Windows.UI.Notifications.ToastNotificationPriority.High;
                    });
        }
    }
}
