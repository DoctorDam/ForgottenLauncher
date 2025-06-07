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

namespace Forgotten_Land_Launcher.Library
{
    internal static class Logger
    {
        private static StreamWriter logWriter;

        static Logger()
        {
            string logDirectory = "launcher logs";
            Directory.CreateDirectory(logDirectory);

            string logFilePath = Path.Combine(logDirectory, $"{DateTime.Now:yyyy-MM-dd HH-mm-ss} - launcher.log");
            logWriter = File.AppendText(logFilePath);
        }

        public static void Log(string message)
        {
            try
            {
                string logEntry = message;
                logWriter.WriteLine(logEntry);
                logWriter.Flush();
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
