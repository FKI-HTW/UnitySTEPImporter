using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VENTUS.UnitySTEPImporter.GeomKernel;

namespace VENTUS.ModelImporter
{
   public static class STEPImporter
   {
      public static ModelObjectData ParseFile(string path)
      {
         int id = -1;
         IntPtr geomKernel = ImportGeomKernel.initGeomKernel(); // ToDo: geomKernel initialize/delete once per session
         if (geomKernel == IntPtr.Zero)
         {
            Debug.Log("geomKernel could not be initialized!");
            return null;
         }
         ResultFileLoading loadResult = ImportGeomKernel.loadObjectsFromFile(path, geomKernel, ref id);
         IntPtr cppModelObject = ImportGeomKernel.getObject(geomKernel, id);
         if (loadResult != ResultFileLoading.CREATE_SUCCESSFUL || cppModelObject == IntPtr.Zero)
         {
            Debug.Log("STP file could not be loaded!");
            return null;
         }
         ModelObjectData modelParent = new()
         {
            ModelType = EModelType.ModelParent,
            Name = path,
            Transformation = Matrix4x4.identity,
            Bounds = new Bounds(),
            Mesh = null,
            Color = Color.white,
            Texture = null
         };

         int numberOfRoots = ImportGeomKernel.getNumberOfRoots(cppModelObject);
         for (int i = 0; i < numberOfRoots; ++i)
         {
            IntPtr cppModelBase = ImportGeomKernel.getRoot(cppModelObject, i);
            if (cppModelBase != IntPtr.Zero)
               modelParent.Children.Add(CreateModelObjectData(cppModelBase));
         }
         ImportGeomKernel.cleanUpGeomKernel(geomKernel);
         return modelParent;
      }

      private static ModelObjectData CreateModelObjectData(IntPtr cppModelBase)
      {
         int type = ImportGeomKernel.getType(cppModelBase);

         switch (type)
         {
            case 1: // PRODUCT
                  ModelObjectData modelObject = new()
                  {
                     ModelType = EModelType.ModelProduct,
                     Name = transferName(cppModelBase),
                     Transformation = transferTransformation(cppModelBase),
                     Bounds = transferBoundingBox(cppModelBase),
                     Mesh = null,
                     Color = Color.white,
                     Texture = null
                  };

                  int numberOfCildren = ImportGeomKernel.getNumberOfChildren(cppModelBase);
                  for (int j = 0; j < numberOfCildren; ++j)
                  {
                     IntPtr cppChildModelBase = ImportGeomKernel.getChild(cppModelBase, j);
                     if (cppChildModelBase != IntPtr.Zero)
                        modelObject.Children.Add(CreateModelObjectData(cppChildModelBase));
                  }
                  return modelObject;
            case 2: // PART
                  return new()
                  {
                     ModelType = EModelType.ModelPart,
                     Name = transferName(cppModelBase),
                     Transformation = transferTransformation(cppModelBase),
                     Bounds = transferBoundingBox(cppModelBase),
                     Mesh = transferMesh(cppModelBase),
                     Color = transferColor(cppModelBase),
                     Texture = transferTexture(cppModelBase)
                  };
            default:
               Debug.Log("Something went wrong creating the step model!");
               return null;
         }
      }

      private static string transferName(IntPtr cppModelBase)
      {
         int size = 0;
         IntPtr name = ImportGeomKernel.getName(cppModelBase, ref size);
         return Marshal.PtrToStringAnsi(name, size);
      }

      private static Matrix4x4 transferTransformation(IntPtr cppModelBase)
      {
         Transformation transformation = new();
         ImportGeomKernel.getTransformation(cppModelBase, ref transformation);
         Vector4 col1 = new(transformation.mA11, transformation.mA21, transformation.mA31, transformation.mA41);
         Vector4 col2 = new(transformation.mA12, transformation.mA22, transformation.mA32, transformation.mA42);
         Vector4 col3 = new(transformation.mA13, transformation.mA23, transformation.mA33, transformation.mA43);
         Vector4 col4 = new(transformation.mA14, transformation.mA24, transformation.mA34, transformation.mA44);
         Matrix4x4 mat = new(col1, col2, col3, col4);
         return OCCToUnity.adapt(mat);
      }

      private static Bounds transferBoundingBox(IntPtr cppModelBase)
      {
         BoundingBox box = new();
         ImportGeomKernel.getBoundingBox(cppModelBase, ref box);
         Coordinate3d ptMin = new(box.mXmin, box.mYmin, box.mZmin);
         Coordinate3d ptMax = new(box.mXmax, box.mYmax, box.mZmax);
         ptMin = OCCToUnity.adapt(ptMin);
         ptMax = OCCToUnity.adapt(ptMax);

         Vector3 center = new();
         center.x = (float)(ptMin.X + ptMax.X) / 2;
         center.y = (float)(ptMin.Y + ptMax.Y) / 2;
         center.z = (float)(ptMin.Z + ptMax.Z) / 2;

         Vector3 size = new();
         size.x = Mathf.Abs((float)(ptMax.X - ptMin.X));
         size.y = Mathf.Abs((float)(ptMax.Y - ptMin.Y));
         size.z = Mathf.Abs((float)(ptMax.Z - ptMin.Z));

         return new Bounds(center, size);
      }

      private static Mesh transferMesh(IntPtr cppModelBase)
      {
         Mesh mesh = new();
         List<Vector3> vertices = new();
         List<int> triangles = new();

         int coordCount = 0;
         int nMeshes = ImportGeomKernel.getNumberOfMeshes(cppModelBase);
         for (int i = 0; i < nMeshes; i++)
         {
            IntPtr cppMesh = ImportGeomKernel.getMesh(cppModelBase, i);
            int nTrinagles = ImportGeomKernel.getNumberOfTriangles(cppMesh);
            for (int j = 1; j <= nTrinagles; j++)
            {
               int ptIdx1 = 0, ptIdx2 = 0, ptIdx3 = 0;
               ImportGeomKernel.getTriangle(cppMesh, j, ref ptIdx1, ref ptIdx2, ref ptIdx3);
               triangles.Add(OCCToUnity.adapt(ptIdx1) + coordCount);
               triangles.Add(OCCToUnity.adapt(ptIdx2) + coordCount);
               triangles.Add(OCCToUnity.adapt(ptIdx3) + coordCount);
            }
            int nCoord = ImportGeomKernel.getNumberOfPoints(cppMesh);
            List<Coordinate3d> coordinates = new List<Coordinate3d>();
            for (int j = 1; j <= nCoord; j++)
            {
               Coordinate3d vertex = new();
               ImportGeomKernel.getPoint(cppMesh, j, ref vertex);
               vertex = OCCToUnity.adapt(vertex);
               vertices.Add(new Vector3((float)vertex.X, (float)vertex.Y, (float)vertex.Z));

            }
            coordCount += nCoord;
         }

         mesh.vertices = vertices.ToArray();
         mesh.triangles = triangles.ToArray();
         mesh.RecalculateBounds();
         mesh.RecalculateNormals();

         return mesh;
      }

      private static Color transferColor(IntPtr cppModelBase)
      {
         int numberOfMeshes = ImportGeomKernel.getNumberOfMeshes(cppModelBase);
         if (numberOfMeshes > 0)
         {
            // multiple colors per part not supported yet - take color of first mesh
            IntPtr cppMesh = ImportGeomKernel.getMesh(cppModelBase, 0);
            IntPtr cppGraphicInfo = ImportGeomKernel.getGraphicInfo(cppMesh);
            RGBAColor color = new();
            ImportGeomKernel.getColor(cppGraphicInfo, ref color);
            return new Color((float)color.mR, (float)color.mB, (float)color.mG, (float)color.mA);
         }
         return new Color();
      }

      private static Texture2D transferTexture(IntPtr cppModelBase)
      {
         // TODO: ImportGeomKernel.getTexture(..);
         return new Texture2D(1, 1);
      }
   }
}
