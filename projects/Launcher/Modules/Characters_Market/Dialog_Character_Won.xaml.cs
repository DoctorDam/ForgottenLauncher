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

namespace Forgotten_Land_Launcher.Modules.Characters_Market
{
    /// <summary>
    /// Interaction logic for Dialog_Character_Won.xaml
    /// </summary>
    public partial class Dialog_Character_Won : UserControl
    {
        private Newton_Main.CharactersMarketBidsNotifications pNotification = new Newton_Main.CharactersMarketBidsNotifications();

        public Dialog_Character_Won(Newton_Main.CharactersMarketBidsNotifications notification)
        {
            InitializeComponent();

            pNotification = notification;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);

            CharNameHolder.Text = pNotification.CharacterName;

            Extensions.SetImageBrushSource(RaceAvatarHolder, WoW_Definitions.GetPlayerRaceIcon(pNotification.CharacterRace, pNotification.CharacterGender), UriKind.Relative);

            Extensions.SetImageBrushSource(ClassAvatarHolder, WoW_Definitions.GetPlayerClassIcon(pNotification.CharacterClass), UriKind.Relative);

            CharLevelHolder.Text = pNotification.CharacterLevel.ToString();

            CharRaceHolder.Text = WoW_Definitions.GetPlayerRaceName(pNotification.CharacterRace);

            CharClassHolder.Text = WoW_Definitions.GetPlayerClassName(pNotification.CharacterClass);

            CharClassHolder.Foreground = Extensions.GetColorFromHex($"#FF{WoW_Definitions.GetPlayerClassHexColor(pNotification.CharacterClass)}");
        }
    }
}
