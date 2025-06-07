/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Forgotten_Land_Launcher.Modules.Shop
{
    /// <summary>
    /// Interaction logic for Shop_Category_Button.xaml
    /// </summary>
    public partial class Shop_Category_Button : Button
    {
        private Brush nameHolderBrush;
        public string pCategory;

        public Shop_Category_Button(string title)
        {
            InitializeComponent();

            pCategory = title;
            nameHolderBrush = nameHolder.Foreground;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            nameHolder.Text = pCategory;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) => 
            nameHolder.Foreground = Extensions.GetColorFromHex("#FFF7A900");

        private void Button_MouseLeave(object sender, MouseEventArgs e) => 
            nameHolder.Foreground = nameHolderBrush;

        private async void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsEnabled)
            {
                await Task.Delay(100); // to fix interface speed design update
                nameHolder.Foreground = Brushes.DarkOrange;
            }
            else
            {
                nameHolder.Foreground = nameHolderBrush;
            }
        }
    }
}
