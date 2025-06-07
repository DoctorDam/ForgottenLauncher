/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System.IO;
using System.Linq;

namespace Forgotten_Land_Launcher.Library
{
    internal class Addon_Handler
    {
        public static bool IsAddonInstalled(string addon_name)
        {
            string addons_path = Launcher_Settings.GamePath + "/interface/addons";

            if (Directory.Exists(addons_path))
            {
                string[] matchingDirectories = Directory.GetDirectories(addons_path, "*", SearchOption.TopDirectoryOnly)
                                               .Where(folder => Path.GetFileName(folder).ToLower().Contains(addon_name.ToLower()))
                                               .ToArray();

                return matchingDirectories.Length > 0;
            }

            return false;
        }
    }
}
