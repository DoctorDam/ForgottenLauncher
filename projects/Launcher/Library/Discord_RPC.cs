/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Diagnostics;

namespace Forgotten_Land_Launcher.Library
{
    internal class Discord_RPC
    {
        public static DiscordRpcClient client;

        // Called when your application first starts.
        // For example, just before your main loop, on OnEnable for unity.
        public static void Initialize()
        {
            try
            {
                client = new DiscordRpcClient(Properties.Settings.Default.DiscordAppID);

                // Set the logger
                client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

                // Subscribe to events
                client.OnReady += (sender, e) =>
                {
                    //MessageBox.Show($"Received Ready from user {e.User.Username}");
                };

                client.OnPresenceUpdate += (sender, e) =>
                {
                    //MessageBox.Show($"Received Update! {e.Presence}");
                };

                // Connect to the RPC
                client.Initialize();

                // Set in lobby status
                Game_Handler.SetInLobbyDiscordRPCStatus();
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        // Called when your application terminates.
        void Deinitialize()
        {
            client.Dispose();
        }
    }
}
