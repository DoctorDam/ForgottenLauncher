/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Account_Info
{
    /// <summary>
    /// Interaction logic for Teleport_Dropdown_Row.xaml
    /// </summary>
    public partial class Teleport_Dropdown_Row : ComboBoxItem
    {
        public Newton_Main.TeleportListResponse pDestination;

        public Teleport_Dropdown_Row(Newton_Main.TeleportListResponse destination)
        {
            InitializeComponent();
            pDestination = destination;
        }

        private void ComboBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            spDPCost.Visibility = pDestination.DPorBPCprice > 0 ? Visibility.Visible : Visibility.Collapsed;
            spVPCost.Visibility = pDestination.VPrice > 0 ? Visibility.Visible : Visibility.Collapsed;

            dpCost.Text = pDestination.DPorBPCprice.ToString();
            vpCost.Text = pDestination.VPrice.ToString();

            titleHolder.Text = pDestination.Name;
        }
    }
}
