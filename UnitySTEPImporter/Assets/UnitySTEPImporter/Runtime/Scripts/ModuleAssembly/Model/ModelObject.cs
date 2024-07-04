using System.Collections.Generic;

namespace VENTUS.StepImporter.ModuleAssembly.Model
{
    public class ModelObject {

        // Fields
        private List<ModelBase> rootModels;

        // Constructors
        public ModelObject() {
            rootModels = new List<ModelBase>();
        }
        public ModelObject(List<ModelBase> rootModels) {
            this.rootModels = rootModels;
        }

        // Accessors
        public List<ModelBase> RootModels
        {
            get { return rootModels; }
            set { rootModels = value; }
        }
    }
}
