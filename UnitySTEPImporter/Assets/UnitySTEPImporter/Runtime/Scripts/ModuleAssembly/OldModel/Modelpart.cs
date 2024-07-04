using System;
using System.Collections.Generic;

namespace VENTUS.StepImporter.ModuleAssembly.OldModel
{
	/// <summary>
	/// The Modelpart class represents a modelpart of a CAD model.
	/// </summary>
	public class Modelpart : Submodel
	{
		private Modelmesh modelmesh;

		/// <summary>
		/// The Modelmesh property represents the mesh and graphicinfo of an object.
		/// </summary>
		/// <value>The Modelmesh property gets/sets the value of the modelmesh field.</value>
		public Modelmesh Modelmesh
		{
			get
			{
				return modelmesh;
			}

			set
			{
				modelmesh = value;
			}
		}

		public Modelpart() : base()
		{
			modelmesh = new Modelmesh();
		}

		/// <summary>
		/// Returns a nicely formatted string of this Modelpart.
		/// </summary>
		/// <returns>Nicely formatted string.</returns>
		public override string ToString()
		{
			string str = base.ToString() +
							"\nModelmesh:\n" + modelmesh.ToString();
			return str;
		}

		/// <summary>
		/// Returns a serialized Modelpart object.
		/// </summary>
		/// <param name="modelpart">Modelpart object which should be serialized.</param>
		/// <returns>Byte array which represents a serialized Modelpart object.</returns>
		public static byte[] Serialize(Modelpart modelpart)
		{
			/* TODO :
			List<byte> data = new List<byte>();
			NetworkWriter networkWriter = new NetworkWriter();

			networkWriter.Write(modelpart.Id);
			networkWriter.Write(modelpart.Transformation);
			networkWriter.Write(modelpart.Name);

			data.AddRange(networkWriter.ToArray());
			data.AddRange(Modelmesh.Serialize(modelpart.Modelmesh));
			*/
			return new byte[0];
		}

		/// <summary>
		/// Returns a deserialized Modelpart object.
		/// </summary>
		/// <param name="data">Byte array which represents a serialized Modelpart object.</param>
		/// <returns>Deserialized Modelpart object.</returns>
		public static Modelpart Deserialize(byte[] data)
		{
			/* TODO :
			Modelpart modelpart = new Modelpart();
			NetworkReader networkReader = new NetworkReader(data);

			modelpart.Id = networkReader.ReadInt32();
			modelpart.Transformation = networkReader.ReadMatrix4x4();
			modelpart.Name = networkReader.ReadString();

			byte[] modelmeshInByte = new byte[data.Length - networkReader.Position];
			Array.Copy(data, networkReader.Position, modelmeshInByte, 0, modelmeshInByte.Length);
			modelpart.Modelmesh = Modelmesh.Deserialize(modelmeshInByte);
			*/
			return null;
		}
	}
}
