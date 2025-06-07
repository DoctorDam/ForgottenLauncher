/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.News;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for News.xaml
    /// </summary>
    public partial class News : UserControl
    {
        public News()
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

            if (Cache.PageOptions.News.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeArticles();
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
            var childrenToRemove = new List<Article_Medium_Span>();

            foreach (var child in spArticles.Children)
            {
                if (child is Article_Medium_Span)
                {
                    childrenToRemove.Add(child as Article_Medium_Span);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                spArticles.Children.Remove(childToRemove);
            }
        }

        private async void InitializeArticles()
        {
            ClearNativeChilds();

            var articles = await Newton_Peek.LauncherArticlesResponse(3);

            if (articles != null)
            {
                foreach (var article in articles)
                {
                    spArticles.Children.Add(new Article_Medium_Span(article));
                }
            }
        }
    }
}
