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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Forgotten_Land_Launcher.Elements.Buttons
{
    /// <summary>
    /// Interaction logic for LeftMenuButton.xaml
    /// </summary>
    public partial class LeftMenuButton : Button
    {
        #region IconA
        public ImageSource IconA
        {
            get { return (ImageSource)GetValue(IconAProperty); }
            set { SetValue(IconAProperty, value); }
        }

        public static readonly DependencyProperty IconAProperty =
            DependencyProperty.Register("IconA", typeof(ImageSource), typeof(LeftMenuButton), new PropertyMetadata(null));
        #endregion
        // -----------------------------------------------------------------------------------------------------------------------------

        #region IconH
        public ImageSource IconH
        {
            get { return (ImageSource)GetValue(IconHProperty); }
            set { SetValue(IconHProperty, value); }
        }

        public static readonly DependencyProperty IconHProperty =
            DependencyProperty.Register("IconH", typeof(ImageSource), typeof(LeftMenuButton), new PropertyMetadata(null));
        #endregion
        // -----------------------------------------------------------------------------------------------------------------------------

        #region Display Text
        public string DisplayText
        {
            get { return (string)GetValue(DisplayTextProperty); }
            set { SetValue(DisplayTextProperty, value); }
        }

        public static readonly DependencyProperty DisplayTextProperty =
            DependencyProperty.Register("DisplayText", typeof(string), typeof(LeftMenuButton), new PropertyMetadata(null));
        #endregion
        // -----------------------------------------------------------------------------------------------------------------------------

        public LeftMenuButton()
        {
            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Library.Extensions.SetImageSource(imgDisplay, 
                IconH.ToString().Replace("pack://application:,,,", string.Empty), UriKind.Relative);
        }

        private void Button_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Library.Extensions.SetImageSource(imgDisplay,
                IconA.ToString().Replace("pack://application:,,,", string.Empty), UriKind.Relative);
        }

        private void myButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsEnabled == false)
            {
                textHolder.Foreground = Brushes.White;
            }
            else
            {
                textHolder.Foreground = Extensions.GetColorFromHex("#FF737373");
            }
        }
    }
}
