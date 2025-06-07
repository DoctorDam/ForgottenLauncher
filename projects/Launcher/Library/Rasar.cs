/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace Forgotten_Land_Launcher.Library
{
    internal class Rasar
    {
        public class OracleAuthTokenData
        {
            public string Token { get; set; }
            public string User { get; set; }
        }

        public class GameLoginData
        {
            public string Pass { get; set; }
        }

        public static void Encrypt_and_Save_Auth_Token_Data(string oatoken, string oauser)
        {
            string json = JsonConvert.SerializeObject(new { Token = oatoken, User = oauser }, Formatting.Indented);

            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string oracleLauncherFolder = Path.Combine(myDocumentsPath, Assembly.GetExecutingAssembly().GetName().Name);

            // Create the "Oracle Launcher" folder if it doesn't exist
            if (!Directory.Exists(oracleLauncherFolder))
            {
                Directory.CreateDirectory(oracleLauncherFolder);
            }

            string keyFilePath = Path.Combine(oracleLauncherFolder, "oracle_oauth_key.bin");
            string ivFilePath = Path.Combine(oracleLauncherFolder, "oracle_oauth_iv.bin");
            string tokenFilePath = Path.Combine(oracleLauncherFolder, "oracle_oauth_token.dat");

            // Generate a new encryption key
            byte[] key = Generate_Encryption_Key();
            // Generate a random IV
            byte[] iv = Generate_IV();

            // Encrypt the text
            byte[] encryptedData = Encrypt(json, key, iv);

            // Save the key and IV to files in the "Oracle Launcher" folder
            File.WriteAllBytes(keyFilePath, key);
            File.WriteAllBytes(ivFilePath, iv);

            // Save the encrypted data to a file in the "Oracle Launcher" folder
            File.WriteAllBytes(tokenFilePath, encryptedData);
        }

        public static void Encrypt_and_Save_Game_Login(string password)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new { Pass = password }, Formatting.Indented);

                byte[] key = new byte[32]; // 256-bit key
                byte[] iv = new byte[16];  // 128-bit IV

                // Populate key and IV with random bytes
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(key);
                    rng.GetBytes(iv);
                }

                Cache.AccountInfo.RasarKey = Convert.ToBase64String(key);
                Cache.AccountInfo.RasarIV = Convert.ToBase64String(iv);

                byte[] encryptedData = Encrypt(json, key, iv);

                Launcher_Settings.LoginGameData = Convert.ToBase64String(encryptedData);
                Launcher_Settings.Save();
            }
            catch (Exception ex)
            {
                Error_Handler.Justify(new StackTrace(), null, null, ex, false);
            }
        }

        public static GameLoginData Decrypt_Game_Login()
        {
            try
            {
                byte[] encryptedData = Convert.FromBase64String(Launcher_Settings.LoginGameData);
                byte[] rasarKeyBytes = Convert.FromBase64String(Cache.AccountInfo.RasarKey);
                byte[] rasarIVBytes = Convert.FromBase64String(Cache.AccountInfo.RasarIV);

                string decrypted = Decrypt(encryptedData, rasarKeyBytes, rasarIVBytes);

                return JsonConvert.DeserializeObject<GameLoginData>(decrypted);
            }
            catch
            {
                return null;
            }
        }

        public static OracleAuthTokenData Decrypt_Auth_Token_Data()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string oracleLauncherFolder = Path.Combine(myDocumentsPath, Assembly.GetExecutingAssembly().GetName().Name);

            string keyFilePath = Path.Combine(oracleLauncherFolder, "oracle_oauth_key.bin");
            string ivFilePath = Path.Combine(oracleLauncherFolder, "oracle_oauth_iv.bin");
            string tokenFilePath = Path.Combine(oracleLauncherFolder, "oracle_oauth_token.dat");

            if (!File.Exists(keyFilePath) || !File.Exists(ivFilePath) || !File.Exists(tokenFilePath))
            {
                // Handle missing files or other error conditions.
                // You may want to return an error or take appropriate action.
                return null;
            }

            // Read the encryption key and IV from files in the "Oracle Launcher" folder
            byte[] key = File.ReadAllBytes(keyFilePath);
            byte[] iv = File.ReadAllBytes(ivFilePath);

            // Read the encrypted data from the file in the "Oracle Launcher" folder
            byte[] encryptedData = File.ReadAllBytes(tokenFilePath);

            return JsonConvert.DeserializeObject<OracleAuthTokenData>(Decrypt(encryptedData, key, iv));
        }

        public static void Delete_Invalid_Auth_Token_Data()
        {
            string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string oracleLauncherFolder = Path.Combine(myDocumentsPath, Assembly.GetExecutingAssembly().GetName().Name);

            Delete_File_If_Exists(oracleLauncherFolder, "oracle_oauth_key.bin");
            Delete_File_If_Exists(oracleLauncherFolder, "oracle_oauth_iv.bin");
            Delete_File_If_Exists(oracleLauncherFolder, "oracle_oauth_token.dat");
        }

        private static void Delete_File_If_Exists(string folderPath, string fileName)
        {
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Error_Handler.Justify(new StackTrace(), null, null, ex, false);
                }
            }
        }


        public static byte[] Generate_IV()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[16]; // 128 bits
                rng.GetBytes(iv);
                return iv;
            }
        }

        public static byte[] Generate_Encryption_Key()
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                return aesAlg.Key;
            }
        }

        private static byte[] Encrypt(string text, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        private static string Decrypt(byte[] encryptedData, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
