/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Ladderboard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Ladderboard.xaml
    /// </summary>
    public partial class Ladderboard : UserControl
    {
        public Ladderboard()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            tableHeader.Visibility = Visibility.Collapsed;
            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.Ladderboard.PageEnabled)
            {
                tableHeader.Visibility = Visibility.Visible;
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeLadderboard();
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
            spLadderboard.Children.Clear();
        }

        private async void InitializeLadderboard()
        {
            List<Newton_Main.LadderboardResponse> ladderboardResponse = await Newton_Peek.LadderboardResponse();

            spLadderboard.Children.Clear();

            if (ladderboardResponse != null)
            {
                var orderedLadderboard = ladderboardResponse.OrderByDescending(player => player.TotalKills);

                int pos = 1;

                foreach (var player in ladderboardResponse)
                {
                    Ladderboard_Row ladderboard_Row = new Ladderboard_Row(player);

                    ladderboard_Row.positionHolder.Text = pos.ToString();

                    spLadderboard.Children.Add(ladderboard_Row);

                    pos++;
                }
            }
        }
    }
}
