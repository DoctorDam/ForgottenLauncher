/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Launcher_Selfupdater.Library;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher_Selfupdater
{
    /// <summary>
    /// Interaction logic for Updater_Window.xaml
    /// </summary>
    public partial class Updater_Window : Window
    {
        Launcher_Updater updater = new Launcher_Updater();

        public Updater_Window()
        {
            InitializeComponent();

            spIntegrity.Visibility = Visibility.Collapsed;

            CheckForUpdates();
        }

        private void windowExitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void windowMinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        public async void CheckForUpdates()
        {
            await Task.Delay(1000);

            spInitials.Visibility = Visibility.Collapsed;

            if (await updater.UpdateList())
            {
                updater.Start();

                downloadBar.Value = 0;

                spDownloads.Visibility = Visibility.Visible; // show

                updater.ProgressChangedEvent += OnLauncherUpdateProgressChanged;

                updater.CompletedEvent += OnLauncherUpdateCompleted;
            }
            else
            {
                downloadBar.Value = 100;

                spIntegrity.Visibility = Visibility.Collapsed;

                spInitials.Visibility = Visibility.Visible;

                initialsHolder.Text = $"Up to date, starting {Cache.ExeTarget}";

                await Task.Delay(1500);

                try
                {
                    Process.Start(Cache.ExeTarget);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Application.Current.Shutdown();
                }
            }
        }

        private void OnLauncherUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadBar.Visibility = Visibility.Visible;
            downloadBar.Value = (long)((double)(updater.TotalSizeDownloaded + e.BytesReceived) / updater.TotalSizeToDownload * 100);
            dlTotalProgress.Text = downloadBar.Value + "%";
            //dlTotalDownloaded.Text = Extensions.SizeSuffix(updater.TotalSizeDownloaded + e.BytesReceived, 2);
            //dlTotalToDownload.Text = Extensions.SizeSuffix(updater.TotalSizeToDownload, 2);
            //dlSpeed.Text = $"@{e.BytesReceived / 1024d / 1024d / updater.SWSpeed.Elapsed.TotalSeconds:0.00} MB/s";
        }

        private async void OnLauncherUpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            downloadBar.Value = 100;

            dlTotalProgress.Text = downloadBar.Value + "%";

            spIntegrity.Visibility = Visibility.Hidden; // hide
            spDownloads.Visibility = Visibility.Hidden; // hide

            spInitials.Visibility = Visibility.Visible;

            initialsHolder.Text = $"Completed, starting {Cache.ExeTarget}";

            await Task.Delay(1500);

            try
            {
                Process.Start(Cache.ExeTarget);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }
    }
}
