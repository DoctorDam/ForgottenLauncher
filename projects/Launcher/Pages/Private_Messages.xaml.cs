/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Private_Messages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Private_Messages.xaml
    /// </summary>
    public partial class Private_Messages : UserControl
    {
        public Private_Messages()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;
            btnNewMessage.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.PrivateMessages.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                btnNewMessage.Visibility = Visibility.Visible;
                InitializePrivateMessages();
            }
            else
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Visible;
            }
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            ClearNativeChilds();
        }

        private void ClearNativeChilds()
        {
            var childrenToRemove = new List<Topic_Span>();

            foreach (var child in spMessages.Children)
            {
                if (child is Topic_Span)
                {
                    childrenToRemove.Add(child as Topic_Span);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                spMessages.Children.Remove(childToRemove);
            }
        }

        private async void InitializePrivateMessages()
        {
            ClearNativeChilds();

            if (Application.Current.MainWindow is Launcher launcher)
            {
                var privateMessagesListResponse = await Newton_Peek.PrivateMessagesListResponse();

                if (privateMessagesListResponse != null)
                {
                    foreach (Newton_Main.PrivateMessagesListResponse pm in privateMessagesListResponse)
                    {
                        Topic_Span topic_Row = new Topic_Span(pm)
                        {
                            Margin = new Thickness(0, 0, 0, 10)
                        };

                        spMessages.Children.Add(topic_Row);

                        await Task.Delay(50);

                        Animations.FadeIn(topic_Row, 750);
                    }
                }
                else
                {
                    Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
                }
            }
        }

        private void btnNewMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                launcher.DisplayNewMessagePage();
            }
        }
    }
}
