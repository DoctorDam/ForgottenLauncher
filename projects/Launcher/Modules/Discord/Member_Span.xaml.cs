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

namespace Forgotten_Land_Launcher.Modules.Discord
{
    /// <summary>
    /// Interaction logic for Member_Span.xaml
    /// </summary>
    public partial class Member_Span : UserControl
    {
        private Newton_Main.DiscordMember pMember;

        public Member_Span(Newton_Main.DiscordMember member)
        {
            InitializeComponent();
            pMember = member;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (pMember.Status)
            {
                case "online":
                    status1Holder.Fill = Brushes.Lime;
                    break;

                case "dnd":
                    status1Holder.Fill = Brushes.IndianRed;
                    break;

                case "idle":
                    status1Holder.Fill = Brushes.Orange;
                    break;

                default:
                    break;
            }

            status2Holder.Text = pMember.Status;

            Extensions.SetImageBrushSource(avatarHolder, pMember.AvatarUrl, UriKind.Absolute);

            usernameHolder.Text = pMember.Username;

            if (pMember.Game != null)
            {
                if (!string.IsNullOrEmpty(pMember.Game.Name) && !string.IsNullOrWhiteSpace(pMember.Game.Name))
                {
                    gamePrefix.Text = "-";
                    gameHolder.Text = $"playing {pMember.Game.Name}";
                    return;
                }
            }

            gamePrefix.Text = string.Empty;
            gameHolder.Text = string.Empty;
        }
    }
}
