/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Account_Info
{
    /// <summary>
    /// Interaction logic for Ticket_Span.xaml
    /// </summary>
    public partial class Ticket_Span : Button
    {
        public Newton_Main.CharactersTicketsListResponse pTicket;

        public Ticket_Span(Newton_Main.CharactersTicketsListResponse ticket)
        {
            InitializeComponent();
            pTicket = ticket;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            if (pTicket.Completed)
            {
                Extensions.SetImageSource(statusIconHolder, "/Forgotten Land Launcher;component/Assets/icons/icon_checked_1.png", UriKind.Relative);
                statusTextHolder.Text = Lang.ResourceManager.GetString("general_text_ticket_completed", CultureInfo.CurrentUICulture);
            }
            else if (pTicket.Closed)
            {
                Extensions.SetImageSource(statusIconHolder, "/Forgotten Land Launcher;component/Assets/icons/icon_x_mark_1_a.png", UriKind.Relative);
                statusTextHolder.Text = Lang.ResourceManager.GetString("general_text_ticket_closed", CultureInfo.CurrentUICulture);
            }
            else if (pTicket.Viewed)
            {
                Extensions.SetImageSource(statusIconHolder, "/Forgotten Land Launcher;component/Assets/icons/icon_eye_1_a.png", UriKind.Relative);
                statusTextHolder.Text = Lang.ResourceManager.GetString("general_text_ticket_viewed", CultureInfo.CurrentUICulture);
            }
            else
            {
                Extensions.SetImageSource(statusIconHolder, "/Forgotten Land Launcher;component/Assets/icons/icon_clock_1_a.png", UriKind.Relative);
                statusTextHolder.Text = Lang.ResourceManager.GetString("general_text_ticket_pending", CultureInfo.CurrentUICulture);
            }

            Extensions.SetImageSource(raceIconHolder, WoW_Definitions.GetPlayerRaceIcon(pTicket.PlayerRace, pTicket.PlayerGender), UriKind.Relative);

            charNameHolder.Text = pTicket.PlayerName;
            lastModifiedTimeHolder.Text = pTicket.LastModifiedTime;
            messageHolder.Text = pTicket.Message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // do what
        }
    }
}
