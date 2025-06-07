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

namespace Forgotten_Land_Launcher.Modules.Private_Messages
{
    /// <summary>
    /// Interaction logic for Message_Span.xaml
    /// </summary>
    public partial class Message_Span : UserControl
    {
        private Newton_Main.PrivateMessageThreadResponse pMessage;

        public Message_Span(Newton_Main.PrivateMessageThreadResponse message)
        {
            InitializeComponent();
            pMessage = message;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            senderHolder.Text = pMessage.SenderNickname;

            Extensions.SetImageBrushSource(avatarHolder, pMessage.SenderAvatarUrl, UriKind.Absolute);

            messageHolder.Text = pMessage.Message;

            dateHolder.Text = pMessage.DateEdited;
        }
    }
}
