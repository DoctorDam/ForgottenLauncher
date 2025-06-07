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
using System.Windows.Input;

namespace Forgotten_Land_Launcher.Elements.Popups
{
    /// <summary>
    /// Interaction logic for Darkness_Fog.xaml
    /// </summary>
    public partial class Darkness_Fog : Border
    {
        public Darkness_Fog()
        {
            InitializeComponent();
        }

        public void Toggle()
        {
            switch (Visibility)
            {
                case Visibility.Visible:
                {
                    Library.Animations.FadeOut(this, 300);
                    break;
                }
                default:
                {
                    Library.Animations.FadeIn(this, 300);
                    break;
                }
            }
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Toggle();

            if (Application.Current.MainWindow is Launcher mainWindow)
            {
                mainWindow.gameMoreOptions.Toggle();
            }
        }
    }
}
