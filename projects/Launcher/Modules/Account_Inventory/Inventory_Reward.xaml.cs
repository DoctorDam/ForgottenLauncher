/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Account_Inventory
{
    /// <summary>
    /// Interaction logic for Inventory_Reward.xaml
    /// </summary>
    public partial class Inventory_Reward : Border
    {
        private Newton_Main.AccountInventoryResponse pReward;

        public Inventory_Reward(Newton_Main.AccountInventoryResponse reward)
        {
            InitializeComponent();
            pReward = reward;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Extensions.SetImageBrushSource(pictureHolder, pReward.PictureUrl, UriKind.Absolute);
            titleHolder.Text = pReward.Title;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher mainWindow)
            {
                var inventory_reward = new Inventory_Reward_Dialog(pReward);

                inventory_reward.ClipToBounds = true;

                Panel.SetZIndex(inventory_reward, 10000);

                inventory_reward.OnConfirmationChanged += (os, be) =>
                {
                    if (inventory_reward.IsUsed)
                    {
                        mainWindow.pageAccountInventory.Load();
                    }
                };

                mainWindow.mainGrid.Children.Add(inventory_reward);
            }
        }
    }
}
