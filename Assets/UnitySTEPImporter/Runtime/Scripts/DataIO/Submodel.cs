using System;
using System.Collections.Generic;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.DataIO
{
	/// <summary>
	/// The Submodel class represents a submodel of a modelobject.
	/// </summary>
	public class Submodel
	{
		private int id;
		private int objectManagerId;
		private Matrix4x4 transformation;
		private string name;
		private Bounds bounds; // axis aligned bounding box  
		private IntPtr cppModelPointer;
		private List<string> annotations;

		/// <summary>
		/// The Id property represents the id of that object.
		/// </summary>
		/// <value>The Id property gets/sets the value of the id field.</value>
		public int Id
		{
			get
			{
				return id;
			}

			set
			{
				id = value;
			}
		}

		/// <summary>
		/// The ObjectManagerID property represents the objectManagerID used by the GeomKernel of that object.
		/// </summary>
		/// <value>The ObjectManagerID property gets/sets the value of the objectManagerID field.</value>
		public int ObjectManagerId
		{
			get
			{
				return objectManagerId;
			}

			set
			{
				objectManagerId = value;
			}
		}

		/// <summary>
		/// The Transformation property represents the transformation matrix of that object.
		/// </summary>
		/// <value>The Transformation property gets/sets the value of the transformation field.</value>
		public Matrix4x4 Transformation
		{
			get
			{
				return transformation;
			}

			set
			{
				transformation = value;
			}
		}

		/// <summary>
		/// The Name property represents the name of that object.
		/// </summary>
		/// <value>The Name property gets/sets the value of the name field.</value>
		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		/// <summary>
		/// The Bounds property represents the axis aligned bounding box of modelobject.
		/// </summary>
		/// <value>The Bounds property gets/sets the value of the bounds field.</value>
		public Bounds Bounds
		{
			get
			{
				return bounds;
			}

			set
			{
				bounds = value;
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

		/// <summary>
		/// The Annotations property represents a list of test annotations.
		/// </summary>
		/// <value>The Annotations property gets/sets the value of the annotations field.</value>
		public List<string> Annotations
		{
			get
			{
				return annotations;
			}

			set
			{
				annotations = value;
			}
		}

		public Submodel()
		{
			id = 0;
			ObjectManagerId = 0;
			transformation = new Matrix4x4();
			name = "";
			annotations = new List<string>();
		}

		/// <summary>
		/// Returns a nicely formatted string of this Submodel.
		/// </summary>
		/// <returns>Nicely formatted string.</returns>
		public override string ToString()
		{
			string str = "Name: " + name.ToString() +
				"\nId: " + id.ToString() +
				"\nTransformation:\n" + transformation.ToString().TrimEnd('\r', '\n');

			return str;
		}
	}
}
