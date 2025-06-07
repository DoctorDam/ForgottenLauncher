/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launcher_Selfupdater.Library
{
    internal class Newton_Peek
    {
        public static async Task<List<Newton_Main.LauncherFilesListResponse>> LauncherUpdateFilesListResponse()
        {
            string json = string.Empty;

            try
            {
                json = await Newton_Main.GetLauncherUpdateFilesListResponseResponse();

                return Newton_Main.LauncherFilesListResponse.FromJson(json);
            }
            catch (Exception ex)
            {
                string message = $"[File '{Extensions.GetCurrentFileName()}' - Method 'LauncherUpdateFilesListResponse']\r\nException error: {ex.Message}\r\nApi message: {json}";

                Logger.Log(message);
            }

            return null;
        }
    }
}
