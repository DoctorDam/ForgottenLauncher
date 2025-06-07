/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Launcher_Selfupdater.Library;
using System.Windows;

namespace Launcher_Selfupdater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0)
            {
                string arg1 = e.Args[0];
                string arg2 = e.Args[1];

                Cache.ApiUrl = arg1;
                Cache.ExeTarget = arg2;
            }
            else
            {
                Current.Shutdown();
            }
        }
    }
}
