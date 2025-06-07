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
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Elements.Popups
{
    /// <summary>
    /// Interaction logic for Popup_Message.xaml
    /// </summary>
    public partial class Popup_Message : UserControl
    {
        #region Popup Title
        public string PopupTitle
        {
            get { return (string)GetValue(PopupTitleProperty); }
            set { SetValue(PopupTitleProperty, value); }
        }

        public static readonly DependencyProperty PopupTitleProperty =
            DependencyProperty.Register("PopupTitle", typeof(string), typeof(Popup_Message), new PropertyMetadata(null));
        #endregion
        // -----------------------------------------------------------------------------------------------------------------------------
        
        #region Popup Message
        public string PopupMessage
        {
            get { return (string)GetValue(PopupMessageProperty); }
            set { SetValue(PopupMessageProperty, value); }
        }

        public static readonly DependencyProperty PopupMessageProperty =
            DependencyProperty.Register("PopupMessage", typeof(string), typeof(Popup_Message), new PropertyMetadata(null));
        #endregion
        // -----------------------------------------------------------------------------------------------------------------------------

        public Popup_Message()
        {
            InitializeComponent();
            Opacity = 0;
        }

        private void myControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.MoveDownAndFadeIn(this, 300, 300, -100);

            // Create a Timer with a 6-second interval
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(6)
            };

            timer.Tick += (ds, de) =>
            {
                timer.Stop();
                DisposeMe();
            };

            timer.Start();
        }

        private void btnDispose_Click(object sender, RoutedEventArgs e) => DisposeMe();

        private async void DisposeMe()
        {
            Animations.FadeOut(this, 300);

            await Task.Delay(300);

            if (Application.Current.MainWindow is Login loginWindow)
            {
                loginWindow.spPopups.Children.Remove(this);
            }
            else if (Application.Current.MainWindow is Launcher launcherWindow)
            {
                launcherWindow.spPopups.Children.Remove(this);
            }
        }
    }
}
