namespace VENTUS.StepImporter.ModuleAssembly.Mesh
{
    public class Color 
	{
		public Color() { }

		public Color(double r, double g, double b, double a)
		{
			this.red = r;
			this.green = g;
			this.blue = b;
			this.alpha = a;
		}

		private double red;
        private double green;
        private double blue ;
        private double alpha;

        public double Red { get { return red; } }
        public double Green { get { return green; }  }
        public double Blue { get { return blue; }  }		
		public double Alpha { get { return alpha; }  }

	}
}
