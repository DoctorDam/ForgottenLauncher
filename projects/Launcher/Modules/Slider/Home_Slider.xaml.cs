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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Modules.Slider
{
    /// <summary>
    /// Interaction logic for Home_Slider.xaml
    /// </summary>
    public partial class Home_Slider : UserControl
    {
        private List<Newton_Main.SliderResponse> sliderResponse = new List<Newton_Main.SliderResponse>();

        private int slider_index;

        private DispatcherTimer slider_timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(Properties.Settings.Default.SliderDelaySeconds),
        };

        public Home_Slider()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            sliderResponse = await Newton_Peek.SliderResponse();
            StartSlider();
        }

        private void StartSlider()
        {
            slider_index = 0;

            SpawnDots();

            SetSelectedVisualDot(slider_index);
            DisplayTitleIndex(slider_index);
            DisplayDescriptionIndex(slider_index);
            DisplayBackgroundIndex(slider_index);

            slider_timer.Tick += Slider_timer_Tick;

            slider_timer.Start();
        }

        private void Slider_timer_Tick(object sender, EventArgs e)
        {
            slider_index = (slider_index + 1) % sliderResponse.Count;

            DisplayBackgroundIndex(slider_index);
            DisplayTitleIndex(slider_index);
            DisplayDescriptionIndex(slider_index);
            SetSelectedVisualDot(slider_index);
        }

        private void SpawnDots()
        {
            spSliderDots.Children.Clear();

            int index = 0;

            try
            {
                foreach (Newton_Main.SliderResponse slide in sliderResponse)
                {
                    Slider_Dot sliderDot = new Slider_Dot() { Slider_Index = index };

                    sliderDot.Click += (s, e) =>
                    {
                        slider_timer.Stop();

                        slider_index = sliderDot.Slider_Index;

                        DisplayBackgroundIndex(sliderDot.Slider_Index);
                        DisplayTitleIndex(slider_index);
                        DisplayDescriptionIndex(slider_index);
                        SetSelectedVisualDot(sliderDot.Slider_Index);

                        slider_timer.Start();
                    };

                    spSliderDots.Children.Add(sliderDot);

                    index++;
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), $"slider index {index}", null, ex, false);
            }
        }

        private void SetSelectedVisualDot(int index)
        {
            foreach (Slider_Dot dot in spSliderDots.Children.OfType<Slider_Dot>())
            {
                dot.IsEnabled = dot.Slider_Index != index;
            }
        }

        private void DisplayBackgroundIndex(int index)
        {
            try
            {
                Extensions.SetImageBrushSource(ArtworkBackgroundHolder, sliderResponse[index].ImageUrl, UriKind.Absolute);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), $"slider index {index}", null, ex, false);
            }
        }

        private void DisplayTitleIndex(int index)
        {
            try
            {
                titleHolder.Text = sliderResponse[index].Title.ToUpper();
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), $"slider index {index}", null, ex, false);
            }
        }

        private void DisplayDescriptionIndex(int index)
        {
            try
            {
                descriptionHolder.Text = sliderResponse[index].Description;
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), $"slider index {index}", null, ex, false);
            }
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            slider_timer.Stop();

            slider_index = (slider_index - 1) % sliderResponse.Count;

            if (slider_index < 0)
            {
                slider_index = sliderResponse.Count - 1;
            }

            DisplayBackgroundIndex(slider_index);
            DisplayTitleIndex(slider_index);
            DisplayDescriptionIndex(slider_index);
            SetSelectedVisualDot(slider_index);

            slider_timer.Start();
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            slider_timer.Stop();

            slider_index = (slider_index + 1) % sliderResponse.Count;

            if (slider_index > sliderResponse.Count - 1)
            {
                slider_index = 0;
            }

            DisplayBackgroundIndex(slider_index);
            DisplayTitleIndex(slider_index);
            DisplayDescriptionIndex(slider_index);
            SetSelectedVisualDot(slider_index);

            slider_timer.Start();
        }

        private void redirectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(sliderResponse[slider_index].RedirectUrl);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), $"slider index {slider_index}", null, ex, false);
            }
        }
    }
}
