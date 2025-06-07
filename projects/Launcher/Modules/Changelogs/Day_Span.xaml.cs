/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Changelogs
{
    /// <summary>
    /// Interaction logic for Day_Span.xaml
    /// </summary>
    public partial class Day_Span : UserControl
    {
        private string pDate;

        public Day_Span(string date)
        {
            InitializeComponent();
            pDate = date;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dateHolder.Text = pDate;
        }
    }
}
