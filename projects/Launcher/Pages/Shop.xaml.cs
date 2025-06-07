/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Shop;
using Forgotten_Land_Launcher.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;
using System.Diagnostics;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Shop.xaml
    /// </summary>
    public partial class Shop : UserControl
    {
        public Shop()
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

            if (Cache.PageOptions.Shop.PageEnabled)
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                InitializeShopList();
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
            wpShopList.Children.Clear();
            spCategoryButtons.Children.Clear();
            EnableAllCategoryButtons();
        }

        private async void InitializeShopList()
        {
            wpShopList.Children.Clear();
            spCategoryButtons.Children.Clear();

            var launcherShopListResponse = await Newton_Peek.ShopListResponse();

            if (launcherShopListResponse != null)
            {
                HashSet<string> uniqueCategories = new HashSet<string>();

                foreach (var shop_item in launcherShopListResponse)
                {
                    var shop_card = new Shop_Card(shop_item);
                    
                    shop_card.Click += (s, e) =>
                    {
                        if (Application.Current.MainWindow is Launcher launcher)
                        {
                            var shop_Article_Dialog = new Shop_Article_Dialog(shop_item);

                            shop_Article_Dialog.ClipToBounds = true;

                            Panel.SetZIndex(shop_Article_Dialog, 10000);

                            shop_Article_Dialog.OnConfirmationChanged += async (os, be) =>
                            {
                                if (shop_Article_Dialog.IsConfirmed)
                                {
                                    var purchase_result = await Newton_Peek.ShopPurchaseResponse(shop_card.pShopItem.Id);

                                    if (purchase_result != null)
                                    {
                                        if (purchase_result.Purchased)
                                        {
                                            shop_Article_Dialog.ShowSuccess(Lang.ResourceManager.GetString("page_shop_dialog_title_success", CultureInfo.CurrentUICulture), purchase_result.Message);
                                            launcher.UpdateVisualCurrencies();
                                        }
                                        else
                                        {
                                            shop_Article_Dialog.ShowFailed(Lang.ResourceManager.GetString("page_shop_dialog_title_fail", CultureInfo.CurrentUICulture), purchase_result.Message);
                                        }
                                    }
                                    else
                                    {
                                        Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
                                    }
                                }
                            };

                            launcher.mainGrid.Children.Add(shop_Article_Dialog);
                        }
                    };

                    wpShopList.Children.Add(shop_card);

                    uniqueCategories.Add(shop_item.Category);
                }

                var all_cat_button = new Shop_Category_Button(Lang.ResourceManager.GetString("page_shop_all_categories", CultureInfo.CurrentUICulture));
                all_cat_button.Click += (s, e) =>
                {
                    EnableAllCategoryButtons();

                    all_cat_button.IsEnabled = false;

                    foreach (Shop_Card shopitem in wpShopList.Children.OfType<Shop_Card>())
                    {
                        shopitem.Visibility = Visibility.Visible;
                    }
                };

                RoutedEventArgs args = new RoutedEventArgs(Button.ClickEvent);
                all_cat_button.RaiseEvent(args);
                spCategoryButtons.Children.Add(all_cat_button);

                // Sort the unique categories alphabetically
                List<string> sortedCategories = uniqueCategories.ToList();
                sortedCategories.Sort();

                foreach (string category in sortedCategories)
                {
                    foreach (var article in launcherShopListResponse)
                    {
                        if (article.Category == category)
                        {
                            var shop_Category_Button = new Shop_Category_Button(article.Category);
                            shop_Category_Button.Click += (s, e) =>
                            {
                                EnableAllCategoryButtons();

                                shop_Category_Button.IsEnabled = false;

                                foreach (Shop_Card shopitem in wpShopList.Children.OfType<Shop_Card>())
                                {
                                    shopitem.Visibility = shopitem.pShopItem.Category.ToLower() == 
                                        shop_Category_Button.pCategory.ToLower() ? Visibility.Visible : Visibility.Collapsed;
                                }
                            };

                            spCategoryButtons.Children.Add(shop_Category_Button);
                            break;
                        }
                    }
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private void EnableAllCategoryButtons()
        {
            foreach (var child in spCategoryButtons.Children)
            {
                if (child is Shop_Category_Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }
    }
}
