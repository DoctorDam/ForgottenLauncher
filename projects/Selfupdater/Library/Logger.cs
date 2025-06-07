/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.IO;

namespace Launcher_Selfupdater.Library
{
    internal static class Logger
    {
        private static StreamWriter logWriter;

        static Logger()
        {
            string logDirectory = "launcher logs";
            Directory.CreateDirectory(logDirectory); // Create directory if it doesn't exist

            string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} - updater.log");
            logWriter = File.AppendText(logFilePath);
        }

        public static void Log(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} ----------------\r\n{message}";
                logWriter.WriteLine(logEntry);
                logWriter.Flush(); // Flush the writer to ensure the data is written immediately
            }
            catch
            {

            }
        }

        public static void Close()
        {
            logWriter.Close();
        }
    }
}
