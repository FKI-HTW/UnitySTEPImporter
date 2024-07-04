using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.StepImporter.ModuleAssembly.OldModel
{
	/// <summary>
	/// The Modelmesh class contains information about the mesh and the visual appearance of an object.
	/// </summary>
	public class Modelmesh
	{
		private int lod;
		private UnityEngine.Mesh mesh;
		private Graphicinfo graphicinfo;
		private IntPtr cppModelPointer;

		/// <summary>
		/// The LoD property represents the meshes level of details.
		/// </summary>
		/// <value>The LoD property gets/sets the value of the lod field.</value>
		public int LoD
		{
			get
			{
				return lod;
			}

			set
			{
				lod = value;
			}
		}

		/// <summary>
		/// The Mesh property represents a mesh.
		/// </summary>
		/// <value>The Mesh property gets/sets the value of the mesh field.</value>
		public UnityEngine.Mesh Mesh
		{
			get
			{
				return mesh;
			}

			set
			{
				mesh = value;
			}
		}

		/// <summary>
		/// The Graphicinfo property represents the graphicinfo of an object.
		/// </summary>
		/// <value>The Graphicinfo property gets/sets the value of the graphicinfo field.</value>
		public Graphicinfo Graphicinfo
		{
			get
			{
				return graphicinfo;
			}

			set
			{
				graphicinfo = value;
			}
		}

		/// <summary>
		/// The CppModelPointer property represents the appropriate pointer of the current submodel in geometry kernel.
		/// This pointer is useful for calling features in geometry kernel.
		/// </summary>
		/// <value>The CppModelPointer property gets/sets the value of the cppModelPointer field.</value>
		public IntPtr CppModelPointer
		{
			get
			{
				return cppModelPointer;
			}

			set
			{
				cppModelPointer = value;
			}
		}

		public Modelmesh()
		{
			mesh = new UnityEngine.Mesh();
			graphicinfo = new Graphicinfo();
		}

		/// <summary>
		/// Returns a nicely formatted string of this Modelmesh.
		/// </summary>
		/// <returns>Nicely formatted string.</returns>
		public override string ToString()
		{
			string str = "Mesh: \n" +
							"LoD: " + LoD + "\n" +
							"Vertices: \n";

			for (int i = 0; i < Mesh.vertices.Length; ++i)
				str += i + ": " + Mesh.vertices[i] + "\n";

			str += "Triangles: \n";
			for (int i = 0; i < Mesh.triangles.Length; i += 3)
				str += i + ": " + Mesh.triangles[i] + ", " + Mesh.triangles[i + 1] + ", " + Mesh.triangles[i + 2] + "\n";
					
			str += "Graphicinfo:\n" + graphicinfo.ToString();

			return str;
		}

		/// <summary>
		/// Returns a serialized Modelmesh object.
		/// </summary>
		/// <param name="modelmesh">Modelmesh object which should be serialized.</param>
		/// <returns>Byte array which represents a serialized Modelmesh object.</returns>
		public static byte[] Serialize(Modelmesh modelmesh)
		{
			/* TODO :
			List<byte> data = new List<byte>();
			NetworkWriter networkWriter = new NetworkWriter();

			networkWriter.Write(modelmesh.LoD);

			networkWriter.Write(modelmesh.Mesh.name);

			networkWriter.Write(modelmesh.Mesh.vertices.Length);
			foreach (Vector3 vector in modelmesh.Mesh.vertices)
				networkWriter.Write(vector);

			data.AddRange(networkWriter.ToArray());

			networkWriter = new NetworkWriter();
			networkWriter.Write(modelmesh.Mesh.triangles.Length);
			foreach (int triangle in modelmesh.Mesh.triangles)
				networkWriter.Write(triangle);

			data.AddRange(networkWriter.ToArray());

			networkWriter = new NetworkWriter();
			networkWriter.Write(modelmesh.Mesh.uv.Length);
			foreach (Vector2 uv in modelmesh.Mesh.uv)
				networkWriter.Write(uv);

			data.AddRange(networkWriter.ToArray());

			data.AddRange(Graphicinfo.Serialize(modelmesh.Graphicinfo));
			*/
			return new byte[0];
		}

		/// <summary>
		/// Returns a deserialized Modelmesh object.
		/// </summary>
		/// <param name="data">Byte array which represents a serialized Modelmesh object.</param>
		/// <returns>Deserialized Modelmesh object.</returns>
		public static Modelmesh Deserialize(byte[] data)
		{
			/* TODO :
			Modelmesh modelmesh = new Modelmesh();
			NetworkReader networkReader = new NetworkReader(data);
			int countMax = 0;

			modelmesh.LoD = networkReader.ReadInt32();

			modelmesh.Mesh.name = networkReader.ReadString();

			List<Vector3> vertices = new List<Vector3>();
			countMax = networkReader.ReadInt32();
			for (int i = 0; i < countMax; ++i)
				vertices.Add(networkReader.ReadVector3());
			modelmesh.Mesh.vertices = vertices.ToArray();

			List<int> triangles = new List<int>();
			countMax = networkReader.ReadInt32();
			for (int i = 0; i < countMax; ++i)
				triangles.Add(networkReader.ReadInt32());
			modelmesh.Mesh.triangles = triangles.ToArray();

			List<Vector2> uv = new List<Vector2>();
			countMax = networkReader.ReadInt32();
			for (int i = 0; i < countMax; ++i)
				uv.Add(networkReader.ReadVector2());
			modelmesh.Mesh.uv = uv.ToArray();

			modelmesh.Mesh.RecalculateBounds();
			modelmesh.Mesh.RecalculateNormals();

			byte[] graphicinfoInByte = new byte[data.Length - networkReader.Position];
			Array.Copy(data, networkReader.Position, graphicinfoInByte, 0, graphicinfoInByte.Length);
			modelmesh.Graphicinfo = Graphicinfo.Deserialize(graphicinfoInByte);
			*/
			return null;
		}
	}
}
