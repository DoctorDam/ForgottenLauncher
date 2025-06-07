/*
Copyright © 2024 by Cybermist2

This software and its source code are intellectual property of Cybermist2.
You may not use, copy, modify, distribute, or sublicense this software or any part of it, 
except as expressly provided under a written agreement with Cybermist2.

Author: cybermist2@gmail.com
Website: https://codebyvision.net
*/

using System;
using System.Security.Cryptography;
using System.Text;

namespace Forgotten_Land_Launcher.Library
{
    internal class Codex_Standard
    {
        public static string ToVendettaWho(string who)
        {
            who = who.ToLower();

            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(who));

            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();

            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string ToVendettaToken(string username, string password)
        {
            string combo = username + ":" + password;

            combo = combo.ToUpper();

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(combo));

                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public static string ToRegObscure(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            string hexString = BitConverter.ToString(bytes).Replace("-", "");

            return hexString;
        }

        public static string FromRegObscure(string output)
        {
            byte[] bytes = new byte[output.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(output.Substring(i * 2, 2), 16);
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
