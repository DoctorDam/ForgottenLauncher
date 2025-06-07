/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Windows;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Forms
{
    /// <summary>
    /// Interaction logic for Password_Change_Form.xaml
    /// </summary>
    public partial class Password_Change_Form : UserControl
    {
        public Password_Change_Form()
        {
            InitializeComponent();
        }

        private void checkboxTos_Checked(object sender, RoutedEventArgs e)
        {
            btnChangePassword.IsEnabled = true;
        }

        private void checkboxTos_Unchecked(object sender, RoutedEventArgs e)
        {
            btnChangePassword.IsEnabled = false;
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

            Animations.MoveRightAndFadeIn(this, 1000, 1000, -400);

            await Task.Delay(5);
            if (Application.Current.MainWindow is Login mainWindow)
            {
                if (mainWindow.textBlockServerName.Opacity == 0 || mainWindow.discordWidget.Opacity == 0)
                {
                    Animations.MoveLeftAndFadeIn(mainWindow.textBlockServerName, 1000, 1000, 400);
                    Animations.MoveLeftAndFadeIn(mainWindow.discordWidget, 1000, 1000, 400);
                }
            }
        }

        public void FadeOut()
        {
            Animations.MoveLeftAndFadeOut(this, 1000, 1000, -400);
        }

        private void btnBackToSignIn_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();

            if (Application.Current.MainWindow is Login mainWindow)
                mainWindow.loginForm.FadeIn();
        }

        private async void btnChangePW_Click(object sender, RoutedEventArgs e)
        {
            warningIconUser.Visibility = Visibility.Hidden;
            warningIconCode.Visibility = Visibility.Hidden;
            warningIconPass1.Visibility = Visibility.Hidden;
            warningIconPass2.Visibility = Visibility.Hidden;

            ToggleLoginControlsVisibility(true);

            var passwordChangeByCodeResponse = await Newton_Peek.PasswordChangeByCodeResponse(textBoxUsername.Text, textBoxCode.Text, textBoxPassword1.Password, textBoxPassword2.Password);

            if (passwordChangeByCodeResponse != null)
            {
                if (passwordChangeByCodeResponse.Changed)
                {
                    btnBackToSignIn_Click(sender, e);

                    if (Application.Current.MainWindow is Login mainWindow)
                    {
                        mainWindow.loginForm.textBoxUsername.Text = textBoxUsername.Text;
                        mainWindow.loginForm.textBoxPassword.Focus();
                    }
                }
                else
                {
                    Animations.MoveUpAndFadeIn(warningIconUser, 300, 300);
                    Animations.MoveUpAndFadeIn(warningIconCode, 500, 500);
                    Animations.MoveUpAndFadeIn(warningIconPass1, 500, 500);
                    Animations.MoveUpAndFadeIn(warningIconPass2, 500, 500);
                    Extensions.ShowPopup("Password Change Failed", passwordChangeByCodeResponse.Message, Application.Current.MainWindow?.GetType());
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            ToggleLoginControlsVisibility(false);
        }

        private void ToggleLoginControlsVisibility(bool isLoggingIn)
        {
            btnChangePassword.Visibility = isLoggingIn ? Visibility.Collapsed : Visibility.Visible;
            registerSpinner.Visibility = isLoggingIn ? Visibility.Visible : Visibility.Collapsed;

            Control[] controlsToDisable = { textBoxUsername, textBoxCode, textBoxPassword1, textBoxPassword2, btnChangePassword, btnBackToSignIn };

            foreach (Control control in controlsToDisable)
            {
                control.IsEnabled = !isLoggingIn;
            }
        }
    }
}
