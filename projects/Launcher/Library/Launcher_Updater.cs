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
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Forgotten_Land_Launcher.Library
{
    internal class Launcher_Updater
    {
        public static async Task<bool> RequiresUpdate()
        {
            var serverFiles = await Newton_Peek.LauncherUpdateFilesListResponse();

            if (serverFiles != null)
            {
                int diff = 0;

                foreach (var file in serverFiles)
                {
                    file.TargetPath = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{file.TargetPath}";

                    if (await FileIsDifferentAsync(file))
                    {
                        diff++;
                    }
                }

                return diff > 0;
            }

            return false;
        }

        public static void StartUpdate()
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Launcher Selfupdater.exe",
                    Arguments = $"\"{Properties.Settings.Default.ApiUrl}\" \"{AppDomain.CurrentDomain.FriendlyName}\"",
                };

                Process process = new Process { StartInfo = processStartInfo };
                process.Start();

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        private static async Task<bool> FileIsDifferentAsync(Newton_Main.LauncherUpdateFilesListResponse file)
        {
            try
            {
                FileInfo localFile = new FileInfo(file.TargetPath);
                return file.MD5Hash != await Extensions.GetLocalFileMD5HashAsync(file.TargetPath);
            }
            catch
            {
                return true;
            }
        }
    }
}
