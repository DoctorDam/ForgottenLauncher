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
using Forgotten_Land_Launcher.Windows;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Forgotten_Land_Launcher.Elements.Dropdowns
{
    /// <summary>
    /// Interaction logic for Language_Dropdown_Menu.xaml
    /// </summary>
    public partial class Language_Dropdown_Menu : ComboBox
    {
        public Language_Dropdown_Menu()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var current_ui_lang = CultureInfo.CurrentUICulture.ToString();

                foreach (var lang in Language_Book.Cultures)
                {
                    var sp = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Children =
                        {
                            new Image { Source = new BitmapImage(new Uri($"/Forgotten Land Launcher;component/Assets/icons/flags/{lang.Key}.png", UriKind.Relative)), Height = 18 },
                            new TextBlock { Text = lang.Value, Margin = new Thickness(7, 0, 0, 0) }
                        }
                    };

                    var cbi = new ComboBoxItem { Name = lang.Key, Content = sp };
                    AddChild(cbi);
                }

                foreach (var cbitem in Items.OfType<ComboBoxItem>())
                {
                    if (cbitem.Name == current_ui_lang)
                    {
                        SelectedItem = cbitem;
                    }
                }

            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selected_language = (SelectedItem as ComboBoxItem);

                Thread.CurrentThread.CurrentCulture = new CultureInfo(selected_language.Name);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(selected_language.Name);

                Launcher_Settings.InterfaceLang = selected_language.Name;
                Launcher_Settings.Save();
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }
    }
}
