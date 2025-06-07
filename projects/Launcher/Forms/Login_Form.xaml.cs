/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Forgotten_Land_Launcher.Windows;
using System.Diagnostics;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;

namespace Forgotten_Land_Launcher.Forms
{
    /// <summary>
    /// Interaction logic for Login_Form.xaml
    /// </summary>
    public partial class Login_Form : UserControl
    {
        public Login_Form()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                e.Handled = true;

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(10)
                };

                timer.Tick += (a1, a2) =>
                {
                    timer.IsEnabled = false;

                    if (element is TextBox textBox)
                    {
                        textBox.SelectAll();
                    }

                    else if (element is PasswordBox passwordBox)
                    {
                        passwordBox.SelectAll();
                    }
                };

                timer.IsEnabled = true;
            }
        }

        public async void FadeIn()
        {
            await Task.Delay(1000);
            Animations.MoveRightAndFadeIn(this, 900, 1000, -400);
        }

        public void FadeOut()
        {
            Animations.MoveLeftAndFadeOut(this, 1000, 1000, -400);
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();

            if (Application.Current.MainWindow is Login mainWindow)
            {
                mainWindow.registerForm.FadeIn();
            }
        }

        private void RecoverButton_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();

            if (Application.Current.MainWindow is Login mainWindow)
            {
                mainWindow.recoveryForm.FadeIn();
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            warningIconUser.Visibility = Visibility.Hidden;
            warningIconPass.Visibility = Visibility.Hidden;

            ToggleLoginControlsVisibility(true);

            Rasar.Encrypt_and_Save_Game_Login(textBoxPassword.Password);

            var loginResponse = await Newton_Peek.LoginResponse(textBoxUsername.Text, textBoxPassword.Password);

            if (loginResponse != null)
            {
                if (loginResponse.Logged)
                {
                    if (checkboxLogin.IsChecked == true)
                    {
                        Rasar.Encrypt_and_Save_Auth_Token_Data(loginResponse.Token, loginResponse.AccInfo.Username);
                    }

                    Cache.AccountInfo   = loginResponse.AccInfo;
                    Cache.AuthToken     = loginResponse.Token;

                    Launcher launcher = new Launcher();

                    if (Application.Current.MainWindow is Login mainWindow)
                    {
                        Wait_Dialog swd = new Wait_Dialog
                        (
                            "Logging in",
                            "Please wait while loading your account information and other data..."
                        );

                        mainWindow.mainGrid.Children.Add(swd);

                        launcher.OnDataLoaded += (os, be) =>
                        {
                            launcher.Show();

                            foreach (var window in Application.Current.Windows.OfType<Login>())
                            {
                                if (window is Login loginWindow)
                                {
                                    loginWindow.Close();
                                }
                            }
                        };
                    }
                }
                else
                {
                    Animations.MoveUpAndFadeIn(warningIconUser, 300, 300);
                    Animations.MoveUpAndFadeIn(warningIconPass, 500, 500);
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            ToggleLoginControlsVisibility(false);
        }

        public async void LoginViaAuthTokenData(string token, string md5username)
        {
            warningIconUser.Visibility = Visibility.Hidden;
            warningIconPass.Visibility = Visibility.Hidden;

            ToggleLoginControlsVisibility(true);

            var authTokenDataResponse = await Newton_Peek.AuthTokenResponse(token, md5username);

            if (authTokenDataResponse != null)
            {
                if (authTokenDataResponse.Authorized)
                {
                    Cache.AccountInfo   = authTokenDataResponse.AccInfo;
                    Cache.AuthToken         = token;

                    Launcher launcher = new Launcher();

                    if (Application.Current.MainWindow is Login mainWindow)
                    {
                        Wait_Dialog swd = new Wait_Dialog
                        (
                            Lang.ResourceManager.GetString("login_window_dialog_logging_in_title", CultureInfo.CurrentUICulture),
                            Lang.ResourceManager.GetString("login_window_dialog_logging_in_text", CultureInfo.CurrentUICulture)
                        );

                        mainWindow.mainGrid.Children.Add(swd);

                        launcher.OnDataLoaded += (os, be) =>
                        {
                            launcher.Show();

                            foreach (var window in Application.Current.Windows.OfType<Login>())
                            {
                                if (window is Login loginWindow)
                                {
                                    loginWindow.Close();
                                }
                            }
                        };
                    }
                }
                else
                {
                    Rasar.Delete_Invalid_Auth_Token_Data();
                    Animations.MoveUpAndFadeIn(warningIconUser, 300, 300);
                    Animations.MoveUpAndFadeIn(warningIconPass, 500, 500);
                    textBoxPassword.Password = string.Empty;
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            ToggleLoginControlsVisibility(false);
        }

        public void ToggleLoginControlsVisibility(bool isLoggingIn)
        {
            btnLogin.Visibility = isLoggingIn ? Visibility.Collapsed : Visibility.Visible;
            loginSpinner.Visibility = isLoggingIn ? Visibility.Visible : Visibility.Collapsed;

            Control[] controlsToDisable = { textBoxUsername, textBoxPassword, checkboxLogin, RegisterButton, RecoverButton };

            foreach (Control control in controlsToDisable)
            {
                control.IsEnabled = !isLoggingIn;
            }
        }

        private void btnVisitHome_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.HomeUrl);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }
    }
}
