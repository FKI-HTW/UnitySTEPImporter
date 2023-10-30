using System.IO;
using System.IO.Compression;

namespace VENTUS.UnitySTEPImporter.DataIO
{
	/// <summary>
	/// The Compressor Class provides functions for compressing and decompressing byte arrays.
	/// </summary>
	public class Compressor
	{
		/// <summary>
		/// Returns a compressed version of the input byte array.
		/// </summary>
		/// <param name="uncompressedBytes">Byte array which should be compressed.</param>
		/// <returns>Compressed version of the input byte array.</returns>
		public static byte[] Compress(byte[] uncompressedBytes)
		{
			byte[] compressedBytes = null;

			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gzipStream.Write(uncompressedBytes, 0, uncompressedBytes.Length);
					gzipStream.Close();
					compressedBytes = memoryStream.ToArray();
				}
			}

			return compressedBytes;
		}

		/// <summary>
		/// Returns a decompressed version of the input byte array.
		/// </summary>
		/// <param name="compressedBytes">Byte array which should be decompressed.</param>
		/// <returns>Decompressed version of the input byte array.</returns>
		public static byte[] Decompress(byte[] compressedBytes)
		{
			byte[] uncompressedBytes = null;

			using (GZipStream gzipStream = new GZipStream(new MemoryStream(compressedBytes), CompressionMode.Decompress))
			{

				byte[] buffer = new byte[8192]; // TODO : App.Settings.BUFFER_SIZE

				using (MemoryStream memoryStream = new MemoryStream())
				{
					int count = 0;

					while ((count = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
						memoryStream.Write(buffer, 0, count);

					uncompressedBytes = memoryStream.ToArray();
				}
			}

			return uncompressedBytes;
		}
	}
}
