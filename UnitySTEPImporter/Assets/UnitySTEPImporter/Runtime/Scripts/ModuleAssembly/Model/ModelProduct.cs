using VENTUS.StepImporter.GeomKernel;
using System.Collections.Generic;
using System.Numerics;

namespace VENTUS.StepImporter.ModuleAssembly.Model
{
    public class ModelProduct : ModelBase {

        // Fields
        private List<ModelBase> childs = new List<ModelBase>();

        // Constructors
        public ModelProduct() { }

        public ModelProduct(ModelType type, int id, string name, 
                            Matrix4x4 transformation, BoundingBox boundingBox) 
            : base(type, id, name, transformation, boundingBox) { }

        public ModelProduct(ModelType type, int id, string name,
                            Matrix4x4 transformation, BoundingBox boundingBox, 
                            ModelBase parent, List<ModelBase> childs) 
            : base(type, id, name, transformation, boundingBox, parent ) {    
            this.childs = childs;
        }

        public ModelProduct(ModelBase modelBase) 
            : base(modelBase.Type, modelBase.Id, modelBase.Name, 
                  modelBase.Transformation, modelBase.BoundingBox, modelBase.Parent) { 
            
        }

        // Accessors
        public List<ModelBase> Childs { get { return childs; } }

    }
}
