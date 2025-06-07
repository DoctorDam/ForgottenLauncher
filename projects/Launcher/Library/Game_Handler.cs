/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using DiscordRPC;
using Forgotten_Land_Launcher.Windows;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Threading;
using static Forgotten_Land_Launcher.Library.Newton_Main;

namespace Forgotten_Land_Launcher.Library
{
    public class Game_Handler
    {
        public static void UpdateRealmlist()
        {
            string gamepath = Launcher_Settings.GamePath;

            string configWTFPath = $@"{gamepath}\WTF\Config.wtf";

            SetRealmlistPerLocale();

            try
            {
                if (File.Exists(configWTFPath))
                {
                    var oldLines = File.ReadAllLines(configWTFPath);

                    var newLines = oldLines.Where(line => !line.ToLower().Contains("set realmlist"));

                    File.WriteAllLines(configWTFPath, newLines);

                    using (var outputFile = new StreamWriter(configWTFPath, true))
                    {
                        outputFile.WriteLine($"SET realmList \"{Cache.RealmListAddress}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        private static void SetRealmlistPerLocale()
        {
            string gamepath = Launcher_Settings.GamePath;

            try
            {
                string[] locales = new string[] 
                { 
                    "enUS", "esMX", "ptBR", "deDE", "enGB", "esES", "frFR", "itIT", "ruRU", "koKR", "zhTW", "zhCN" 
                };

                foreach (var d in Directory.GetDirectories($@"{gamepath}\data"))
                {
                    var dir = new DirectoryInfo(d);

                    var dirName = dir.Name;

                    if (locales.Contains(dirName))
                    {
                        string configWTFPath = $@"{gamepath}\data\{dirName}\Realmlist.wtf";

                        if (File.Exists(configWTFPath))
                        {
                            var oldLines = File.ReadAllLines(configWTFPath);

                            var newLines = oldLines.Where(line => !line.ToLower().Contains("set realmlist"));

                            File.WriteAllLines(configWTFPath, newLines);

                            using (var outputFile = new StreamWriter(configWTFPath, true))
                            {
                                outputFile.WriteLine($"SET realmList \"{Cache.RealmListAddress}\"");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        public static void ClearCache()
        {
            string gamepath = Launcher_Settings.GamePath;

            try
            {
                if (Directory.Exists($"{gamepath}\\Cache") && Launcher_Settings.ClearCache)
                {
                    var dir = new DirectoryInfo($"{gamepath}\\Cache");
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        public static async void SetGameAccount()
        {
            var emulator_id = EmulatorIDResponse.FromJson(await GetEmulatorIDResponse()).EmulatorID;
            var account_to_write = Cache.AccountInfo.Username;

            if (emulator_id == 3 || emulator_id == 18)
            {
                account_to_write = Cache.AccountInfo.Email;
            }

            string gamepath = Launcher_Settings.GamePath;

            string configWTFPath = $@"{gamepath}\WTF\Config.wtf";

            try
            {
                if (File.Exists(configWTFPath))
                {
                    var oldLines = File.ReadAllLines(configWTFPath);

                    var newLines = oldLines.Where(line => !line.ToLower().Contains("set accountname"));

                    File.WriteAllLines(configWTFPath, newLines);

                    using (var outputFile = new StreamWriter(configWTFPath, true))
                    {
                        outputFile.WriteLine($"SET accountName \"{account_to_write}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        public static void SetReadTOSandWindowMode()
        {
            string gamepath = Launcher_Settings.GamePath;

            string configWTFPath = $@"{gamepath}\WTF\Config.wtf";

            try
            {
                if (File.Exists(configWTFPath))
                {
                    var oldLines = File.ReadAllLines(configWTFPath);

                    var newLines = oldLines.Where(line => !line.ToLower().Contains("set readtos") 
                        && !line.ToLower().Contains("set readeula")
                        && !line.ToLower().Contains("set gxwindow "));

                    File.WriteAllLines(configWTFPath, newLines);

                    using (var outputFile = new StreamWriter(configWTFPath, true))
                    {
                        outputFile.WriteLine($"SET readTOS \"1\"");
                        outputFile.WriteLine($"SET readEULA \"1\"");
                        outputFile.WriteLine($"SET showToolsUI \"1\"");
                        outputFile.WriteLine($"SET gxWindow \"1\"");
                    }
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        public static async Task DetectGameStarted(Game_Login_Overlay glo)
        {
            Process wowProcess = null;
            bool processFound = false;

            try
            {
                while (!processFound)
                {
                    foreach (Process process in Process.GetProcesses())
                    {
                        if (process.ProcessName.Equals("Wow", StringComparison.OrdinalIgnoreCase))
                        {
                            wowProcess = process;
                            processFound = true;
                            break;
                        }
                    }

                    if (!processFound)
                    {
                        await Task.Delay(1000); // Sleep for 1 second before checking again
                    }
                }

                Bitmap initialScreenshot = CaptureProcessWindow(wowProcess);

                while (!wowProcess.HasExited)
                {
                    Bitmap currentScreenshot = CaptureProcessWindow(wowProcess);

                    bool pixelsChanged = CheckPixelChange(initialScreenshot, currentScreenshot);

                    if (pixelsChanged)
                    {
                        glo.OnGameStarted(wowProcess);
                        break; // Exit the while loop if pixels have changed
                    }

                    initialScreenshot.Dispose();

                    initialScreenshot = currentScreenshot;

                    await Task.Delay(1000);
                }
            }
            catch
            {
                await Task.Delay(1000);
                await DetectGameStarted(glo);
            }
        }

        private static Bitmap CaptureProcessWindow(Process process)
        {
            IntPtr mainWindowHandle = process.MainWindowHandle;

            if (mainWindowHandle == IntPtr.Zero)
            {
                //throw new ArgumentException("Invalid window handle");
            }

            NativeMethods.RECT windowRect;
            NativeMethods.GetClientRect(mainWindowHandle, out windowRect);

            Bitmap screenshot = new Bitmap(windowRect.Right, windowRect.Bottom, PixelFormat.Format32bppArgb);

            using (Graphics gfx = Graphics.FromImage(screenshot))
            {
                IntPtr hdcBitmap = gfx.GetHdc();
                try
                {
                    NativeMethods.PrintWindow(mainWindowHandle, hdcBitmap, 0);
                }
                finally
                {
                    gfx.ReleaseHdc(hdcBitmap);
                }
            }

            return screenshot;
        }

        private static bool CheckPixelChange(Bitmap initialScreenshot, Bitmap currentScreenshot)
        {
            if (initialScreenshot.Size != currentScreenshot.Size)
                return true;

            for (int x = 0; x < initialScreenshot.Width; x++)
            {
                for (int y = 0; y < initialScreenshot.Height; y++)
                {
                    Color initialPixel = initialScreenshot.GetPixel(x, y);
                    Color currentPixel = currentScreenshot.GetPixel(x, y);

                    if (currentPixel.A != 0 && currentPixel != Color.White && currentPixel != Color.Black)
                        return true;
                }
            }

            return false;
        }

        public static void KillProcess(string processName)
        {
            foreach (var process in Process.GetProcessesByName(processName))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit(); // Optional
                }
                catch (Exception ex)
                {
                    Error_Handler.Justify(new StackTrace(), null, null, ex, false);
                }
            }
        }

        private static DispatcherTimer discord_rpc_timer = new DispatcherTimer();

        public static void StartDiscordRPCUpdater()
        {
            discord_rpc_timer.Interval = TimeSpan.FromSeconds(10);

            discord_rpc_timer.Tick += Discord_rpc_timer_Tick;

            discord_rpc_timer.Start();
        }

        public static void StopDiscordRPCUpdater()
        {
            discord_rpc_timer.Stop();

            SetInLobbyDiscordRPCStatus();
        }

        private static async void Discord_rpc_timer_Tick(object sender, EventArgs e)
        {
            var online_character = await Newton_Peek.DiscordRPCOnlineCharacter();

            if (online_character != null)
            {
                SetIngameDiscordRPCStatus(online_character);
            }
        }

        public static void RemoveReadOnlyAttribute(string directoryPath)
        {
            try
            {
                var dirInfo = new DirectoryInfo(directoryPath);

                foreach (var fileInfo in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    fileInfo.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        public static void SetDirectoryPermissions(string directoryPath)
        {
            try
            {
                var dInfo = new DirectoryInfo(directoryPath);

                var dSecurity = dInfo.GetAccessControl();

                dSecurity.AddAccessRule(new FileSystemAccessRule
                (
                    new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null),
                    FileSystemRights.FullControl,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                    PropagationFlags.NoPropagateInherit,
                    AccessControlType.Allow)
                );

                dInfo.SetAccessControl(dSecurity);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        public static void SetInLobbyDiscordRPCStatus()
        {
            Discord_RPC.client.SetPresence(new RichPresence()
            {
                Assets = new Assets()
                {
                    LargeImageKey = Properties.Settings.Default.DiscordAppPicture,
                    LargeImageText = Properties.Settings.Default.HomeUrl,
                    SmallImageKey = "https://media4.giphy.com/media/xmOMPI63SsyZyKz2Tx/giphy.gif",
                    SmallImageText = "Verified",
                },

                Details = Properties.Settings.Default.HomeUrl,
                State = $"In launcher lobby..",

                //Timestamps = Timestamps.FromTimeSpan(1000),
                Buttons = new Button[]
                {
                    new Button() { Label = "Join me", Url = Properties.Settings.Default.HomeUrl }
                }
            });
        }

        public static void SetIngameDiscordRPCStatus(DiscordRPCOnlineCharacter character)
        {
            string race_img_url = $"{Extensions.BreakDownApiUrl()}/" +
                $"{Extensions.GetStringAfterWord(WoW_Definitions.GetPlayerRaceIcon(character.Race, character.Gender), ";component/").ToLower()}";
            
            string class_img_url = $"{Extensions.BreakDownApiUrl()}/" +
                $"{Extensions.GetStringAfterWord(WoW_Definitions.GetPlayerClassIcon(character.Class), ";component/").ToLower()}";

            Discord_RPC.client.SetPresence(new RichPresence()
            {
                Assets = new Assets()
                {
                    LargeImageKey = race_img_url,
                    LargeImageText = WoW_Definitions.GetPlayerRaceName(character.Race),
                    SmallImageKey = class_img_url,
                    SmallImageText = WoW_Definitions.GetPlayerClassName(character.Class),
                },

                Details = $"{character.Name} - {character.RealmName}",
                State = character.ZoneName,

                //Timestamps = Timestamps.FromTimeSpan(1000),
                Buttons = new Button[]
                {
                    new Button() { Label = "Join me", Url = Properties.Settings.Default.HomeUrl }
                }
            });
        }
    }

    internal class NativeMethods
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, uint nFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}
