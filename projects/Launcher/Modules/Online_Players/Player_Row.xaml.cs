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
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Online_Players
{
    /// <summary>
    /// Interaction logic for Player_Row.xaml
    /// </summary>
    public partial class Player_Row : UserControl
    {
        public Newton_Main.OnlinePlayer pPlayer;

        public Player_Row(Newton_Main.OnlinePlayer player)
        {
            InitializeComponent();

            pPlayer = player;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            nameHolder.Text = pPlayer.Name;
            nameHolder.Foreground = Extensions.GetColorFromHex($"#FF{WoW_Definitions.GetPlayerClassHexColor(pPlayer.Class)}");

            levelHolder.Text = pPlayer.Level.ToString();

            Extensions.SetImageBrushSource(raceHolder, WoW_Definitions.GetPlayerRaceIcon(pPlayer.Race, pPlayer.Gender), 
                UriKind.Relative);

            Extensions.SetImageBrushSource(classHolder, WoW_Definitions.GetPlayerClassIcon(pPlayer.Class),
                UriKind.Relative);

            factionHolder.Text = $"({WoW_Definitions.GetPlayerFactionName(pPlayer.Race)})";
            factionHolder.Foreground = Extensions.GetColorFromHex($"#FF{WoW_Definitions.GetPlayerFactionHexColor(pPlayer.Race)}");

            zoneHolder.Text = pPlayer.Zone;

            realmHolder.Text = pPlayer.RealmName;
        }
    }
}
