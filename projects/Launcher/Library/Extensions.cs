/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Elements.Popups;
using Forgotten_Land_Launcher.Windows;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Forgotten_Land_Launcher.Library
{
    internal class Extensions
    {
        public static SolidColorBrush GetColorFromHex(string hexColor)
        {
            if (string.IsNullOrWhiteSpace(hexColor) || hexColor.Length != 9 || !hexColor.StartsWith("#"))
                return new SolidColorBrush(Colors.White);

            try
            {
                byte a = Convert.ToByte(hexColor.Substring(1, 2), 16);
                byte r = Convert.ToByte(hexColor.Substring(3, 2), 16);
                byte g = Convert.ToByte(hexColor.Substring(5, 2), 16);
                byte b = Convert.ToByte(hexColor.Substring(7, 2), 16);

                return new SolidColorBrush(Color.FromArgb(a, r, g, b));
            }
            catch
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public static async void SetImageSource(Image image, string uri, UriKind uriKind)
        {
            if (string.IsNullOrWhiteSpace(uri)) return;

            try
            {
                if (uriKind == UriKind.Absolute &&
                    (!Uri.IsWellFormedUriString(uri, UriKind.Absolute) || !await ImageExistsAtUrl(uri)))
                    return;

                image.Source = new BitmapImage(new Uri(uri, uriKind));
            }
            catch
            {
                // Handle exceptions if necessary
            }
        }

        public static async void SetImageBrushSource(ImageBrush imageBrush, string uri, UriKind uriKind)
        {
            if (string.IsNullOrWhiteSpace(uri))
                return;

            try
            {
                if (uriKind == UriKind.Absolute)
                {
                    if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute) || !await ImageExistsAtUrl(uri))
                        return;
                }
                else if (uriKind == UriKind.Relative)
                {
                    uri = $"pack://application:,,,{uri}";
                }

                imageBrush.ImageSource = new BitmapImage(new Uri(uri, uriKind));
            }
            catch
            {
                // Log or handle exceptions if necessary
            }
        }

        public static async Task<bool> ImageExistsAtUrl(string url)
        {
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse httpRes = (HttpWebResponse)await httpReq.GetResponseAsync())
                {
                    return httpRes.StatusCode != HttpStatusCode.NotFound;
                }
            }
            catch
            {
                return false;
            }
        }

        public static string GetCurrentFileName([CallerFilePath] string fileName = "")
        {
            fileName = Path.GetFileName(fileName);
            return fileName;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            const string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public static void ShowPopup(string title, string message, Type windowType)
        {
            if (Application.Current.MainWindow != null && windowType.IsInstanceOfType(Application.Current.MainWindow))
            {
                Popup_Message popup = new Popup_Message
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 10, 10, 0),
                    PopupTitle = title,
                    PopupMessage = message
                };

                if (windowType == typeof(Launcher))
                {
                    ((Launcher)Application.Current.MainWindow).spPopups.Children.Insert(0, popup);
                }
                else if (windowType == typeof(Login))
                {
                    ((Login)Application.Current.MainWindow).spPopups.Children.Insert(0, popup);
                }
            }
        }

        public static long GetFolderSize(string folderPath)
        {
            try
            {
                return new DirectoryInfo(folderPath)
                    .EnumerateFiles("*.*", SearchOption.AllDirectories)
                    .Sum(file => file.Length);
            }
            catch
            {
                return 0;
            }
        }

        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) throw new ArgumentOutOfRangeException(nameof(decimalPlaces));
            if (value < 0) return "-" + SizeSuffix(-value, decimalPlaces);
            if (value == 0) return $"0 bytes";

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag++;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public static async Task<string> GetLocalFileMD5HashAsync(string filePath)
        {
            using (var md5 = MD5.Create())
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                var hash = await Task.Run(() => md5.ComputeHash(stream));
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string BreakDownApiUrl()
        {
            return Properties.Settings.Default.ApiUrl
                .TrimEnd('/')
                .Replace("/vision.php", "");
        }

        public static string GetStringAfterWord(string sentence, string word)
        {
            int index = sentence.IndexOf(word);
            return index != -1 ? sentence.Substring(index + word.Length) : sentence;
        }
    }
}
