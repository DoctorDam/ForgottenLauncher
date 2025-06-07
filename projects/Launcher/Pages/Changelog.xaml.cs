/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Changelogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Changelog.xaml
    /// </summary>
    public partial class Changelog : UserControl
    {
        public Changelog()
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

            if (Cache.PageOptions.Changelogs.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeChangelogs();
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
            var Changelog_Day_childrenToRemove = new List<Day_Span>();

            foreach (var child in spChangelogs.Children)
            {
                if (child is Day_Span)
                {
                    Changelog_Day_childrenToRemove.Add(child as Day_Span);
                }
            }

            foreach (var childToRemove in Changelog_Day_childrenToRemove)
            {
                spChangelogs.Children.Remove(childToRemove);
            }

            var Changelog_Row_childrenToRemove = new List<Changelog_Span>();

            foreach (var child in spChangelogs.Children)
            {
                if (child is Changelog_Span)
                {
                    Changelog_Row_childrenToRemove.Add(child as Changelog_Span);
                }
            }

            foreach (var childToRemove in Changelog_Row_childrenToRemove)
            {
                spChangelogs.Children.Remove(childToRemove);
            }
        }

        private async void InitializeChangelogs()
        {
            ClearNativeChilds();

            var changelogs = await Newton_Peek.LauncherChangelogsResponse();

            if (changelogs != null)
            {
                // Sort the changelogs by date in descending order (newest to oldest)
                changelogs = changelogs.OrderByDescending(c => DateTimeOffset.FromUnixTimeSeconds(c.DateTimestamp).DateTime).ToList();

                DateTime? currentDate = null; // Track the current date

                foreach (Newton_Main.WebsiteChangelogsResponse changelog in changelogs)
                {
                    DateTime changelogDate = DateTimeOffset.FromUnixTimeSeconds(changelog.DateTimestamp).DateTime;

                    // Check if the date has changed
                    if (!currentDate.HasValue || currentDate.Value.Date != changelogDate.Date)
                    {
                        // Display a new Changelog_Day for the new date
                        spChangelogs.Children.Add(new Day_Span(changelogDate.ToString("d MMMM yyyy")));
                        currentDate = changelogDate;
                    }

                    // Display the Changelog_Row for each changelog
                    spChangelogs.Children.Add(new Changelog_Span(changelog.CategoryName, changelog.Description));
                }
            }
        }
    }
}
