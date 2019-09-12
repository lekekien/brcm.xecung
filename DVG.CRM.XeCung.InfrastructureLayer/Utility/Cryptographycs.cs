using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVG.CRM.XeCung.InfrastructureLayer.Utility
{
    public static class Cryptography
    {
        private const string SecurityKey = "dvsitdepartment";

        /// <summary>
        /// Encrypt Data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Encrypt(this string input)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            hashmd5.Clear();
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Encrypt Data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MoneyEncrypt(this string input)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            hashmd5.Clear();
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Encrypt Data
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(this string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey + key));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            hashmd5.Clear();
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Decrypt Data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Decrypt(this string input)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            hashmd5.Clear();
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Decrypt Data
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MoneyDecrypt(this string input)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            hashmd5.Clear();
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(this string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey + key));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            hashmd5.Clear();
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToMD5(this string source)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));

            var sBuilder = new StringBuilder();

            for (var i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        public static string EncriptTracking(this string input, string key = "")
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(input + key);
            var base64_1 = Convert.ToBase64String(plainTextBytes);
            var reverseText = Reverse(base64_1);
            var base64_2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(reverseText + key));
            return Reverse(base64_2);
        }

        public static string DecriptTracking(this string input, string key = "")
        {
            var base64EncodedBytes = System.Convert.FromBase64String(Reverse(input));
            var base64_2 = Encoding.UTF8.GetString(base64EncodedBytes);
            var base64_1Text = Reverse(base64_2.Replace(key, string.Empty));
            var result = System.Convert.FromBase64String(base64_1Text);
            return System.Text.Encoding.UTF8.GetString(result).Replace(key, string.Empty);
        }
        private static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
