/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using HtmlAgilityPack;
using Forgotten_Land_Launcher.Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Web;

namespace Forgotten_Land_Launcher.Modules.News
{
    /// <summary>
    /// Interaction logic for Article_Card.xaml
    /// </summary>
    public partial class Article_Card : Button
    {
        private Newton_Main.WebsiteArticlesResponse pArticle;

        public Article_Card(Newton_Main.WebsiteArticlesResponse article)
        {
            InitializeComponent();
            pArticle = article;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pArticle.PictureUrl.ToLower().EndsWith(".webp") ||
                    pArticle.PictureUrl.ToLower().EndsWith(".png") ||
                    pArticle.PictureUrl.ToLower().EndsWith(".jpg") ||
                    pArticle.PictureUrl.ToLower().EndsWith(".jpeg"))
                {
                    Extensions.SetImageSource(imageLoader, pArticle.PictureUrl, UriKind.Absolute);
                }
                else
                {
                    LoadMedia(pArticle.PictureUrl.Replace("https://", "http://")); // fixes security issues
                }

                titleHolder.Text = pArticle.Headline_EN;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml($"<body>{pArticle.Content_EN}</body>");

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                if (bodyNode != null)
                {
                    // Decode HTML entities explicitly
                    var decodedText = HtmlEntity.DeEntitize(bodyNode.InnerText);
                    contentHolder.Text = decodedText;
                }

                if (string.IsNullOrEmpty(pArticle.RedirectUrl) || string.IsNullOrWhiteSpace(pArticle.RedirectUrl))
                    Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                launcher.EnableAllMenuButtons();
                await launcher.UnloadPages();

                launcher.pageArticleViewer.Load(pArticle);
            }
        }

        private void LoadMedia(string url)
        {
            try
            {
                movieLoader.Source = new Uri(url);
                movieLoader.Position = TimeSpan.Zero;
                movieLoader.Play();
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), $"video url \"{url}\"", null, ex, false);
            }
        }

        private void movieLoader_MediaEnded(object sender, RoutedEventArgs e)
        {
            movieLoader.Position = TimeSpan.Zero;
            movieLoader.Play();
        }
    }
}
