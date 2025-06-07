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
using Forgotten_Land_Launcher.Modules.Private_Messages;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Private_Message_View.xaml
    /// </summary>
    public partial class Private_Message_View : UserControl
    {
        private int pMessageId = 0;

        public Private_Message_View()
        {
            InitializeComponent();
        }

        public void Load(int id)
        {
            pMessageId = id;
            Animations.FadeIn(this, 300);
            InitializeMessageThread(pMessageId);
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            spMessages.Children.Clear();
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

        private async void InitializeMessageThread(int message_id)
        {
            spMessages.Children.Clear();

            var privateMessageThread = await Newton_Peek.PrivateMessageThreadResponse(message_id);

            if (privateMessageThread != null)
            {
                foreach (var pmt in privateMessageThread)
                {
                    Message_Span message_Spawn = new Message_Span(pmt)
                    {
                        Margin = new Thickness(0, 0, 0, 15)
                    };

                    spMessages.Children.Add(message_Spawn);

                    Animations.MoveRightAndFadeIn(message_Spawn, 250, 250, -1000);

                    await Task.Delay(50);

                    if (pmt.Title.Length > 0)
                        titleHolder.Text = pmt.Title;
                }
            }
            else
            {
                Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
            }

            await Task.Delay(250);

            swMessages.ScrollToVerticalOffset(swMessages.ScrollableHeight);
        }

        private void btnReply_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                var request_Confirmation_Dialog = new Request_Confirmation_Dialog
                (
                    Lang.ResourceManager.GetString("page_message_view_dialog_title", CultureInfo.CurrentUICulture),
                    Lang.ResourceManager.GetString("page_message_view_dialog_info", CultureInfo.CurrentUICulture)
                );

                request_Confirmation_Dialog.ClipToBounds = true;

                Panel.SetZIndex(request_Confirmation_Dialog, 10000);

                request_Confirmation_Dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (request_Confirmation_Dialog.IsConfirmed)
                    {
                        var newMessageReplyResponse = await Newton_Peek.NewMessageReplyResponse(pMessageId, replyBox.messageHolder.Text);

                        if (newMessageReplyResponse != null)
                        {
                            if (newMessageReplyResponse.Sent)
                            {
                                request_Confirmation_Dialog.ShowSuccess(Lang.ResourceManager.GetString("page_message_view_dialog_delivery_title", CultureInfo.CurrentUICulture), newMessageReplyResponse.Message);
                                InitializeMessageThread(pMessageId);
                                replyBox.messageHolder.Text = Lang.ResourceManager.GetString("page_message_view_textbox_reply", CultureInfo.CurrentUICulture);
                            }
                            else
                            {
                                request_Confirmation_Dialog.ShowFailed(Lang.ResourceManager.GetString("page_message_view_dialog_delivery_title", CultureInfo.CurrentUICulture), newMessageReplyResponse.Message);
                            }
                        }
                        else
                        {
                            Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
                        }
                    }
                };

                launcher.mainGrid.Children.Add(request_Confirmation_Dialog);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                var request_Confirmation_Dialog = new Request_Confirmation_Dialog
                (
                    "Delete Message",
                    $"Would you like to delete this private message for you?\r\n" +
                    $"\r\nThis will not delete the message for the other user!"
                );

                request_Confirmation_Dialog.ClipToBounds = true;

                Panel.SetZIndex(request_Confirmation_Dialog, 10000);

                request_Confirmation_Dialog.OnConfirmationChanged += async (os, be) =>
                {
                    if (request_Confirmation_Dialog.IsConfirmed)
                    {
                        var deletePrivateMessageResponse = await Newton_Peek.DeletePrivateMessageResponse(pMessageId);

                        if (deletePrivateMessageResponse != null)
                        {
                            if (deletePrivateMessageResponse.Deleted)
                            {
                                request_Confirmation_Dialog.ShowSuccess("Deletion", deletePrivateMessageResponse.Message);
                                launcher.DisplayPrivateMessagesPage();
                            }
                            else
                            {
                                request_Confirmation_Dialog.ShowFailed("Deletion", deletePrivateMessageResponse.Message);
                            }
                        }
                        else
                        {
                            Error_Handler.Justify(new StackTrace(), "invalid json response", null, null, true);
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
