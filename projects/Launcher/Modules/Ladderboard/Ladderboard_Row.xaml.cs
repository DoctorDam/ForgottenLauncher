/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Forgotten_Land_Launcher.Library;

namespace Forgotten_Land_Launcher.Modules.Ladderboard
{
    /// <summary>
    /// Interaction logic for Ladderboard_Row.xaml
    /// </summary>
    public partial class Ladderboard_Row : UserControl
    {
        public Newton_Main.LadderboardResponse pLadderboard;

        public Ladderboard_Row(Newton_Main.LadderboardResponse ladderboard)
        {
            InitializeComponent();
            pLadderboard = ladderboard;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                nameHolder.Text = pLadderboard.PlayerName;

                Extensions.SetImageBrushSource(raceHolder, WoW_Definitions.GetPlayerRaceIcon(pLadderboard.PlayerRace, pLadderboard.PlayerGender), UriKind.Relative);

                Extensions.SetImageBrushSource(classHolder, WoW_Definitions.GetPlayerClassIcon(pLadderboard.PlayerClass), UriKind.Relative);

                todayKillsHolder.Text = pLadderboard.TodayKills.ToString();
                yesterdayKillsHolder.Text = pLadderboard.YesterdayKills.ToString();
                totalKillsHolder.Text = pLadderboard.TotalKills.ToString();

                realmHolder.Text = pLadderboard.RealmName;
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }
    }
}
