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
			Array.Resize<byte>(ref passwordBytes, passwordBytes.Length + 32);
			Array.Copy(salt, 0, passwordBytes, passwordBytes.Length, 32);
			byte[] hash = shaM.ComputeHash(passwordBytes);
			return hash;
        }
	}
}
