// --------------------------------------------------------------------------
// <copyright file="Cryptography.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.3">Created</change>
// </history>

namespace Fanex.LiveCharts.ClientAuthentication.Core
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Cryptography
    {
        #region Const Params

        private const string CfsKey = "&％#@?,:*!~^.";
        private static readonly DESCryptoServiceProvider CryptoServiceProvider = new DESCryptoServiceProvider();
        private static readonly byte[] RgbKey = new byte[] { 0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc };
        private static readonly byte[] RgbIv = Encoding.UTF8.GetBytes("&％#@?,:*!~^.");

        #endregion Const Params

        /// <summary>
        /// Encrypt text.
        /// </summary>
        /// <param name="plainText">Text need be encrypted.</param>
        /// <returns>String be encrypted.</returns>
        public static string CfsEnCrypt(string plainText)
        {
            var encryptedText = string.Empty;
            try
            {
                byte[] rgbIV = new byte[0];
                byte[] rgbKey = new byte[] { 0x21, 0x63, 0x54, 0x5a, 0x91, 0xac, 110, 0xcc };
                rgbIV = Encoding.UTF8.GetBytes(CfsKey);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream stream2 = new CryptoStream(
                        stream,
                        provider.CreateEncryptor(rgbKey, rgbIV),
                        CryptoStreamMode.Write))
                    {
                        stream2.Write(bytes, 0, bytes.Length);
                        stream2.FlushFinalBlock();
                        encryptedText = Convert.ToBase64String(stream.ToArray());
                    }
                }
            }
            catch
            {
                throw;
            }

            return encryptedText;
        }

        /// <summary>
        /// Decrypt a non-empty encrypted string.
        /// </summary>
        /// <param name="encryptedText">Encrypted text need to be decrypt.</param>
        /// <returns>Text already decrypt.</returns>
        public static string CfsDeCrypt(string encryptedText)
        {
            var decryptedText = string.Empty;

            if (!string.IsNullOrEmpty(encryptedText))
            {
                var buffer = Convert.FromBase64String(encryptedText);
                using (var stream = CreateMemoryStream(buffer))
                {
                    decryptedText = Encoding.UTF8.GetString(stream.ToArray());
                }
            }

            return decryptedText;
        }

        /// <summary>
        /// Use using keyword to dispose object.
        /// </summary>
        /// <param name="buffer">Buffer for create stream.</param>
        /// <returns>Memory stream return.</returns>
        private static MemoryStream CreateMemoryStream(byte[] buffer)
        {
            var stream = new MemoryStream();
            using (var stream2 = new CryptoStream(
                stream,
                CryptoServiceProvider.CreateDecryptor(RgbKey, RgbIv),
                CryptoStreamMode.Write))
            {
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
            }

            return stream;
        }
    }
}