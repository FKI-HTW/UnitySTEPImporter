using System.IO;
using System.Security.Cryptography;

namespace VENTUS.UnitySTEPImporter.DataIO
{
	/// <summary>
	/// the Cryptograph class provides funtion for encrypting (AES) and decrypting (AES) a byte array.
	/// </summary>
	public class Cryptograph
	{
		private const int Iteration = 300;

		/// <summary>
		/// Returns a encrypted version of the input byte array.
		/// </summary>
		/// <param name="bytesToEncrypt">Byte array which should be encrypted.</param>
		/// <param name="salt">Salt which should be used to generate the key.</param>
		/// <param name="keySize">Size of the key to be generated.</param>
		/// <param name="blockSize">Size of a block.</param>
		/// <param name="password">Password which should be used to generate the key.</param>
		/// <returns>Encrypted version of the input byte array</returns>
		public static byte[] Encrypt(byte[] bytesToEncrypt, byte[] salt, int keySize, int blockSize, string password)
		{
			byte[] encryptedBytes;

			using (AesManaged aes = new AesManaged())
			{
				aes.KeySize = keySize;
				aes.BlockSize = blockSize;

				aes.Key = GenerateKey(password, salt, aes.KeySize / 8);
				aes.IV = GenerateKey(password, salt, aes.BlockSize / 8);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
						cryptoStream.Close();
					}

					encryptedBytes = memoryStream.ToArray();
				}
			}

			return encryptedBytes;
		}

		/// <summary>
		/// Returns a decrypted version of the input byte array.
		/// </summary>
		/// <param name="bytesToDecrypt">Byte array which should be decrypted.</param>
		/// <param name="salt">Salt which was used to generate the encryption key.</param>
		/// <param name="keySize">Size of the encryption key.</param>
		/// <param name="blockSize">Size of a block which was used for the encryption.</param>
		/// <param name="password">Password which was used to generate the encryption key.</param>
		/// <returns>Decrypted version of the input byte array.</returns>
		public static byte[] Decrypt(byte[] bytesToDecrypt, byte[] salt, int keySize, int blockSize, string password)
		{
			byte[] decryptedBytes;

			using (AesManaged aes = new AesManaged())
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					aes.KeySize = keySize;
					aes.BlockSize = blockSize;

					aes.Key = GenerateKey(password, salt, aes.KeySize / 8);
					aes.IV = GenerateKey(password, salt, aes.BlockSize / 8);

					using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cryptoStream.Write(bytesToDecrypt, 0, bytesToDecrypt.Length);
						cryptoStream.Close();
					}

					decryptedBytes = memoryStream.ToArray();
				}
			}

			return decryptedBytes;
		}

		private static byte[] GenerateKey(string password, byte[] salt, int keyBytes)
		{
			Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(password, salt, Iteration);
			return keyGenerator.GetBytes(keyBytes);
		}
	}
}
