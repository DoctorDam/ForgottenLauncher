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
    /// Interaction logic for Register_Tos_Form.xaml
    /// </summary>
    public partial class Register_Tos_Form : UserControl
    {
        public Register_Tos_Form()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var registrationTOS = await Newton_Peek.RegistrationTOSResponse();

                if (registrationTOS != null)
                {
                    tosTextBlock.Text = registrationTOS.Text;
                }
                else
                {
                    DispatcherTimer timer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(30)
                    };

                    timer.Tick += (ds, de) =>
                    {
                        timer.Stop();
                        UserControl_Loaded(sender, e);
                    };

                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            FadeOut();
        }

        public async void FadeIn()
        {
            if (Application.Current.MainWindow is Login mainWindow)
            {
                Animations.MoveRightAndFadeOut(mainWindow.textBlockServerName, 1000, 1000, 400);
                Animations.MoveRightAndFadeOut(mainWindow.discordWidget, 1000, 1000, 400);

                await Task.Delay(1000);

                Animations.FadeIn(this, 500);
            }
        }

        public void FadeOut()
        {
            if (Application.Current.MainWindow is Login mainWindow)
            {
                Animations.FadeOut(this, 300);

                mainWindow.registerForm.FadeIn();
            }
        }
    }
}
