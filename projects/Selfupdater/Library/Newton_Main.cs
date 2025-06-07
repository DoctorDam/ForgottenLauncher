/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Launcher_Selfupdater.Library
{
    internal partial class Newton_Main
    {
        #region LAUNCHER FILES LIST RESPONSE

        public class LauncherFilesListResponse
        {
            [JsonProperty("Name")]
            public string Name { get; set; }

            [JsonProperty("Size")]
            public long Size { get; set; }

            [JsonProperty("Timestamp")]
            public int Timestamp { get; set; }

            [JsonProperty("TargetPath")]
            public string TargetPath { get; set; }

            [JsonProperty("Url")]
            public string Url { get; set; }

            public static List<LauncherFilesListResponse> FromJson(string json) => 
                JsonConvert.DeserializeObject<List<LauncherFilesListResponse>>(json, Converter.Settings);
        }

        public static async Task<string> GetLauncherUpdateFilesListResponseResponse()
        {
            return await GetStringFromPOST(APIUrl, new Dictionary<string, string>
            {
                { "execute", "109" },
            });
        }

        #endregion
    }
}
