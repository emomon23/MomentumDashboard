using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace Test.IEmosoft.com.BusinessLogic
{


    /// <summary>
    /// AES is a symmetric 256-bit encryption algorthm.
    /// Read more: http://en.wikipedia.org/wiki/Advanced_Encryption_Standard
    /// </summary>
    public static class AES
    {      
        private static string _password = "SomePassword";
        private static byte[] _saltBytes;
        private static byte[] _initVectorBytes;

        static AES()
        {
            SaltVector saltVector = new SaltVector();

            _saltBytes = Encoding.UTF8.GetBytes(saltVector.Salt);
            _initVectorBytes = Encoding.UTF8.GetBytes(saltVector.InitVector);
        }


        /// <summary>
        /// Encrypts a string with AES
        /// </summary>
        /// <param name="plainText">Text to be encrypted</param>
        /// <param name="password">Password to encrypt with</param>   
        /// <param name="salt">Salt to encrypt with</param>    
        /// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
        /// <returns>An encrypted string</returns>        
        public static string Encrypt(string plainText, string salt = null, string initialVector = null)
        {
            return Convert.ToBase64String(EncryptToBytes(plainText,  salt, initialVector));
        }

        /// <summary>
        /// Encrypts a string with AES
        /// </summary>
        /// <param name="plainText">Text to be encrypted</param>
        /// <param name="password">Password to encrypt with</param>   
        /// <param name="salt">Salt to encrypt with</param>    
        /// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
        /// <returns>An encrypted string</returns>        
        public static byte[] EncryptToBytes(string plainText, string salt = null, string initialVector = null)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return EncryptToBytes(plainTextBytes, salt, initialVector);
        }

        /// <summary>
        /// Encrypts a string with AES
        /// </summary>
        /// <param name="plainTextBytes">Bytes to be encrypted</param>
        /// <param name="password">Password to encrypt with</param>   
        /// <param name="salt">Salt to encrypt with</param>    
        /// <param name="initialVector">Needs to be 16 ASCII characters long</param>    
        /// <returns>An encrypted string</returns>        
        public static byte[] EncryptToBytes(byte[] plainTextBytes,  string salt = null, string initialVector = null)
        {
            int keySize = 256;

            byte[] initialVectorBytes = string.IsNullOrEmpty(initialVector) ? _initVectorBytes : Encoding.UTF8.GetBytes(initialVector);
            byte[] saltValueBytes = string.IsNullOrEmpty(salt) ? _saltBytes : Encoding.UTF8.GetBytes(salt);
            byte[] keyBytes = new Rfc2898DeriveBytes(_password, saltValueBytes).GetBytes(keySize / 8);

            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;

                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            return memStream.ToArray();
                        }
                    }
                }
            }
        }

        /// <summary>  
        /// Decrypts an AES-encrypted string. 
        /// </summary>  
        /// <param name="cipherText">Text to be decrypted</param> 
        /// <param name="password">Password to decrypt with</param> 
        /// <param name="salt">Salt to decrypt with</param> 
        /// <param name="initialVector">Needs to be 16 ASCII characters long</param> 
        /// <returns>A decrypted string</returns>
        public static string Decrypt(string cipherText, string salt = null, string initialVector = null)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText.Replace(' ', '+'));
            return Decrypt(cipherTextBytes,  salt, initialVector).TrimEnd('\0');
        }

        /// <summary>  
        /// Decrypts an AES-encrypted string. 
        /// </summary>  
        /// <param name="cipherText">Text to be decrypted</param> 
        /// <param name="password">Password to decrypt with</param> 
        /// <param name="salt">Salt to decrypt with</param> 
        /// <param name="initialVector">Needs to be 16 ASCII characters long</param> 
        /// <returns>A decrypted string</returns>
        public static string Decrypt(byte[] cipherTextBytes,string salt = null, string initialVector = null)
        {
            int keySize = 256;

            byte[] initialVectorBytes = string.IsNullOrEmpty(initialVector) ? _initVectorBytes : Encoding.UTF8.GetBytes(initialVector);
            byte[] saltValueBytes = string.IsNullOrEmpty(salt) ? _saltBytes : Encoding.UTF8.GetBytes(salt);
            byte[] keyBytes = new Rfc2898DeriveBytes(_password, saltValueBytes).GetBytes(keySize / 8);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;

                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
                {
                    using (MemoryStream memStream = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                        {
                            int byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
                        }
                    }
                }
            }
        }
    }

    public class SaltVector
    {
        private string saltFilePath = "";
        private string[] fileContents = new string[0];
        private Random random = new Random(DateTime.Now.Millisecond * DateTime.Now.Millisecond);

        public SaltVector()
        {
            saltFilePath = string.Format("{0}MomentumDataFiles", Path.GetPathRoot(Directory.GetCurrentDirectory()));
            if (!Directory.Exists(saltFilePath))
            {
                Directory.CreateDirectory(saltFilePath);
            }

            saltFilePath += "\\MomentumSaltVector.dat";

            if (!ReadFileContents())
            {
                CreateNewSaltValues();
                ReadFileContents();
            }
        }

        public string Salt
        {
            get
            {
                return fileContents.Length > 0 ? fileContents[0] : null;
            }
        }

        public string InitVector
        {
            get
            {
                return fileContents.Length > 1 ? fileContents[1] : null;
            }
        }
        private void CreateNewSaltValues()
        {
            string rawFileContents = string.Format("{0}{2}{1}{2}", GenerateNewSalt(), GenerateNewInitVector(), Environment.NewLine);
            File.WriteAllText(saltFilePath, rawFileContents);
        }

        private bool ReadFileContents()
        {
           
            if (File.Exists(saltFilePath))
            {
                fileContents = File.ReadAllLines(saltFilePath);

                var backupFile = saltFilePath.Replace(".dat", ".backup");
                if (!File.Exists(backupFile) && fileContents.Length > 0)
                {
                    File.WriteAllLines(backupFile, fileContents);
                }
            }

            return fileContents.Length > 0;
        }

        private string GenerateNewSalt()
        {
            string result = GetRandomString(1);
            result += GetRandomNumber(2);
            result += GetRandomString(2);
            result += GetRandomString(1, true);
            result += GetRandomNumber(2);

            return result;
        }

        private string GetRandomString(int length, bool returnUpperCase = false)
        {
            StringBuilder builder = new StringBuilder();
           
            char ch;
            for (int i = 0; i < length; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return returnUpperCase? builder.ToString().ToUpper() : builder.ToString().ToLower();
        }

        private string GetRandomNumber(int numberOfDigits)
        {
            string minValueString = "1";
            string maxValueString = "9";

            for (int i = 0; i < numberOfDigits - 1; i++)
            {
                minValueString += "0";
                maxValueString += "0";
            }

            return random.Next(int.Parse(minValueString), int.Parse(maxValueString)).ToString();
        }

        private string GenerateNewInitVector()
        {
            var result = GetRandomString(3, true);
            result += GetRandomString(2);
            result += GetRandomNumber(2);
            result += GetRandomString(1) + "*";
            result += GetRandomString(3);
            result += GetRandomNumber(2);
            result += GetRandomString(1);
            result += GetRandomString(1, true);

            return result;
        }

    }
}