/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;
using System.Diagnostics;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Private_Message_New.xaml
    /// </summary>
    public partial class Private_Message_New : UserControl
    {
        public Private_Message_New()
        {
            InitializeComponent();
        }

        public void Load()
        {
            Animations.FadeIn(this, 300);
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            receiverHolder.Text = Lang.ResourceManager.GetString("page_create_message_textbox_nickname", CultureInfo.CurrentUICulture);
            titleHolder.Text = Lang.ResourceManager.GetString("page_create_message_textbox_message_subject", CultureInfo.CurrentUICulture);
            messageHolder.Text = string.Empty;
        }

        private void receiverHolder_GotFocus(object sender, RoutedEventArgs e)
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

        private void messageHolder_TextChanged(object sender, TextChangedEventArgs e)
        {
            charCount.Text = messageHolder.Text.Length.ToString();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                var request_Confirmation_Dialog = new Request_Confirmation_Dialog
                (
                    Lang.ResourceManager.GetString("page_create_message_dialog_title", CultureInfo.CurrentUICulture),
                    Lang.ResourceManager.GetString("page_create_message_dialog_info", CultureInfo.CurrentUICulture)
                );

                request_Confirmation_Dialog.ClipToBounds = true;

                Panel.SetZIndex(request_Confirmation_Dialog, 10000);

                request_Confirmation_Dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (request_Confirmation_Dialog.IsConfirmed)
                    {
                        var newPrivateMessageResponse = await Newton_Peek.NewPrivateMessageResponse(receiverHolder.Text, titleHolder.Text, messageHolder.Text);

                        if (newPrivateMessageResponse != null)
                        {
                            if (newPrivateMessageResponse.Sent)
                            {
                                request_Confirmation_Dialog.ShowSuccess(Lang.ResourceManager
                                    .GetString("page_create_message_dialog_delivery_title", CultureInfo.CurrentUICulture), 
                                        newPrivateMessageResponse.Message);

                                launcher.DisplayPrivateMessagesPage();
                            }
                            else
                            {
                                request_Confirmation_Dialog.ShowFailed(Lang.ResourceManager
                                    .GetString("page_create_message_dialog_delivery_title", CultureInfo.CurrentUICulture), 
                                        newPrivateMessageResponse.Message);
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
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                launcher.DisplayPrivateMessagesPage();
            }
        }
    }
}
