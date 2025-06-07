/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Vote;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Vote.xaml
    /// </summary>
    public partial class Vote : UserControl
    {
        public Vote()
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

            if (Cache.PageOptions.Vote.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeVoteSites();
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
            var childrenToRemove = new List<Vote_Span>();

            foreach (var child in spVoteSites.Children)
            {
                if (child is Vote_Span)
                {
                    childrenToRemove.Add(child as Vote_Span);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                spVoteSites.Children.Remove(childToRemove);
            }
        }

        private async void InitializeVoteSites()
        {
            ClearNativeChilds();

            var voteSites = await Newton_Peek.VoteSitesResponse();

            if (voteSites != null)
            {
                foreach (Newton_Main.VoteSitesResponse vote_site in voteSites)
                {
                    spVoteSites.Children.Add(new Vote_Span(vote_site));
                }
            }
        }
    }
}
