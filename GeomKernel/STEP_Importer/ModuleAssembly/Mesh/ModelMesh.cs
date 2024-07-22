using StepImporter.GeomKernel;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;

namespace StepImporter.ModuleAssembly.Mesh {
	public enum LoDStatus {
		NOT_LOADED,
		CONTOUR,
		DETAILED,
		COMPLETE
	};
	internal class ModelMesh {

        // Constants
        private Coordinate3d mysteriousPoint = new Coordinate3d(1707.0505, 1707.0505, 1707.0505);

        // Fields
        private GraphicInfo graphicInfo;
        private LoDStatus lodStatus;
        private List<List<int>> triangles;
		private List<Coordinate3d> coordinates;

        // Constructors
        public ModelMesh() {
			graphicInfo = new GraphicInfo();
			lodStatus = LoDStatus.NOT_LOADED;
            triangles = new List<List<int>>();
			coordinates = new List<Coordinate3d>();
		}
		public ModelMesh(GraphicInfo graphicInfo, LoDStatus lodStatus, List<List<int>> triangles, List<Coordinate3d> coordinates) {
			this.graphicInfo = graphicInfo;
			this.lodStatus = lodStatus;
			this.triangles = triangles;
			this.coordinates = coordinates;
		}			
						 
        // Accessors
        public Coordinate3d MysteriousPoint { get { return mysteriousPoint; } }		
		public GraphicInfo GraphicInfo { get { return graphicInfo; } }
        public LoDStatus LodStatus { get { return lodStatus; } }		
		public List<List<int>> Triangles { get { return triangles; } }
		public List<Coordinate3d> Coordinates { get { return coordinates; } }
	}
}
