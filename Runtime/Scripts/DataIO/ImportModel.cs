using UnityEngine;
using VENTUS.UnitySTEPImporter.GeomKernel;
using System;
using System.Runtime.InteropServices;

namespace VENTUS.UnitySTEPImporter.DataIO
{
    // class ImportModel create an Modelobject(C#)
    // with imported data from the VENTUS geometry kernel data structure
    public class ImportModel
    {
        // initialize the geometry kernel 
        // !!! call before other function calls in this class
        public unsafe void 
            InitGeomKernel(int defaultLOD = 2)
        {
            CPPKernel_Handle.InitCPPKernel(defaultLOD);
        }

        public unsafe int 
            LoadModelobjectFromFile( string filepath)
        {
            int objectManagerID = CPPKernel_Handle.CreateNewObjectManager(filepath);
            if (0 == objectManagerID)
            {
                Debug.Log("objectManager is not created!");
                return 0;
            }
            
            bool fileToOCCLoaded 
                = CPPKernel_Loader.LoadObjectFromFileToOCC(CPPKernel_Handle.CPPKernel,
                                                            objectManagerID);

            if (!fileToOCCLoaded)
            {
                Debug.Log("Modelobject: File to OCC was NOT successful!");
                return 0;
            }

            bool OCCToEdGeLoaded
                = CPPKernel_Loader.LoadObjectFromOCCToEdGe(CPPKernel_Handle.CPPKernel,
                                                            objectManagerID);

            if (!OCCToEdGeLoaded)
            {
                Debug.Log("Modelobject OCC to edGe was NOT successful!");
                return 0;
            }

            return objectManagerID;
        }

        public unsafe int 
            LoadModelobjectFromFile( string filepath, 
                                        int LOD)
        {
            int objectManagerID = CPPKernel_Handle.CreateNewObjectManager(filepath);
            if (0 == objectManagerID)
            {
                Debug.Log("objectManager is not created!");
                return 0;
            }

            bool fileToOCCLoaded
                = CPPKernel_Loader.LoadObjectFromFileToOCC(CPPKernel_Handle.CPPKernel,
                                                            objectManagerID);

            if (!fileToOCCLoaded)
            {
                Debug.Log("Modelobject: File to OCC was NOT successful!");
                return 0;
            }


            bool OCCToEdGeLoaded 
                = CPPKernel_Loader.LoadObjectFromOCCToEdGeLOD( objectManagerID, 
                                                                LOD);

            if (!OCCToEdGeLoaded)
            {
                Debug.Log("Modelobject OCC to edGe was NOT successful!");
                return 0;
            }


            return objectManagerID;
        }

        public unsafe Modelobject 
            getModelobjectFromKernel(int objectManagerID)
        {
            return CPPtoUnityKernel_AssemblyTransferer.TransferModelobject(objectManagerID);
        }

        public unsafe void 
            ChangeLOD( int objectManagerID, 
                        Modelobject modelobject, 
                        int newLOD)
        {
            CPPKernel_ModelLODChanger.ChangeLOD( objectManagerID,
                                                    modelobject, 
                                                    newLOD);
        }

        public unsafe void 
            ChangeLOD( int objectManagerID, 
                        Modelproduct modelproduct, 
                        int newLOD)
        {
            CPPKernel_ModelLODChanger.ChangeLOD( objectManagerID,
                                                    modelproduct, 
                                                    newLOD);
        }

        public unsafe void 
            ChangeLOD( int objectManagerID, 
                        Modelpart modelpart, 
                        int newLOD)
        {
            CPPKernel_ModelLODChanger.ChangeLOD( objectManagerID, 
                                                    modelpart, 
                                                    newLOD);
        }


        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ProgressCallback(IntPtr pEdGeModelpart, int LOD);

        public unsafe void
            ChangeLODThreadJob(ProgressCallback callback,
                                int objectManagerID,
                                Modelpart modelpart,
                                int newLOD)
        {
            CPPKernel_ModelLODChanger.ChangeLODThreadJob(callback,
                                                            objectManagerID,
                                                            modelpart,
                                                            newLOD);
        }

        // if no changed Modelpart,then returned null
        public unsafe Modelpart 
            GetChangedLODModelpart()
        {
            return CPPKernel_ModelLODChanger.RequestChangeLODModelpart();
        }

        public unsafe Modelpart transferModelpart(IntPtr pEdGeModelpart)
        {
            Modelpart modelpart = CPPtoUnityKernel_AssemblyTransferer.TransferModelpart(pEdGeModelpart);
            TransformationUtilities.TranslateCoordSysOriginToBBoxCenterNewMesh(modelpart);
            modelpart.Modelmesh.Mesh.RecalculateBounds();
            modelpart.Modelmesh.Mesh.RecalculateNormals();
            return modelpart;
        }
    }
}
