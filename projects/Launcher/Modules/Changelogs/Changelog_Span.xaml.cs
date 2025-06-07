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
    /// Interaction logic for Changelog_Span.xaml
    /// </summary>
    public partial class Changelog_Span : UserControl
    {
        private string pCategoryName;
        private string pChangelog;

        public Changelog_Span(string category_name, string chagelog)
        {
            InitializeComponent();

            pCategoryName = $"[{category_name}]";
            pChangelog = chagelog;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            categoryNameHolder.Text = pCategoryName;
            descriptionHolder.Text = pChangelog;
        }
    }
}
