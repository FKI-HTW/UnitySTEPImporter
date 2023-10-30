using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.DataIO
{ 
	/// <summary>
	/// The Graphicinfo class contains information about the visual appearance of an object.
	/// </summary>
	public class Graphicinfo
	{
		private Color color;
		private Texture2D texture;

		/// <summary>
		/// The Color property represents a RGBA color.
		/// </summary>
		/// <value>The Color property gets/sets the value of the color field.</value>
		public Color Color
		{
			get
			{
				return color;
			}

			set
			{
				color = value;
			}
		}

		/// <summary>
		/// The Texture property represents an Image.
		/// </summary>
		/// <value>The Texture property gets/sets the value of the texture field.</value>
		public Texture2D Texture
		{
			get
			{
				return texture;
			}

			set
			{
				texture = value;
			}
		}

		public Graphicinfo()
		{
			color = new Color();
			texture = new Texture2D(1, 1);
		}

		/// <summary>
		/// Returns a nicely formatted string of this Graphicinfo.
		/// </summary>
		/// <returns>Nicely formatted string.</returns>
		public override string ToString()
		{
			string str = "Color: " + color.ToString() +
							"\nTexture: " + texture.ToString();

			return str;
		}

		/// <summary>
		/// Returns a serialized Graphicinfo object.
		/// </summary>
		/// <param name="graphicinfo">Graphicinfo object which should be serialized.</param>
		/// <returns>Byte array which represents a serialized Graphicinfo object.</returns>
		public static byte[] Serialize(Graphicinfo graphicinfo)
		{
			/* TODO :
			List<byte> data = new List<byte>();
			NetworkWriter networkWriter = new NetworkWriter();

			networkWriter.Write(graphicinfo.Color);

			if (graphicinfo.Texture != null)
			{
				networkWriter.Write(true);
				networkWriter.Write(graphicinfo.Texture.name);

				data.AddRange(networkWriter.ToArray());
				data.AddRange((graphicinfo.Texture as Texture2D).EncodeToPNG());
			}
			else
			{
				networkWriter.Write(false);

				data.AddRange(networkWriter.ToArray());
			}
			*/
			return new byte[0];
		}

		/// <summary>
		/// Returns a deserialized Graphicinfo object.
		/// </summary>
		/// <param name="data">Byte array which represents a serialized Graphicinfo object.</param>
		/// <returns>Deserialized Graphicinfo object.</returns>
		public static Graphicinfo Deserialize(byte[] data)
		{
			/* TODO :
			Graphicinfo graphicinfo = new Graphicinfo();
			NetworkReader networkReader = new NetworkReader(data);

			graphicinfo.Color = networkReader.ReadColor();

			if (networkReader.ReadBoolean())
			{
				Texture2D texture2D = new Texture2D(1, 1);
				texture2D.name = networkReader.ReadString();

				byte[] textureInByte = new byte[data.Length - networkReader.Position];
				Array.Copy(data, networkReader.Position, textureInByte, 0, textureInByte.Length);
				texture2D.LoadImage(textureInByte);

				graphicinfo.Texture = texture2D;
			}
			else
				graphicinfo.Texture = null;
			*/
			return null;
		}
	}
}
