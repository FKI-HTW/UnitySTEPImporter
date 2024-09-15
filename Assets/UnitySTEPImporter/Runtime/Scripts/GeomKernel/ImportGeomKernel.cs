using System;
using System.Runtime.InteropServices;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{

   // ExportGeomKernel DLL API
   public class ImportGeomKernel
   {
      // C:\\Users\\admin2\\Desktop\\VENTUS2\\UnitySTEPImporter_dev_II\\Assets\\UnitySTEPImporter\\Runtime\\DLLs\\
      public const string DLLName = "VENTUS_GeomKernel.dll";

      [DllImport(DLLName, EntryPoint = "initGeomKernel", CallingConvention = CallingConvention.StdCall)]
      public unsafe static extern IntPtr initGeomKernel();
      [DllImport(DLLName, EntryPoint = "cleanUpGeomKernel", CallingConvention = CallingConvention.StdCall)]
      public unsafe static extern void cleanUpGeomKernel(IntPtr cppKernel);

      /* === Export_Assembly === */
      [DllImport(DLLName, EntryPoint = "loadObjectsFromFile", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern ResultFileLoading loadObjectsFromFile(string filePath, IntPtr geomKernel, ref int objID);
      [DllImport(DLLName, EntryPoint = "getObject", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern IntPtr getObject(IntPtr cppKernel, int objID);
      [DllImport(DLLName, EntryPoint = "getNumberOfRoots", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern int getNumberOfRoots(IntPtr modelObject);
      [DllImport(DLLName, EntryPoint = "getRoot", CallingConvention = CallingConvention.StdCall)]
      public unsafe static extern IntPtr getRoot(IntPtr modelObject, int index);
      [DllImport(DLLName, EntryPoint = "getNumberOfChildren", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern int getNumberOfChildren(IntPtr modelProduct);
      [DllImport(DLLName, EntryPoint = "getChild", CallingConvention = CallingConvention.StdCall)]
      public unsafe static extern IntPtr getChild(IntPtr modelProduct, int index);

      /* === Export_MeshAttribute === */
      [DllImport(DLLName, EntryPoint = "getNumberOfMeshes", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern int getNumberOfMeshes(IntPtr modelPart);
      [DllImport(DLLName, EntryPoint = "getMesh", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern IntPtr getMesh(IntPtr modelPart, int index);
      [DllImport(DLLName, EntryPoint = "getNumberOfTriangles", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern int getNumberOfTriangles(IntPtr modelMesh);
      [DllImport(DLLName, EntryPoint = "getTriangle", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern void getTriangle(IntPtr modelMesh, int index, ref int point1, ref int point2, ref int point3);
      [DllImport(DLLName, EntryPoint = "getNumberOfPoints", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern int getNumberOfPoints(IntPtr modelMesh);
      [DllImport(DLLName, EntryPoint = "getPoint", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern void getPoint(IntPtr modelMesh, int index, ref Coordinate3d coordinateArray);
      [DllImport(DLLName, EntryPoint = "getGraphicInfo", CallingConvention = CallingConvention.StdCall)]
      public unsafe static extern IntPtr getGraphicInfo(IntPtr modelMesh);
      [DllImport(DLLName, EntryPoint = "getColor", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern void getColor(IntPtr graphicinfo, ref RGBAColor RGBAColor);

      /* === Export_ModelBaseAttribute === */
      [DllImport(DLLName, EntryPoint = "getType", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern int getType(IntPtr modelBase);
      [DllImport(DLLName, EntryPoint = "getTransformation", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern void getTransformation(IntPtr modelBase, ref Transformation Transformation);
      [DllImport(DLLName, CharSet = CharSet.Ansi, EntryPoint = "getName", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern IntPtr getName(IntPtr modelBase, ref int size);
      [DllImport(DLLName, EntryPoint = "getBoundingBox", CallingConvention = CallingConvention.StdCall)] 
      public unsafe static extern void getBoundingBox(IntPtr modelBase, ref BoundingBox BoundingBox);
   }
}
