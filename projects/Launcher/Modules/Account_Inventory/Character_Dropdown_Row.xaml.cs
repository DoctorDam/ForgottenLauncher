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
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Account_Inventory
{
    /// <summary>
    /// Interaction logic for Character_Dropdown_Row.xaml
    /// </summary>
    public partial class Character_Dropdown_Row : UserControl
    {
        public Newton_Main.CharactersListResponse pCharacter;

        public Character_Dropdown_Row(Newton_Main.CharactersListResponse character)
        {
            InitializeComponent();
            pCharacter = character;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Extensions.SetImageBrushSource(raceHolder, WoW_Definitions.GetPlayerRaceIcon(pCharacter.Race, pCharacter.Gender), UriKind.Relative);

            Extensions.SetImageBrushSource(classHolder, WoW_Definitions.GetPlayerClassIcon(pCharacter.Class), UriKind.Relative);

            charNameHolder.Text = pCharacter.Name;

            charNameHolder.Foreground = Extensions.GetColorFromHex($"#FF{WoW_Definitions.GetPlayerClassHexColor(pCharacter.Class)}");

            levelHolder.Text = pCharacter.Level.ToString();

            realmNameHolder.Text = pCharacter.RealmName;
        }
    }
}
