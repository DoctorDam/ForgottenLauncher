/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Dialogs;
using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Modules.Vote
{
    /// <summary>
    /// Interaction logic for Vote_Span.xaml
    /// </summary>
    public partial class Vote_Span : UserControl
    {
        private Newton_Main.VoteSitesResponse pVoteSite;
        private DispatcherTimer timer;

        public Vote_Span(Newton_Main.VoteSitesResponse vote_site)
        {
            InitializeComponent();
            pVoteSite = vote_site;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            nameHolder.Text = pVoteSite.Title;

            if (pVoteSite.SecondsLeft > 0)
            {
                int hours = pVoteSite.SecondsLeft / 3600;
                int minutes = (pVoteSite.SecondsLeft % 3600) / 60;
                int seconds = pVoteSite.SecondsLeft % 60;

                string formattedTime = "";

                if (hours > 0)
                {
                    formattedTime += $"{hours}h ";
                }

                if (minutes > 0 || hours > 0)
                {
                    formattedTime += $"{minutes}m ";
                }

                formattedTime += $"{seconds}s";

                cooldownHolder.Text = $"{formattedTime}"; // add live timer

                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            else
            {
                cooldownHolder.Text = Lang.ResourceManager.GetString("vote_text_ready", CultureInfo.CurrentUICulture);
                cooldownHolder.Foreground = Brushes.Lime;
                voteButton.IsEnabled = true;
            }

            Extensions.SetImageBrushSource(imageHolder, pVoteSite.ImageUrl, UriKind.Absolute);
            pointsRewardHolder.Text = pVoteSite.PointsReward.ToString();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pVoteSite.SecondsLeft > 0)
            {
                int hours = pVoteSite.SecondsLeft / 3600;
                int minutes = (pVoteSite.SecondsLeft % 3600) / 60;
                int seconds = pVoteSite.SecondsLeft % 60;

                string formattedTime = "";

                if (hours > 0)
                {
                    formattedTime += $"{hours}h ";
                }

                if (minutes > 0 || hours > 0)
                {
                    formattedTime += $"{minutes}m ";
                }

                formattedTime += $"{seconds}s";

                cooldownHolder.Text = formattedTime;

                pVoteSite.SecondsLeft--;
            }
            else
            {
                timer.Stop();
                cooldownHolder.Text = Lang.ResourceManager.GetString("vote_text_ready", CultureInfo.CurrentUICulture);
                cooldownHolder.Foreground = Brushes.Lime;
                voteButton.IsEnabled = true;
            }
        }

        private void voteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                Confirmation_Dialog simple_Confirmation_Dialog = new Confirmation_Dialog
                (
                    Lang.ResourceManager.GetString("vote_dialog_title", CultureInfo.CurrentUICulture),
                    Lang.ResourceManager.GetString("vote_dialog_text", CultureInfo.CurrentUICulture).Replace("{0}", nameHolder.Text)
                );

                simple_Confirmation_Dialog.ClipToBounds = true;

                Panel.SetZIndex(simple_Confirmation_Dialog, 10000);

                simple_Confirmation_Dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (simple_Confirmation_Dialog.IsConfirmed)
                    {
                        Newton_Main.VoteResponse voteResponse = await Newton_Peek.VoteResponse(pVoteSite.Id);

                        if (voteResponse != null)
                        {
                            if (voteResponse.Voted)
                            {
                                launcher.votePointsHolder.Text = (int.Parse(launcher.votePointsHolder.Text.Replace(",", string.Empty)) + voteResponse.Points).ToString();
                                Process.Start(pVoteSite.VoteUrl);
                                launcher.pageVote.Load();
                            }
                        }
                        else
                        {
                            Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
                        }
                    }
                };

                launcher.mainGrid.Children.Add(simple_Confirmation_Dialog);
            }
        }
    }
}
