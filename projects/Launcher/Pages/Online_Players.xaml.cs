/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Online_Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Online_Players.xaml
    /// </summary>
    public partial class Online_Players : UserControl
    {
        public Online_Players()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;
            RealmsDropdown.Visibility = Visibility.Collapsed;
            onlineStatsHolder.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.OnlinePlayers.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;

                InitializeOnlinePlayers();
            }
            else
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Visible;
                RealmsDropdown.Visibility = Visibility.Collapsed;
                onlineStatsHolder.Visibility = Visibility.Collapsed;
            }
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            spOnlinePlayers.Children.Clear();
        }

        private async void InitializeOnlinePlayers()
        {
            Newton_Main.OnlinePlayersResponse online_players = await Newton_Peek.OnlinePlayersResponse();

            spOnlinePlayers.Children.Clear();

            if (online_players != null)
            {
                Dictionary<int, string> unique_realms = new Dictionary<int, string>();

                foreach (var player in online_players.Players)
                {
                    spOnlinePlayers.Children.Add(new Player_Row(player));

                    if (!unique_realms.ContainsKey(player.RealmId))
                    {
                        unique_realms.Add(player.RealmId, player.RealmName);
                    }
                }

                playersCountHolder.Text = string.Format(new CultureInfo("en-US"), "{0:N0} players", online_players.TotalPlayers);
                displayCountHolder.Text = string.Format(new CultureInfo("en-US"), "({0:N0} displayed)", online_players.Players.Count);

                if (unique_realms.Count > 0)
                {
                    RealmsDropdown.Items.Add(new ComboBoxItem()
                    {
                        Content = "All Realms",
                        Tag = 0,
                    });

                    foreach (var realm in unique_realms)
                    {
                        RealmsDropdown.Items.Add(new ComboBoxItem()
                        {
                            Content = realm.Value,
                            Tag = realm.Key,
                        });
                    }

                    RealmsDropdown.IsEnabled = true;
                    RealmsDropdown.SelectedIndex = 0;
                }

                RealmsDropdown.Visibility = Visibility.Visible;
                onlineStatsHolder.Visibility = Visibility.Visible;
            }
        }

        private void RealmsDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedRealm = RealmsDropdown.SelectedItem as ComboBoxItem;

                int selectedRealmID = int.Parse(selectedRealm.Tag.ToString());

                foreach (var player in spOnlinePlayers.Children.OfType<Player_Row>())
                {
                    if (selectedRealmID == 0)
                    {
                        player.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (player.pPlayer.RealmId == selectedRealmID)
                        {
                            player.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            player.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }
    }
}
