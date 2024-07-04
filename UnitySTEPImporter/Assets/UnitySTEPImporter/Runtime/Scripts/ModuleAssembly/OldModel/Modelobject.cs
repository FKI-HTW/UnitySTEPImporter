using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.StepImporter.ModuleAssembly.OldModel
{
	/// <summary>
	/// The Modelobject class represents a modelobject of a CAD model.
	/// </summary>
	public class Modelobject
	{	
		/// <summary>
			/// The Id property represents the id of that object.
			/// </summary>
			/// <value>The Id property gets/sets the value of the id field.</value>
		public int Id { get; set; }

		/// <summary>
		/// The ObjectManagerID property represents the objectManagerID used by the GeomKernel of that object.
		/// </summary>
		/// <value>The ObjectManagerID property gets/sets the value of the objectManagerID field.</value>
		public int ObjectManagerId { get; set; }

		/// <summary>
		/// The Transformation property represents the transformation matrix of that object.
		/// </summary>
		/// <value>The Transformation property gets/sets the value of the transformation field.</value>
		public Matrix4x4 Transformation { get; set; }

		/// <summary>
		/// The Name property represents the name of that object.
		/// </summary>
		/// <value>The Name property gets/sets the value of the name field.</value>
		public string Name { get; set; }

		/// <summary>
		/// The Path property represents the path where the origin file lies.
		/// </summary>
		/// <value>The Path property gets/sets the value of the path field.</value>
		public string Path { get; set; }

		/// <summary>
		/// The Submodels property represents a list of submodels.
		/// </summary>
		/// <value>The Submodels property gets/sets the value of the submodels field.</value>
		public List<Submodel> Submodels { get; set; }

		/// <summary>
		/// The Annotations property represents a list of test annotations.
		/// </summary>
		/// <value>The Annotations property gets/sets the value of the annotations field.</value>
		public List<string> Annotations { get; set; }

		/// <summary>
		/// The Bounds property represents the axis aligned bounding box of modelobject.
		/// </summary>
		/// <value>The Bounds property gets/sets the value of the bounds field.</value>
		public Bounds Bounds { get; set; }

		/// <summary>
		/// The CppModelPointer property represents the appropriate pointer of the current modelobject in geometry kernel.
		/// This pointer is useful for calling features in geometry kernel.
		/// </summary>
		/// <value>The CppModelPointer property gets/sets the value of the cppModelPointer field.</value>
		public IntPtr CppModelPointer { get; set; }
			
		public Modelobject()
		{
			Id = 0;
			ObjectManagerId = 0;
			Transformation = new Matrix4x4();
			Name = "";
			Path = "";
			Annotations = new List<string>();
			Submodels = new List<Submodel>();
		}

		/// <summary>
		/// Returns a nicely formatted string of this Modelobject.
		/// </summary>
		/// <returns>Nicely formatted string.</returns>
		//public override string ToString()
		//{
		//	string str = "Name: " + Name.ToString() +
		//		"\nId: " + Id.ToString() +
		//		"\nTransformation:\n" + Transformation.ToString().TrimEnd('\r', '\n');
		//
		//	for (int i = 0; i < Submodels.Count; ++i)
		//	{
		//		if (Submodels[i].GetType() == typeof(Modelpart))
		//			str += "\n\nSubmodel " + i + ":\n" + ((Modelpart)Submodels[i]).ToString();
		//		else if (Submodels[i].GetType() == typeof(Modelproduct))
		//			str += "\n\nSubmodel " + i + ":\n" + ((Modelproduct)Submodels[i]).ToString();
		//	}
		//
		//	return str;
		//}

		/// <summary>
		/// Returns a serialized Modelobject object.
		/// </summary>
		/// <param name="modelobject">Modelobject object which should be serialized.</param>
		/// <returns>Byte array which represents a serialized Modelobject object.</returns>
		public static byte[] Serialize(Modelobject modelobject)
		{
			/* TODO :
			List<byte> data = new List<byte>();
			NetworkWriter networkWriter = new NetworkWriter();
			byte[] lengthInByte;
			byte[] payloadInByte;

			networkWriter.Write(modelobject.Id);
			networkWriter.Write(modelobject.Transformation);
			networkWriter.Write(modelobject.Name);

			data.AddRange(networkWriter.ToArray());

			lengthInByte = BitConverter.GetBytes(modelobject.Submodels.Count);
			data.AddRange(lengthInByte);
			foreach (Submodel submodel in modelobject.Submodels)
			{
				if (submodel.GetType() == typeof(Modelpart))
				{
					payloadInByte = Modelpart.Serialize((Modelpart)submodel);
					lengthInByte = BitConverter.GetBytes(payloadInByte.Length);
					data.AddRange(BitConverter.GetBytes(0));
					data.AddRange(lengthInByte);
					data.AddRange(payloadInByte);
				}
				else if (submodel.GetType() == typeof(Modelproduct))
				{
					payloadInByte = Modelproduct.Serialize((Modelproduct)submodel);
					lengthInByte = BitConverter.GetBytes(payloadInByte.Length);
					data.AddRange(BitConverter.GetBytes(1));
					data.AddRange(lengthInByte);
					data.AddRange(payloadInByte);
				}
			}
			*/
			return new byte[0];
		}

		/// <summary>
		/// Returns a deserialized Modelobject object.
		/// </summary>
		/// <param name="data">Byte array which represents a serialized Modelobject object.</param>
		/// <returns>Deserialized Modelobject object.</returns>
		public static Modelobject Deserialize(byte[] data)
		{
			/* TODO :
			Modelobject modelobject = new Modelobject();
			NetworkReader networkReader = new NetworkReader(data);
			int countMax = 0;
			int readerPosition = 0;
			int length = 0;
			int submodelType = 0;

			modelobject.Id = networkReader.ReadInt32();
			modelobject.Transformation = networkReader.ReadMatrix4x4();
			modelobject.Name = networkReader.ReadString();

			readerPosition = Convert.ToInt32(networkReader.Position);

			countMax = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
			for (int i = 0; i < countMax; ++i)
			{
				submodelType = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
				if (submodelType == 0)
				{
					length = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
					modelobject.Submodels.Add(Modelpart.Deserialize(GetPartOfByteArray(data, length, ref readerPosition)));
				}
				else if (submodelType == 1)
				{
					length = BitConverter.ToInt32(GetPartOfByteArray(data, 4, ref readerPosition), 0);
					modelobject.Submodels.Add(Modelproduct.Deserialize(GetPartOfByteArray(data, length, ref readerPosition)));
				}
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
