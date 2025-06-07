/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Characters_Market;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Character_Market_Notifications_Dialog.xaml
    /// </summary>
    public partial class Character_Market_Notifications_Dialog : Border
    {
        private List<Newton_Main.CharactersMarketBidsNotifications> pNotifications = new List<Newton_Main.CharactersMarketBidsNotifications>();

        public Character_Market_Notifications_Dialog(List<Newton_Main.CharactersMarketBidsNotifications> notifications)
        {
            InitializeComponent();

            pNotifications = notifications;
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var notification in pNotifications)
            {
                spCharacters.Children.Add(new Dialog_Character_Won(notification));
            }
        }

        public void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }
    }
}
