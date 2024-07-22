using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using StepImporter.GeomKernel;

namespace StepImporter.ModuleAssembly.Model {
    internal class ModelObject {

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
