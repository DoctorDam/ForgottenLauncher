/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Login_Rewards
{
    /// <summary>
    /// Interaction logic for Reward_Span.xaml
    /// </summary>
    public partial class Reward_Span : Button
    {
        private string pTitle;
        private string pDescription;
        private string pPictureUrl;
        private int pMonth;
        private int pDay;
        private bool pClaimed;
        private int pRewardId = 0;

        public Reward_Span(string title, string description, string picture_url, int month, int day, bool claimed, int reward_id)
        {
            InitializeComponent();
            pTitle = title;
            pDescription = description;
            pPictureUrl = picture_url;
            pMonth = month;
            pDay = day;
            pClaimed = claimed;
            pRewardId = reward_id;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Extensions.SetImageBrushSource(pictureHolder, pPictureUrl, UriKind.Absolute);
            dayHolder.Text = AddSuffix(pDay);

            if (pDay < DateTime.Now.Day)
            {
                IsEnabled = false;
                claimOverlayHolder.Visibility = Visibility.Visible;

                if (pRewardId != 0)
                {
                    if (pClaimed)
                    {
                        Extensions.SetImageSource(claimOverlayHolder, "/Forgotten Land Launcher;component/Assets/login_claimed_overlay.png", UriKind.Relative);
                    }
                    else
                    {
                        Extensions.SetImageSource(claimOverlayHolder, "/Forgotten Land Launcher;component/Assets/login_unclaimed_overlay.png", UriKind.Relative);
                    }
                }
                else
                {
                    Extensions.SetImageSource(claimOverlayHolder, "/Forgotten Land Launcher;component/Assets/login_undefined_reward_day.png", UriKind.Relative);
                }

                Opacity = 0.6;
            }
            else
            {
                if (pClaimed)
                {
                    IsEnabled = false;
                    claimOverlayHolder.Visibility = Visibility.Visible;
                    Extensions.SetImageSource(claimOverlayHolder, "/Forgotten Land Launcher;component/Assets/login_claimed_overlay.png", UriKind.Relative);
                }
                else
                {
                    if (pRewardId != 0 && DateTime.Now.Day == pDay)
                    {
                        claimNowHolder.Visibility = Visibility.Visible;
                        mainButton.BorderBrush = Extensions.GetColorFromHex("#FFFF7500");
                    }
                    else
                    {
                        IsEnabled = false;
                    }
                }

                if (pDay > DateTime.Now.Day)
                    Opacity = 0.3;
            }
        }

        static string AddSuffix(int number)
        {
            // Check for special cases (11, 12, and 13)
            if (number % 100 >= 11 && number % 100 <= 13)
            {
                return $"{number}th";
            }

            // Determine the suffix based on the last digit
            switch (number % 10)
            {
                case 1:
                    return $"{number}st";
                case 2:
                    return $"{number}nd";
                case 3:
                    return $"{number}rd";
                default:
                    return $"{number}th";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher mainWindow)
            {
                Login_Reward_Dialog lrd = new Login_Reward_Dialog(pTitle, pDescription, pPictureUrl, pMonth, pDay);
                lrd.ClipToBounds = true;

                Panel.SetZIndex(lrd, 10000);

                lrd.OnConfirmationChanged += (os, be) =>
                {
                    if (lrd.IsClaimed)
                    {
                        mainWindow.pageLoginRewards.Load();
                    }
                };
                mainWindow.mainGrid.Children.Add(lrd);
            }
        }
    }
}
