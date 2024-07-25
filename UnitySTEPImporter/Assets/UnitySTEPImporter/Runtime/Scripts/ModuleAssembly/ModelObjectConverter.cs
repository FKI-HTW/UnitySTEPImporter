using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VENTUS.StepImporter.UnityExtentions;
using VENTUS.StepImporter.ModuleAssembly.Model;
using VENTUS.StepImporter.GeomKernel;
using VENTUS.StepImporter.ModuleAssembly.Mesh;
using VENTUS.StepImporter.ModuleAssembly.OldModel;
using Codice.Client.Commands.TransformerRule;

namespace VENTUS.StepImporter.UnitySTEPImporter.Runtime.Scripts.ModuleAssembly {
    public class ModelObjectConverter {

        public static int SCALE_FACTOR = 1000;

        private static readonly List<double> XMins = new List<double>();
        private static readonly List<double> XMaxs = new List<double>();
        private static readonly List<double> YMins = new List<double>();
        private static readonly List<double> YMaxs = new List<double>();
        private static readonly List<double> ZMins = new List<double>();
        private static readonly List<double> ZMaxs = new List<double>();
        
        public static Modelobject ConvertToOldModel(ModelObject modelObject, string path) {
            // Modelobject assumes only one root element
            Modelobject modelobject = InitModelProduct(modelObject, path);
            foreach (ModelBase modelBase in modelObject.RootModels) {
                if (modelBase.Type == ModelBase.ModelType.PRODUCT) {
                    Modelproduct modelproduct = TransferModelProduct((ModelProduct)modelBase);
                    modelobject.Submodels.Add(modelproduct);
                } else {
                    Modelpart modelpart = TransferModelPart((ModelPart)modelBase);
                    modelobject.Submodels.Add(modelpart);
                }
            }

            modelobject.Bounds = TransferWholeObjectBounds(modelobject.Transformation);
            return modelobject;
        }

        private static Bounds TransferWholeObjectBounds(Matrix4x4 transformation) {
            BoundingBox box = new BoundingBox {
                mXmin = XMins.Min(),
                mXmax = XMaxs.Max(),
                mYmin = YMins.Min(),
                mYmax = YMaxs.Max(),
                mZmin = ZMins.Min(),
                mZmax = ZMaxs.Max()
            };
            return TransferBounds(box, transformation);
        }

        private static void CollectBounds(BoundingBox boundingBox) {
            XMins.Add(boundingBox.mXmin);
            XMaxs.Add(boundingBox.mXmax);
            YMins.Add(boundingBox.mYmin);
            YMaxs.Add(boundingBox.mYmax);
            ZMins.Add(boundingBox.mZmin);
            ZMaxs.Add(boundingBox.mZmin);
        }

        private static Modelobject InitModelProduct(ModelObject modelObject, string path) {
            Modelobject modelobject = new Modelobject();
            ModelBase firstRoot = modelObject.RootModels[0];
            modelobject.Id = firstRoot.Id;
            modelobject.ObjectManagerId = firstRoot.Id;
            modelobject.Transformation = TransferTransformation(firstRoot.Transformation);
            modelobject.Name = firstRoot.Name;
            modelobject.Path = path;
            modelobject.Bounds = TransferBounds(firstRoot.BoundingBox, modelobject.Transformation);
            CollectBounds(firstRoot.BoundingBox);
            modelobject.CppModelPointer = new IntPtr();
            return modelobject;
        }

        private static Modelpart TransferModelPart(ModelBase modelBase) {
            Modelpart modelpart = new Modelpart();
            modelpart.Id = modelBase.Id;
            modelpart.ObjectManagerId = modelBase.Id;
            modelpart.Transformation = TransferTransformation(modelBase.Transformation);
            modelpart.Name = modelBase.Name;
            modelpart.Bounds = TransferBounds(modelBase.BoundingBox, modelpart.Transformation);
            CollectBounds(modelBase.BoundingBox);
            modelpart.CppModelPointer = new IntPtr();
            modelpart.Annotations = new List<string>();

            modelpart.Modelmesh.Mesh = TransferMesh(((ModelPart)modelBase).Meshes);
            modelpart.Modelmesh.Graphicinfo = TransferGraphicInfo(((ModelPart)modelBase).Meshes[0].GraphicInfo);
            return modelpart;
        }

        private static Modelproduct TransferModelProduct(ModelProduct modelProduct) {
            Modelproduct modelproduct = new Modelproduct();
            modelproduct.Id = modelProduct.Id;
            modelproduct.ObjectManagerId = modelProduct.Id;
            modelproduct.Transformation = TransferTransformation(modelProduct.Transformation);
            modelproduct.Name = modelProduct.Name;
            modelproduct.Bounds = TransferBounds(modelProduct.BoundingBox, modelproduct.Transformation);
            CollectBounds(modelProduct.BoundingBox);
            modelproduct.CppModelPointer = new IntPtr();
            modelproduct.Annotations = new List<string>();

            if (modelProduct.Childs != null) {
                foreach (ModelBase modelBase in modelProduct.Childs) {
                    if (modelBase.Type == ModelBase.ModelType.PRODUCT) {
                        Modelproduct modelproductChild = TransferModelProduct((ModelProduct)modelBase);
                        modelproduct.Modelproducts.Add(modelproductChild);
                    } else {
                        Modelpart modelpart = TransferModelPart(modelBase);
                        modelproduct.Modelparts.Add(modelpart);
                    }
                }
            }

            return modelproduct;
        }

        private static Graphicinfo TransferGraphicInfo(GraphicInfo graphicInfo) {
            UnityEngine.Color color = TransferColor(graphicInfo.Color);
            Graphicinfo graphicinfoUnity = new Graphicinfo();
            graphicinfoUnity.Color = color;
            graphicinfoUnity.Texture = new Texture2D(1, 1);
            return graphicinfoUnity;
        }

        private static UnityEngine.Color TransferColor(VENTUS.StepImporter.ModuleAssembly.Mesh.Color color) {
            UnityEngine.Color unityColor = new UnityEngine.Color(
                (float)color.Red, (float)color.Green, (float)color.Blue, (float)color.Alpha);
            return unityColor;
        }

        private static Matrix4x4 TransferTransformation(System.Numerics.Matrix4x4 trafo) {
            Matrix4x4 transformation;

            transformation.m00 = trafo.M11;
            transformation.m01 = trafo.M12;
            transformation.m02 = trafo.M13;
            transformation.m03 = trafo.M14;

            transformation.m10 = trafo.M21;
            transformation.m11 = trafo.M22;
            transformation.m12 = trafo.M23;
            transformation.m13 = trafo.M24;

            transformation.m20 = trafo.M31;
            transformation.m21 = trafo.M32;
            transformation.m22 = trafo.M33;
            transformation.m23 = trafo.M34;

            transformation.m30 = trafo.M41;
            transformation.m31 = trafo.M42;
            transformation.m32 = trafo.M43;
            transformation.m33 = trafo.M44;

            return transformation;
        }

        private static EModelType TransferModelType(ModelBase.ModelType type) {
            EModelType modelType;
            if (type == ModelBase.ModelType.PRODUCT) {
                modelType = EModelType.ModelProduct;
            } else {
                modelType = EModelType.ModelPart;
            }
            return modelType;
        }

        private static Bounds TransferBounds(BoundingBox box, Matrix4x4 transformation) {
            Vector3 center = new Vector3();
            center.x = (float)(box.mXmax - box.mXmin) / 2;
            center.y = (float)(box.mZmax - box.mZmin) / 2;
            center.z = (float)(box.mYmax - box.mYmin) / 2;

            Vector3 size = new Vector3();
            size.x = Mathf.Abs((float)(box.mXmax - box.mXmin));
            size.y = Mathf.Abs((float)(box.mZmax - box.mZmin));
            size.z = Mathf.Abs((float)(box.mYmax - box.mYmin));

            // Unity adaptation
            Vector3 position = transformation.ExtractPosition();
            center += position;
            center /= SCALE_FACTOR;
            size /= SCALE_FACTOR;

            return new Bounds(center, size);
        }

        private static Mesh TransferMesh(List<ModelMesh> modelMeshes) {
            Mesh mesh = new Mesh();
            List<Vector3> vertieces = new List<Vector3>();
            List<int> triangles = new List<int>();

            int coordCount = 0;
            foreach (ModelMesh modelMesh in modelMeshes) {
                foreach (List<int> triangle in modelMesh.Triangles) {
                    triangles.Add(triangle[0] + coordCount);
                    triangles.Add(triangle[1] + coordCount);
                    triangles.Add(triangle[2] + coordCount);
                }
                foreach (Coordinate3d coordinate in modelMesh.Coordinates) {
                    // Adjusment to unity
                    Vector3 unityCoordinate = new Vector3(-1 * (float)coordinate.X, (float)coordinate.Z, -1 * (float)coordinate.Y) / SCALE_FACTOR;
                    vertieces.Add(unityCoordinate);
                    coordCount++;
                }
            }

            mesh.vertices = vertieces.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
