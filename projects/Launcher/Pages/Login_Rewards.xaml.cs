/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Login_Rewards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Login_Rewards.xaml
    /// </summary>
    public partial class Login_Rewards : UserControl
    {
        public Login_Rewards()
        {
            InitializeComponent();
        }
        public async void Load()
        {
            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.LoginRewards.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeLoginRewards();
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
            var childrenToRemove = new List<Reward_Span>();

            foreach (var child in wpLoginRewards.Children)
            {
                if (child is Reward_Span)
                {
                    childrenToRemove.Add(child as Reward_Span);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                wpLoginRewards.Children.Remove(childToRemove);
            }
        }

        private async void InitializeLoginRewards()
        {
            ClearNativeChilds();

            monthHolder.Text = DateTime.Now.ToString("MMMM").ToUpper();

            var loginRewards = await Newton_Peek.LoginRewardsResponse();

            if (loginRewards != null)
            {
                int currentMonthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

                for (int day = 1; day <= currentMonthDays; day++)
                {
                    var loginReward = loginRewards.Find(lr => lr.Day == day);

                    string title = loginReward?.Title ?? string.Empty;
                    string description = loginReward?.Description ?? string.Empty;
                    string picture_url = loginReward?.PictureUrl ?? string.Empty;
                    int month = loginReward?.Month ?? 0;
                    bool claimed = loginReward?.Claimed ?? false;
                    int rewardid = loginReward?.Id ?? 0;
                    bool req_player = loginReward?.RequiresPlayer ?? false;
                    bool req_input = loginReward?.RequiresInput ?? false;

                    wpLoginRewards.Children.Add(new Reward_Span(title, description, picture_url, month, day, claimed, rewardid));
                }
            }
        }
    }
}
