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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Forgotten_Land_Launcher.Library
{
    internal class Game_Updater
    {
        // Define a delegate for the progress change event
        public delegate void ProgressChangedEventHandler(object sender, DownloadProgressChangedEventArgs e);
        // Define a delegate for the completed event
        public delegate void CompletedEventHandler(object sender, AsyncCompletedEventArgs e);

        // Declare the progress change event using the delegate
        public event ProgressChangedEventHandler ProgressChangedEvent;
        // Declare the completed event using the delegate
        public event CompletedEventHandler CompletedEvent;
        // Declare the completed event using the delegate
        public event CompletedEventHandler StoppedEvent;

        // Hook for ProgressChangedEvent
        private void OnProgressChanged(DownloadProgressChangedEventArgs e) => ProgressChangedEvent?.Invoke(this, e);
        // Hook for CompletedEvent
        private void OnCompleted(AsyncCompletedEventArgs e) => CompletedEvent?.Invoke(this, e);
        // Hook for StoppedEvent
        private void OnStopped(AsyncCompletedEventArgs e) => StoppedEvent?.Invoke(this, e);

        private List<Newton_Main.GameFilesListResponse> DownloadList = new List<Newton_Main.GameFilesListResponse>();
        private List<Newton_Main.GameFilesListResponse> ServerClientFiles = new List<Newton_Main.GameFilesListResponse>();
        private List<Newton_Main.GameFilesListResponse> UserClientFiles = new List<Newton_Main.GameFilesListResponse>();
        private WebClient updater = new WebClient();
        public Stopwatch SWSpeed = new Stopwatch();
        private DateTime WstartTime;

        public bool StopRequested = false;

        public long TotalSizeToDownload; // in bytes
        public long TotalSizeDownloaded; // in bytes

        /// <summary>
        /// Builds up the downloads list
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateList()
        {
            DownloadList.Clear();
            ServerClientFiles.Clear();
            UserClientFiles.Clear();

            TotalSizeToDownload = 0;
            TotalSizeDownloaded = 0;

            if (Application.Current.MainWindow is Launcher launcher)
            {
                launcher.checksumFile.Text = "All files..";

                var gameFilesList = await Newton_Peek.GameFilesListResponse();

                if (gameFilesList != null)
                {
                    launcher.spChecksum.Visibility = Visibility.Visible;

                    foreach (var file in gameFilesList)
                    {
                        file.TargetPath = file.TargetPath.Replace("/", "\\");

                        if (file.IsProgramData)
                        {
                            string winDir = Environment.GetEnvironmentVariable("WINDIR");
                            string winPart = winDir.Replace(winDir.Substring(winDir.IndexOf("\\") + 1), string.Empty);

                            file.TargetPath = winPart + @"ProgramData" + file.TargetPath;
                        }
                        else
                        {
                            file.TargetPath = Launcher_Settings.GamePath + file.TargetPath;

                            if (file.TargetPath.ToLower().EndsWith(".mpq"))
                                ServerClientFiles.Add(file);
                        }

                        if (file.IsHD && !Launcher_Settings.HDTextures)
                            continue; // skip file if "hd textures" is disabled

                        if (file.IsBase && !Launcher_Settings.CheckFullGame)
                            continue; // skip file if "check full game" is disabled

                        if (FileIsDifferentAsync(file))
                        {
                            DownloadList.Add(file);
                        }
                    }

                    DownloadList = DownloadList
                        .OrderBy(file => file.Url.Contains("base-client/") ? 0 : 1)
                            .ThenBy(file => file.Url.Contains("base-hd/") ? 0 : 1)
                                .ThenBy(file => file.Url.Contains("push-updates/") ? 0 : 1)
                    .ToList();

                    DownloadList = DownloadList.GroupBy(x => x.TargetPath)
                           .SelectMany(g => g.OrderByDescending(x => x.Timestamp).Take(1))
                           .ToList();

                    TotalSizeToDownload = DownloadList.Sum(item => item.Size);

                    launcher.spChecksum.Visibility = Visibility.Hidden;

                    return DownloadList.Count > 0;
                }
            }

            return false;
        }

        /// <summary>
        /// Starts the downloader
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            StopRequested = false;

            if (DownloadList.Count > 0)
            {
                var file = DownloadList[0]; // Get the first file

                if (!string.IsNullOrEmpty(Path.GetDirectoryName(file.TargetPath)) 
                    && !string.IsNullOrWhiteSpace(Path.GetDirectoryName(file.TargetPath)))
                    if (!Directory.Exists(Path.GetDirectoryName(file.TargetPath)))
                        Directory.CreateDirectory(Path.GetDirectoryName(file.TargetPath));

                DownloadFile(file.Url, file.TargetPath);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Stops the downloader
        /// </summary>
        public void Stop()
        {
            StopRequested = true;
        }

        /// <summary>
        /// Starts downloading a file
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destination"></param>
        private void DownloadFile(string url, string destination)
        {
            WstartTime = DateTime.Now;

            using (updater = new WebClient())
            {
                updater.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged); // progress change event

                updater.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed); // completed event

                Uri downloadURL = new Uri(url);

                SWSpeed.Start(); // Start the stopwatch which we will be using to calculate the download speed

                Logger.Log("Started downloading file: " + downloadURL);

                updater.DownloadFileAsync(downloadURL, destination);
            }
        }

        /// <summary>
        /// Event on progress changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TimeSpan span = DateTime.Now - WstartTime;

            if (span.TotalMilliseconds >= 250)
            {
                WstartTime = DateTime.Now;

                OnProgressChanged(e);
            }

            if (StopRequested)
            {
                CancelUpdating();
            }
        }

        /// <summary>
        /// Stops the downloader
        /// </summary>
        private void CancelUpdating()
        {
            updater.CancelAsync();
        }

        /// <summary>
        /// Event on downloads completed or stopped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            SWSpeed.Reset(); // download speed timer

            if (e.Cancelled == true)
            {
                Logger.Log("Updater stopped at file: " + DownloadList[0].Url);
                OnStopped(e);
            }
            else
            {
                TotalSizeDownloaded += DownloadList[0].Size;

                Logger.Log("Successfuly downloaded file: " + DownloadList[0].Url);

                DownloadList.RemoveAt(0);

                if (DownloadList.Count > 0) // continue what's left
                {
                    Start();
                }
                else // all downloads completed
                {
                    OnCompleted(e);
                }
            }
        }

        /// <summary>
        /// Checks if file is different based on some criterias
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool FileIsDifferentAsync(Newton_Main.GameFilesListResponse file)
        {
            try
            {
                if (file.TargetPath.ToLower().Contains("config.wtf") || file.IsBase || file.IsHD) // config.wtf and base game files
                {
                    return !File.Exists(file.TargetPath); // returns true if file is missing or false if exists
                }
                else // other files
                {
                    string targetPath = file.TargetPath.Replace(Launcher_Settings.GamePath, string.Empty);

                    if (Properties.Settings.Default.IgnoreDefaultFiles && Game_Default_Files.IgnoreList.Any(ignoreFile =>
                        ignoreFile.ToLower().Contains(targetPath.ToLower())))
                    {
                        //Logger.Log($"File {targetPath} was ignored because it's marked to be ignored if exists by default.");
                        return false;
                    }

                    FileInfo localFile = new FileInfo(file.TargetPath);

                    // for some webhosts wow client's html files return different size
                    // if local html file is 0kb
                    return file.TargetPath.ToLower().EndsWith(".html") ? localFile.Length == 0 : localFile.Length != file.Size;
                }
            }
            catch
            {
                return true; // file doesn't exist
            }
        }

        /// <summary>
        /// Syncs user's client patches
        /// </summary>
        private void SyncUserClientPatches()
        {
            string[] extensions = { ".mpq", ".MPQ" };
            string[] allfiles = Directory.GetFiles(Launcher_Settings.GamePath, "*.*", SearchOption.AllDirectories)
                                          .Where(file => extensions.Contains(Path.GetExtension(file)))
                                          .ToArray();

            foreach (var file in allfiles)
            {
                UserClientFiles.Add(new Newton_Main.GameFilesListResponse()
                {
                    Name = Path.GetFileName(file),
                    TargetPath = file,
                });
            }
        }

        /// <summary>
        /// Checks if user's patch is extra
        /// </summary>
        /// <param name="serverPatches"></param>
        /// <param name="userPatch"></param>
        /// <returns></returns>
        private bool IsExtraClientPatch(List<Newton_Main.GameFilesListResponse> serverPatches, Newton_Main.GameFilesListResponse userPatch)
        {
            foreach(var server_patch in serverPatches)
            {
                if (server_patch.TargetPath.ToLower() == userPatch.TargetPath.ToLower())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Backup user's extra patches
        /// </summary>
        public void BackupExtraGameFiles()
        {
            try
            {
                SyncUserClientPatches();

                foreach (Newton_Main.GameFilesListResponse user_patch in UserClientFiles)
                {
                    if (IsExtraClientPatch(ServerClientFiles, user_patch))
                    {
                        string newBackupPath = $"{Launcher_Settings.GamePath}\\Backup-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}\\{user_patch.TargetPath.Replace(Launcher_Settings.GamePath + "\\", string.Empty)}";

                        string directoryPath = Path.GetDirectoryName(newBackupPath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        File.Move(user_patch.TargetPath, newBackupPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }
    }
}
