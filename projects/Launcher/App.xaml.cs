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
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Forgotten_Land_Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Launcher_Settings.Initialize();

            Language_Book.Update();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Launcher_Settings.InterfaceLang);
        }
    }
}
