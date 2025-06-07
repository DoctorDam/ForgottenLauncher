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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfAnimatedGif;
using System.Windows.Media.Imaging;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;
using System.Diagnostics;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Login_Reward_Dialog.xaml
    /// </summary>
    public partial class Login_Reward_Dialog : UserControl
    {
        public event EventHandler<bool> OnConfirmationChanged;

        public bool IsClaimed { get; private set; }

        private string pTitle;
        private string pDescription;
        private string pPictureUrl;
        private int pMonth;
        private int pDay;

        public Login_Reward_Dialog(string title, string description, string picture_url, int month, int day)
        {
            InitializeComponent();
            pTitle = title;
            pDescription = description;
            pPictureUrl = picture_url;
            pMonth = month;
            pDay = day;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);

            dialogTitle.Text = pTitle;
            Extensions.SetImageBrushSource(pictureHolder, pPictureUrl, UriKind.Absolute);
            dialogDescription.Text = pDescription;
        }

        private async void okButton_Click(object sender, RoutedEventArgs e)
        {
            spClaimDialog.Visibility = Visibility.Collapsed;
            spWaitDialog.Visibility = Visibility.Visible;

            await Task.Delay(500);

            var claimLoginRewardResponse = await Newton_Peek.ClaimLoginRewardResponse(pMonth, pDay);

            if (claimLoginRewardResponse != null)
            {
                IsClaimed = claimLoginRewardResponse.Claimed;

                if (claimLoginRewardResponse.Claimed)
                {
                    waitTitle.Text = Lang.ResourceManager.GetString("dialog_login_reward_title", CultureInfo.CurrentUICulture);
                    waitDescription.Text = claimLoginRewardResponse.Message;
                    ImageBehavior.SetAnimatedSource(checkMarkGif, new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/checkmark_2.gif", UriKind.Relative)));
                }
                else
                {
                    waitTitle.Text = Lang.ResourceManager.GetString("dialog_login_reward_title", CultureInfo.CurrentUICulture);
                    waitDescription.Text = claimLoginRewardResponse.Message;
                    ImageBehavior.SetAnimatedSource(checkMarkGif, new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/xmark_1.gif", UriKind.Relative)));
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            vbSpinner.Visibility = Visibility.Collapsed;
            vbCheckmark.Visibility = Visibility.Visible;
            checkMarkGif.Opacity = 0;
            Animations.FadeIn(checkMarkGif, 1000);

            var controller = ImageBehavior.GetAnimationController(checkMarkGif);
            controller.Play();

            await Task.Delay(3500); // animation duration

            OnConfirmationChanged?.Invoke(this, IsClaimed);
            CloseDialog();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsClaimed = false;
            OnConfirmationChanged?.Invoke(this, IsClaimed);
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
