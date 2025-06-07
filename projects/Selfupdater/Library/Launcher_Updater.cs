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
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Launcher_Selfupdater.Library
{
    internal class Launcher_Updater
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

        private List<Newton_Main.LauncherFilesListResponse> DownloadList = new List<Newton_Main.LauncherFilesListResponse>();
        private WebClient updater = new WebClient();
        public Stopwatch SWSpeed = new Stopwatch();
        private DateTime WstartTime;

        public bool StopRequested = false;

        public long TotalSizeToDownload; // in bytes
        public long TotalSizeDownloaded; // in bytes

        /// <summary>
        /// Returns true only if there is something new to download
        /// </summary>
        /// <returns>true or false</returns>
        public async Task<bool> UpdateList()
        {
            DownloadList.Clear();
            TotalSizeToDownload = 0;
            TotalSizeDownloaded = 0;

            List<Newton_Main.LauncherFilesListResponse> gameFilesList = await Newton_Peek.LauncherUpdateFilesListResponse();

            if (gameFilesList != null)
            {
                Updater_Window launcherWindow = Application.Current.MainWindow as Updater_Window;

                foreach (Newton_Main.LauncherFilesListResponse file in gameFilesList)
                {
                    // update and combine to full client path
                    file.TargetPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + file.TargetPath;

                    await Task.Delay(5);

                    launcherWindow.spIntegrity.Visibility = Visibility.Visible;

                    launcherWindow.checkFile.Text = file.Name;

                    if (FileIsDifferentAsync(file))
                    {
                        DownloadList.Add(file);
                    }
                }

                TotalSizeToDownload = DownloadList.Sum(item => item.Size);

                launcherWindow.spIntegrity.Visibility = Visibility.Collapsed;

                return DownloadList.Count > 0;
            }
            else
            {
                // ..
            }

            return false;
        }

        /// <summary>
        /// Returns true or false if file is different or doesn't exist
        /// </summary>
        /// <param name="file"></param>
        /// <returns>true or false</returns>
        private bool FileIsDifferentAsync(Newton_Main.LauncherFilesListResponse file)
        {
            try
            {
                FileInfo localFile = new FileInfo(file.TargetPath);

                return localFile.Length != file.Size ||
                      int.Parse(ConvertToUnixTime(File.GetLastWriteTimeUtc(Path.GetDirectoryName(file.TargetPath)))) != file.Timestamp;
            }
            catch (Exception ex)
            {
                Logger.Log($"[File '{Extensions.GetCurrentFileName()}' - Method 'FileIsDifferentAsync']\r\nException error ({file.TargetPath}): {ex.Message}");

                return true;
            }
        }

        /// <summary>
        /// Converts DateTime to Universal Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private string ConvertToUnixTime(DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime.ToUniversalTime());
            return dto.ToUnixTimeSeconds().ToString();
        }

        /// <summary>
        /// Converts Universal Time to DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Starts downloading new or missing game files
        /// </summary>
        public bool Start()
        {
            StopRequested = false;

            try
            {
                if (DownloadList.Count > 0)
                {
                    Newton_Main.LauncherFilesListResponse file = DownloadList[0]; // Get the first file

                    if (file !=null)
                    {
                        if (!string.IsNullOrEmpty(Path.GetDirectoryName(file.TargetPath))
                            && !string.IsNullOrWhiteSpace(Path.GetDirectoryName(file.TargetPath)))
                            if (!Directory.Exists(Path.GetDirectoryName(file.TargetPath)))
                                Directory.CreateDirectory(Path.GetDirectoryName(file.TargetPath));

                        DownloadFile(file.Url, file.TargetPath);

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("error");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"[File '{Extensions.GetCurrentFileName()}' - Method 'Start']\r\nException error: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Stops the downloader but doesn't delete what's downloaded
        /// </summary>
        public void Stop()
        {
            StopRequested = true;
        }

        /// <summary>
        /// Downloads a file from specified url to destionation path
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
        /// Event for download progress
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
        /// Cancels the downloader
        /// </summary>
        private void CancelUpdating()
        {
            updater.CancelAsync();
        }

        /// <summary>
        /// Event for download completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Completed(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                SWSpeed.Reset(); // download speed timer

                if (e.Cancelled == true)
                {
                    Logger.Log("Updater stopped at file: " + DownloadList[0].Url);
                    OnStopped(e);
                }
                else
                {
                    await Task.Delay(1000);

                    File.SetLastWriteTime(DownloadList[0].TargetPath, UnixTimeStampToDateTime(DownloadList[0].Timestamp));
                    
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
            catch (Exception ex)
            {
                Logger.Log($"[File '{Extensions.GetCurrentFileName()}' - Method 'Completed']\r\nException error: {ex.Message}");
            }
        }
    }
}
