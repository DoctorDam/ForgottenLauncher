/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Forgotten_Land_Launcher.Dialogs;
using Forgotten_Land_Launcher.Languages;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Forgotten_Land_Launcher.Modules.Addons
{
    /// <summary>
    /// Interaction logic for Local_Addon_Span.xaml
    /// </summary>
    public partial class Local_Addon_Span : UserControl
    {
        private string pFolderPath;
        private string pTitle;
        private string pAuthor;
        private string pVersion;
        private string pSize;

        public Local_Addon_Span(string folderPath, string title, string author, string version, string size)
        {
            InitializeComponent();

            pFolderPath = folderPath;
            pTitle = title;
            pAuthor = author;
            pVersion = version;
            pSize = size;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            nameHolder.Text = pTitle;
            authorHolder.Text = pAuthor;
            versionHolder.Text = pVersion;
            sizeHolder.Text = pSize;
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is Launcher launcher)
            {
                var simple_Confirmation_Dialog = new Confirmation_Dialog
                (
                    Lang.ResourceManager.GetString("dialog_addon_remove_title", CultureInfo.CurrentUICulture),
                    Lang.ResourceManager.GetString("dialog_addon_remove_info", CultureInfo.CurrentUICulture).Replace("{0}", pTitle)
                );
                
                simple_Confirmation_Dialog.ClipToBounds = true;

                Panel.SetZIndex(simple_Confirmation_Dialog, 10000);

                simple_Confirmation_Dialog.OnConfirmationChanged += (os, be) =>
                {
                    if (simple_Confirmation_Dialog.IsConfirmed)
                    {
                        if (Directory.Exists(pFolderPath))
                        {
                            try
                            {
                                var dir = new DirectoryInfo(@pFolderPath);
                                dir.Delete(true);

                                launcher.pageAddons.Load();
                            }
                            catch
                            {

                            }
                        }
                    }
                };

                launcher.mainGrid.Children.Add(simple_Confirmation_Dialog);
            }
        }
    }
}
