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
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        public void Load()
        {
            Animations.FadeIn(this, 300);
            InitializeBottomArticles();
            //InitializeRightSideArticles();
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            wpArticlesBottom.Children.Clear();
            //spRightSideArticles.Children.Clear();
        }

        private async void InitializeBottomArticles()
        {
            wpArticlesBottom.Children.Clear();

            var articles = await Newton_Peek.LauncherArticlesResponse(1);

            if (articles != null)
            {
                foreach (var article in articles)
                {
                    wpArticlesBottom.Children.Add(new Article_Card(article));
                }
            }
        }

        //public async void InitializeRightSideArticles()
        //{
        //    spRightSideArticles.Children.Clear();

        //    var articles = await Newton_Peek.LauncherArticlesResponse(2);

        //    if (articles != null)
        //    {
        //        foreach (var article in articles)
        //        {
        //            spRightSideArticles.Children.Add(new Article_Small_Span(article));
        //        }
        //    }
        //}
    }
}
