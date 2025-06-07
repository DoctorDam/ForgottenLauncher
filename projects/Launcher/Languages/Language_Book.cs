/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System.Collections.Generic;
using System.Globalization;

namespace Forgotten_Land_Launcher.Languages
{
    internal static class Language_Book
    {
        public static Dictionary<string, string> Cultures = new Dictionary<string, string>();

        public static void Update()
        {
            Cultures = new Dictionary<string, string>()
            {
                { "en", Lang.ResourceManager.GetString("language_english", CultureInfo.CurrentUICulture) },
                { "bg", Lang.ResourceManager.GetString("language_bulgarian", CultureInfo.CurrentUICulture) },
                { "de", Lang.ResourceManager.GetString("language_german", CultureInfo.CurrentUICulture) },
                { "es", Lang.ResourceManager.GetString("language_spanish", CultureInfo.CurrentUICulture) },
                { "ro", Lang.ResourceManager.GetString("language_romanian", CultureInfo.CurrentUICulture) },
                { "ru", Lang.ResourceManager.GetString("language_russian", CultureInfo.CurrentUICulture) },
                { "ko", Lang.ResourceManager.GetString("language_korean", CultureInfo.CurrentUICulture) },
            };
        }
    }
}
