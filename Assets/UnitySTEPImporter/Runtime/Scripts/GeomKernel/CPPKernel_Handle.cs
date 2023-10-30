using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    /// <summary>
    /// CPPKernel stand for operation only on CPPKernel. 
    /// </summary>
    class CPPKernel_Handle
    {
        internal static class CPPKernelDLLCaller
        {
            public enum ResultCreateObjectManager
            {
                CREATE_SUCCESSFUL,
                WRONG_FILE_EXTENSION,            // current file extesion is not allowed 
                FILE_NOT_FOUND,                  // file not found
                LOADING_EMPTY,
                FILE_DEFAULT_ERROR               // developer error
            }

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                initGeomKernelLOD(int defaultLOD);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern ResultCreateObjectManager
                createObjectManager(string filepathFromUnity,
                                    IntPtr cppKernel,
                                    ref int refObjectManagerID);
        };

        private static IntPtr cppKernel = IntPtr.Zero;

        public static void InitCPPKernel(int defaultLOD = 2)
        {
            if (IntPtr.Zero == cppKernel)
            {
                cppKernel = CPPKernelDLLCaller.initGeomKernelLOD(defaultLOD);
            }
        }

        public static unsafe int
        CreateNewObjectManager(string filepath)
        {
            int objectManagerID = 0;
            CPPKernelDLLCaller.ResultCreateObjectManager result =
                  CPPKernelDLLCaller.createObjectManager(filepath,
                                                         cppKernel,
                                                         ref objectManagerID);

            CheckValidCreate(result);

            return objectManagerID;
        }

        private static bool
        CheckValidCreate(CPPKernelDLLCaller.ResultCreateObjectManager loadResult)
        {
            switch (loadResult)
            {
                case CPPKernelDLLCaller.ResultCreateObjectManager.CREATE_SUCCESSFUL:
                    return true;

                case CPPKernelDLLCaller.ResultCreateObjectManager.WRONG_FILE_EXTENSION:
                    //TODO EXCEPTION
                    Debug.Log("File extension is not allowed.");
                    break;

                case CPPKernelDLLCaller.ResultCreateObjectManager.FILE_NOT_FOUND:
                    //TODO EXCEPTION
                    Debug.Log("File path  was not found.");
                    break;

                case CPPKernelDLLCaller.ResultCreateObjectManager.FILE_DEFAULT_ERROR:
                    //TODO EXCEPTION
                    Debug.Log("File default Error.");
                    break;
            }
            return false;
        }

        public static IntPtr CPPKernel
        {
            get
            {
                return cppKernel;
            }
        }

    }
}