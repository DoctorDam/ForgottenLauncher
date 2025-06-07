/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Dialogs;
using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Windows
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            backgroundHolder.Play();

            Application.Current.MainWindow = this;

            if (await Launcher_Updater.RequiresUpdate() && !Properties.Settings.Default.DisableLauncherUpdates)
            {
                Launcher_Updater.StartUpdate();
            }
            else
            {
                // LOGIN VIA TOKEN IF TOKEN FOUND AND VALIDATED
                try
                {
                    Rasar.OracleAuthTokenData oracleAuthTokenData = Rasar.Decrypt_Auth_Token_Data();

                    if (oracleAuthTokenData != null)
                    {
                        loginForm.textBoxUsername.Text = oracleAuthTokenData.User.ToLower();
                        loginForm.checkboxLogin.IsChecked = true;
                        loginForm.ToggleLoginControlsVisibility(true);
                        loginForm.LoginViaAuthTokenData(oracleAuthTokenData.Token, Codex_Standard.ToVendettaWho(oracleAuthTokenData.User));
                    }
                }
                catch (Exception ex)
                {
                    Error_Handler.Justify(new StackTrace(), null, null, ex, false);
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (loginForm.checkboxLogin.IsChecked == false)
            {
                Rasar.Delete_Invalid_Auth_Token_Data();
            }
        }

        private void windowExitButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmation_Dialog simple_Confirmation_Dialog = new Confirmation_Dialog
            (
                Lang.ResourceManager.GetString("exit_dialog_title", CultureInfo.CurrentUICulture),
                Lang.ResourceManager.GetString("exit_dialog_text", CultureInfo.CurrentUICulture)
            );

            simple_Confirmation_Dialog.ClipToBounds = true;

            Panel.SetZIndex(simple_Confirmation_Dialog, 10000);

            mainGrid.Children.Add(simple_Confirmation_Dialog);

            simple_Confirmation_Dialog.OnConfirmationChanged += (os, be) =>
            {
                if (simple_Confirmation_Dialog.IsConfirmed)
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void windowMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void backgroundHolder_MediaEnded(object sender, RoutedEventArgs e)
        {
            backgroundHolder.Position = TimeSpan.FromSeconds(0);
            backgroundHolder.Play();
        }
    }
}
