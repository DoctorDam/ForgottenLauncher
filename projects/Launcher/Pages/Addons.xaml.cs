/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Library;
using Forgotten_Land_Launcher.Modules.Addons;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Pages
{
    /// <summary>
    /// Interaction logic for Addons.xaml
    /// </summary>
    public partial class Addons : UserControl
    {
        public Addons()
        {
            InitializeComponent();
        }

        public async void Load()
        {
            spAddons.Children.Clear();

            spLoading.Visibility = Visibility.Visible;
            spDisabled.Visibility = Visibility.Collapsed;

            spButtons.Visibility = Visibility.Collapsed;

            Animations.FadeIn(this, 300);

            await Task.Delay(1000);

            Page_Options.ApplySettings();

            if (Cache.PageOptions.Addons.PageEnabled)
            {
                InitializeInstalledAddons();
                await InitializeServerAddons();
                ShowServerAddons();

                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Collapsed;
                spButtons.Visibility = Visibility.Visible;
            }
            else
            {
                spLoading.Visibility = Visibility.Collapsed;
                spDisabled.Visibility = Visibility.Visible;
            }

            Animations.FadeIn(this, 300);
        }

        private void ShowLocalAddons()
        {
            foreach (var local_addon in spAddons.Children.OfType<Local_Addon_Span>())
            {
                local_addon.Visibility = Visibility.Visible;
            }
        }

        private void HideLocalAddons()
        {
            foreach (var local_addon in spAddons.Children.OfType<Local_Addon_Span>())
            {
                local_addon.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowServerAddons()
        {
            foreach (var server_addon in spAddons.Children.OfType<Server_Addon_Span>())
            {
                server_addon.Visibility = Visibility.Visible;
            }
        }

        private void HideServerAddons()
        {
            foreach (var server_addon in spAddons.Children.OfType<Server_Addon_Span>())
            {
                server_addon.Visibility = Visibility.Collapsed;
            }
        }

        public void Unload()
        {
            Animations.FadeOut(this, 300);
            spAddons.Children.Clear();
        }

        private void InitializeInstalledAddons()
        {
            string directoryPath = Launcher_Settings.GamePath + @"\Interface\AddOns\";

            if (Directory.Exists(directoryPath))
            {
                string[] folders = Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly);

                foreach (string folder in folders)
                {
                    if (!Path.GetFileName(folder).StartsWith("Blizzard"))
                    {
                        string tocPath = folder + $@"\{Path.GetFileName(folder)}.toc";

                        string author = string.Empty;
                        string version = string.Empty;
                        string size = string.Empty;

                        if (File.Exists(tocPath))
                        {
                            string[] lines = File.ReadAllLines(tocPath);

                            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                            {
                                if (lines[lineNumber].Contains("## Author: "))
                                {
                                    author = lines[lineNumber].Replace("## Author: ", string.Empty);
                                    break;
                                }
                            }

                            for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
                            {
                                if (lines[lineNumber].Contains("## Version: "))
                                {
                                    version = lines[lineNumber].Replace("## Version: ", string.Empty);
                                    break;
                                }
                            }

                            size = Extensions.SizeSuffix(Extensions.GetFolderSize(folder), 2);
                        }

                        var local_addon = new Local_Addon_Span(folder, Path.GetFileName(folder), author, version, size);
                        spAddons.Children.Add(local_addon);
                    }
                }
            }
        }

        private async Task InitializeServerAddons()
        {
            var server_addons = await Newton_Peek.AddonsListResponse();

            if (server_addons != null)
            {
                foreach (var addon in server_addons)
                {
                    var server_addon = new Server_Addon_Span(addon) { Visibility = Visibility.Collapsed };
                    spAddons.Children.Add(server_addon);
                }
            }
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void ButtonInstalledAddons_Click(object sender, RoutedEventArgs e)
        {
            HideServerAddons();
            ShowLocalAddons();
        }

        private void ButtonServerAddons_Click(object sender, RoutedEventArgs e)
        {
            HideLocalAddons();
            ShowServerAddons();
        }
    }
}
