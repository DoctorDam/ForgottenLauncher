/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Forgotten_Land_Launcher.Modules.Realms
{
    /// <summary>
    /// Interaction logic for Server_Status_Span.xaml
    /// </summary>
    public partial class Server_Status_Span : UserControl
    {
        private Newton_Main.RealmsStatusResponse pServer;

        public Server_Status_Span(Newton_Main.RealmsStatusResponse server)
        {
            InitializeComponent();
            pServer = server;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            statusHolder.Fill = pServer.OnlineStatus ? Brushes.Lime : Brushes.Red;
            nameHolder.Text = pServer.RealmName;
        }
    }
}
