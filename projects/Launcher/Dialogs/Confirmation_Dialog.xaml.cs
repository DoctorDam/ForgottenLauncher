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
    /// Interaction logic for Confirmation_Dialog.xaml
    /// </summary>
    public partial class Confirmation_Dialog : UserControl
    {
        public event EventHandler<bool> OnConfirmationChanged;

        public bool IsConfirmed { get; private set; }

        private string pTitle;
        private string pText;

        public Confirmation_Dialog(string title, string text)
        {
            InitializeComponent();
            pTitle = title;
            pText = text;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            confirmationTitle.Text = pTitle;
            confirmationText.Text = pText;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeIn(this, 200);

            IsConfirmed = true;
            OnConfirmationChanged?.Invoke(this, IsConfirmed);
            CloseDialog();
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
