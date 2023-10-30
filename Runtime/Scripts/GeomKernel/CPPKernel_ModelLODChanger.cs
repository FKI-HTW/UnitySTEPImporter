using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using VENTUS.UnitySTEPImporter.DataIO;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    /// <summary>
    /// CPPKernel stand for operation only on CPPKernel. 
    /// </summary>
    class CPPKernel_ModelLODChanger
    {

       
        internal static class CPPKernelDLLCaller
        {
            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern bool
                changeLODModelpart( IntPtr pObjectManager,
                                    IntPtr pEdGemodelpart,
                                    int newLOD);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern bool 
                changeLODModelproduct( IntPtr pObjectManager,
                                       IntPtr pEdGemodelproduct,
                                       int newLOD);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern bool
                changeLODModelobject( IntPtr pObjectManager,
                                      IntPtr pEdGemodelobject,
                                      int newLOD);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                requestChangedModelpartID(IntPtr pGeomKernel,
                                     ref int objectManagerID,
                                     ref int edGeModelpart);


            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern bool
                changeLODModelpartThreadJob([MarshalAs(UnmanagedType.FunctionPtr)]ImportModel.ProgressCallback callbackPointer,
                                             IntPtr pObjectManager,
                                             IntPtr pModelpart,
                                             int newLOD);

        }

        //////////////////////////////////////////////////

        public static unsafe void
            ChangeLOD(int objectManagerID,
                       Modelobject modelobject,
                       int newLOD)
        {
            bool isChanged
                = ChangeLODEdGeModelobject(objectManagerID,
                                            modelobject,
                                            newLOD);
            if (isChanged)
            {
                TransferNewMeshOfSUBMODELList(modelobject.Submodels);
            }
        }

        public static unsafe void
            ChangeLOD(int objectManagerID,
                       Modelproduct modelproduct,
                       int newLOD)
        {

            bool isChanged
                = ChangeLODEdGeModelproduct(objectManagerID,
                                             modelproduct,
                                             newLOD);

            if (isChanged)
            {
                TransferNewMeshOfModelproduct(modelproduct);
            }

        }

        public static unsafe void
            ChangeLOD( int objectManagerID, 
                       Modelpart modelpart,
                       int newLOD)
        {
            bool isChanged = ChangeLODEdGeModelpart( objectManagerID,
                                                     modelpart, 
                                                     newLOD);
            
            if (isChanged)
            {                
                TransferNewMeshOfModelpart(modelpart);
                modelpart.Modelmesh.LoD = newLOD;
            }
        }

        public static unsafe Modelpart
            RequestChangeLODModelpart()
        {
            int objectManagerID = 0;
            int modelpartID = 0;
            CPPKernelDLLCaller.requestChangedModelpartID( CPPKernel_Handle.CPPKernel,
                                                          ref objectManagerID,
                                                          ref modelpartID);

            if ( 0 == objectManagerID
              || 0 == modelpartID)
            {
                return null;
            }

            IntPtr pEdGeModelManager 
                = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            IntPtr pEdGeModelpart
                = CPPToUnityKernel_CPPPointerRequester.RequestPartOverID( pEdGeModelManager, 
                                                                          modelpartID);
			Modelpart modelpart =  CPPtoUnityKernel_AssemblyTransferer.TransferModelpart(pEdGeModelpart);

			TransformationUtilities.TranslateCoordSysOriginToBBoxCenterNewMesh(modelpart);
			return modelpart;
        }


        public static unsafe void
            ChangeLODThreadJob( ImportModel.ProgressCallback callback,
                                int objectManagerID,
                                Modelpart modelpart,
                                int newLOD)
        {
            IntPtr pObjectManager 
                = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(modelpart.ObjectManagerId);

            bool result 
                = CPPKernelDLLCaller.changeLODModelpartThreadJob(callback,
                                                                 pObjectManager,
                                                                 modelpart.CppModelPointer,
                                                                 newLOD);
            //if (!result)
            //{
            //   // todo exception
            //}
        }

        //////////////////////////////////////////////////

        private static unsafe bool
        ChangeLODEdGeModelobject(int objectManagerID,
                                 Modelobject modelobject,
                                 int newLOD)
        {
            IntPtr pObjectManager
                = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            IntPtr pEdGeModelobject = modelobject.CppModelPointer;

            bool isChanged 
                = CPPKernelDLLCaller.changeLODModelobject( pObjectManager,
                                                           pEdGeModelobject,
                                                           newLOD);
            //if (!isChanged)
            //{
            //    // TODO EXCEPTION
            //}

            return isChanged;
        }

        private static unsafe bool
            ChangeLODEdGeModelproduct(int objectManagerID,
                                      Modelproduct modelproduct,
                                      int newLOD)
        {
            IntPtr pObjectManager
                = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            IntPtr pEdGeModelproduct = modelproduct.CppModelPointer;

            bool isChanged
                 = CPPKernelDLLCaller.changeLODModelproduct(pObjectManager,
                                                            pEdGeModelproduct,
                                                            newLOD);
            //if (!isChanged)
            //{
            //    // TODO EXCEPTION
            //}
            return isChanged;
        }

        private static unsafe bool
            ChangeLODEdGeModelpart(int objectManagerID,
                                   Modelpart modelpart,
                                   int newLOD)
        {
            IntPtr pObjectManager
               = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            IntPtr pEdGeModelpart = modelpart.CppModelPointer;

            bool isChanged
                = CPPKernelDLLCaller.changeLODModelpart(pObjectManager, 
                                                        pEdGeModelpart,
                                                        newLOD);
            //if (!isChanged)
            //{
            //   // todo exception
            //}

            return isChanged;
        }

        //////////////////////////////////////////////////

        private static unsafe void
            TransferNewMeshOfSUBMODELList(List<Submodel> submodels)
        {
            foreach (Submodel currSubmodel in submodels)
            {
                if (typeof(Modelpart) == currSubmodel.GetType())
                {
                    TransferNewMeshOfModelpart((Modelpart)currSubmodel);

                }
                else
                {
                    TransferNewMeshOfModelproduct((Modelproduct)currSubmodel);
                }
            }
        }

        private static unsafe void
            TransferNewMeshOfMODELPRODUCTList(List<Modelproduct> modelproducts)
        {
            foreach (Modelproduct currModelproduct in modelproducts)
            {
                TransferNewMeshOfModelproduct(currModelproduct);
            }
        }

        private static unsafe void
            TransferNewMeshOfMODELPARTList(List<Modelpart> modelparts)
        {
            foreach (Modelpart currModelpart in modelparts)
            {
                TransferNewMeshOfModelpart(currModelpart);
            }
        }

        ////////////////////////////////////////////////

        private static unsafe void
            TransferNewMeshOfModelproduct(Modelproduct modelproduct)
        {
            TransferNewMeshOfMODELPRODUCTList(modelproduct.Modelproducts);
            TransferNewMeshOfMODELPARTList(modelproduct.Modelparts);
        }

        private static unsafe void
            TransferNewMeshOfModelpart(Modelpart modelpart)
        {
            IntPtr pEdGeModelpart = modelpart.CppModelPointer;
            
            IntPtr pEdGeModelmesh
                = CPPToUnityKernel_CPPPointerRequester.RequestModelmeshPointer(pEdGeModelpart);

            modelpart.Modelmesh.Mesh = CPPtoUnityKernel_AssemblyTransferer.TransferMesh(pEdGeModelmesh);
            modelpart.Modelmesh.Mesh.RecalculateBounds();
            modelpart.Modelmesh.Mesh.RecalculateNormals();


            //TransformationUtilities.TranslateCoordSysOriginToBBoxCenterNewMesh(modelpart);
        }


    }
}