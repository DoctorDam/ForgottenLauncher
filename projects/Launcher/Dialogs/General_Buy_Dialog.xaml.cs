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
using System.Diagnostics;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for General_Buy_Dialog.xaml
    /// </summary>
    public partial class General_Buy_Dialog : UserControl
    {
        public event EventHandler<bool> OnConfirmationChanged;

        public bool IsConfirmed { get; private set; }

        private string pTitle;
        private string pText;
        private int pDPprice;
        private int pVPprice;

        public General_Buy_Dialog(string title, string text, int dp_price, int vp_price)
        {
            InitializeComponent();
            pTitle = title;
            pText = text;
            pDPprice = dp_price;
            pVPprice = vp_price;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);

            confirmationTitle.Text = pTitle;
            confirmationText.Text = pText;

            bool Battle_Pay_Credits_As_DP = false;

            var bpcesr = await Newton_Peek.BattlePayCreditsEnableStatusResponse();
            
            if (bpcesr != null)
            {
                Battle_Pay_Credits_As_DP = bpcesr.BattlePayCreditsAsDP;
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            spCost.Visibility = pDPprice + pVPprice > 0 ? Visibility.Visible : Visibility.Collapsed;
            spDPCost.Visibility = pDPprice > 0 ? Visibility.Visible : Visibility.Collapsed;
            spVPCost.Visibility = pVPprice > 0 ? Visibility.Visible : Visibility.Collapsed;

            dpPrice.Text = pDPprice.ToString();
            dpNameHolder.Text = Battle_Pay_Credits_As_DP ? "BATTLE PAY CREDITS" : "DP";
            vpPrice.Text = pVPprice.ToString();
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
