using StepImporter.GeomKernel;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StepImporter.ModuleAssembly.Model {
    internal abstract class ModelBase {

        // Constatnts
        public enum ModelType {
            PRODUCT,
            PART
        }

        // Fields
        private ModelType type;
        protected int id;
        protected string name;
        protected Matrix4x4 transformation;
        protected BoundingBox boundingBox;
        protected ModelBase parent;

        // Constructors
        protected ModelBase() {
            id = 0;
            name = "";
            transformation = new Matrix4x4();
            boundingBox = new BoundingBox();
            parent = null;
        }

        protected ModelBase(ModelType type, ModelBase parent, Matrix4x4 transformation, string name) {
            this.type = type;
            this.name = name;
            this.transformation = transformation;
            this.parent = parent;
        }

        protected ModelBase(ModelType type, int id, string name,
                            Matrix4x4 transformation, BoundingBox boundingBox) {
            this.type = type;
            this.id = id;
            this.name = name;
            this.transformation = transformation;
            this.boundingBox = boundingBox;
            this.parent = null;
        }

        protected ModelBase(ModelType type, int id, string name, 
                            Matrix4x4 transformation, BoundingBox boundingBox, ModelBase parent) {
            this.type = type;
            this.id = id;
            this.name = name;
            this.transformation = transformation;
            this.boundingBox = boundingBox;
            this.parent = parent;
        }

        // Accessors
        public ModelType Type { get { return type; } }
        public int Id { get { return id; } }
        public string Name {
            get { return name; }
            set { name = value; }
        }
        public Matrix4x4 Transformation {
            get { return transformation; }
            set { transformation = value; }
        }
        public BoundingBox BoundingBox {
            get { return boundingBox; }
            set { boundingBox = value; }
        }
        public ModelBase Parent {
            get { return parent; }
            set { parent = value; }
        }

    }
}
