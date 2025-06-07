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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Forms
{
    /// <summary>
    /// Interaction logic for Recovery_Form.xaml
    /// </summary>
    public partial class Recovery_Form : UserControl
    {
        public Recovery_Form()
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

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();

            if (Application.Current.MainWindow is Login mainWindow)
            {
                mainWindow.loginForm.FadeIn();
            }
        }

        private void RecoverButton_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();
        }

        private async void btnRecover_Click(object sender, RoutedEventArgs e)
        {
            warningIconEmail.Visibility = Visibility.Hidden;

            ToggleLoginControlsVisibility(true);

            await Newton_Peek.SendPasswordRecoveryRequest(textBoxEmail.Text);

            if (Extensions.IsValidEmail(textBoxEmail.Text))
            {
                btnCode_Click(sender, e);
            
                if (Application.Current.MainWindow is Login mainWindow)
                {
                    mainWindow.pwChangeForm.textBoxUsername.Text = "Username";
                    mainWindow.pwChangeForm.textBoxCode.Text = "recovery code..";
                    mainWindow.pwChangeForm.textBoxPassword1.Password = "password1";
                    mainWindow.pwChangeForm.textBoxPassword2.Password = "password12";
                    mainWindow.pwChangeForm.textBoxUsername.Focus();
                }
            }
            else
            {
                Animations.MoveUpAndFadeIn(warningIconEmail, 300, 300);
                Animations.MoveUpAndFadeIn(warningIconEmail, 500, 500);
                Extensions.ShowPopup("Request Failed", "Email format is invalid..", Application.Current.MainWindow?.GetType());
            }

            ToggleLoginControlsVisibility(false);
        }

        public void ToggleLoginControlsVisibility(bool isActing)
        {
            btnRecover.Visibility = isActing ? Visibility.Collapsed : Visibility.Visible;
            loginSpinner.Visibility = isActing ? Visibility.Visible : Visibility.Collapsed;

            Control[] controlsToDisable = { textBoxEmail, loginButton, btnCode };

            foreach (Control control in controlsToDisable)
            {
                control.IsEnabled = !isActing;
            }
        }

        private void btnCode_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();

            if (Application.Current.MainWindow is Login mainWindow)
            {
                mainWindow.pwChangeForm.FadeIn();
            }
        }
    }
}
