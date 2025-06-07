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
using System.Collections.Generic;
using Forgotten_Land_Launcher.Modules.Account_Inventory;
using System.Globalization;
using Forgotten_Land_Launcher.Languages;

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Inventory_Reward_Dialog.xaml
    /// </summary>
    public partial class Inventory_Reward_Dialog : UserControl
    {
        private Newton_Main.AccountInventoryResponse pReward;
        public event EventHandler<bool> OnConfirmationChanged;

        public bool IsUsed { get; private set; }

        public Inventory_Reward_Dialog(Newton_Main.AccountInventoryResponse reward)
        {
            InitializeComponent();
            pReward = reward;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 300);

            dialogTitle.Text = pReward.Title;
            Extensions.SetImageBrushSource(pictureHolder, pReward.PictureUrl, UriKind.Absolute);
            dialogDescription.Text = pReward.Description;
            acquiredHolder.Text = pReward.AcquiredOn;
            spPlayerSelection.Visibility = Visibility.Collapsed;
            spInput.Visibility = Visibility.Collapsed;

            if (pReward.RequiresPlayer || pReward.RequiresInput)
            {
                if (pReward.RequiresPlayer)
                {
                    spPlayerSelection.Visibility = Visibility.Visible;
                    UpdateCharactersList();
                }
                
                if (pReward.RequiresInput)
                {
                    spInput.Visibility = Visibility.Visible;
                }
            }
            else
            {
                okButton.IsEnabled = true;
            }
        }

        private async void UpdateCharactersList()
        {
            spPlayerSelectionLoaded.Visibility = Visibility.Collapsed;

            cbCharacters.Items.Clear();

            var charactersList = await Newton_Peek.CharactersListResponse();

            if (charactersList != null)
            {
                if (charactersList.Count > 0)
                {
                    TextBlock selectHolder = new TextBlock();
                    selectHolder.Text = Lang.ResourceManager.GetString("dialog_inventory_item_text_require_character", CultureInfo.CurrentUICulture);
                    selectHolder.Foreground = Extensions.GetColorFromHex("#FFB5B5B5");
                    cbCharacters.Items.Add(selectHolder);
                    cbCharacters.SelectedIndex = 0;

                    foreach (var character in charactersList)
                    {
                        cbCharacters.Items.Add(new Character_Dropdown_Row(character));
                    }
                }
                else
                {
                    TextBlock noCharTB = new TextBlock();
                    noCharTB.Text = Lang.ResourceManager.GetString("dialog_inventory_item_text_no_characters", CultureInfo.CurrentUICulture);
                    noCharTB.Foreground = Extensions.GetColorFromHex("#FFB5B5B5");
                    cbCharacters.Items.Add(noCharTB);
                    cbCharacters.SelectedIndex = 0;
                }
            }

            spPlayerSelectionLoading.Visibility = Visibility.Collapsed;
            spPlayerSelectionLoaded.Visibility = Visibility.Visible;
        }

        private async void okButton_Click(object sender, RoutedEventArgs e)
        {
            spItemDialog.Visibility = Visibility.Collapsed;
            spWaitDialog.Visibility = Visibility.Visible;

            await Task.Delay(500);

            var useInventoryItem = await Newton_Peek.UseInventoryItemResponse
            (
                (cbCharacters.SelectedItem as Character_Dropdown_Row)?.pCharacter.Guid ?? 0,
                pReward.Id, 
                (cbCharacters.SelectedItem as Character_Dropdown_Row)?.pCharacter.RealmId ?? 0
            );

            if (useInventoryItem != null)
            {
                IsUsed = useInventoryItem.Used;

                if (useInventoryItem.Used)
                {
                    waitTitle.Text = Lang.ResourceManager.GetString("dialog_inventory_item_claim_title", CultureInfo.CurrentUICulture);
                    waitDescription.Text = useInventoryItem.Message;
                    ImageBehavior.SetAnimatedSource(checkMarkGif,
                        new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/checkmark_2.gif", UriKind.Relative)));
                }
                else
                {
                    waitTitle.Text = Lang.ResourceManager.GetString("dialog_inventory_item_claim_title", CultureInfo.CurrentUICulture);
                    waitDescription.Text = useInventoryItem.Message;
                    ImageBehavior.SetAnimatedSource(checkMarkGif,
                        new BitmapImage(new Uri("/Forgotten Land Launcher;component/Assets/gifs/xmark_1.gif", UriKind.Relative)));
                }
            }

            vbSpinner.Visibility = Visibility.Collapsed;
            vbCheckmark.Visibility = Visibility.Visible;
            checkMarkGif.Opacity = 0;
            Animations.FadeIn(checkMarkGif, 1000);

            var controller = ImageBehavior.GetAnimationController(checkMarkGif);
            controller.Play();

            await Task.Delay(3500); // animation duration

            OnConfirmationChanged?.Invoke(this, IsUsed);
            CloseDialog();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsUsed = false;
            OnConfirmationChanged?.Invoke(this, IsUsed);
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }

        private void cbCharacters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            okButton.IsEnabled = cbCharacters.SelectedItem is Character_Dropdown_Row;
        }
    }
}
