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
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Elements.Popups
{
    /// <summary>
    /// Interaction logic for Game_Path_Request.xaml
    /// </summary>
    public partial class Game_Path_Request : Border
    {
        public Game_Path_Request()
        {
            InitializeComponent();
        }

        public void Show() => Animations.FadeIn(this, 300);

        public void Close() => Animations.FadeOut(this, 300);

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                if (!launcher.isUpdatingGame && !launcher.isCheckingGame)
                {
                    try
                    {
                        using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                        {
                            fbd.SelectedPath = Launcher_Settings.GamePath;
                            System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                            if (result.ToString() == "OK" && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                            {

                                Launcher_Settings.GamePath = fbd.SelectedPath;
                                Launcher_Settings.Save();

                                launcher.CheckForUpdates();
                                Close();
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Error_Handler.Justify(new StackTrace(), null, null, ex, true);
                    }
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
