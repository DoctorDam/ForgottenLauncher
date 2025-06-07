/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Faq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Faq.xaml
    /// </summary>
    public partial class Faq : UserControl
    {
        public Faq()
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

            if (Cache.PageOptions.Faq.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeFaq();
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
            var childrenToRemove = new List<Faq_Span>();

            foreach (var child in spFaq.Children)
            {
                if (child is Faq_Span)
                {
                    childrenToRemove.Add(child as Faq_Span);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                spFaq.Children.Remove(childToRemove);
            }
        }

        private async void InitializeFaq()
        {
            ClearNativeChilds();

            var launcherFaq = await Newton_Peek.LauncherFaqResponse();

            if (launcherFaq != null)
            {
                foreach (var faq in launcherFaq)
                {
                    spFaq.Children.Add(new Faq_Span(faq));
                }
            }

            await Task.Delay(10);

            foreach (Faq_Span faqRow in spFaq.Children.OfType<Faq_Span>())
            {
                faqRow.CollapseToggle();
            }
        }
    }
}
