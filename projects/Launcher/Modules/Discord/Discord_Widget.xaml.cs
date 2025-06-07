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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Modules.Discord
{
    /// <summary>
    /// Interaction logic for Discord_Widget.xaml
    /// </summary>
    public partial class Discord_Widget : Button
    {
        private string invite_url = "http://discord.com";

        public Discord_Widget()
        {
            InitializeComponent();
        }

        private async void Button_Loaded(object sender, RoutedEventArgs e)
        {
            spinner.Visibility = Visibility.Visible;
            spDiscordInfo.Visibility = Visibility.Hidden;

            await Task.Delay(1000);

            try
            {
                var discordServerInfo = await Newton_Peek.DiscordServerInfoResponse();

                if (discordServerInfo != null)
                {
                    discordCount.Text = discordServerInfo.MembersCount.ToString();
                    serverName.Text = discordServerInfo.ServerName.ToString();
                    invite_url = discordServerInfo.InviteUrl;
                }

                DispatcherTimer timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(30)
                };

                timer.Tick += (ds, de) =>
                {
                    timer.Stop();
                    Button_Loaded(sender, e);
                };
                timer.Start();
            }
            catch
            {
                // known issue for CN users
            }

            spinner.Visibility = Visibility.Hidden;
            spDiscordInfo.Visibility = Visibility.Visible;
        }

        private void Login_Discord_Widget_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(invite_url);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }
    }
}
