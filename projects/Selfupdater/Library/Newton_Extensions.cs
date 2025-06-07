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

namespace Launcher_Selfupdater.Library
{
    internal partial class Newton_Main
    {
        private static string APIUrl
        {
            get
            {
                string url = Cache.ApiUrl;
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
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task SendJsonPOST(string URL, Dictionary<string, string> values)
        {
            try
            {
                var content = new FormUrlEncodedContent(values);

                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(URL, content);
                }
            }
            catch
            {

            }
        }
    }
}
