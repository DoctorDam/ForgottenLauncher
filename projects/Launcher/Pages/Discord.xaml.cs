/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Discord;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Discord.xaml
    /// </summary>
    public partial class Discord : UserControl
    {
        private bool working = false;

        public Discord()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;
            spDiscord.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.Discord.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                spDiscord.Visibility = Visibility.Visible;

                if (!working) // wait for the api response to avoid violently api spam requests
                {
                    working = true;

                    discordWidgetHolder.Children.Add(new Discord_Widget());

                    await InitializeMembers();

                    working = false;
                }
            }
            else
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Visible;
            }
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);

            if (!working)
            {
                discordWidgetHolder.Children.Clear();
                spMembers.Children.Clear();
            }
        }

        private async Task InitializeMembers()
        {
            spMembers.Children.Clear();

            var discordServerInfoResponse = await Newton_Peek.DiscordServerInfoResponse();

            if (discordServerInfoResponse != null)
            {
                foreach (Newton_Main.DiscordMember discordMember in discordServerInfoResponse.Members)
                {
                    spMembers.Children.Add(new Member_Span(discordMember));
                }
            }
        }
    }
}
