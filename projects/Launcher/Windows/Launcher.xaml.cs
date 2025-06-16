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
using Forgotten_Land_Launcher.Windows;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Forgotten_Land_Launcher
{
    /// <summary>
    /// Interaction logic for Launcher.xaml
    /// </summary>
    public partial class Launcher : Window
    {
        public event EventHandler<bool> OnDataLoaded;

        Game_Updater gameUpdater = new Game_Updater();

        public bool isUpdatingGame = false;
        public bool isCheckingGame = false;

        public bool IsDataLoaded { get; private set; }

        public Launcher()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Launcher_Settings.InterfaceLang);

            Discord_RPC.Initialize();

            LoadLauncherData();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
            Animations.FadeIn(this, 300);
            RoutedEventArgs args = new RoutedEventArgs(Button.ClickEvent);
            lmButtonHome.RaiseEvent(args);
        }

        private async void LoadLauncherData()
        {
            LoadRealmlistAddress();

            await LoadUserInfo();

            IsDataLoaded = true;
            OnDataLoaded?.Invoke(this, IsDataLoaded);

            Page_Options.ApplySettings();

            if (!string.IsNullOrEmpty(Launcher_Settings.GamePath) && !string.IsNullOrWhiteSpace(Launcher_Settings.GamePath))
            {
                if (Properties.Settings.Default.CheckActiveStatus && !Cache.AccountInfo.IsActive)
                {
                    btnPlay.IsEnabled = false;
                    btnPlay.Content = "PLEASE ACTIVATE";
                }
                else
                {
                    CheckForUpdates();
                }
            }
            else
            {
                gamePathRequest.Show();
            }
        }

        public async void CheckForUpdates()
        {
            btnPlay.IsEnabled = false;
            isCheckingGame = true;

            if (await gameUpdater.UpdateList())
            {
                btnPlay.Visibility = Visibility.Collapsed; // hide
                btnUpdate.Visibility = Visibility.Visible; // show
                btnCancelUpdate.Visibility = Visibility.Collapsed; // hide
            }

            btnPlay.IsEnabled = true;
            isCheckingGame = false;
        }

        private async Task LoadUserInfo()
        {
            bool Battle_Pay_Credits_As_DP = false;

            var bpcesr = await Newton_Peek.BattlePayCreditsEnableStatusResponse();

            if (bpcesr != null)
            {
                Battle_Pay_Credits_As_DP = bpcesr.BattlePayCreditsAsDP;
            }

            if (Cache.AccountInfo != null)
            {
                usernameHolder.Text = Cache.AccountInfo.Username;
                rankHolder.Text     = Cache.AccountInfo.RankName;
                nicknameHolder.Text = Cache.AccountInfo.Nickname;

                if (Battle_Pay_Credits_As_DP)
                {
                    dpNameHolder.Text = "battle pay credits";
                    donatePointsHolder.Text = Cache.AccountInfo.BattlePayCredits.ToString("N0");
                }
                else
                {
                    donatePointsHolder.Text = Cache.AccountInfo.DonatePoints.ToString("N0");
                }

                votePointsHolder.Text = Cache.AccountInfo.VotePoints.ToString("N0");
                Extensions.SetImageBrushSource(avatarHolder, Cache.AccountInfo.AvatarUrl, UriKind.Absolute);
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private async void LoadRealmlistAddress()
        {
            var realmlistResponse = await Newton_Peek.RealmlistResponse();

            if (realmlistResponse != null)
            {
                Cache.RealmListAddress = realmlistResponse.RealmlistAddress;
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private void TogglePopupGameMoreOptions(bool hide = false)
        {
            if (hide)
            {
                if (darknessFog.Visibility == Visibility.Visible)
                {
                    gameMoreOptions.Toggle();
                    darknessFog.Toggle();
                }
            }
            else
            {
                gameMoreOptions.Toggle();
                darknessFog.Toggle();
            }
        }

        private void windowExitButton_Click(object sender, RoutedEventArgs e)
        {
            Confirmation_Dialog simple_Confirmation_Dialog = new Confirmation_Dialog
            (
                Lang.ResourceManager.GetString("exit_dialog_title", CultureInfo.CurrentUICulture),
                Lang.ResourceManager.GetString("exit_dialog_text", CultureInfo.CurrentUICulture)
            );

            simple_Confirmation_Dialog.ClipToBounds = true;

            Panel.SetZIndex(simple_Confirmation_Dialog, 10000);

            mainGrid.Children.Add(simple_Confirmation_Dialog);

            simple_Confirmation_Dialog.OnConfirmationChanged += (os, be) =>
            {
                if (simple_Confirmation_Dialog.IsConfirmed)
                {
                    Application.Current.Shutdown();
                }
            };
        }

        private void windowMinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnGameMoreOptions_Click(object sender, RoutedEventArgs e)
        {
            TogglePopupGameMoreOptions();
        }

        private async void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            TogglePopupGameMoreOptions(true);

            string WoWExeName = Launcher_Settings.HDTextures ? Properties.Settings.Default.WoWExe_HD : Properties.Settings.Default.WoWExe_SD;
            string WowExePath = $"{Launcher_Settings.GamePath}\\{WoWExeName}";

            if (File.Exists(WowExePath))
            {
                gameUpdater.BackupExtraGameFiles();

                btnPlay.Content = "LAUNCHING...";
                btnPlay.IsEnabled = false;

                try
                {
                    Game_Handler.ClearCache();
                    Game_Handler.UpdateRealmlist();
                    Game_Handler.SetGameAccount();
                    Game_Handler.SetReadTOSandWindowMode();

                    await Task.Delay(2000);

                    WindowState = WindowState.Minimized;

                    Process process = Process.Start(WowExePath);

                    process.EnableRaisingEvents = true;
                    process.Exited += (ps, pe) =>
                    {
                        Game_Handler.StopDiscordRPCUpdater();
                    };

                    Game_Handler.StartDiscordRPCUpdater();

                    if (Launcher_Settings.AutoLogin)
                    {
                        Game_Login_Overlay game_Login = new Game_Login_Overlay();

                        game_Login.Show();
                    }

                    btnPlay.Content = "PLAY";
                    btnPlay.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Error_Handler.Justify(new StackTrace(), null, null, ex, false);
                }
            }
            else
            {
                gamePathRequest.Show();

                Dispatcher.Invoke(() =>
                {
                    btnPlay.IsEnabled = true;
                    btnPlay.Content = "PLAY"; // Reset the button text if the game path doesn't exist
                });
            }
        }

        private async void btnNav_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                EnableAllMenuButtons();

                button.IsEnabled = false; // current button

                await UnloadPages();

                switch (button)
                {
                    case var _ when button == btnNavHome:
                    {
                        pageHome.Load();
                        lmButtonHome.IsEnabled = false;
                    }
                    break;

                    case var _ when button == lmButtonHome:
                    {
                        pageHome.Load();
                        btnNavHome.IsEnabled = false;
                    }
                    break;

                    case var _ when button == btnNavNews:
                    {
                        pageNews.Load();
                    }
                    break;

                    case var _ when button == btnInventory:
                    {
                        pageAccountInventory.Load();
                    }
                    break;

                    case var _ when button == btnNavEvents:
                    {
                        pageEvents.Load();
                    }
                    break;

                    case var _ when button == btnNavDiscord:
                    {
                        pageDiscord.Load();
                    }
                    break;

                    case var _ when button == btnNavChangelog:
                    {
                        pageChangelogs.Load();
                    }
                    break;

                    case var _ when button == btnLoginRewards:
                    {
                        pageLoginRewards.Load();
                    }
                    break;

                    case var _ when button == btnProfile:
                    {
                        pageAccountInfo.Load();
                    }
                    break;

                    case var _ when button == btnMessages:
                    {
                        pagePrivateMessages.Load();
                    }
                    break;

                    case var _ when button == lmButtonShop:
                    {
                        pageShop.Load();
                    }
                    break;

                    case var _ when button == lmButtonCharactersMarket:
                    {
                        pageCharactersMarket.Load();
                    }
                    break;

                    case var _ when button == lmButtonFAQ:
                    {
                        pageFaq.Load();
                    }
                    break;

                    case var _ when button == lmButtonAddons:
                    {
                        pageAddons.Load();
                    }
                    break;

                    case var _ when button == lmButtonVote:
                    {
                        pageVote.Load();
                    }
                    break;

                    case var _ when button == lmButtonOnlinePlayers:
                    {
                        pageOnlinePlayers.Load();
                    }
                    break;

                    case var _ when button == lmButtonLadderboard:
                    {
                        pageLadderboard.Load();
                    }
                    break;
                }
            }
        }

        private void btnRedeemGift_Click(object sender, RoutedEventArgs e)
        {
            var redeem_Gift_Dialog = new Redeem_Gift_Dialog();

            redeem_Gift_Dialog.ClipToBounds = true;

            Panel.SetZIndex(redeem_Gift_Dialog, 10000);

            redeem_Gift_Dialog.OnCheckingCode += async (os, be) =>
            {
                if (redeem_Gift_Dialog.IsChecking)
                {
                    var giftCodePreview = await Newton_Peek.GiftPreview(redeem_Gift_Dialog.textBoxGiftCode.Text);

                    if (giftCodePreview != null)
                    {
                        if (giftCodePreview.IsValid)
                        {
                            redeem_Gift_Dialog.ShowPreview("Gift Preview", giftCodePreview.Message, giftCodePreview.PictureUrl);
                        }
                        else
                        {
                            redeem_Gift_Dialog.ShowFailed("Invalid Gift", giftCodePreview.Message);
                        }
                    }
                    else
                    {
                        Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
                    }
                }
            };

            redeem_Gift_Dialog.OnClaimGift += async (os, be) =>
            {
                if (redeem_Gift_Dialog.IsClaimed)
                {
                    var redeemGiftCode = await Newton_Peek.RedeemGiftCodeResponse(redeem_Gift_Dialog.textBoxGiftCode.Text);

                    if (redeemGiftCode != null)
                    {
                        if (redeemGiftCode.Redeemed)
                        {
                            redeem_Gift_Dialog.ShowSuccess("Success", redeemGiftCode.Message, redeemGiftCode.PictureUrl);
                        }
                        else
                        {
                            redeem_Gift_Dialog.ShowFailed("Unsuccessful", redeemGiftCode.Message);
                        }
                    }
                    else
                    {
                        Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
                    }
                }
            };

            mainGrid.Children.Add(redeem_Gift_Dialog);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var scd = new Confirmation_Dialog
            (
                "Wish to logout?", 
                "This action will logout your current account and redirect you to the login window."
            );

            scd.ClipToBounds = true;

            Panel.SetZIndex(scd, 10000);

            scd.OnConfirmationChanged += (os, be) =>
            {
                if (scd.IsConfirmed)
                {
                    // pass authorization data
                    Cache.AccountInfo   = new Newton_Main.AccountInfo();
                    Cache.AuthToken     = null;

                    // delete any saved token to avoid autologin
                    Rasar.Delete_Invalid_Auth_Token_Data();

                    // bring up login window
                    Login login = new Login();
                    login.Show();

                    // close any launcher window opened, because once login is show then is set to mainWindow
                    foreach (var window in Application.Current.Windows.OfType<Launcher>())
                    {
                        if (window is Launcher launcherWindow)
                        {
                            launcherWindow.Close();
                        }
                    }
                }
            };
            mainGrid.Children.Add(scd);
        }

        private void btnAvatar_Click(object sender, RoutedEventArgs e)
        {
            var asd = new Avatar_Settings_Dialog((avatarHolder.ImageSource as BitmapImage)?.UriSource?.OriginalString);

            asd.ClipToBounds = true;

            Panel.SetZIndex(asd, 10000);

            asd.OnConfirmationChanged += (os, be) =>
            {
                if (asd.AvatarChanged)
                {
                    UpdateUserAvatar(asd.pNewAvatarUrl);
                }
            };
            mainGrid.Children.Add(asd);
        }

        private async void UpdateUserAvatar(string image_url)
        {
            var userAvatar = await Newton_Peek.AvatarUpdateResponse(image_url);

            if (userAvatar != null)
            {
                if (userAvatar.Updated)
                {
                    Extensions.SetImageBrushSource(avatarHolder, image_url, UriKind.Absolute);
                }
                else
                {
                    Extensions.SetImageBrushSource(avatarHolder, "/Forgotten Land Launcher;component/Assets/default_avatar.jpg", UriKind.Relative);
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        public void EnableAllMenuButtons()
        {
            foreach (var child in spNavButtons.Children)
            {
                if (child is Button button)
                {
                    button.IsEnabled = true;
                }
            }

            foreach (var child in spLeftMenu.Children)
            {
                if (child is Button button)
                {
                    button.IsEnabled = true;
                }
            }

            foreach (var child in spProfileButtons.Children)
            {
                if (child is Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }

        public async Task UnloadPages()
        {
            pageHome.Unload();
            pageNews.Unload();
            pageArticleViewer.Unload();
            pageAccountInventory.Unload();
            pageEvents.Unload();
            pageDiscord.Unload();
            pageChangelogs.Unload();
            pageLoginRewards.Unload();
            pageAccountInfo.Unload();
            pagePrivateMessages.Unload();
            pageNewMessage.Unload();
            pagePrivateMessageView.Unload();
            pageShop.Unload();
            pageCharactersMarket.Unload();
            pageFaq.Unload();
            pageAddons.Unload();
            pageVote.Unload();
            pageOnlinePlayers.Unload();
            pageLadderboard.Unload();

            await Task.Delay(350); // this is important
        }

        public async void DisplayNewMessagePage()
        {
            await UnloadPages();
            pageNewMessage.Load();
        }

        public async void DisplayPrivateMessageThread(int id)
        {
            await UnloadPages();
            pagePrivateMessageView.Load(id);
        }

        public async void DisplayPrivateMessagesPage()
        {
            await UnloadPages();
            pagePrivateMessages.Load();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            TogglePopupGameMoreOptions(true);

            string gamepath = Launcher_Settings.GamePath;

            if (!Directory.Exists(gamepath))
            {
                Directory.CreateDirectory(gamepath);
            }

            string WoWExeName = Launcher_Settings.HDTextures ? Properties.Settings.Default.WoWExe_HD : Properties.Settings.Default.WoWExe_SD;

            Game_Handler.KillProcess(WoWExeName);

            Game_Handler.RemoveReadOnlyAttribute(gamepath);

            Game_Handler.SetDirectoryPermissions(gamepath);

            if (gameUpdater.Start())
            {
                isUpdatingGame = true;

                downloadBar.Value = 0;

                btnPlay.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                btnCancelUpdate.Visibility = Visibility.Visible;
                spDownloadDetails.Visibility = Visibility.Visible;

                gameUpdater.ProgressChangedEvent += OnGameUpdateProgressChanged;
                gameUpdater.CompletedEvent += OnGameUpdateCompleted;
            }
        }

        private void btnCancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            isUpdatingGame = false;
            gameUpdater.Stop();
            gameUpdater.StoppedEvent += OnGameUpdateStopped;
        }

        private void OnGameUpdateProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            downloadBar.Visibility = Visibility.Visible;
            downloadBar.Value = (long)((double)(gameUpdater.TotalSizeDownloaded + e.BytesReceived) / gameUpdater.TotalSizeToDownload * 100);
            dlTotalProgress.Text = downloadBar.Value + "%";
            dlTotalDownloaded.Text = Extensions.SizeSuffix(gameUpdater.TotalSizeDownloaded + e.BytesReceived, 2);
            dlTotalToDownload.Text = Extensions.SizeSuffix(gameUpdater.TotalSizeToDownload, 2);
            dlSpeed.Text = $"{e.BytesReceived / 1024d / 1024d / gameUpdater.SWSpeed.Elapsed.TotalSeconds:0.00} MB/s";
        }

        private void OnGameUpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            isUpdatingGame = false;
            downloadBar.Value = 100;
            btnPlay.Visibility = Visibility.Visible; // show
            btnUpdate.Visibility = Visibility.Collapsed; // hide
            btnCancelUpdate.Visibility = Visibility.Collapsed; // hide
            spDownloadDetails.Visibility = Visibility.Hidden; // hide
        }

        private void OnGameUpdateStopped(object sender, AsyncCompletedEventArgs e)
        {
            isUpdatingGame = false;
            downloadBar.Value = 0;
            dlTotalProgress.Text = "0%";
            btnPlay.Visibility = Visibility.Collapsed; // hide
            btnUpdate.Visibility = Visibility.Visible; // show
            btnCancelUpdate.Visibility = Visibility.Collapsed; // hide
            spDownloadDetails.Visibility = Visibility.Hidden; // hide
        }

        private void lsButtonForums_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(Properties.Settings.Default.ForumUrl);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private void btnReloadCurrencies_Click(object sender, RoutedEventArgs e)
        {
            UpdateVisualCurrencies();
        }

        public async void UpdateVisualCurrencies()
        {
            btnReloadCurrencies.IsEnabled = false;
            btnReloadCurrencies.Visibility = Visibility.Hidden;
            reloadCurrenciesSpinner.Visibility = Visibility.Visible;

            var accCurrencies = await Newton_Peek.AccountCurrenciesResponse();

            if (accCurrencies != null)
            {
                if (accCurrencies.Authorized)
                {
                    donatePointsHolder.Text = accCurrencies.DonatePoints.ToString("N0");
                    votePointsHolder.Text = accCurrencies.VotePoints.ToString("N0");
                }
            }

            btnReloadCurrencies.IsEnabled = true;
            btnReloadCurrencies.Visibility = Visibility.Visible;
            reloadCurrenciesSpinner.Visibility = Visibility.Hidden;
        }
    }
}
