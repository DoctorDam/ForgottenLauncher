/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System.Windows;
using System;
using System.Diagnostics;

namespace Forgotten_Land_Launcher.Library
{
    internal class Page_Options
    {
        public static async void LoadSettings()
        {
            Cache.PageOptions = await Newton_Peek.PagesOptions();
        }

        public static async void ApplySettings()
        {
            try
            {
                Cache.PageOptions = await Newton_Peek.PagesOptions();

                if (Application.Current.MainWindow is Launcher launcher)
                {
                    launcher.btnInventory.Visibility = Cache.PageOptions.AccountInventory.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.btnMessages.Visibility = Cache.PageOptions.PrivateMessages.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.btnLoginRewards.Visibility = Cache.PageOptions.LoginRewards.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.btnNavNews.Visibility = Cache.PageOptions.News.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.btnNavEvents.Visibility = Cache.PageOptions.Events.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.btnNavDiscord.Visibility = Cache.PageOptions.Discord.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.btnNavChangelog.Visibility = Cache.PageOptions.Changelogs.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.lmButtonShop.Visibility = Cache.PageOptions.Shop.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.lmButtonCharactersMarket.Visibility = Cache.PageOptions.CharactersMarket.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.lmButtonAddons.Visibility = Cache.PageOptions.Addons.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.lmButtonVote.Visibility = Cache.PageOptions.Vote.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;
                    
                    launcher.lmButtonOnlinePlayers.Visibility = Cache.PageOptions.OnlinePlayers.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.lmButtonLadderboard.Visibility = Cache.PageOptions.Ladderboard.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;

                    launcher.lmButtonFAQ.Visibility = Cache.PageOptions.Faq.ButtonVisible ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }
    }
}
