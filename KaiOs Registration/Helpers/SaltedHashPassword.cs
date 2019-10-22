using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
namespace KaiOs_Registration.Helpers
{
    public class SaltedHashPassword
    {
        private Byte[] hash;
        private Byte[] salt;
        public Byte[] Hash { get { return hash; } }
        public Byte[] Salt { get { return salt; } }

        /// <summary>
        /// Default size of the hash, in bytes.
        /// </summary>
        public static UInt32 HashSizeBytes { get { return 20; } }

        /// <summary>
        /// Default size of the salt, in bytes.
        /// </summary>
        public static UInt32 SaltSizeBytes { get { return 8; } }

        /// <summary>
        /// Computes the hash for a password given a salt.
        /// </summary>
        /// <param name="salt">The salt to use in the hashing.</param>
        /// <param name="password">The password to hash.</param>
        /// <returns>A byte array storing the computed hash.</returns>
        private static Byte[] ComputeSaltedHash(Byte[] salt, String password)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("password cannot be empty");

            string saltAsString = Encoding.UTF8.GetString(salt);
            string combinedPasswordSalt = password + saltAsString;

            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] hash = encoder.GetBytes(combinedPasswordSalt);

            SHA512 sha512 = SHA512.Create();
            Byte[] computedHash = sha512.ComputeHash(hash);

            return computedHash;
        }

        private static Byte[] ComputeSaltedHash(string salt, String password)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] saltBytes = encoder.GetBytes(salt);
            return ComputeSaltedHash(saltBytes, password);
        }


        /// <summary>
        /// Tests if the passed hash is valid. Currently, a valid hash only has to match
        /// in size.
        /// </summary>
        /// <param name="hash">The hash to validate.</param>
        /// <returns>True if valid, False if not.</returns>
        private Boolean IsValidHash(Byte[] hash)
        {
            Boolean ret = true;
            if (hash.Length != HashSizeBytes)
            {
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// Tests if the passed salt is valid. Currently, a valid salt only has to match
        /// in size.
        /// </summary>
        /// <param name="salt">The salt to validate.</param>
        /// <returns>True if valid, False if not.</returns>
        private Boolean IsValidSalt(Byte[] salt)
        {
            Boolean ret = true;
            if (salt.Length != SaltSizeBytes)
            {
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// The function creates a random salt byte array of the defined default size.
        /// </summary>
        /// <returns>The random salt byte array.</returns>
        private Byte[] CreateRandomSalt()
        {
            return CreateRandomBytes(Convert.ToInt32(SaltSizeBytes));
        }

        private byte[] CreateRandomBytes(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            Debug.Write(result);
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetBytes(result);
        }


        /// <summary>
        /// Function for creating an array of random bytes of the specified length.
        /// </summary>
        /// <param name="length">The size of the array to create.</param>
        /// <returns>The array of random bytes.</returns>
        /*private Byte[] CreateRandomBytes(UInt32 length)
        {
            Byte[] randomBytes = new Byte[length];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            return randomBytes;
        }*/


        /// <summary>
        /// Returns true if the passed password is a match for this password. Only works for SaltedHashPasswords.
        /// </summary>
        /// <param name="password">The password to compare.</param>
        /// <returns>True if a match.</returns>
        public Boolean Matches(SaltedHashPassword password)
        {
            Boolean ret = false;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            ret = ArraysEqual<Byte>(this.Hash, password.Hash);
            return ret;
        }


        /// <summary>
        /// Compares if the two arrays contents are equals.
        /// </summary>
        /// <typeparam name="T">The type of the two arrays.</typeparam>
        /// <param name="array1">The first array.</param>
        /// <param name="array2">The second array.</param>
        /// <returns>True on success, false on fail</returns>
        public static Boolean ArraysEqual<T>(T[] array1, T[] array2)
        {
            bool equal = true;
            if (array1.Length != array2.Length)
            {
                equal = false;
            }
            else
            {
                for (Int32 index = 0; index < array1.Length; index++)
                {
                    if (array1[index].Equals(array2[index]) == false)
                    {
                        equal = false;
                        break;
                    }
                }
            }
            return equal;
        }


        /// <summary>
        /// Constructs a password with the specified salt and hash. No computation is done.
        /// This allows previously computed hashes and salts to be used in the construction of
        /// SaltedHashPassword objects for comparison/validation purposes.
        /// </summary>
        /// <param name="salt">The salt of the password.</param>
        /// <param name="hash">The hash of the password.</param>
        public SaltedHashPassword(Byte[] salt, Byte[] hash)
        {
            this.salt = new Byte[salt.Length];
            this.hash = new Byte[hash.Length];
            Array.Copy(salt, this.salt, salt.Length);
            Array.Copy(hash, this.hash, hash.Length);
        }


        /// <summary>
        /// Constructs a SaltedHashPassword object from the passed string password.
        /// The Salt is randomly generated and the password hashed. The string password is
        /// never stored internally, only in encrypted form.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        public SaltedHashPassword(String password)
        {
            this.salt = CreateRandomSalt();
            this.hash = ComputeSaltedHash(this.salt, password);
        }


        /// <summary>
        /// Constructs a SaltedHashPassword object from the passed string password
        /// and the specified Salt.The string password is never stored internally, only in encrypted form.
        /// </summary>
        /// <param name="salt">The salt of the password.</param>
        /// <param name="password">The password to hash.</param>
        public SaltedHashPassword(Byte[] salt, String password)
        {
            IsValidSalt(salt);
            this.salt = new Byte[salt.Length];
            Array.Copy(salt, this.salt, salt.Length);
            this.hash = ComputeSaltedHash(this.salt, password);
        }

        public SaltedHashPassword(String salt, String password)
        {
            this.salt = JsonConvert.DeserializeObject<Byte[]>(JsonConvert.SerializeObject(salt));
            this.hash = ComputeSaltedHash(salt, password);
        }
    }
}