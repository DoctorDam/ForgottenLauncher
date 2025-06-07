/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : UserControl
    {
        public Events()
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

            if (Cache.PageOptions.Events.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeEvents();
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
            var childrenToRemove = new List<Events_Card>();

            foreach (var child in spEvents.Children)
            {
                if (child is Events_Card)
                {
                    childrenToRemove.Add(child as Events_Card);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                spEvents.Children.Remove(childToRemove);
            }
        }

        private async void InitializeEvents()
        {
            ClearNativeChilds();

            var events = await Newton_Peek.LauncherEventsResponse();

            if (events != null)
            {
                foreach (var m_event in events)
                {
                    spEvents.Children.Add(new Events_Card(m_event));
                }
            }
        }
    }
}
