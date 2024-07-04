using System;
using System.Collections.Generic;
using UnityEngine;
using VENTUS.StepImporter.UnityExtentions;
using VENTUS.StepImporter.ModuleAssembly.Model;
using VENTUS.StepImporter.GeomKernel;
using VENTUS.StepImporter.ModuleAssembly.Mesh;
using VENTUS.StepImporter.ModuleAssembly.OldModel;
using PlasticPipe.PlasticProtocol.Messages;
using System.Linq;
using UnityEngine.Rendering;

namespace VENTUS.StepImporter.Assets.UnitySTEPImporter.Runtime.Scripts.ModuleAssembly {
    public class ModelObjectConverter
    {

        public static Modelobject convertToOldModel(ModelObject modelObject, string path)
        {
            // Modelobject assumes only one root element
            Modelobject modelobject = initModelProduct(modelObject, path);
            foreach (ModelBase modelBase in modelObject.RootModels)
            {
                if (modelBase.Type == ModelBase.ModelType.PRODUCT)
                {
                    Modelproduct modelproduct = transferModelProduct((ModelProduct)modelBase);
                    modelobject.Submodels.Add(modelproduct);
                }
                else
                {
                    Modelpart modelpart = transferModelPart((ModelPart)modelBase);
                    modelobject.Submodels.Add(modelpart);
                }
            }
            return modelobject;
        }

        private static Modelobject initModelProduct(ModelObject modelObject, string path)
        {
            Modelobject modelobject = new Modelobject();
            ModelBase firstRoot = modelObject.RootModels[0];
            modelobject.Id = firstRoot.Id;
            modelobject.ObjectManagerId = firstRoot.Id;
            modelobject.Transformation = transferTransformation(firstRoot.Transformation);
            modelobject.Name = firstRoot.Name;
            modelobject.Path = path;
            modelobject.Bounds = transferBounds(firstRoot.BoundingBox);
            modelobject.CppModelPointer = new IntPtr();
            return modelobject;
        }

        private static Submodel transferModelBase(ModelBase modelBase)
        {
            Submodel submodel = new Submodel();
            submodel.Id = modelBase.Id;
            submodel.ObjectManagerId = modelBase.Id;
            submodel.Transformation = transferTransformation(modelBase.Transformation);
            submodel.Name = modelBase.Name;
            submodel.Bounds = transferBounds(modelBase.BoundingBox);
            submodel.CppModelPointer = new IntPtr();
            submodel.Annotations = new List<string>();
            return submodel;
        }

        private static Modelpart transferModelPart(ModelBase modelBase)
        {
            //Modelpart modelpart = (Modelpart)transferModelBase(modelBase);
            Modelpart modelpart = new Modelpart();
            modelpart.Id = modelBase.Id;
            modelpart.ObjectManagerId = modelBase.Id;
            modelpart.Transformation = transferTransformation(modelBase.Transformation);
            modelpart.Name = modelBase.Name;
            modelpart.Bounds = transferBounds(modelBase.BoundingBox);
            modelpart.CppModelPointer = new IntPtr();
            modelpart.Annotations = new List<string>();

            modelpart.Modelmesh.Mesh = transferMesh(((ModelPart)modelBase).Meshes);
            modelpart.Modelmesh.Graphicinfo = transferGraphicInfo(((ModelPart)modelBase).Meshes[0].GraphicInfo);
            return modelpart;
        }

        private static Modelproduct transferModelProduct(ModelProduct modelProduct)
        {
            //Modelproduct modelproduct = (Modelproduct)transferModelBase(modelProduct);
            Modelproduct modelproduct = new Modelproduct();
            modelproduct.Id = modelProduct.Id;
            modelproduct.ObjectManagerId = modelProduct.Id;
            modelproduct.Transformation = transferTransformation(modelProduct.Transformation);
            modelproduct.Name = modelProduct.Name;
            modelproduct.Bounds = transferBounds(modelProduct.BoundingBox);
            modelproduct.CppModelPointer = new IntPtr();
            modelproduct.Annotations = new List<string>();

            if (modelProduct.Childs != null)
            {
                foreach (ModelBase modelBase in modelProduct.Childs)
                {
                    if (modelBase.Type == ModelBase.ModelType.PRODUCT)
                    {
                        Modelproduct modelproductChild = transferModelProduct((ModelProduct)modelBase);
                        modelproduct.Modelproducts.Add(modelproductChild);
                    }
                    else
                    {
                        Modelpart modelpart = transferModelPart(modelBase);
                        modelproduct.Modelparts.Add(modelpart);
                    }
                }
            }

            return modelproduct;
        }

        private static Graphicinfo transferGraphicInfo(GraphicInfo graphicInfo)
        {
            UnityEngine.Color color = transferColor(graphicInfo.Color);
            Graphicinfo graphicinfoUnity = new Graphicinfo();
            graphicinfoUnity.Color = color;
            graphicinfoUnity.Texture = new Texture2D(1, 1);
            return graphicinfoUnity;
        }

        private static UnityEngine.Color transferColor(VENTUS.StepImporter.ModuleAssembly.Mesh.Color color)
        {
            UnityEngine.Color unityColor = new UnityEngine.Color(
                (float)color.Red, (float)color.Green, (float)color.Blue, (float)color.Alpha);
            return unityColor;
        }

        private static Matrix4x4 transferTransformation(System.Numerics.Matrix4x4 trafo)
        {
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

        private static EModelType transferModelType(ModelBase.ModelType type)
        {
            EModelType modelType;
            if (type == ModelBase.ModelType.PRODUCT)
            {
                modelType = EModelType.ModelProduct;
            }
            else
            {
                modelType = EModelType.ModelPart;
            }
            return modelType;
        }

        private static Bounds transferBounds(BoundingBox box)
        {
            Vector3 center = new Vector3();
            center.x = (box.mXmax - box.mXmin) / 2;
            center.y = (box.mYmax - box.mYmin) / 2;
            center.z = (box.mZmax - box.mZmin) / 2;

            Vector3 size = new Vector3();
            size.x = box.mXmax - box.mXmin;
            size.y = box.mYmax - box.mYmin;
            size.z = box.mZmax - box.mZmin;

            return new Bounds(center, size);
        }

        //private static Mesh transferMesh(List<ModelMesh> modelMeshes) {
        //    Mesh mesh = new Mesh();
        //    List<Vector3> vertieces = new List<Vector3>();
        //    List<Coordinate3d> vertiecesNotAdjusted = new List<Coordinate3d>();
        //    List<int> triangles = new List<int>();
        //
        //    foreach (ModelMesh modelMesh in modelMeshes) {
        //        foreach (Coordinate3d coordinate in modelMesh.Coordinates) {
        //            // Adjusment to unity
        //            Vector3 unityCoordinate = new Vector3(-1 * (float)coordinate.X, (float)coordinate.Z, -1 * (float)coordinate.Y) / 1000;
        //            vertieces.Add(unityCoordinate);
        //            // just for searching of triangles 
        //            vertiecesNotAdjusted.Add(coordinate);
        //        }
        //    }
        //
        //    for (int meshCount = 0; meshCount < modelMeshes.Count; meshCount++) {
        //        for (int triangleCount = 0; triangleCount < modelMeshes[meshCount].Triangles.Count; triangleCount++) {
        //            Coordinate3d point1 = modelMeshes[meshCount].Coordinates[modelMeshes[meshCount].Triangles[triangleCount][0]];
        //            Coordinate3d point2 = modelMeshes[meshCount].Coordinates[modelMeshes[meshCount].Triangles[triangleCount][1]];
        //            Coordinate3d point3 = modelMeshes[meshCount].Coordinates[modelMeshes[meshCount].Triangles[triangleCount][2]];
        //            
        //            triangles.Add(vertiecesNotAdjusted.IndexOf(point1));
        //            triangles.Add(vertiecesNotAdjusted.IndexOf(point2));
        //            triangles.Add(vertiecesNotAdjusted.IndexOf(point3));
        //        }
        //    }
        //
        //    mesh.vertices = vertieces.ToArray();
        //    mesh.triangles = triangles.ToArray();
        //    mesh.RecalculateBounds();
        //    mesh.RecalculateNormals();
        //    return mesh;
        //}

        private static Mesh transferMesh(List<ModelMesh> modelMeshes) {
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
                    Vector3 unityCoordinate = new Vector3(-1 * (float)coordinate.X, (float)coordinate.Z, -1 * (float)coordinate.Y) / 1000;
                    vertieces.Add(unityCoordinate);
                    coordCount++;
                }
            }

            mesh.vertices = vertieces.ToArray();
            mesh.triangles = triangles.ToArray();
            // new int[] { 1, 0, 2, 1, 2, 3, 4, 5, 6, 6, 5, 7, 11, 9, 8, 11, 8, 10, 13, 15, 12, 12, 15, 14, 19, 17, 16, 19, 16, 18, 21, 23, 20, 20, 23, 22 };
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            return mesh;
        }
    }
}
