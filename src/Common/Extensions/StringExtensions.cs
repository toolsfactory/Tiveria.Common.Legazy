using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Tiveria.Common.Extensions
{
    public static class StringExtensions
    {
        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        public static string ToMD5Hash(this string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static bool CompareMd5Hash(this string input, string hash)
        {
            // Hash the input.
            string hashOfInput = input.ToMD5Hash();

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            // Now compare the hashes
            if (0 == comparer.Compare(hashOfInput, hash))
                return true;
            else
                return false;
        }


        public static bool IsValidEmail(this string input)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(input,
                   @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        public static bool IsValidUrl(this string url)
        {
            Uri uri;
            var ok = Uri.TryCreate(url, UriKind.Absolute, out uri);
            if (!ok)
                return false;

            var result = Uri.CheckHostName(uri.Host);
            return (result != UriHostNameType.Unknown);
        }

        public static bool IsValidIP(this string url)
        {
            System.Net.IPAddress address;
            return System.Net.IPAddress.TryParse(url, out address);
        }

        public static bool IsValidHostName(this string url)
        {
            var regex = new Regex(@"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$");
            return regex.IsMatch(url);
        }

        public static bool FromFile(this string input, string fileName, out string result)
        {
            result = null;
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(fileName, Encoding.ASCII))
                {
                    result = sr.ReadToEnd();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool ToFile(this string input, string fileName)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamWriter sr = new StreamWriter(fileName, false, Encoding.ASCII))
                {
                    sr.WriteLine(input);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string ReplaceInvalidPathChars(this string original, char replacement)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                if (original.IndexOf(c) != -1)
                    original = original.Replace(c, '_');

            return original;
        }

        public static bool IsNullOrWhiteSpace(this string input)
        {
            return String.IsNullOrWhiteSpace(input);
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return String.IsNullOrEmpty(input);
        }
    }
}
