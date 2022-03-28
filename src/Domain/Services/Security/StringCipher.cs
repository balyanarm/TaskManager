using System;
using System.Security.Cryptography;

namespace Domain.Services.Security
{
    public static class StringCipher
    {
        public static string HashString(string plainText, string passPhrase)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                return String.Empty;
            }

            // Uses SHA256 to create the hash
            using (var sha = new SHA256Managed())
            {
                // Convert the string to a byte array first, to be processed
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(plainText + passPhrase);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                // Convert back to a string, removing the '-' that BitConverter adds
                string hash = BitConverter
                    .ToString(hashBytes)
                    .Replace("-", String.Empty);

                return hash;
            }
        }       
    }
}