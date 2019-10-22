using System;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.IO;
namespace KaiOs_Registration.Helpers
{
    public static class Encryption
    {
        //New Token generation to include userid or voucher code
        // Token generation with string value (for voucher code)

        public static string GetTodaysEncryptedToken()
        {
            return GetTodaysEncryptedToken("");
        }

        public static string GetTodaysEncryptedToken(int value)
        {
            return GetTodaysEncryptedToken(value.ToString());
        }

        public static string GetTodaysEncryptedToken(string value)
        {
            string token = GetToken(value);
            string desEncryptionKey = ConfigurationManager.AppSettings["DES_EncryptionKey"];
            string desIv = ConfigurationManager.AppSettings["DES_IV"];
            return EncryptTodaysToken(token, desEncryptionKey, desIv);
        }

        public static string GetTodaysEncryptedToken(string value, string secretKey)
        {
            string desEncryptionKey = ConfigurationManager.AppSettings["DES_EncryptionKey"];
            string desIv = ConfigurationManager.AppSettings["DES_IV"];
            return GetTodaysEncryptedToken(value, secretKey, desEncryptionKey, desIv);
        }

        public static string GetTodaysEncryptedToken(string value, string secretKey, string desEncryptionKey, string desIv)
        {
            string token = GetToken(value, secretKey);
            return EncryptTodaysToken(token, desEncryptionKey, desIv);
        }

        public static string GetTodaysEncryptedBusinessToken(string value, string secretKey, string desEncryptionKey, string desIv)
        {
            string token = GetBusinessPartnerToken(value, secretKey);
            return EncryptTodaysToken(token, desEncryptionKey, desIv);
        }
        public static string GetContactsEncryptedToken(int userID, int deviceID, string contacts)
        {
            string desEncryptionKey = ConfigurationManager.AppSettings["DES_EncryptionKey"];
            string userDescEncryptionKey = string.Format("{0}-{1}-{2}", desEncryptionKey, deviceID.ToString(), userID.ToString());
            return EncryptMD5(contacts, userDescEncryptionKey);
            //return EncryptContactsToken(contacts, userDescEncryptionKey, desIv);
        }
        public static string GetDecryptedContacts(int userID, int deviceID, string contacts)
        {
            string desEncryptionKey = ConfigurationManager.AppSettings["DES_EncryptionKey"];
            string userDescEncryptionKey = string.Format("{0}-{1}-{2}", desEncryptionKey, deviceID.ToString(), userID.ToString());
            return DecryptMD5(contacts, userDescEncryptionKey);
        }
        private static string EncryptTodaysToken(string token, string desEncryptionKey, string desIv)
        {
            var tripleDes = new TripleDESImplementation(desEncryptionKey, desIv);
            return HttpUtility.UrlEncode(tripleDes.Encrypt(token));
        }
        public static string GetToken()
        {
            return GetToken("");
        }

        public static string GetToken(int value)
        {
            return GetToken(value.ToString());
        }

        public static string GetToken(string value)
        {
            string secretKey = ConfigurationManager.AppSettings["SecretKey"];
            return GetToken(value, secretKey);
        }

        public static string GetToken(string value, string secretKey)
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalDays;
            StringBuilder sb = new StringBuilder();
            sb.Append(unixTimestamp.ToString());
            sb.Append(":");
            sb.Append(secretKey);
            if (!String.IsNullOrEmpty(value))
            {
                sb.Append(":");
                sb.Append(value);
            }
            return sb.ToString();
        }
        public static string GetBusinessPartnerToken(string value, string secretKey)
        {
            DateTime dt = DateTime.UtcNow;
            string formattedDT = dt.ToString("ddMMyy");
            StringBuilder sb = new StringBuilder();
            sb.Append(secretKey);
            sb.Append("-");
            sb.Append(value);
            sb.Append("-");
            sb.Append(formattedDT);

            return sb.ToString();
        }
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, 1000))
            {
                var keyBytes = password.GetBytes(256 / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }


        public static string EncryptMD5(string toEncrypt, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);




            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string DecryptMD5(string cipherString, string key)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}