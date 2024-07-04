namespace VENTUS.StepImporter.ModuleAssembly.Mesh
{
    public class GraphicInfo
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
