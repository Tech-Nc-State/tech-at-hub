using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace Tech_HubAPI.Services
{
    public class HashingService
    {
        private RNGCryptoServiceProvider rngP = new RNGCryptoServiceProvider();
        private SHA512 shaM = new SHA512Managed();

        /// <summary>
        /// Get a randomized 32 byte salt
        /// </summary>
        /// <returns>The salt byte array</returns>
        public byte[] GetSalt()
        {
            byte[] salt = new byte[32];
            rngP.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// Get a hash from a plain text password and a byte[] salt.
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="salt">the salt</param>
        /// <returns>The hash</returns>
        public byte[] HashPassword(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

            int passwordLength = passwordBytes.Length;
            Array.Resize<byte>(ref passwordBytes, passwordLength + 32);
            Array.Copy(salt, 0, passwordBytes, passwordLength, 32);

            byte[] hash = shaM.ComputeHash(passwordBytes);
            return hash;
        }

        /// <summary>
        /// Compares two byte arrays for equality
        /// </summary>
        /// <param name="b1">First byte array</param>
        /// <param name="b2">Second byte array</param>
        /// <returns>Whether the two arrays are equal</returns>
        public bool ByteCheck(byte[] b1, byte[] b2)
        {
            if (b1.Length != b2.Length)
            {
                return false;
            }

            for (int i = 0; i < b1.Length; i++)
            {
                if (!b1[i].Equals(b2[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
