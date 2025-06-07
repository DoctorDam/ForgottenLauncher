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
using System.Windows.Media.Animation;

namespace Forgotten_Land_Launcher.Modules.Faq
{
    /// <summary>
    /// Interaction logic for Faq_Span.xaml
    /// </summary>
    public partial class Faq_Span : UserControl
    {
        private Newton_Main.LauncherFaqResponse pFAQ;
        private bool isElementCollapsed = false;

        public Faq_Span(Newton_Main.LauncherFaqResponse faq)
        {
            InitializeComponent();
            pFAQ = faq;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            faqTitleHolder.Text = pFAQ.Title;
            faqTextHolder.Text = pFAQ.Text;
        }

        private void btnFaqTitle_Click(object sender, RoutedEventArgs e) => CollapseToggle();

        public void CollapseToggle()
        {
            if (isElementCollapsed)
            {
                // Expand the element
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = 0,
                    To = (double?)faqTextHolder.Tag, // Use the original height stored in Tag
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                faqTextHolder.BeginAnimation(FrameworkElement.HeightProperty, animation);
            }
            else
            {
                // Collapse the element
                faqTextHolder.Tag = faqTextHolder.ActualHeight; // Store the current height in Tag
                DoubleAnimation animation = new DoubleAnimation
                {
                    From = faqTextHolder.ActualHeight,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                faqTextHolder.BeginAnimation(FrameworkElement.HeightProperty, animation);
            }

            // Toggle the collapse state
            isElementCollapsed = !isElementCollapsed;
        }
    }
}
