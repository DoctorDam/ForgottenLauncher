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

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Wait_Dialog.xaml
    /// </summary>
    public partial class Wait_Dialog : Border
    {
        private string pTitle;
        private string pText;

        public Wait_Dialog(string title, string text)
        {
            InitializeComponent();

            pTitle = title;
            pText = text;
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);

            waitingTitle.Text = pTitle;
            waitingText.Text = pText;
        }

        public void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }
    }
}
