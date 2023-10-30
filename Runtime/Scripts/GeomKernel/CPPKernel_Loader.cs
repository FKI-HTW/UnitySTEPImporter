using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    /// <summary>
    /// CPPKernel stand for operation only on CPPKernel. 
    /// </summary>
    class CPPKernel_Loader
    {
        internal static class CPPKernelDLLCaller
        {
            // NOTE: change enums and structs only with the enums and structs in exporter(see ExportInfo.h)

            public enum ResultFileLoad
            {
                CREATE_SUCCESSFUL,     // execution was successful, data is loaded
                WRONG_FILE_EXTENSION,  // execution was successful but nothing to load
                FILE_NOT_FOUND,        // execution was NOT successful, because of an error in command or in input data
                LOADING_EMPTY,         // execution was successful and has failed
                OCC_ERROR              // indicates end or stop (such as Raise)
            };

            public enum ResultBackgroundTransfer
            {
                TRANSFER_SUCCESSFUL,              // successful transfer
                TRANSFER_EMPTY,                   // initial state or nothing to transfer in data from reader
                TRANSFER_ERROR,                   // transfer was not successful   
                DATA_NOT_IN_READER,               // data was not red from file 
                TRANSFER_DEFAULT_ERROR            // developer error
            };

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern ResultFileLoad
                transferFileToOCC(IntPtr pObjectManager);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern ResultBackgroundTransfer
                transferOCCToEdGe(IntPtr pObjectManager);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern ResultBackgroundTransfer
                transferOCCToEdGeLOD(IntPtr pObjectManager,
                                     int LOD);
        };

        //////////////////////////////////////////////////

        public static unsafe bool
            LoadObjectFromFileToOCC(IntPtr pCppGeomKernel,
                                    int objectManagerID)
        {
            IntPtr pObjectManagerCPP
                 = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            CPPKernelDLLCaller.ResultFileLoad resultTransferFileToOCC
                = CPPKernelDLLCaller.transferFileToOCC(pObjectManagerCPP);

            return IsValidLoadResult(resultTransferFileToOCC);
        }

        public static unsafe bool
            LoadObjectFromOCCToEdGe(IntPtr pCppGeomKernel,
                                    int objectManagerID)
        {
            IntPtr pObjectManagerCPP = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            CPPKernelDLLCaller.ResultBackgroundTransfer resultOCCToEdGe
                = CPPKernelDLLCaller.transferOCCToEdGe(pObjectManagerCPP);

            CheckTransferResult(resultOCCToEdGe);

            return true;
        }
        public static unsafe bool
            LoadObjectFromOCCToEdGeLOD(int objectManagerID, int LOD)
        {

            IntPtr pObjectManagerCPP = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            CPPKernelDLLCaller.ResultBackgroundTransfer resultOCCToEdGe
                = CPPKernelDLLCaller.transferOCCToEdGeLOD(pObjectManagerCPP, LOD);

            CheckTransferResult(resultOCCToEdGe);
            return true;
        }

        //////////////////////////////////////////////////

        private static void
            CheckTransferResult(CPPKernelDLLCaller.ResultBackgroundTransfer resultTransfer)
        {
            switch (resultTransfer)
            {
                case CPPKernelDLLCaller.ResultBackgroundTransfer.TRANSFER_SUCCESSFUL:

                   // Debug.Log("Transfer was successful.");
                    break;

                case CPPKernelDLLCaller.ResultBackgroundTransfer.TRANSFER_EMPTY:
                    // TODO EXCEPTION
                    Debug.Log("Initial state or nothing to transfer " +
                               "in data from reader.");
                    break;

                case CPPKernelDLLCaller.ResultBackgroundTransfer.TRANSFER_ERROR:
                    // TODO EXCEPTION
                    Debug.Log("Transfer error.");
                    break;

                case CPPKernelDLLCaller.ResultBackgroundTransfer.TRANSFER_DEFAULT_ERROR:
                    // TODO EXCEPTION
                    Debug.Log("Developer error.");
                    break;

                case CPPKernelDLLCaller.ResultBackgroundTransfer.DATA_NOT_IN_READER:
                    // TODO EXCEPTION
                    Debug.Log("Reading before Transfer was not done "
                             + "or was not successful.");
                    break;
            }
        }

        private static bool
            IsValidLoadResult(CPPKernelDLLCaller.ResultFileLoad loadResult)
        {
            switch (loadResult)
            {
                case CPPKernelDLLCaller.ResultFileLoad.CREATE_SUCCESSFUL:
                    //Debug.Log("Model loaded successful from file to OCC.");
                    return true;
                case CPPKernelDLLCaller.ResultFileLoad.WRONG_FILE_EXTENSION:
                    //TODO:EXCEPTION
                    Debug.Log("Model wrong extension error.");
                    break;
                case CPPKernelDLLCaller.ResultFileLoad.FILE_NOT_FOUND:
                    //TODO:EXCEPTION
                    Debug.Log("Model file not found.");
                    break;
                case CPPKernelDLLCaller.ResultFileLoad.LOADING_EMPTY:
                    //TODO:EXCEPTION
                    Debug.Log("Model loading successful but there is no data "
                             + "or no faces to triangulate for the import.");
                    break;
                case CPPKernelDLLCaller.ResultFileLoad.OCC_ERROR:
                    //TODO:EXCEPTION
                    Debug.Log("OCC load default ERROR.");
                    break;

            }
            return false;
        }


    }
}

