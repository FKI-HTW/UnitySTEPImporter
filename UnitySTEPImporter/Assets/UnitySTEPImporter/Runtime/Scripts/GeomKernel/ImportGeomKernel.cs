using System;
using System.Runtime.InteropServices;

namespace VENTUS.StepImporter.GeomKernel
{

    // ExportGeomKernel DLL API
    public class ImportGeomKernel {
        // C:\\Users\\admin2\\Desktop\\VENTUS2\\UnitySTEPImporter_dev_II\\Assets\\UnitySTEPImporter\\Runtime\\DLLs\\
        public const string DLLName = "VENTUS_GeomKernel.dll";

        /* === Export_GeomKernel === */
        [DllImport(DLLName, EntryPoint = "initGeomKernelLOD", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr initGeomKernelLOD(int defaultLOD);
        [DllImport(DLLName, EntryPoint = "loadModelObjectFromFile", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern ResultFileLoading loadModelObjectFromFile(string filePath, IntPtr geomKernel, ref int objectManagerID);
        [DllImport(DLLName, EntryPoint = "getModelObject", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr getModelObject(IntPtr cppKernel, int objectManagerID);


        /* === Export_Assembly === */
        [DllImport(DLLName, EntryPoint = "transferNumberOfRoots", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfRoots(IntPtr modelObject);
        [DllImport(DLLName, EntryPoint = "transferChildsNumberOfARoot", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferChildsNumberOfARoot(IntPtr modelProduct);
        [DllImport(DLLName, EntryPoint = "transferNumberOfParts", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfParts(IntPtr modelObject);
        [DllImport(DLLName, EntryPoint = "transferNumberOfProducts", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfProducts(IntPtr modelObject);
        [DllImport(DLLName, EntryPoint = "transferNumberOfChildParts", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfChildParts(IntPtr modelProduct);
        [DllImport(DLLName, EntryPoint = "transferNumberOfChildProducts", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfChildProducts(IntPtr modelProduct);

        /* === Export_MeshAttribute === */
        [DllImport(DLLName, EntryPoint = "transferNumberOfMeshes", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfMeshes(IntPtr modelPart);
        [DllImport(DLLName, EntryPoint = "requestModelMesh", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr requestModelMesh(IntPtr modelPart, int indicesArray);
        [DllImport(DLLName, EntryPoint = "transferNumberOfTriangles", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferNumberOfTriangles(IntPtr modelMesh);
        [DllImport(DLLName, EntryPoint = "transferTriangle", CallingConvention = CallingConvention.StdCall)] public unsafe static extern void transferTriangle(IntPtr modelMesh, int index, ref int point1, ref int point2, ref int point3);
        [DllImport(DLLName, EntryPoint = "transferCoordinateCount", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferCoordinateCount(IntPtr modelMesh);
        [DllImport(DLLName, EntryPoint = "transferCoordinate", CallingConvention = CallingConvention.StdCall)] public unsafe static extern void transferCoordinate(IntPtr modelMesh, int index, ref Coordinate3d coordinateArray);
        [DllImport(DLLName, EntryPoint = "transferLodStatus", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferLodStatus(IntPtr modelMesh);
        [DllImport(DLLName, EntryPoint = "transferColor", CallingConvention = CallingConvention.StdCall)] public unsafe static extern void transferColor(IntPtr graphicinfo, ref RGBAColor RGBAColor);

        /* === Export_ModelBaseAttribute === */
        [DllImport(DLLName, EntryPoint = "transferType", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern int transferType(IntPtr modelBase);
        [DllImport(DLLName, EntryPoint = "transferTransformation", CallingConvention = CallingConvention.StdCall)] public unsafe static extern void transferTransformation(IntPtr modelBase, ref Transformation Transformation);
        [DllImport(DLLName, EntryPoint = "transferID", CallingConvention = CallingConvention.StdCall)] public unsafe static extern void transferID(IntPtr modelBase, ref int refID);
        [DllImport(DLLName, CharSet = CharSet.Ansi, EntryPoint = "transferName", CallingConvention = CallingConvention.StdCall)] public unsafe static extern IntPtr transferName(IntPtr modelBase, ref int size);
        [DllImport(DLLName, EntryPoint = "transferBoundingBox", CallingConvention = CallingConvention.StdCall)] public unsafe static extern void transferBoundingBox(IntPtr modelBase, ref BoundingBox BoundingBox);
    
        /* === Export_ModelLODChanger === */
        [DllImport(DLLName)] internal unsafe static extern bool changeLODModelpart(IntPtr geomKernel, IntPtr modelpart, int lod);

        /* === Export_Pointer === */
        [DllImport(DLLName, EntryPoint = "requestModelobject", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr requestModelobject(IntPtr geomKernel, int objID);
        [DllImport(DLLName, EntryPoint = "requestRootFromObject", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr requestRootFromObject(IntPtr modelObject, int index);
        [DllImport(DLLName, EntryPoint = "requestChildFromRoot", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr requestChildFromRoot(IntPtr modelProduct, int index);
        [DllImport(DLLName, EntryPoint = "requestGraphicInfo", CallingConvention = CallingConvention.StdCall)] internal unsafe static extern IntPtr requestGraphicInfo(IntPtr modelMesh);        
    }
}
