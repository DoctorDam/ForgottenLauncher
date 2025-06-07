/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Forgotten_Land_Launcher.Library;
using WpfAnimatedGif;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Shop_Article_Dialog.xaml
    /// </summary>
    public partial class Shop_Article_Dialog : UserControl
    {
        private Newton_Main.ShopListResponse pShopItem;
        public event EventHandler<bool> OnConfirmationChanged;

        public bool IsConfirmed { get; private set; }

        public Shop_Article_Dialog(Newton_Main.ShopListResponse shop_item)
        {
            InitializeComponent();
            pShopItem = shop_item;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);

            titleHolder.Text = pShopItem.Title;

            if (pShopItem.PictureUrl != null && !string.IsNullOrEmpty(pShopItem.PictureUrl) && !string.IsNullOrWhiteSpace(pShopItem.PictureUrl))
            {
                pictureHolder.Visibility = Visibility.Visible;
                Extensions.SetImageSource(pictureHolder, pShopItem.PictureUrl, UriKind.Absolute);
            }
            else
            {
                pictureHolder.Visibility = Visibility.Collapsed;
            }

            descriptionHolder.Text = pShopItem.Description;

            bool Battle_Pay_Credits_As_DP = false;

            var battlePayCreditsEnableStatusResponse = await Newton_Peek.BattlePayCreditsEnableStatusResponse();

            if (battlePayCreditsEnableStatusResponse != null)
            {
                Battle_Pay_Credits_As_DP = battlePayCreditsEnableStatusResponse.BattlePayCreditsAsDP;
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            spCost.Visibility = pShopItem.DPorBPCprice + pShopItem.VPrice > 0 ? Visibility.Visible : Visibility.Collapsed;
            spDPCost.Visibility = pShopItem.DPorBPCprice > 0 ? Visibility.Visible : Visibility.Collapsed;
            spVPCost.Visibility = pShopItem.VPrice > 0 ? Visibility.Visible : Visibility.Collapsed;

            dpPrice.Text = pShopItem.DPorBPCprice.ToString();
            dpNameHolder.Text = Battle_Pay_Credits_As_DP ? "BATTLE PAY CREDITS" : "DP";
            vpPrice.Text = pShopItem.VPrice.ToString();
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

        private void okButton_Click(object sender, RoutedEventArgs e)
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
    }
}
