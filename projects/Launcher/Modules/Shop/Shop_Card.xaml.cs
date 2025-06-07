/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Shop
{
    /// <summary>
    /// Interaction logic for Shop_Card.xaml
    /// </summary>
    public partial class Shop_Card : Button
    {
        public Newton_Main.ShopListResponse pShopItem;

        public Shop_Card(Newton_Main.ShopListResponse shop_item)
        {
            InitializeComponent();
            pShopItem = shop_item;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            Extensions.SetImageBrushSource(pictureHolder, pShopItem.PictureUrl, UriKind.Absolute);
            titleHolder.Text = pShopItem.Title;

            dpPriceHolder.Text = pShopItem.DPorBPCprice.ToString();
            vpPriceHolder.Text = pShopItem.VPrice.ToString();
            
            spDPPrice.Visibility = pShopItem.DPorBPCprice > 0 ? Visibility.Visible : Visibility.Collapsed;
            spVPPrice.Visibility = pShopItem.VPrice > 0 ? Visibility.Visible : Visibility.Collapsed;

            spFREE.Visibility = pShopItem.DPorBPCprice <= 0 && pShopItem.VPrice <= 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
