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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Article_Viewer.xaml
    /// </summary>
    public partial class Article_Viewer : UserControl
    {
        public Article_Viewer()
        {
            InitializeComponent();
        }

        public async void Load(Newton_Main.WebsiteArticlesResponse article)
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
                InitializeArticle(article);
            }
            else
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Visible;
            }
        }

        public void Unload()
        {
            svArticle.Visibility = Visibility.Collapsed;
            Animations.FadeOut(this, 300);
        }


        private void InitializeArticle(Newton_Main.WebsiteArticlesResponse article)
        {
            svArticle.Visibility = Visibility.Visible;

            try
            {
                if (article.PictureUrl.ToLower().EndsWith(".webp"))
                {
                    Extensions.SetImageBrushSource(bannerHolder, article.PictureUrl, UriKind.Absolute);
                }
                else
                {
                    LoadMedia(article.PictureUrl.Replace("https://", "http://")); // fixes security issues
                }

                titleHolder.Text = article.Headline_EN;
                dateHolder.Text = article.Date;
                authorHolder.Text = article.Author;

                ButtonRedirect.Tag = article.RedirectUrl;

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml($"<body>{article.Content_EN}</body>");

                var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                if (bodyNode != null)
                {
                    // Decode HTML entities explicitly
                    var decodedText = HtmlEntity.DeEntitize(bodyNode.InnerText);

                    decodedText = decodedText.Replace("&nbsp;", " "); // Replace non-breaking spaces with regular spaces

                    var lines = decodedText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    var formattedText = string.Join(Environment.NewLine + Environment.NewLine, lines);

                    contentHolder.Text = formattedText;
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        private void ButtonRedirect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(ButtonRedirect.Tag.ToString());
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
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
