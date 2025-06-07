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

namespace Forgotten_Land_Launcher.Modules.Account_Info
{
    /// <summary>
    /// Interaction logic for Inventory_Span.xaml
    /// </summary>
    public partial class Inventory_Span : UserControl
    {
        private Newton_Main.AccountInventoryResponse pReward;

        public Inventory_Span(Newton_Main.AccountInventoryResponse reward)
        {
            InitializeComponent();
            pReward = reward;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Extensions.SetImageBrushSource(pictureHolder, pReward.PictureUrl, UriKind.Absolute);
            titleHolder.Text = pReward.Title;
        }
    }
}
