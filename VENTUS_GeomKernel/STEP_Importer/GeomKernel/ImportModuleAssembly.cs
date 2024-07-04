// System imports
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Numerics;
// Project imports
using StepImporter;
using StepImporter.GeomKernel;
using StepImporter.ModuleAssembly.Model;
using static StepImporter.ModuleAssembly.Model.ModelBase;
using System.Collections;
using StepImporter.ModuleAssembly.Mesh;

namespace StepImporter {
    class ImportModuleAssembly {

        // Constants
        int defaultLOD = 2;

        // Fields
        private int lod;
        private IntPtr geomKernel;
       
        // Constructors
        public ImportModuleAssembly() {
            lod = defaultLOD;
            geomKernel = IntPtr.Zero;
        }
        public ImportModuleAssembly(int lod) {
            geomKernel = IntPtr.Zero;
            this.lod = lod; 
            geomKernel = ImportGeomKernel.initGeomKernelLOD(lod);
        }

        // Methods
        public ModelObject transferModelObject(string path) {
            int id = -1;
            ResultFileLoading loadResult = ImportGeomKernel.loadModelObjectFromFile(path, geomKernel, ref id);
            if (loadResult != ResultFileLoading.CREATE_SUCCESSFUL) { 
                return null;
            }
            IntPtr cppModelObject = ImportGeomKernel.getModelObject(geomKernel, id);
            List<ModelBase> rootModels = transferModelRoots(cppModelObject);
            return new ModelObject(rootModels);
        }
        private List<ModelBase> transferModelRoots(IntPtr cppModelObject) {
            List<ModelBase> roots = new List<ModelBase>();
            int numberOfRoots = ImportGeomKernel.transferNumberOfRoots(cppModelObject);
            for (int i = 0; i < numberOfRoots; ++i) {
                IntPtr cppModelBase = ImportGeomKernel.requestRootFromObject(cppModelObject, i);
                ModelBase modelBase = trnsferModelBaseProperties(cppModelBase);
                ModelBase child = transferChilds(cppModelBase, modelBase);
                roots.Add(child);
            }
            return roots;
        }

        private ModelBase transferChilds(IntPtr cppModelBase, ModelBase modelBase) {
            if (modelBase.Type == ModelType.PRODUCT) {
                ModelProduct root = (ModelProduct)modelBase;
                int numberOfCilds = ImportGeomKernel.transferChildsNumberOfARoot(cppModelBase);
                for (int j = 0; j < numberOfCilds; ++j) {
                    IntPtr cppChildModelBase = ImportGeomKernel.requestChildFromRoot(cppModelBase, j);
                    ModelBase childModelBase = trnsferModelBaseProperties(cppChildModelBase);
                    ModelBase child = transferChilds(cppChildModelBase, childModelBase);
                    root.Childs.Add(child);
                }
                return root;
            } else { 
                List<ModelMesh> meshes = transferModelMesh(cppModelBase);
                ModelPart root = new ModelPart(modelBase, meshes);
                return root;
            }
        }

        private unsafe List<ModelMesh> transferModelMesh(IntPtr cppModelPart) {
            List<ModelMesh> meshes = new List<ModelMesh>();
            int numberOfMeshes = ImportGeomKernel.transferNumberOfMeshes(cppModelPart);
            int count = 0;
            for (int i = 0; i < numberOfMeshes; i++) {
                IntPtr cppMesh = ImportGeomKernel.requestModelMesh(cppModelPart, i);

                IntPtr cppGraphicInfo = ImportGeomKernel.requestGraphicInfo(cppMesh);
                RGBAColor color = new RGBAColor();
                ImportGeomKernel.transferColor(cppGraphicInfo, ref color);
                GraphicInfo graphicInfo = new GraphicInfo(new Color(color.mR, color.mG, color.mB, color.mA));

                int lodStatusType = ImportGeomKernel.transferLodStatus(cppMesh);
                LoDStatus lodStatus;
                if (lodStatusType == 0) {
                    lodStatus = LoDStatus.COMPLETE;
                } else if (lodStatusType == 1) {
                    lodStatus = LoDStatus.CONTOUR;
                } else if (lodStatusType == 2) {
                    lodStatus = LoDStatus.DETAILED;
                } else {
                    lodStatus = LoDStatus.NOT_LOADED;
                }

                List<List<int>> triangles = new List<List<int>>();
                count = ImportGeomKernel.transferNumberOfTriangles(cppMesh);
                for (int j = 1; j <= count; j++) {
                    int point1 = new int();
                    int point2 = new int(); 
                    int point3 = new int();
                    ImportGeomKernel.transferTriangle(cppMesh, j, ref point1, ref point2, ref point3);
                    List<int> trinagle = new List<int> {point1-1, point2-1, point3-1};
                    triangles.Add(trinagle);
                }

                count = ImportGeomKernel.transferCoordinateCount(cppMesh);
                List<Coordinate3d> coordinates = new List<Coordinate3d>();
                for (int j = 1; j <= count; j++) {
                    Coordinate3d coordinate = new Coordinate3d();
                    ImportGeomKernel.transferCoordinate(cppMesh, j, ref coordinate);
                    coordinates.Add(coordinate);
                }

                meshes.Add(new ModelMesh(graphicInfo, lodStatus, triangles, coordinates));
            }
            return meshes;
        }

        private ModelBase trnsferModelBaseProperties(IntPtr cppModelBAse) {
            ModelType type = transferType(cppModelBAse);
            int id = transferId(cppModelBAse);
            string name = transferName(cppModelBAse);
            Matrix4x4 transformation = transferTransformation(cppModelBAse);
            BoundingBox box = transferBoundingBox(cppModelBAse);
            if(type == ModelType.PRODUCT) {
                ModelProduct product = new ModelProduct(type, id, name, transformation, box);
                return product;
            }
            ModelPart part = new ModelPart(type, id, name, transformation, box);
            return part;
        }

        private ModelType transferType(IntPtr cppModelBase) {
            int type = ImportGeomKernel.transferType(cppModelBase);
            if (type == 1) {
                return ModelBase.ModelType.PRODUCT;
            }
            return ModelBase.ModelType.PART;
        }

        private Matrix4x4 transferTransformation(IntPtr cppModelBase) {
            Transformation transformation = new Transformation();
            ImportGeomKernel.transferTransformation(cppModelBase, ref transformation);
            return new Matrix4x4(transformation.mA11, transformation.mA12, transformation.mA13, transformation.mA14,
                                 transformation.mA21, transformation.mA22, transformation.mA23, transformation.mA24,
                                 transformation.mA31, transformation.mA32, transformation.mA33, transformation.mA34,
                                 transformation.mA41, transformation.mA42, transformation.mA43, transformation.mA44);
        }

        private int transferId(IntPtr cppModelBase) {
            int id = new int();
            ImportGeomKernel.transferID(cppModelBase, ref id);
            return id;
        }

        private string transferName(IntPtr cppModelBase) {
            int size = new int();
            IntPtr name = ImportGeomKernel.transferName(cppModelBase, ref size);
            return Marshal.PtrToStringAnsi(name, size);
        }

        private BoundingBox transferBoundingBox(IntPtr cppModelBase) {
            BoundingBox box = new BoundingBox();
            ImportGeomKernel.transferBoundingBox(cppModelBase, ref box);
            return box;
        }

        // Accessors
        public int Lod {
            get { return lod; }
            set { lod = value; } }

        // May be not needed
        //public IntPtr GeomKernel { get { return geomKernel;  } }
    }
}
