/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Diagnostics;
using System.Windows;

namespace Forgotten_Land_Launcher.Library
{
    internal class Error_Handler
    {
        public static async void Justify(StackTrace stack_trace, string extra_info, string json_response, Exception exception, bool show_popup)
        {
            var stack_frame = stack_trace.GetFrame(0);

            string exception_message = exception != null ? exception.Message : "not provided.";

            extra_info = extra_info != null ? extra_info : "not provided.";

            json_response = json_response != null ? json_response : "not provided.";

            if (show_popup)
            {
                string popup_title = $"{stack_frame.GetMethod().DeclaringType.Name}:{stack_frame.GetMethod().Name}";
                string popup_message = $"Error logged..";

                Extensions.ShowPopup(popup_title, popup_message, Application.Current.MainWindow?.GetType());
            }

            string build_message = $"[Date: {DateTime.Now:yyyy-MM-dd}, Time: {DateTime.Now:HH:mm:ss}]" +
                $"\r\nError in \"{stack_frame.GetMethod().DeclaringType.Name}\" : \"{stack_frame.GetMethod().Name}\"" +
                $"\r\nException message: {exception_message}" +
                $"\r\nExtra information: {extra_info}" +
                $"\r\nJSON response: {json_response}" +
                $"\r\n ";

            Logger.Log(build_message);

            if (exception != null)
            {
                try
                {
                    await Newton_Main.SendIssueToDiscord(Cache.AccountInfo.Username, build_message);
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while trying to send issue to discord: {ex.Message}");
                }
            }
        }
    }
}
