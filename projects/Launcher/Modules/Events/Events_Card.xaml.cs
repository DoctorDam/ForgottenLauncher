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
using System.Windows.Input;

namespace Forgotten_Land_Launcher.Modules.Events
{
    /// <summary>
    /// Interaction logic for Events_Card.xaml
    /// </summary>
    public partial class Events_Card : Border
    {
        private Newton_Main.LauncherEventsResponse pEvent;

        public Events_Card(Newton_Main.LauncherEventsResponse m_event)
        {
            InitializeComponent();
            pEvent = m_event;
        }

        private void mainBorder_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Extensions.SetImageBrushSource(bannerHolder, pEvent.PictureUrl, UriKind.Absolute);
                titleHolder.Text = pEvent.Title.ToUpper();
                contentHolder.Text = pEvent.Content;
                expiryDateHolder.Text = pEvent.ExpiryDate;

                if (string.IsNullOrEmpty(pEvent.RedirectUrl) || string.IsNullOrWhiteSpace(pEvent.RedirectUrl))
                    Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private void btnReadMore_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(pEvent.RedirectUrl) && !string.IsNullOrWhiteSpace(pEvent.RedirectUrl))
            {
                try
                {
                    Process.Start(pEvent.RedirectUrl);
                }
                catch (Exception ex)
                {
                    Error_Handler.Justify(new StackTrace(), null, null, ex, false);
                }
            }
        }
    }
}
