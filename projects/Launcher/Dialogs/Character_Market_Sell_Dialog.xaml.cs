/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Account_Inventory;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Character_Market_Sell_Dialog.xaml
    /// </summary>
    public partial class Character_Market_Sell_Dialog : UserControl
    {
        public event EventHandler<bool> OnConfirmationChanged;
        public bool AddedToMarket { get; private set; }

        public Character_Market_Sell_Dialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            spCharacters.Visibility = Visibility.Collapsed;
            spDuration.Visibility = Visibility.Collapsed;
            spPrice.Visibility = Visibility.Collapsed;
            Animations.FadeIn(this, 300);
            UpdateCharactersList();
        }

        private async void UpdateCharactersList()
        {
            CharactersDropdown.Items.Clear();
            DurationDropdown.Items.Clear();

            var charactersList = await Newton_Peek.CharactersListResponse();

            if (charactersList != null)
            {
                if (charactersList.Count > 0)
                {
                    CharactersDropdown.Items.Add(new TextBlock()
                    {
                        Text = Lang.ResourceManager.GetString
                        (
                            "dialog_inventory_item_text_require_character", 
                            CultureInfo.CurrentUICulture
                        ),
                        Foreground = Extensions.GetColorFromHex("#FFB5B5B5")
                    });

                    foreach (int duration in Cache.charactersMarketSettings.HoursDurations)
                    {
                        DurationDropdown.Items.Add(new TextBlock()
                        {
                            Foreground = Extensions.GetColorFromHex("#FFB5B5B5"),
                            Text = $"{duration}h",
                            Tag = duration
                        });
                    }

                    DurationDropdown.SelectedIndex = 0;

                    foreach (var character in charactersList)
                    {
                        CharactersDropdown.Items.Add(new Character_Dropdown_Row(character));
                    }

                    CharactersDropdown.SelectedIndex = 0;
                }
                else
                {
                    CharactersDropdown.Items.Add(new TextBlock()
                    {
                        Text = Lang.ResourceManager.GetString
                        (
                            "dialog_inventory_item_text_no_characters",
                            CultureInfo.CurrentUICulture
                        ),
                        Foreground = Extensions.GetColorFromHex("#FFB5B5B5")
                    });

                    CharactersDropdown.SelectedIndex = 0;
                }
            }

            spLoading.Visibility = Visibility.Collapsed;
            spCharacters.Visibility = Visibility.Visible;
            spDuration.Visibility = Visibility.Visible;
            spPrice.Visibility = Visibility.Visible;
        }

        private async void sellButton_Click(object sender, RoutedEventArgs e)
        {
            spItemDialog.Visibility = Visibility.Collapsed;
            spWaitDialog.Visibility = Visibility.Visible;

            await Task.Delay(500);

            try
            {
                var selected_character = (CharactersDropdown.SelectedItem as Character_Dropdown_Row).pCharacter;
                int duration = int.Parse((DurationDropdown.SelectedItem as TextBlock).Tag.ToString());
                int price = int.Parse(priceHolder.Text);

                var sell_response = await Newton_Peek.CharactersMarketSellResponse(selected_character.Guid, selected_character.RealmId, duration, (bool)biddingSwitch.IsChecked, price);

                if (sell_response != null) 
                {
                    if (sell_response.Authorized)
                    {
                        waitTitle.Text = Lang.ResourceManager.GetString("dialog_characters_market_add_sale_title", CultureInfo.CurrentUICulture);
                        waitDescription.Text = sell_response.Message;
                        ImageBehavior.SetAnimatedSource(checkMarkGif,
                            new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/checkmark_2.gif", UriKind.Relative)));
                    }
                    else
                    {
                        waitTitle.Text = Lang.ResourceManager.GetString("dialog_characters_market_add_sale_title", CultureInfo.CurrentUICulture);
                        waitDescription.Text = sell_response.Message;
                        ImageBehavior.SetAnimatedSource(checkMarkGif,
                            new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/xmark_1.gif", UriKind.Relative)));
                    }

                    AddedToMarket = sell_response.Authorized;
                }
                else
                {
                    Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }

            vbSpinner.Visibility = Visibility.Collapsed;
            vbCheckmark.Visibility = Visibility.Visible;
            checkMarkGif.Opacity = 0;
            Animations.FadeIn(checkMarkGif, 1000);

            var controller = ImageBehavior.GetAnimationController(checkMarkGif);
            controller.Play();

            await Task.Delay(3500); // animation duration

            OnConfirmationChanged?.Invoke(this, AddedToMarket);
            CloseDialog();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            AddedToMarket = false;
            OnConfirmationChanged?.Invoke(this, AddedToMarket);
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }

        private void CharactersDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sellButton.IsEnabled = CharactersDropdown.SelectedItem is Character_Dropdown_Row;
        }

        private void priceHolder_GotFocus(object sender, RoutedEventArgs e)
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

        private void priceHolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //CultureInfo culture = new CultureInfo("en-US");
                //var valueBefore = int.Parse(priceHolder.Text, NumberStyles.AllowThousands);
                //priceHolder.Text = string.Format(culture, "{0:N0}", valueBefore);
                //priceHolder.Select(priceHolder.Text.Length, 0);
            }
            catch
            {

            }
        }

        private void priceHolder_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+"); // allows only numbers to be typed
        }

        private void priceHolder_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                string badColor = "#FF941B1B";
                string okColor = "#FF00FF00";

                int required_minimum_price = Cache.charactersMarketSettings.MinimumPrice;
                int maximum_allowed_price = Cache.charactersMarketSettings.MaximumPrice;
                int market_commission = Cache.charactersMarketSettings.CommissionPercent;

                int price = int.Parse(priceHolder.Text);
                double final_price = price - (price * ((double)market_commission / 100));

                if (price < required_minimum_price)
                {
                    priceErrorHolder.Visibility = Visibility.Visible;
                    priceErrorHolder.Foreground = Extensions.GetColorFromHex(badColor);
                    priceErrorHolder.Text = $"{Lang.ResourceManager.GetString("dialog_characters_market_text_min_price", CultureInfo.CurrentUICulture)} {required_minimum_price}";
                }
                else if (price > maximum_allowed_price)
                {
                    priceErrorHolder.Visibility = Visibility.Visible;
                    priceErrorHolder.Foreground = Extensions.GetColorFromHex(badColor);
                    priceErrorHolder.Text = $"{Lang.ResourceManager.GetString("dialog_characters_market_text_max_price", CultureInfo.CurrentUICulture)} {maximum_allowed_price}";
                }
                else
                {
                    priceErrorHolder.Foreground = Extensions.GetColorFromHex(okColor);
                    priceErrorHolder.Text = $"{Lang.ResourceManager.GetString("dialog_characters_market_text_you_receive", CultureInfo.CurrentUICulture)} {Math.Round(final_price)} dp";
                }
            }
            catch
            {

            }
        }

        private void biddingSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            priceTitleHolder.Text = $"{Lang.ResourceManager.GetString("page_characters_market_requested_price", CultureInfo.CurrentUICulture)}";
        }

        private void biddingSwitch_Checked(object sender, RoutedEventArgs e)
        {
            priceTitleHolder.Text = $"{Lang.ResourceManager.GetString("page_characters_market_starting_bid_price", CultureInfo.CurrentUICulture)}";
        }
    }
}
