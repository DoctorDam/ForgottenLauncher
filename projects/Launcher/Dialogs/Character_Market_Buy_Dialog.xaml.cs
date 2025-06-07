/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;
using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Characters_Market;
using System.Diagnostics;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Character_Market_Buy_Dialog.xaml
    /// </summary>
    public partial class Character_Market_Buy_Dialog : UserControl
    {
        private Newton_Main.CharactersMarketList pSaleInfo = new Newton_Main.CharactersMarketList();
        public event EventHandler<bool> OnConfirmationChanged;

        public bool IsConfirmed { get; private set; }

        public Character_Market_Buy_Dialog(Newton_Main.CharactersMarketList sale)
        {
            InitializeComponent();
            pSaleInfo = sale;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 250);

            CharNameHolder.Text = pSaleInfo.CharInfo.Name;

            Extensions.SetImageBrushSource(BackgroundHolder, WoW_Definitions.GetPlayerClassCard(pSaleInfo.CharInfo.Class), UriKind.Relative);

            Extensions.SetImageBrushSource(RaceAvatarHolder, WoW_Definitions.GetPlayerRaceIcon(pSaleInfo.CharInfo.Race, pSaleInfo.CharInfo.Gender), UriKind.Relative);

            Extensions.SetImageBrushSource(ClassAvatarHolder, WoW_Definitions.GetPlayerClassIcon(pSaleInfo.CharInfo.Class), UriKind.Relative);

            CharLevelHolder.Text = pSaleInfo.CharInfo.Level.ToString();

            CharRaceHolder.Text = WoW_Definitions.GetPlayerRaceName(pSaleInfo.CharInfo.Race);

            CharClassHolder.Text = WoW_Definitions.GetPlayerClassName(pSaleInfo.CharInfo.Class);

            CharClassHolder.Foreground = Extensions.GetColorFromHex($"#FF{WoW_Definitions.GetPlayerClassHexColor(pSaleInfo.CharInfo.Class)}");

            CharGoldHolder.Text = WoW_Definitions.ConvertCopperToGold(pSaleInfo.CharInfo.Money);

            CharKillsHolder.Text = pSaleInfo.CharInfo.Totalkills.ToString("N0");

            CharProfessionsCount.Text = pSaleInfo.Professions.Count.ToString();

            foreach (var profession in pSaleInfo.Professions)
            {
                spProfessions.Children.Add(new Profession_Span(profession.SkillId, profession.Level, profession.Max));
            }

            if (pSaleInfo.AllowBidding)
            {
                PurchaseButton.IsEnabled = false;
                priceTypeHolder.Text = Lang.ResourceManager.GetString("page_characters_market_dialog_text_current_bid", CultureInfo.CurrentUICulture);
                spBiddingPrice.Visibility = Visibility.Visible;
                PurchaseButton.Content = Lang.ResourceManager.GetString("general_button_set_bid", CultureInfo.CurrentUICulture);
                biddingPriceHolder.Text = pSaleInfo.Price.ToString();
            }
            else
            {
                spBiddingPrice.Visibility = Visibility.Collapsed;
            }

            PriceHolder.Text = pSaleInfo.Price.ToString();
        }

        public async void ShowSuccess(string title, string message)
        {
            waitTitle.Text = title;
            waitDescription.Text = message;
            ImageBehavior.SetAnimatedSource(checkMarkGif, new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/checkmark_2.gif", UriKind.Relative)));

            vbSpinner.Visibility = Visibility.Collapsed;
            vbCheckmark.Visibility = Visibility.Visible;
            checkMarkGif.Opacity = 0;
            Animations.FadeIn(checkMarkGif, 1000);

            var controller = ImageBehavior.GetAnimationController(checkMarkGif);
            controller.Play();

            await Task.Delay(3500); // animation duration
            CloseDialog();
        }

        public async void ShowFailed(string title, string message)
        {
            waitTitle.Text = title;
            waitDescription.Text = message;
            ImageBehavior.SetAnimatedSource(checkMarkGif, new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/xmark_1.gif", UriKind.Relative)));

            vbSpinner.Visibility = Visibility.Collapsed;
            vbCheckmark.Visibility = Visibility.Visible;
            checkMarkGif.Opacity = 0;
            Animations.FadeIn(checkMarkGif, 1000);

            var controller = ImageBehavior.GetAnimationController(checkMarkGif);
            controller.Play();

            await Task.Delay(3500); // animation duration
            CloseDialog();
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            spFirstDialog.Visibility = Visibility.Collapsed;
            spWaitDialog.Visibility = Visibility.Visible;

            IsConfirmed = true;
            OnConfirmationChanged?.Invoke(this, IsConfirmed);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            OnConfirmationChanged?.Invoke(this, IsConfirmed);
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }

        private void armoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Cache.charactersMarketSettings.ArmoryUrl);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        private void biddingPriceHolder_GotFocus(object sender, RoutedEventArgs e)
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

        private void biddingPriceHolder_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                string badColor = "#FF941B1B";
                string okColor = "#FF00FF00";

                int required_minimum_price = pSaleInfo.Price;
                int maximum_allowed_price = Cache.charactersMarketSettings.MaximumPrice;
                int market_commission = Cache.charactersMarketSettings.CommissionPercent;

                int price = int.Parse(biddingPriceHolder.Text);
                double final_price = price - (price * ((double)market_commission / 100));

                if (price <= required_minimum_price)
                {
                    priceErrorHolder.Visibility = Visibility.Visible;
                    priceErrorHolder.Foreground = Extensions.GetColorFromHex(badColor);
                    priceErrorHolder.Text = $"{Lang.ResourceManager.GetString("dialog_characters_market_text_min_price", CultureInfo.CurrentUICulture)} {required_minimum_price + 1}";
                    PurchaseButton.IsEnabled = false;
                }
                else if (price > maximum_allowed_price)
                {
                    priceErrorHolder.Visibility = Visibility.Visible;
                    priceErrorHolder.Foreground = Extensions.GetColorFromHex(badColor);
                    priceErrorHolder.Text = $"{Lang.ResourceManager.GetString("dialog_characters_market_text_max_price", CultureInfo.CurrentUICulture)} {maximum_allowed_price}";
                    PurchaseButton.IsEnabled = false;
                }
                else
                {
                    priceErrorHolder.Foreground = Extensions.GetColorFromHex(okColor);
                    priceErrorHolder.Text = "ok";
                    PurchaseButton.IsEnabled = true;
                }
            }
            catch
            {

            }
        }

        private void biddingPriceHolder_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+"); // allows only numbers to be typed
        }
    }
}
