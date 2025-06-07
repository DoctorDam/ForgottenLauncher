/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace Forgotten_Land_Launcher.Library
{
    internal class Launcher_Settings
    {
        public static string GamePath = "";
        public static bool HDTextures = false;
        public static bool ClearCache = false;
        public static bool AutoLogin = false;
        public static string LoginGameData = "";
        public static bool CheckFullGame = true;
        public static string InterfaceLang = "en";

        public static void Initialize()
        {
            if (!Exists())
            {
                Save(); // this also generates a new file
            }
            else
            {
                Load();
            }
        }

        public static bool Exists()
        {
            string keyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Launcher_Settings.xml");

            return File.Exists(keyFilePath);
        }

        public static void Load()
        {
            try
            {
                string keyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Launcher_Settings.xml");

                if (!File.Exists(keyFilePath))
                    return;

                var doc = XDocument.Load(keyFilePath);

                GamePath        = GetNodeValue(doc, "GamePath");
                HDTextures      = GetBooleanNodeValue(doc, "HDTextures");
                ClearCache      = GetBooleanNodeValue(doc, "ClearCache");
                AutoLogin       = GetBooleanNodeValue(doc, "AutoLogin");
                LoginGameData   = GetNodeValue(doc, "LoginGameData");
                CheckFullGame   = GetBooleanNodeValue(doc, "CheckFullGame");
                InterfaceLang   = GetNodeValue(doc, "InterfaceLang");
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }

        private static string GetNodeValue(XDocument doc, string nodeName)
        {
            var node = doc.Root?.Element(nodeName);
            return node?.Value;
        }

        private static bool GetBooleanNodeValue(XDocument doc, string nodeName)
        {
            var node = doc.Root?.Element(nodeName);
            return node != null && bool.TryParse(node.Value, out var result) ? result : false;
        }

        public static void Save()
        {
            try
            {
                string keyFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Launcher_Settings.xml");

                var doc = new XDocument
                (
                    new XElement
                    (
                        "Settings",
                        new XElement("GamePath",        GamePath),
                        new XElement("HDTextures",      HDTextures),
                        new XElement("ClearCache",      ClearCache),
                        new XElement("AutoLogin",       AutoLogin),
                        new XElement("LoginGameData",   LoginGameData),
                        new XElement("CheckFullGame",   CheckFullGame),
                        new XElement("InterfaceLang",   InterfaceLang)
                    )
                );

                doc.Save(keyFilePath);
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, true);
            }
        }
    }
}
