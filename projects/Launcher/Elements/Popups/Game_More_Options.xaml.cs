/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Elements.Spinners;
using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Realms;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Elements.Popups
{
    /// <summary>
    /// Interaction logic for Game_More_Options.xaml
    /// </summary>
    public partial class Game_More_Options : UserControl
    {
        public Game_More_Options()
        {
            InitializeComponent();
            InitializeRealmsStatus();
        }

        public void Toggle()
        {
            switch (Visibility)
            {
                case Visibility.Visible:
                {
                    Animations.FadeOut(this, 300);
                    break;
                }
                default:
                {
                    Animations.MoveUpAndFadeIn(this, 1000, 1000);
                    break;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // selected language
            try // used try for the buggy visual studio designer window
            {
                selectedLangHolder.Text = Language_Book.Cultures[Launcher_Settings.InterfaceLang];
            }
            catch
            {
                selectedLangHolder.Text = "Unknown";
            }

            // populate languages list
            spLanguages.Children.Clear();
            foreach (var lang in Language_Book.Cultures)
            {
                Button button = new Button()
                {
                    Width = 230,
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Style = (Style)Application.Current.Resources["btnPopupMenuItem"],
                    Content = new TextBlock()
                    {
                        FontSize = 14,
                        Padding = new Thickness(5, 0, 0, 0),
                        Text = lang.Value,
                    },
                };

                button.Click += (ss, ee) =>
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang.Key);
                    Launcher_Settings.InterfaceLang = lang.Key;
                    Launcher_Settings.Save();

                    UserControl_Loaded(sender, e);
                    Animations.FadeOut(spLanguageSelection, 300);

                    Extensions.ShowPopup("Interface Language", "Interface Language changed", Application.Current.MainWindow?.GetType());
                };

                spLanguages.Children.Add(button);
            }

            hdStatusHolder.Text = Launcher_Settings.HDTextures ? 
                Lang.ResourceManager.GetString("game_popup_more_toggle_status_enabled", CultureInfo.CurrentUICulture) :
                Lang.ResourceManager.GetString("game_popup_more_toggle_status_disabled", CultureInfo.CurrentUICulture);

            hdStatusHolder.Foreground = Extensions.GetColorFromHex(Launcher_Settings.HDTextures ? "#FF00FF00" : "#FFFF0000");

            // cache
            cacheStatusHolder.Text = Launcher_Settings.ClearCache ?
                Lang.ResourceManager.GetString("game_popup_more_toggle_status_on", CultureInfo.CurrentUICulture) :
                Lang.ResourceManager.GetString("game_popup_more_toggle_status_off", CultureInfo.CurrentUICulture);

            cacheStatusHolder.Foreground = Extensions.GetColorFromHex(Launcher_Settings.ClearCache ? "#FF00FF00" : "#FFFF0000");

            // update options
            updateOptionHolder.Text = Launcher_Settings.CheckFullGame ?
                Lang.ResourceManager.GetString("game_popup_more_toggle_full_game", CultureInfo.CurrentUICulture) :
                Lang.ResourceManager.GetString("game_popup_more_toggle_only_updates", CultureInfo.CurrentUICulture);

            // auto login
            autoLoginStatusHolder.Text = Launcher_Settings.AutoLogin ?
                Lang.ResourceManager.GetString("game_popup_more_toggle_status_enabled", CultureInfo.CurrentUICulture) :
                Lang.ResourceManager.GetString("game_popup_more_toggle_status_disabled", CultureInfo.CurrentUICulture);

            autoLoginStatusHolder.Foreground = Extensions.GetColorFromHex(Launcher_Settings.AutoLogin ? "#FF00FF00" : "#FFFF0000");
        }

        private async void InitializeRealmsStatus()
        {
            spRealmsStatus.Children.Add(new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 25,
                Height = 25,
                Margin = new Thickness(0, 15, 0, 0),
            });

            var realmsStatusResponse = await Newton_Peek.RealmsStatusResponse();

            spRealmsStatus.Children.Clear();

            if (realmsStatusResponse != null)
            {
                foreach (Newton_Main.RealmsStatusResponse server in realmsStatusResponse)
                {
                    spRealmsStatus.Children.Add(new Server_Status_Span(server)
                    {
                        Margin = new Thickness(0, 15, 0, 0)
                    });
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(60)
            };

            timer.Tick += (ds, de) =>
            {
                timer.Stop();
                InitializeRealmsStatus();
            };
            timer.Start();
        }

        private void btnHDMenu_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(spHDClientMenu, 300);
        }

        private void btnHDEnable_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.HDTextures = true;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spHDClientMenu, 300);
        }

        private void btnHDDisable_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.HDTextures = false;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spHDClientMenu, 300);
        }

        private void btnAutoClesarCache_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(spClearCacheMenu, 300);
        }

        private void btnClearCacheOn_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.ClearCache = true;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spClearCacheMenu, 300);
        }

        private void btnClearCacheOff_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.ClearCache = false;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spClearCacheMenu, 300);
        }

        private void btnAutoLogin_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(spAutoLoginMenu, 300);
        }

        private void btnAutoLoginOn_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.AutoLogin = true;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spAutoLoginMenu, 300);
        }

        private void btnAutoLoginOff_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.AutoLogin = false;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spAutoLoginMenu, 300);
        }

        private void btnLocateGame_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                if (!launcher.isUpdatingGame)
                {
                    try
                    {
                        using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                        {
                            fbd.SelectedPath = Launcher_Settings.GamePath;
                            System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                            if (result.ToString() == "OK" && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                            {

                                Launcher_Settings.GamePath = fbd.SelectedPath;
                                Launcher_Settings.Save();

                                launcher.CheckForUpdates();
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Error_Handler.Justify(new StackTrace(), null, null, ex, true);
                    }
                }
            }
        }

        private void btnInterfaceLanguage_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(spLanguageSelection, 300);
        }

        private void btnUpdateOptions_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(spUpdateOptionsMenu, 300);
        }

        private void btnUpdateOptionsFull_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.CheckFullGame = true;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spUpdateOptionsMenu, 300);
        }

        private void btnUpdateOptionsOnlyUpdates_Click(object sender, RoutedEventArgs e)
        {
            Launcher_Settings.CheckFullGame = false;
            Launcher_Settings.Save();

            UserControl_Loaded(sender, e);
            Animations.FadeOut(spUpdateOptionsMenu, 300);
        }

        private void btnCheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                if (!launcher.isUpdatingGame && !launcher.isCheckingGame)
                {
                    launcher.CheckForUpdates();
                }
            }
        }
    }
}
