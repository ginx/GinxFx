// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptoUtils.cs" company="Ginx Corp.">
//   Copyright © 2013 · Ginx Corp. · All rights reserved.
// </copyright>
// <summary>
//   Defines the CryptoUtils type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GinxFx.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Utility class to help usage of cryptography functions.
    /// </summary>
    public static class CryptoUtils
    {
        /// <summary>
        /// Defines the shared salt, used to help encrypt the password.
        /// </summary>
        private static readonly byte[] SharedSalt = Encoding.ASCII.GetBytes("{31FAE506-E550-4041-963C-7D5F6323DAD4}");

        /// <summary>
        /// Creates a random salt.
        /// A salt is used to strong the encrypted password.
        /// </summary>
        /// <param name="size">
        /// The size of the generated salt.
        /// </param>
        /// <returns>
        /// The generated salt.
        /// </returns>
        public static string CreateSalt(int size)
        {
            var saltBytes = new byte[size];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Decrypts the <paramref name="cipherData"/> previously encrypted by <see cref="EncryptBytes"/>.
        /// </summary>
        /// <param name="cipherData">
        /// The cipher data.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The decrypted data.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", 
            "CA2202:Do not dispose objects multiple times", 
            Justification = "Safe usage of streams. Suppressed message for simplicity of code.")]
        public static byte[] DecryptBytes(byte[] cipherData, string password)
        {
            if (cipherData == null)
            {
                throw new ArgumentNullException("cipherData");
            }

            using (var cipher = new RijndaelManaged())
            {
                using (var key = new Rfc2898DeriveBytes(password, SharedSalt))
                {
                    cipher.Key = key.GetBytes(cipher.KeySize / 8);
                    cipher.IV = key.GetBytes(cipher.BlockSize / 8);
                }

                using (var decryptor = cipher.CreateDecryptor(cipher.Key, cipher.IV))
                {
                    using (var memoryStream = new MemoryStream(cipherData))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var output = new byte[cipherData.Length];

                            cryptoStream.Read(output, 0, output.Length);

                            return output;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts the <paramref name="cipherText" /> previously encrypted by
        /// <seealso cref="EncryptText" />.
        /// </summary>
        /// <param name="cipherText">The cipher text encoded in Base64.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        public static string DecryptText(string cipherText, string password)
        {
            var cipherData = Convert.FromBase64String(cipherText);
            using (var cipher = new RijndaelManaged())
            {
                using (var key = new Rfc2898DeriveBytes(password, SharedSalt))
                {
                    cipher.Key = key.GetBytes(cipher.KeySize / 8);
                    cipher.IV = key.GetBytes(cipher.BlockSize / 8);
                }

                using (var decryptor = cipher.CreateDecryptor(cipher.Key, cipher.IV))
                {
                    using (var memoryStream = new MemoryStream(cipherData))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the <paramref name="plainData"/> array of bytes using specified <paramref name="password"/>.
        /// </summary>
        /// <param name="plainData">
        /// The plain data.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The raw encrypted data.
        /// </returns>
        public static byte[] EncryptBytes(byte[] plainData, string password)
        {
            if (plainData == null)
            {
                throw new ArgumentNullException("plainData");
            }

            using (var cipher = new RijndaelManaged())
            {
                using (var key = new Rfc2898DeriveBytes(password, SharedSalt))
                {
                    cipher.Key = key.GetBytes(cipher.KeySize / 8);
                    cipher.IV = key.GetBytes(cipher.BlockSize / 8);
                }

                using (var encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainData, 0, plainData.Length);

                            return memoryStream.ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The one way encrypted password.
        /// </returns>
        public static string EncryptPassword(string password)
        {
            return EncryptPassword(password, string.Empty);
        }

        /// <summary>
        /// Encrypts the password using a salt.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="salt">
        /// The salt of password.
        /// </param>
        /// <returns>
        /// The one way encrypted password.
        /// </returns>
        public static string EncryptPassword(string password, string salt)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(password + salt);

            byte[] hashBytes;

            using (HashAlgorithm hash = new SHA512Managed())
            {
                hashBytes = hash.ComputeHash(buffer);
            }

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Encrypts the <paramref name="plaintext"/> string using the specified <paramref name="password"/>.
        /// </summary>
        /// <param name="plaintext">
        /// The plain text string.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The encrypted <paramref name="plaintext"/> encoded in Base64.
        /// </returns>
        public static string EncryptText(string plaintext, string password)
        {
            using (var cipher = new RijndaelManaged())
            {
                using (var key = new Rfc2898DeriveBytes(password, SharedSalt))
                {
                    cipher.Key = key.GetBytes(cipher.KeySize / 8);
                    cipher.IV = key.GetBytes(cipher.BlockSize / 8);
                }

                using (var encryptor = cipher.CreateEncryptor(cipher.Key, cipher.IV))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (var streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(plaintext);

                                return Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }
                    }
                }
            }
        }
    }
}