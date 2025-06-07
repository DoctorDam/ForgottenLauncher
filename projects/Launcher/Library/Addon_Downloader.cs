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
using System.IO;
using System.Net;

namespace Forgotten_Land_Launcher.Library
{
    internal class Addon_Downloader
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

        private List<Newton_Main.AddonFile> DownloadList = new List<Newton_Main.AddonFile>();

        private WebClient updater = new WebClient();

        private DateTime WstartTime;

        public long TotalSizeDownloaded; // in bytes

        public bool StopRequested = false;

        public void Prepare(Newton_Main.AddonsListResponse addon)
        {
            DownloadList.Clear();
            TotalSizeDownloaded = 0;

            if (addon != null)
            {
                foreach (var file in addon.Files)
                {
                    file.LocalTarget = file.LocalTarget.Replace("/", "\\");
                    file.LocalTarget = Launcher_Settings.GamePath + "\\" + file.LocalTarget;

                    DownloadList.Add(file);
                }
            }
        }

        public bool Start()
        {
            StopRequested = false;

            if (DownloadList.Count > 0)
            {
                var file = DownloadList[0]; // Get the first file

                if (!string.IsNullOrEmpty(Path.GetDirectoryName(file.LocalTarget))
                    && !string.IsNullOrWhiteSpace(Path.GetDirectoryName(file.LocalTarget)))
                    if (!Directory.Exists(Path.GetDirectoryName(file.LocalTarget)))
                        Directory.CreateDirectory(Path.GetDirectoryName(file.LocalTarget));

                DownloadFile(file.DownloadUrl, file.LocalTarget);

                return true;
            }

            return false;
        }

        public void Stop()
        {
            StopRequested = true;
        }

        private void DownloadFile(string url, string destination)
        {
            using (updater = new WebClient())
            {
                updater.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged); // progress change event

                updater.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed); // completed event

                Uri downloadURL = new Uri(url);

                Logger.Log("Started downloading addon file: " + downloadURL);

                updater.DownloadFileAsync(downloadURL, destination);
            }
        }


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
                updater.CancelAsync();
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                Logger.Log("Updater stopped at addon file: " + DownloadList[0].DownloadUrl);
                OnStopped(e);
            }
            else
            {
                Logger.Log("Successfuly downloaded addon file: " + DownloadList[0].DownloadUrl);

                TotalSizeDownloaded += DownloadList[0].Size;

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
    }
}
