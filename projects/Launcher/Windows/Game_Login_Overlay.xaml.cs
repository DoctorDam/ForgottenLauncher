/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Languages;
using Forgotten_Land_Launcher.Library;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Forgotten_Land_Launcher.Windows
{
    /// <summary>
    /// Interaction logic for Game_Login_Overlay.xaml
    /// </summary>
    public partial class Game_Login_Overlay : Window
    {
        public static class WindowsServices
        {
            public const int HWND_TOPMOST = -1;
            public const uint SWP_NOMOVE = 0x0002;
            public const uint SWP_NOSIZE = 0x0001;

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]

            public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        Process pProcess;

        public Game_Login_Overlay()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            serverNameHolder.Text = $"{Properties.Settings.Default.ServerName} -";
            usernameHolder.Text = Cache.AccountInfo.Username;
            WindowState = WindowState.Maximized;

            if (Application.Current.MainWindow is Launcher launcher)
            {
                avatarHolder.ImageSource = launcher.avatarHolder.ImageSource;
            }

            var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowPos(hwnd, WindowsServices.HWND_TOPMOST, 0, 0, 0, 0, WindowsServices.SWP_NOMOVE | WindowsServices.SWP_NOSIZE);

            await Game_Handler.DetectGameStarted(this);
        }

        public void OnGameStarted(Process process_found)
        {
            pProcess = process_found;

            waitPlaceholder.Text = Lang.ResourceManager.GetString("window_game_login_text_game_loaded", CultureInfo.CurrentUICulture);
            waitPlaceholder.Foreground = System.Windows.Media.Brushes.Lime;
            loadingSpinner.Visibility = Visibility.Collapsed;
            ButtonLogin.IsEnabled = true;
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            IntPtr zero = IntPtr.Zero;

            for (int i = 0; (i < 60) && (zero == IntPtr.Zero); i++)
            {
                Thread.Sleep(500);
                zero = FindWindow(null, pProcess.MainWindowTitle);
            }
            if (zero != IntPtr.Zero)
            {
                SetForegroundWindow(zero);

                if (Rasar.Decrypt_Game_Login() != null)
                {
                    System.Windows.Forms.SendKeys.SendWait(Rasar.Decrypt_Game_Login().Pass);
                    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                    System.Windows.Forms.SendKeys.Flush();
                }
                else
                {
                    Animations.FadeOut(this, 250);
                    await Task.Delay(300);
                    Close();

                    MessageBox.Show("Please log off the launcher and log back in for the game password to be stored and encrypted.");
                }
            }

            Animations.FadeOut(this, 250);
            await Task.Delay(300);
            Close();
        }

        private async void ButtonContinueMnaually_Click(object sender, RoutedEventArgs e)
        {
            Animations.FadeOut(this, 250);
            await Task.Delay(300);
            Close();
        }
    }
}
