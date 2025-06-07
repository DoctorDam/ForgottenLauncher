/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Addons
{
    /// <summary>
    /// Interaction logic for Server_Addon_Span.xaml
    /// </summary>
    public partial class Server_Addon_Span : UserControl
    {
        private Newton_Main.AddonsListResponse pAddon = new Newton_Main.AddonsListResponse();
        private Addon_Downloader addon_downloader = new Addon_Downloader();

        public Server_Addon_Span(Newton_Main.AddonsListResponse addon)
        {
            InitializeComponent();

            pAddon = addon;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Extensions.SetImageBrushSource(pictureHolder, pAddon.PictureUrl, UriKind.Absolute);
            nameHolder.Text = pAddon.Name;
            descriptionHolder.Text = pAddon.Description;
            sizeHolder.Text = Extensions.SizeSuffix(pAddon.TotalSize);

            if (Addon_Handler.IsAddonInstalled(pAddon.Name))
            {
                ButtonInstall.Content = "Re-Install";
                ButtonInstall.Style = (Style)Application.Current.Resources["buttonStyle4c"];
            }
        }

        private async void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {
            ButtonInstall.IsEnabled = false;
            ButtonInstall.Content = "Preparing..";

            await Task.Delay(1000);

            addon_downloader.Prepare(pAddon);
            addon_downloader.Start();

            addon_downloader.ProgressChangedEvent += Addon_downloader_ProgressChangedEvent;
            addon_downloader.CompletedEvent += Addon_downloader_CompletedEvent;
            addon_downloader.StoppedEvent += Addon_downloader_StoppedEvent;
        }

        private void Addon_downloader_ProgressChangedEvent(object sender, System.Net.DownloadProgressChangedEventArgs e)
        {
            var progress = (long)((double)(addon_downloader.TotalSizeDownloaded + e.BytesReceived) / pAddon.TotalSize * 100);
            ButtonInstall.Content = $"Installing {progress}%";
        }

        private async void Addon_downloader_CompletedEvent(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            ButtonInstall.Content = "Completed 100%";
            await Task.Delay(1000);

            ButtonInstall.IsEnabled = false;
            ButtonInstall.Content = "Installed";

            await Task.Delay(1000);

            ButtonInstall.IsEnabled = true;
            ButtonInstall.Content = "Re-Install";
            ButtonInstall.Style = (Style)Application.Current.Resources["buttonStyle4c"];
        }

        private void Addon_downloader_StoppedEvent(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            ButtonInstall.IsEnabled = true;
            ButtonInstall.Content = "Install";
        }
    }
}
