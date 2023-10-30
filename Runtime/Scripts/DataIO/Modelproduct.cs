using System;
using System.Collections.Generic;

namespace VENTUS.UnitySTEPImporter.DataIO
{
	/// <summary>
	/// The Modelproduct class represents a modelproduct of a CAD model.
	/// </summary>
	public class Modelproduct : Submodel
	{
		private List<Modelpart> modelparts;
		private List<Modelproduct> modelproducts;

		/// <summary>
		/// The Modelparts property represents a list of modelparts.
		/// </summary>
		/// <value>The Modelparts property gets/sets the value of the modelparts field.</value>
		public List<Modelpart> Modelparts
		{
			get
			{
				return modelparts;
			}

			set
			{
				modelparts = value;
			}
		}

		/// <summary>
		/// The Modelproducts property represents a list of modelproducts.
		/// </summary>
		/// <value>The Modelproducts property gets/sets the value of the modelproducts field.</value>
		public List<Modelproduct> Modelproducts
		{
			get
			{
				return modelproducts;
			}

			set
			{
				modelproducts = value;
			}
		}

		public Modelproduct() : base()
		{
			modelparts = new List<Modelpart>();
			modelproducts = new List<Modelproduct>();
		}

		/// <summary>
		/// Returns a nicely formatted string of this Modelproduct.
		/// </summary>
		/// <returns>Nicely formatted string.</returns>
		public override string ToString()
		{
			string str = base.ToString();

			for (int i = 0; i < modelparts.Count; ++i)
				str += "\n\nModelpart " + i + ":\n" + Modelparts[i].ToString();


			for (int i = 0; i < modelproducts.Count; ++i)
				str += "\n\nModelproduct " + i + ":\n" + Modelproducts[i].ToString();

			return str;
		}

		/// <summary>
		/// Returns a serialized Modelproduct object.
		/// </summary>
		/// <param name="modelproduct">Modelproduct object which should be serialized.</param>
		/// <returns>Byte array which represents a serialized Modelproduct object.</returns>
		public static byte[] Serialize(Modelproduct modelproduct)
		{
			/* TODO :
			List<byte> data = new List<byte>();
			NetworkWriter networkWriter = new NetworkWriter();
			byte[] lengthInByte;
			byte[] payloadInByte;

			networkWriter.Write(modelproduct.Id);
			networkWriter.Write(modelproduct.Transformation);
			networkWriter.Write(modelproduct.Name);

			data.AddRange(networkWriter.ToArray());

			lengthInByte = BitConverter.GetBytes(modelproduct.Modelparts.Count);
			data.AddRange(lengthInByte);
			foreach (Modelpart modelpart in modelproduct.Modelparts)
			{
				payloadInByte = Modelpart.Serialize(modelpart);
				lengthInByte = BitConverter.GetBytes(payloadInByte.Length);
				data.AddRange(lengthInByte);
				data.AddRange(payloadInByte);
			}

			lengthInByte = BitConverter.GetBytes(modelproduct.Modelproducts.Count);
			data.AddRange(lengthInByte);
			foreach (Modelproduct modelP in modelproduct.Modelproducts)
			{
				payloadInByte = Modelproduct.Serialize(modelP);
				lengthInByte = BitConverter.GetBytes(payloadInByte.Length);
				data.AddRange(lengthInByte);
				data.AddRange(payloadInByte);
			}
			*/
			return new byte[0];
		}

		/// <summary>
		/// Returns a deserialized Modelproduct object.
		/// </summary>
		/// <param name="data">Byte array which represents a serialized Modelproduct object.</param>
		/// <returns>Deserialized Modelproduct object.</returns>
		public static Modelproduct Deserialize(byte[] data)
		{
			/* TODO :
			Modelproduct modelproduct = new Modelproduct();
			NetworkReader networkReader = new NetworkReader(data);
			int countMax = 0;
			int readerPosition = 0;
			int length = 0;

			modelproduct.Id = networkReader.ReadInt32();
			modelproduct.Transformation = networkReader.ReadMatrix4x4();
			modelproduct.Name = networkReader.ReadString();

			readerPosition = Convert.ToInt32(networkReader.Position);

			countMax = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
			for (int i = 0; i < countMax; ++i)
			{
				length = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
				modelproduct.Modelparts.Add(Modelpart.Deserialize(GetPartOfByteArray(data, length, ref readerPosition)));
			}

			countMax = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
			for (int i = 0; i < countMax; ++i)
			{
				length = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
				modelproduct.Modelproducts.Add(Modelproduct.Deserialize(GetPartOfByteArray(data, length, ref readerPosition)));
			}
			*/
			return null;
		}

		/// <summary>
		/// Returns a part of a source byte array.
		/// </summary>
		/// <param name="src">Scource byte array.</param>
		/// <param name="length">Length of the part.</param>
		/// <param name="offset">Point where to start the part in the source array.</param>
		/// <returns>Part of the source byte array.</returns>
		private static byte[] GetPartOfByteArray(byte[] src, int length, ref int offset)
		{
			byte[] copy = new byte[length];

			Array.Copy(src, offset, copy, 0, copy.Length);
			offset += length;

			return copy;
		}
	}
}
