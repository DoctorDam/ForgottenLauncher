/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

namespace Forgotten_Land_Launcher.Library
{
    internal class Cache
    {
        public static string AuthToken;

        public static Newton_Main.AccountInfo AccountInfo = new Newton_Main.AccountInfo();

        public static string RealmListAddress;

        public static Newton_Main.PagesOptions PageOptions;

        public static Newton_Main.CharactersMarketSettings charactersMarketSettings;
    }
}
