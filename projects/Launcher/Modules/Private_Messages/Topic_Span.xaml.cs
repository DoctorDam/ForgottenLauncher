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

namespace Forgotten_Land_Launcher.Modules.Private_Messages
{
    /// <summary>
    /// Interaction logic for Topic_Span.xaml
    /// </summary>
    public partial class Topic_Span : Button
    {
        private Newton_Main.PrivateMessagesListResponse pMessagee;

        public Topic_Span(Newton_Main.PrivateMessagesListResponse message)
        {
            InitializeComponent();

            pMessagee = message;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (pMessagee.SenderId != Cache.AccountInfo.Id)
            {
                Extensions.SetImageSource(arrowIcon, "/Forgotten Land Launcher;component/Assets/icons/icon_arrow_right_2_a.png", UriKind.Relative);
                RotateTransform rotateTransform = new RotateTransform(180);
                arrowIcon.RenderTransform = rotateTransform;

                Extensions.SetImageBrushSource(avatarHolder, pMessagee.SenderAvatarUrl, UriKind.Absolute);
                receiverHolder.Text = pMessagee.SenderNickname;
            }
            else // if we are the sender
            {
                Extensions.SetImageBrushSource(avatarHolder, pMessagee.ReceiverAvatarUrl, UriKind.Absolute);
                receiverHolder.Text = pMessagee.ReceiverNickname;
            }

            titleHolder.Text = pMessagee.Title;
            dateHolder.Text = pMessagee.DateEdited;
            messageHolder.Text = pMessagee.Message;

            spSeen.Visibility = pMessagee .Seen? Visibility.Visible : Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                launcher.DisplayPrivateMessageThread(pMessagee.Id);
            }
        }
    }
}
