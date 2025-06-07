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
using System.Windows.Threading;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;
using Forgotten_Land_Launcher.Elements.Popups;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Redeem_Gift_Dialog.xaml
    /// </summary>
    public partial class Redeem_Gift_Dialog : UserControl
    {
        public event EventHandler<bool> OnClaimGift;
        public event EventHandler<bool> OnCheckingCode;

        public bool IsClaimed { get; private set; }
        public bool IsChecking { get; private set; }

        public Redeem_Gift_Dialog()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);
            textBoxGiftCode.Focus();
        }

        public void ShowPreview(string title, string message, string picture_url)
        {
            waitTitle.Text = title;

            waitDescription.Text = message;

            vbSpinner.Visibility = Visibility.Collapsed;

            borderPictureHolder.Visibility = Visibility.Visible;

            Extensions.SetImageBrushSource(pictureHolder, picture_url, UriKind.Absolute);

            claimButton.Visibility = Visibility.Visible;
            cancelButton2.Visibility = Visibility.Visible;
        }

        public async void ShowSuccess(string title, string message, string picture_url)
        {
            waitTitle.Text = title;
            waitDescription.Text = message;
            ImageBehavior.SetAnimatedSource(checkMarkGif, new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/checkmark_2.gif", UriKind.Relative)));

            vbSpinner.Visibility = Visibility.Collapsed;
            vbCheckmark.Visibility = Visibility.Visible;
            vbCheckmark.Opacity = 0;
            Animations.FadeIn(vbCheckmark, 750);

            borderPictureHolder.Visibility = Visibility.Visible;

            Extensions.SetImageBrushSource(pictureHolder, picture_url, UriKind.Absolute);

            ImageAnimationController controller = ImageBehavior.GetAnimationController(checkMarkGif);
            controller.Play();

            await Task.Delay(5000); // animation duration

            if (Properties.Settings.Default.CheckActiveStatus)
            {
                var activeStatus = await Newton_Peek.AccountActiveStatus();

                if (activeStatus != null)
                {
                    if (activeStatus.IsActive)
                    {
                        if (Application.Current.MainWindow is Launcher launcher)
                        {
                            launcher.btnPlay.IsEnabled = true;
                            launcher.Content = Lang.ResourceManager.GetString("game_button_play", CultureInfo.CurrentUICulture);

                            if (!string.IsNullOrEmpty(Launcher_Settings.GamePath) && !string.IsNullOrWhiteSpace(Launcher_Settings.GamePath))
                            {
                                launcher.CheckForUpdates();
                            }
                            else
                            {
                                launcher.gamePathRequest.Show();
                            }
                        }
                    }
                }
            }

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

        private async void claimButton_Click(object sender, RoutedEventArgs e)
        {
            waitTitle.Text = "Processing";
            waitDescription.Text = Lang.ResourceManager.GetString("dialog_rquest_confirmation_text_wait", CultureInfo.CurrentUICulture);
            borderPictureHolder.Visibility = Visibility.Collapsed;
            claimButton.Visibility = Visibility.Collapsed;
            cancelButton2.Visibility = Visibility.Collapsed;
            spFirstDialog.Visibility = Visibility.Collapsed;
            spWaitDialog.Visibility = Visibility.Visible;
            vbSpinner.Visibility = Visibility.Visible;

            await Task.Delay(1000);

            IsClaimed = true;
            OnClaimGift?.Invoke(this, IsClaimed);
        }

        private async void checkButton_Click(object sender, RoutedEventArgs e)
        {
            claimButton.Visibility = Visibility.Collapsed;
            cancelButton2.Visibility = Visibility.Collapsed;
            spFirstDialog.Visibility = Visibility.Collapsed;
            spWaitDialog.Visibility = Visibility.Visible;

            await Task.Delay(1000);

            IsChecking = true;
            OnCheckingCode?.Invoke(this, IsChecking);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsClaimed = false;
            OnClaimGift?.Invoke(this, IsClaimed);
            CloseDialog();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
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

        private void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }
    }
}
