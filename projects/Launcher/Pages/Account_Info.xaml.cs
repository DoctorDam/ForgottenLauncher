/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Dialogs;
using Forgotten_Land_Launcher.Elements.Spinners;
using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Account_Info;
using Forgotten_Land_Launcher.Modules.Account_Inventory;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Account_Info.xaml
    /// </summary>
    public partial class Account_Info : UserControl
    {
        public Account_Info()
        {
            InitializeComponent();
        }
        public void Load()
        {
            Animations.FadeIn(this, 300);

            lastLoginHolder.Text = Cache.AccountInfo.LastLogin;
            lastIPHolder.Text = Cache.AccountInfo.LastIP;

            cbUnstuckCharacters.Items.Add(new ComboBoxItem 
            {
                Content = Lang.ResourceManager.GetString("page_account_info_text_loading_characters", CultureInfo.CurrentUICulture)
            });
            cbUnstuckCharacters.SelectedIndex = 0;
            btnUnstuck.IsEnabled = false;

            cbTeleportCharacters.Items.Add(new ComboBoxItem 
            {
                Content = Lang.ResourceManager.GetString("page_account_info_text_loading_characters", CultureInfo.CurrentUICulture)
            });
            cbTeleportCharacters.SelectedIndex = 0;
            btnTeleport.IsEnabled = false;

            cbTeleportList.Items.Add(new ComboBoxItem
            {
                Content = Lang.ResourceManager.GetString("page_account_info_text_loading_destinations", CultureInfo.CurrentUICulture)
            });
            cbTeleportList.SelectedIndex = 0;

            InitializePublicNickname();
            InitializeAccountBanStatus();
            InitializeCharactersList();
            InitializeAccountInventory();
            InitializeTeleportList();
            InitializeTicketsList();
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            lastLoginHolder.Text = "Unknown";
            lastIPHolder.Text = "unknown";
            nicknameTextbox.Text = "enter a profile id";
            spCharactersList.Children.Clear();
            spInventoryList.Children.Clear();

            cbUnstuckCharacters.Items.Clear();
            cbUnstuckCharacters.IsEnabled = false;

            cbTeleportCharacters.Items.Clear();
            cbTeleportCharacters.IsEnabled = false;

            cbTeleportList.Items.Clear();
            cbTeleportList.IsEnabled = false;

            btnUnstuck.IsEnabled = false;
            btnTeleport.IsEnabled = false;

            spTicketRealms.Children.Clear();
            spTicketsList.Children.Clear();

            AddLoadingSpinners();
        }

        private void AddLoadingSpinners()
        {
            spCharactersList.Children.Add(new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 25,
                Height = 25
            });

            spInventoryList.Children.Add(new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 25,
                Height = 25
            });

            spTicketRealms.Children.Add(new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 25,
                Height = 25,
            });

            spTicketsList.Children.Add(new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 25,
                Height = 25,
                Margin = new Thickness(0, 15, 0, 0),
            });
        }

        private async void InitializePublicNickname()
        {
            var publicNicknameResponse = await Newton_Peek.PublicNicknameResponse();

            if (publicNicknameResponse != null)
            {
                if (publicNicknameResponse.Authorized)
                {
                    nicknameTextbox.Text = publicNicknameResponse.Nickname;
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private async void InitializeAccountBanStatus()
        {
            var accountBanStatus = await Newton_Peek.AccountBanStatus();

            if (accountBanStatus != null)
            {
                if (accountBanStatus.IsBanned)
                {
                    accountStatusHolder.Foreground = Brushes.IndianRed;
                    accountStatusHolder.Text = $"{Lang.ResourceManager.GetString("page_account_info_account_banned_until", CultureInfo.CurrentUICulture)}" +
                        $"\r\n\r\n{accountBanStatus.BanDuration}";
                }
                else
                {
                    accountStatusHolder.Foreground = Brushes.Green;
                    accountStatusHolder.Text = Lang.ResourceManager.GetString("page_account_info_account_active", CultureInfo.CurrentUICulture);
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private async void InitializeCharactersList()
        {
            spCharactersList.Children.Clear();
            cbUnstuckCharacters.Items.Clear();
            cbTeleportCharacters.Items.Clear();

            var charactersList = await Newton_Peek.CharactersListResponse();

            if (charactersList != null)
            {
                if (charactersList.Count > 0)
                {
                    bool isFirstRealm = true;

                    foreach (var realmGroup in charactersList.GroupBy(c => c.RealmName))
                    {
                        spCharactersList.Children.Add(new TextBlock()
                        {
                            Text = realmGroup.Key,
                            Margin = new Thickness(0, isFirstRealm ? 5 : 25, 0, 5)
                        });

                        foreach (var character in realmGroup)
                        {
                            spCharactersList.Children.Add(new Character_Span(character)
                            {
                                Margin = new Thickness(0, 5, 0, 0)
                            });
                        }

                        isFirstRealm = false; // Set to false after processing the first realm
                    }

                    // CHARACTERS UNSTUCKER DROPDOWN LIST
                    cbUnstuckCharacters.Items.Add(new TextBlock 
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_require_character", CultureInfo.CurrentUICulture), 
                        Foreground = Extensions.GetColorFromHex("#FFB5B5B5") 
                    });

                    foreach (var character in charactersList)
                    {
                        Character_Dropdown_Row dcr = new Character_Dropdown_Row(character);
                        cbUnstuckCharacters.Items.Add(dcr);
                    }

                    cbUnstuckCharacters.SelectedIndex = 0;
                    cbUnstuckCharacters.IsEnabled = true;

                    // CHARACTERS TELEPORTER DROPDOWN LIST
                    cbTeleportCharacters.Items.Add(new TextBlock 
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_require_character", CultureInfo.CurrentUICulture), 
                        Foreground = Extensions.GetColorFromHex("#FFB5B5B5") 
                    });

                    foreach (var character in charactersList)
                    {
                        cbTeleportCharacters.Items.Add(new Character_Dropdown_Row(character));
                    }

                    cbTeleportCharacters.SelectedIndex = 0;
                    cbTeleportCharacters.IsEnabled = true;
                }
                else
                {
                    spCharactersList.Children.Add(new TextBlock 
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_no_characters", CultureInfo.CurrentUICulture), 
                        Foreground = Brushes.Red 
                    });

                    cbUnstuckCharacters.Items.Add(new TextBlock 
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_no_characters", CultureInfo.CurrentUICulture), 
                        Foreground = Brushes.Red 
                    });

                    cbUnstuckCharacters.SelectedIndex = 0;

                    cbTeleportCharacters.Items.Add(new TextBlock 
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_no_characters", CultureInfo.CurrentUICulture), 
                        Foreground = Brushes.Red 
                    });

                    cbTeleportCharacters.SelectedIndex = 0;
                }
            }
        }

        private async void InitializeAccountInventory()
        {
            Page_Options.ApplySettings();

            if (!Cache.PageOptions.AccountInventory.PageEnabled)
            {
                return;
            }

            spInventoryList.Children.Clear();

            var accountInventory = await Newton_Peek.AccountInventoryResponse();

            if (accountInventory != null)
            {
                int count = 0;
                foreach (var reward in accountInventory)
                {
                    ++count;

                    spInventoryList.Children.Add(new Inventory_Span(reward)
                    {
                        Margin = new Thickness(0, count > 1 ? 10 : 3, 0, 0)
                    });
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private async void InitializeTeleportList()
        {
            cbTeleportList.Items.Clear();

            var teleportList = await Newton_Peek.TeleportListResponse();

            if (teleportList != null)
            {
                if (teleportList.Count > 0)
                {
                    cbTeleportList.Items.Add(new TextBlock 
                    {
                        Text = Lang.ResourceManager.GetString("page_account_info_text_require_destination", CultureInfo.CurrentUICulture),
                        Foreground = Extensions.GetColorFromHex("#FFB5B5B5")
                    });

                    foreach (var destination in teleportList)
                    {
                        cbTeleportList.Items.Add(new Teleport_Dropdown_Row(destination));
                    }

                    cbTeleportList.SelectedIndex = 0;
                    cbTeleportList.IsEnabled = true;
                }
                else
                {
                    cbTeleportList.Items.Add(new TextBlock
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_no_destinations", CultureInfo.CurrentUICulture), 
                        Foreground = Brushes.Red
                    });

                    cbTeleportCharacters.SelectedIndex = 0;
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
        }

        private async void InitializeTicketsList()
        {
            var charactersTicketsListResponses = await Newton_Peek.CharactersTicketsListResponse();

            spTicketRealms.Children.Clear();
            spTicketsList.Children.Clear();

            if (charactersTicketsListResponses != null)
            {
                if (charactersTicketsListResponses.Count > 0)
                {
                    // spawn realm buttons
                    int realmsCount = 0;

                    foreach (var realmGroup in charactersTicketsListResponses.GroupBy(c => c.RealmName))
                    {
                        realmsCount++;

                        // spawn realms buttons
                        Button realmButton = new Button
                        {
                            Content = realmGroup.Key,
                            Background = Extensions.GetColorFromHex("#00000000"),
                            Foreground = Extensions.GetColorFromHex("#FFBBBBBB"),
                            FontWeight = FontWeights.Bold,
                            BorderBrush = null,
                            Cursor = Cursors.Hand,
                            Style = (Style)Application.Current.Resources["accountInfoRealmButton"]
                        };

                        realmButton.Click += (sender, e) =>
                        {
                            foreach (Ticket_Span ticket in spTicketsList.Children.OfType<Ticket_Span>())
                            {
                                ticket.Visibility = ticket.pTicket.RealmName == (sender as Button).Content.ToString() ? Visibility.Visible : Visibility.Collapsed;
                            }

                            foreach (Button rBtn in spTicketRealms.Children.OfType<Button>())
                            {
                                rBtn.IsEnabled = true;
                            }

                            (sender as Button).IsEnabled = false;
                        };

                        spTicketRealms.Children.Add(realmButton);

                        if (realmsCount < charactersTicketsListResponses.GroupBy(c => c.RealmName).Count())
                        {
                            // spawn dividers
                            spTicketRealms.Children.Add(new Border
                            {
                                BorderThickness = new Thickness(1),
                                BorderBrush = Extensions.GetColorFromHex("#FF212121"),
                                Margin = new Thickness(5, 0, 5, 0),
                                Height = 12,
                                VerticalAlignment = VerticalAlignment.Center
                            });
                        }
                    }

                    foreach (var ticket in charactersTicketsListResponses)
                    {
                        spTicketsList.Children.Add(new Ticket_Span(ticket));
                    }

                    spTicketRealms.Children.OfType<Button>().First().RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else
                {
                    spTicketsList.Children.Add(new TextBlock 
                    { 
                        Text = Lang.ResourceManager.GetString("page_account_info_text_no_tickets", CultureInfo.CurrentUICulture), 
                        Foreground = Brushes.DimGray 
                    });
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
            }
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

        private void btnCopyIP_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(lastIPHolder.Text);
        }

        private void btnUpdatePID_Click(object sender, RoutedEventArgs e)
        {
            object cholder = btnUpdatePID.Content;

            btnUpdatePID.IsEnabled = false;

            btnUpdatePID.Content = new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 15,
                Height = 15
            };

            if (Application.Current.MainWindow is Launcher launcher)
            {
                var request_Confirmation_Dialog = new Request_Confirmation_Dialog
                (
                    Lang.ResourceManager.GetString("page_account_info_dialog_profile_title", CultureInfo.CurrentUICulture),
                    $"{Lang.ResourceManager.GetString("page_account_info_dialog_profile_info", CultureInfo.CurrentUICulture)} \"{nicknameTextbox.Text}\""
                );

                request_Confirmation_Dialog.ClipToBounds = true;

                Panel.SetZIndex(request_Confirmation_Dialog, 10000);

                request_Confirmation_Dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (request_Confirmation_Dialog.IsConfirmed)
                    {
                        var updatePublicNicknameResponse = await Newton_Peek.UpdatePublicNicknameResponse(nicknameTextbox.Text);

                        if (updatePublicNicknameResponse != null)
                        {
                            if (updatePublicNicknameResponse.Updated)
                            {
                                request_Confirmation_Dialog.ShowSuccess(Lang.ResourceManager.GetString("page_account_info_dialog_profile_title", CultureInfo.CurrentUICulture), updatePublicNicknameResponse.Message);
                                launcher.nicknameHolder.Text = nicknameTextbox.Text;
                            }
                            else
                            {
                                request_Confirmation_Dialog.ShowFailed(Lang.ResourceManager.GetString("page_account_info_dialog_profile_title", CultureInfo.CurrentUICulture), updatePublicNicknameResponse.Message);
                            }
                        }
                        else
                        {
                            Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
                        }
                    }
                };

                launcher.mainGrid.Children.Add(request_Confirmation_Dialog);
            }

            btnUpdatePID.IsEnabled = true;
            btnUpdatePID.Content = cholder;
        }

        private void btnUnstuck_Click(object sender, RoutedEventArgs e)
        {
            object cholder = btnUnstuck.Content;
            btnUnstuck.IsEnabled = false;

            btnUnstuck.Content = new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 15,
                Height = 15
            };

            if (Application.Current.MainWindow is Launcher launcher)
            {
                if (cbUnstuckCharacters.SelectedItem is Character_Dropdown_Row character_row)
                {
                    var request_Confirmation_Dialog = new Request_Confirmation_Dialog
                    (
                        Lang.ResourceManager.GetString("page_account_info_dialog_unstuck_title", CultureInfo.CurrentUICulture),
                        $"{Lang.ResourceManager.GetString("page_account_info_dialog_unstuck_info", CultureInfo.CurrentUICulture)} \"{character_row.charNameHolder.Text}\""
                    );

                    request_Confirmation_Dialog.ClipToBounds = true;

                    Panel.SetZIndex(request_Confirmation_Dialog, 10000);

                    request_Confirmation_Dialog.OnConfirmationChanged += async (os, be) =>
                    {
                        if (request_Confirmation_Dialog.IsConfirmed)
                        {
                            var cu = await Newton_Peek.CharacterUnstuckResponse(character_row.pCharacter.Guid, character_row.pCharacter.RealmId);

                            if (cu != null)
                            {
                                if (cu.Unstucked)
                                {
                                    request_Confirmation_Dialog.ShowSuccess(Lang.ResourceManager.GetString("page_account_info_dialog_unstuck_title", CultureInfo.CurrentUICulture), cu.Message);
                                    launcher.nicknameHolder.Text = nicknameTextbox.Text;
                                }
                                else
                                {
                                    request_Confirmation_Dialog.ShowFailed(Lang.ResourceManager.GetString("page_account_info_dialog_unstuck_title", CultureInfo.CurrentUICulture), cu.Message);
                                }
                            }
                            else
                            {
                                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
                            }

                            btnUnstuck.Content = cholder;
                            btnUnstuck.IsEnabled = true;
                        }
                        else
                        {

                            btnUnstuck.Content = cholder;
                            btnUnstuck.IsEnabled = true;
                        }
                    };

                    launcher.mainGrid.Children.Add(request_Confirmation_Dialog);
                }
            }
        }

        private void btnTeleport_Click(object sender, RoutedEventArgs e)
        {
            object cholder = btnTeleport.Content;
            btnTeleport.IsEnabled = false;

            btnTeleport.Content = new Viewbox
            {
                Child = new Dotted_Spinner(),
                Width = 15,
                Height = 15
            };

            if (Application.Current.MainWindow is Launcher launcher)
            {
                if (cbTeleportCharacters.SelectedItem is Character_Dropdown_Row character_row 
                    && cbTeleportList.SelectedItem is Teleport_Dropdown_Row teleport_row)
                {
                    var dialog_message = Lang.ResourceManager.GetString("page_account_info_dialog_teleport_info", CultureInfo.CurrentUICulture)
                        .Replace("{0}", $"\"{character_row.charNameHolder.Text}\"")
                        .Replace("{1}", $"\"{teleport_row.pDestination.Name}\"");

                    var buy_dialog = new General_Buy_Dialog
                    (
                        Lang.ResourceManager.GetString("page_account_info_dialog_teleport_title", CultureInfo.CurrentUICulture),
                        dialog_message, teleport_row.pDestination.DPorBPCprice, teleport_row.pDestination.VPrice
                    );

                    buy_dialog.ClipToBounds = true;

                    Panel.SetZIndex(buy_dialog, 10000);

                    buy_dialog.OnConfirmationChanged += async (os, be) =>
                    {
                        if (buy_dialog.IsConfirmed)
                        {
                            var ct = await Newton_Peek.CharacterTeleportResponse(character_row.pCharacter.Guid, character_row.pCharacter.RealmId, teleport_row.pDestination.Id);

                            if (ct != null)
                            {
                                if (ct.Teleported)
                                {
                                    buy_dialog.ShowSuccess(Lang.ResourceManager.GetString("page_account_info_dialog_teleport_title", CultureInfo.CurrentUICulture), ct.Message);
                                    launcher.UpdateVisualCurrencies();
                                }
                                else
                                {
                                    buy_dialog.ShowFailed(Lang.ResourceManager.GetString("page_account_info_dialog_teleport_title", CultureInfo.CurrentUICulture), ct.Message);
                                }
                            }
                            else
                            {
                                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, false);
                            }

                            btnTeleport.Content = cholder;
                            btnTeleport.IsEnabled = true;
                        }
                        else
                        {
                            btnTeleport.Content = cholder;
                            btnTeleport.IsEnabled = true;
                        }
                    };

                    launcher.mainGrid.Children.Add(buy_dialog);
                }
            }
        }

        private void cbUnstuckCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUnstuck.IsEnabled = cbUnstuckCharacters.SelectedItem is Character_Dropdown_Row;
        }

        private void cbTeleportCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnTeleport.IsEnabled = cbTeleportList.SelectedItem is Teleport_Dropdown_Row && cbTeleportCharacters.SelectedItem is Character_Dropdown_Row;
        }

        private void cbTeleportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnTeleport.IsEnabled = cbTeleportList.SelectedItem is Teleport_Dropdown_Row && cbTeleportCharacters.SelectedItem is Character_Dropdown_Row;
        }
    }
}
