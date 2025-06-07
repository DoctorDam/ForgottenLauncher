/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Account_Inventory;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Account_Inventory.xaml
    /// </summary>
    public partial class Account_Inventory : UserControl
    {
        public Account_Inventory()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();
            
            if (Cache.PageOptions.AccountInventory.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeAccountInventory();
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
            ClearNativeChilds();
        }

        private void ClearNativeChilds()
        {
            var childrenToRemove = new List<Inventory_Reward>();

            foreach (var child in wpInventory.Children)
            {
                if (child is Inventory_Reward)
                {
                    childrenToRemove.Add(child as Inventory_Reward);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                wpInventory.Children.Remove(childToRemove);
            }
        }

        private async void InitializeAccountInventory()
        {
            ClearNativeChilds();

            var accountInventory = await Newton_Peek.AccountInventoryResponse();

            if (accountInventory != null)
            {
                foreach (var reward in accountInventory)
                {
                    wpInventory.Children.Add(new Inventory_Reward(reward)
                    {
                        Margin = new Thickness(0, 15, 20, 15)
                    });
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }
    }
}
