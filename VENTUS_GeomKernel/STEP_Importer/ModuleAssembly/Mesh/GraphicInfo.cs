using System;
using System.Collections.Generic;
using System.Text;

namespace StepImporter.ModuleAssembly.Mesh
{
    internal class GraphicInfo
    {
		public GraphicInfo() { }

		public GraphicInfo(Color color)
		{
			this.color = color;
		}

		private Color color;
		public Color Color { get { return color; } set { color = value; } }
	}
}
