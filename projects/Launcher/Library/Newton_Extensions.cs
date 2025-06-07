/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forgotten_Land_Launcher.Library
{
    partial class Newton_Main
    {
        private static string APIUrl
        {
            get
            {
                string url = Properties.Settings.Default.ApiUrl;
                return url.EndsWith(".php") ? url : url.EndsWith("/") ? url : url += "/";
            }
        }

        internal static class Converter
        {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,

                Converters =
                {
                    new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
            };
        }

        public static async Task<string> GetStringFromPOST(string URL, Dictionary<string, string> values)
        {
            if (string.IsNullOrWhiteSpace(URL) || values == null || values.Count == 0)
                return string.Empty;

            try
            {
                using (var client = new HttpClient())
                using (var content = new FormUrlEncodedContent(values))
                {
                    var response = await client.PostAsync(URL, content).ConfigureAwait(false);
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task SendJsonPOST(string URL, Dictionary<string, string> values)
        {
            if (string.IsNullOrWhiteSpace(URL) || values == null || values.Count == 0)
                return;

            try
            {
                using (var client = new HttpClient())
                using (var content = new FormUrlEncodedContent(values))
                {
                    await client.PostAsync(URL, content);
                }
            }
            catch
            {
                // Log or handle the exception as needed
            }
        }
    }
}
