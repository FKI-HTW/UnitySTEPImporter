using VENTUS.StepImporter.GeomKernel;
using VENTUS.StepImporter.ModuleAssembly.Mesh;
using System.Collections.Generic;
using System.Numerics;

namespace VENTUS.StepImporter.ModuleAssembly.Model
{
    public class ModelPart : ModelBase {

        // Fields
        private List<ModelMesh> meshes = new List<ModelMesh>();

        // Constructors
        public ModelPart() {}

        public ModelPart(ModelType type, int id, string name, 
                         Matrix4x4 transformation, BoundingBox boundingBox) 
            : base(type, id, name, transformation, boundingBox) {}

        public ModelPart(ModelType type, int id, string name,
                         Matrix4x4 transformation, BoundingBox boundingBox,
                         ModelBase parent, List<ModelMesh> meshes)
          : base(type, id, name, transformation, boundingBox, parent) {
            this.meshes = meshes;
        }

        public ModelPart(ModelBase modelBase, List<ModelMesh> meshes)
          : base(modelBase.Type, modelBase.Id, modelBase.Name, modelBase.Transformation, 
                modelBase.BoundingBox, modelBase.Parent) {
            this.meshes = meshes;
        }

        // Accessors
        public List<ModelMesh> Meshes { get { return meshes; } }
    }
}
