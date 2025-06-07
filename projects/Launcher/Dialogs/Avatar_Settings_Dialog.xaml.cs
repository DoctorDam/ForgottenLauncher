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

namespace Forgotten_Land_Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for Avatar_Settings_Dialog.xaml
    /// </summary>
    public partial class Avatar_Settings_Dialog : UserControl
    {
        public event EventHandler<bool> OnConfirmationChanged;
        public string pNewAvatarUrl;

        public bool AvatarChanged { get; private set; }

        private string pCurrAvatarUrl;

        public Avatar_Settings_Dialog(string currAvatarUrl)
        {
            InitializeComponent();
            pCurrAvatarUrl = currAvatarUrl;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);
            try
            {
                Extensions.SetImageBrushSource(avatarHolder, pCurrAvatarUrl, UriKind.Absolute);
                urlHolder.Text = pCurrAvatarUrl;
            }
            catch
            {

            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            AvatarChanged = true;
            OnConfirmationChanged?.Invoke(this, AvatarChanged);
            CloseDialog();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            AvatarChanged = false;
            OnConfirmationChanged?.Invoke(this, AvatarChanged);
            CloseDialog();
        }

        private void CloseDialog()
        {
            if (Parent is Panel parentPanel)
            {
                parentPanel.Children.Remove(this);
            }
        }

        private void urlHolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Extensions.SetImageBrushSource(avatarHolder, urlHolder.Text, UriKind.Absolute);
                pNewAvatarUrl = urlHolder.Text;
            }
            catch
            {

            }
        }
    }
}
