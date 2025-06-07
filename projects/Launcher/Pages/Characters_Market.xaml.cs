/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Dialogs;
using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Characters_Market;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Characters_Market.xaml
    /// </summary>
    public partial class Characters_Market : UserControl
    {
        public Characters_Market()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            Cache.charactersMarketSettings = await Newton_Peek.CharactersMarketSettings();

            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.CharactersMarket.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeCharactersMarket();
                HandleBidsNotifications();
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

        public void ClearNativeChilds()
        {
            var childrenToRemove = new List<Character_Card>();

            foreach (var child in wpCharacters.Children)
            {
                if (child is Character_Card)
                {
                    childrenToRemove.Add(child as Character_Card);
                }
            }

            foreach (var childToRemove in childrenToRemove)
            {
                wpCharacters.Children.Remove(childToRemove);
            }
        }

        private async void InitializeCharactersMarket()
        {
            ClearNativeChilds();
            RealmsDropdown.Items.Clear();

            try
            {
                var marketList = await Newton_Peek.CharactersMarketList();

                if (marketList != null)
                {
                    Dictionary<int, string> unique_realms = new Dictionary<int, string>();

                    foreach (var sale in marketList)
                    {
                        wpCharacters.Children.Add(new Character_Card(sale));

                        if (!unique_realms.ContainsKey(sale.RealmId))
                        {
                            unique_realms.Add(sale.RealmId, sale.RealmName);
                        }
                    }

                    if (unique_realms.Count > 0)
                    {
                        RealmsDropdown.Items.Add(new ComboBoxItem()
                        {
                            Content = "All Realms",
                            Tag = 0,
                        });

                        foreach (var realm in unique_realms)
                        {
                            RealmsDropdown.Items.Add(new ComboBoxItem()
                            {
                                Content = realm.Value,
                                Tag = realm.Key,
                            });
                        }

                        RealmsDropdown.IsEnabled = true;
                        RealmsDropdown.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private async void HandleBidsNotifications()
        {
            try
            {
                var notifications = await Newton_Peek.CharactersMarketBidsNotifications();

                if (notifications != null)
                {
                    if (Application.Current.MainWindow is Launcher launcher)
                    {
                        if (notifications.Count > 0)
                        {
                            var notifications_dialog = new Character_Market_Notifications_Dialog(notifications);

                            notifications_dialog.ClipToBounds = true;

                            Panel.SetZIndex(notifications_dialog, 10000);

                            launcher.mainGrid.Children.Add(notifications_dialog);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private void searchHolder_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is UIElement element)
            {
                e.Handled = true;

                var timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(10)
                };

                timer.Tick += (a1, a2) =>
                {
                    timer.IsEnabled = false;

                    if (element is TextBox textBox)
                    {
                        textBox.SelectAll();
                    }

                    else if (element is PasswordBox passwordBox)
                    {
                        passwordBox.SelectAll();
                    }
                };

                timer.IsEnabled = true;
            }
        }

        private void searchHolder_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(searchHolder.Text) || string.IsNullOrWhiteSpace(searchHolder.Text))
                searchHolder.Text = Lang.ResourceManager.GetString("page_characters_market_textbox_player_search", 
                    CultureInfo.CurrentUICulture);
        }

        private void RealmsDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem selectedRealm = RealmsDropdown.SelectedItem as ComboBoxItem;

                int selectedRealmID = int.Parse(selectedRealm.Tag.ToString());

                string selectedRealmName = selectedRealm.Content.ToString();

                string defaultSearchText = Lang.ResourceManager.GetString("page_characters_market_textbox_player_search", 
                    CultureInfo.CurrentUICulture);

                if (selectedRealmID == 0)
                {
                    if (searchHolder.Text != defaultSearchText)
                    {
                        foreach (Character_Card card in wpCharacters.Children.OfType<Character_Card>())
                        {
                            card.Visibility = card.CharNameHolder.Text.ToLower().Contains(searchHolder.Text.ToLower()) ? Visibility.Visible : Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        foreach (Character_Card card in wpCharacters.Children.OfType<Character_Card>())
                        {
                            card.Visibility = Visibility.Visible;
                        }
                    }
                }
                else
                {
                    foreach (Character_Card card in wpCharacters.Children.OfType<Character_Card>())
                    {
                        if (searchHolder.Text != defaultSearchText)
                        {
                            card.Visibility = (selectedRealmName == card.RealmHolder.Text && card.CharNameHolder.Text.ToLower().Contains(searchHolder.Text.ToLower())) ? Visibility.Visible : Visibility.Collapsed;
                        }
                        else
                        {
                            card.Visibility = (selectedRealmName == card.RealmHolder.Text) ? Visibility.Visible : Visibility.Collapsed;
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string defaultSearchText = Lang.ResourceManager.GetString("page_characters_market_textbox_player_search", 
                CultureInfo.CurrentUICulture);

            if (searchHolder.Text != defaultSearchText)
            {
                foreach (Character_Card card in wpCharacters.Children.OfType<Character_Card>())
                {
                    if (card.CharNameHolder.Text.ToLower().Contains(searchHolder.Text.ToLower()))
                    {
                        card.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        card.Visibility = Visibility.Collapsed;
                    }
                }
            }  
            else
            {
                RealmsDropdown_SelectionChanged(sender, null);
            }
        }

        private void mySalesButton_Click(object sender, RoutedEventArgs e)
        {
            RealmsDropdown.SelectedIndex = 0;

            searchHolder.Text = Lang.ResourceManager.GetString("page_characters_market_textbox_player_search", 
                CultureInfo.CurrentUICulture);

            foreach (Character_Card card in wpCharacters.Children.OfType<Character_Card>())
            {
                card.Visibility = card.pSaleInfo.OwnerAccountId == Cache.AccountInfo.Id ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void sellButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                Character_Market_Sell_Dialog sell_dialog = new Character_Market_Sell_Dialog();

                sell_dialog.ClipToBounds = true;

                Panel.SetZIndex(sell_dialog, 10000);

                sell_dialog.OnConfirmationChanged += (os, be) =>
                {
                    if (sell_dialog.AddedToMarket)
                    {
                        ClearNativeChilds();
                        Load();
                    }
                };

                launcher.mainGrid.Children.Add(sell_dialog);
            }
        }
    }
}
