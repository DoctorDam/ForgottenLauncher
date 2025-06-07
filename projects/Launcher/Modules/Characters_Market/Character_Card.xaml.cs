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
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Modules.Characters_Market
{
    /// <summary>
    /// Interaction logic for Character_Card.xaml
    /// </summary>
    public partial class Character_Card : UserControl
    {
        public Newton_Main.CharactersMarketList pSaleInfo = new Newton_Main.CharactersMarketList();
        private DispatcherTimer timer;

        public Character_Card(Newton_Main.CharactersMarketList sale_info)
        {
            InitializeComponent();
            pSaleInfo = sale_info;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int hours = pSaleInfo.SecondsLeft / 3600;
            int minutes = (pSaleInfo.SecondsLeft % 3600) / 60;
            int seconds = pSaleInfo.SecondsLeft % 60;

            string formattedTime = "";

            if (hours > 0)
            {
                formattedTime += $"{hours}h ";
            }

            if (minutes > 0 || hours > 0)
            {
                formattedTime += $"{minutes}m ";
            }

            formattedTime += $"{seconds}s";

            expiringTimeHolder.Text = $"{formattedTime}"; // add live timer

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            Extensions.SetImageBrushSource(BackgroundHolder, WoW_Definitions.GetPlayerClassCard(pSaleInfo.CharInfo.Class), UriKind.Relative);

            Extensions.SetImageBrushSource(RaceAvatarHolder, WoW_Definitions.GetPlayerRaceIcon(pSaleInfo.CharInfo.Race, pSaleInfo.CharInfo.Gender), UriKind.Relative);
            
            Extensions.SetImageBrushSource(ClassAvatarHolder, WoW_Definitions.GetPlayerClassIcon(pSaleInfo.CharInfo.Class), UriKind.Relative);

            CharNameHolder.Text = pSaleInfo.CharInfo.Name;

            CharRaceHolder.Text = WoW_Definitions.GetPlayerRaceName(pSaleInfo.CharInfo.Race);

            CharClassHolder.Text = WoW_Definitions.GetPlayerClassName(pSaleInfo.CharInfo.Class);

            CharClassHolder.Foreground = Extensions.GetColorFromHex($"#FF{WoW_Definitions.GetPlayerClassHexColor(pSaleInfo.CharInfo.Class)}");

            CharLevelHolder.Text = pSaleInfo.CharInfo.Level.ToString();

            RealmHolder.Text = pSaleInfo.RealmName;

            PriceHolder.Text = pSaleInfo.Price.ToString();

            if (pSaleInfo.OwnerAccountId == Cache.AccountInfo.Id)
                ButtonCancelSale.Visibility = Visibility.Visible;

            if (pSaleInfo.AllowBidding)
            {
                priceTypeHolder.Text = Lang.ResourceManager.GetString("page_characters_market_card_text_highest_bid", CultureInfo.CurrentUICulture);
                priceTypeHolder.Foreground = Extensions.GetColorFromHex("#FFFFD291");
                Extensions.SetImageSource(priceIcon, $"/Forgotten Land Launcher;component/Assets/icons/icon_auction_coin_1.png", UriKind.Relative);
                cardBorder.BorderBrush = Extensions.GetColorFromHex("#FFFFD291");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pSaleInfo.SecondsLeft > 0)
            {
                int hours = pSaleInfo.SecondsLeft / 3600;
                int minutes = (pSaleInfo.SecondsLeft % 3600) / 60;
                int seconds = pSaleInfo.SecondsLeft % 60;

                string formattedTime = "";

                if (hours > 0)
                {
                    formattedTime += $"{hours}h ";
                }

                if (minutes > 0 || hours > 0)
                {
                    formattedTime += $"{minutes}m ";
                }

                formattedTime += $"{seconds}s";

                expiringTimeHolder.Text = formattedTime;

                pSaleInfo.SecondsLeft--;
            }
            else
            {
                timer.Stop();
                
                if (Parent is Panel parentPanel)
                {
                    parentPanel.Children.Remove(this);
                }
            }
        }

        private void ButtonMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                var buy_dialog = new Character_Market_Buy_Dialog(pSaleInfo);

                buy_dialog.ClipToBounds = true;

                Panel.SetZIndex(buy_dialog, 10000);

                buy_dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (buy_dialog.IsConfirmed)
                    {
                        try
                        {
                            if (pSaleInfo.AllowBidding)
                            {
                                int bid_amount = int.Parse(buy_dialog.biddingPriceHolder.Text);
                                var bid_response = await Newton_Peek.CharactersMarketBidResponse(pSaleInfo.Id, bid_amount);
                                
                                if (bid_response != null)
                                {
                                    if (bid_response.Authorized)
                                    {
                                        launcher.UpdateVisualCurrencies();
                                        launcher.pageCharactersMarket.ClearNativeChilds();
                                        launcher.pageCharactersMarket.Load();
                                        buy_dialog.ShowSuccess("Characters Market", bid_response.Message);
                                    }
                                    else
                                    {
                                        buy_dialog.ShowFailed("Characters Market", bid_response.Message);
                                    }
                                }
                                else
                                {
                                    string message = "Json string is invalid or empty!";
                                    Extensions.ShowPopup("Failed to get characters market sale response", message, Application.Current.MainWindow?.GetType());
                                }
                            }
                            else
                            {
                                var buy_response = await Newton_Peek.CharactersMarketBuyResponse(pSaleInfo.Id);

                                if (buy_response != null)
                                {
                                    if (buy_response.Authorized)
                                    {
                                        launcher.UpdateVisualCurrencies();
                                        launcher.pageCharactersMarket.ClearNativeChilds();
                                        launcher.pageCharactersMarket.Load();
                                        buy_dialog.ShowSuccess("Characters Market", buy_response.Message);
                                    }
                                    else
                                    {
                                        buy_dialog.ShowFailed("Characters Market", buy_response.Message);
                                    }
                                }
                                else
                                {
                                    string message = "Json string is invalid or empty!";
                                    Extensions.ShowPopup("Failed to get characters market sale response", message, Application.Current.MainWindow?.GetType());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Error_Handler.Justify(new StackTrace(), null, null, ex, true);
                        }
                    }
                    else
                    {

                    }
                };

                launcher.mainGrid.Children.Add(buy_dialog);
            }
        }

        private void ButtonCancelSale_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {

                var confirmation_dialog = new Confirmation_Dialog
                (
                    "Cancel Market Sale",
                    "Are you sure you want to cancel your sale?"
                );

                confirmation_dialog.ClipToBounds = true;

                Panel.SetZIndex(confirmation_dialog, 10000);

                confirmation_dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (confirmation_dialog.IsConfirmed)
                    {
                        try
                        {
                            var cancel_response = await Newton_Peek.CharactersMarketCancelResponse(pSaleInfo.Id);

                            if (cancel_response != null)
                            {
                                if (cancel_response.Authorized)
                                {
                                    launcher.pageCharactersMarket.ClearNativeChilds();
                                    launcher.pageCharactersMarket.Load();
                                }
                            }
                            else
                            {
                                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
                            }
                        }
                        catch (Exception ex)
                        {
                            Error_Handler.Justify(new StackTrace(), null, null, ex, true);
                        }
                    }
                };

                launcher.mainGrid.Children.Add(confirmation_dialog);
            }
        }
    }
}
