using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    static class CPPToUnityKernel_CPPPointerRequester
    {
        internal static class CPPKernelDLLCaller
        {
            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestObjectManager(IntPtr geomKernelCPPPointer,
                                      int objectManagerID);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestModelobject(IntPtr pObjectManager);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestSubPartFromObject(IntPtr pEdGeModelBase,
                                      int i);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestSubPartFromProduct(IntPtr pEdGeModelBase,
                                       int i);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestSubProductFromProduct(IntPtr pEdGeModelproduct,
                                          int i);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestSubProductFromObject(IntPtr pEdGeModelObject,
                                         int i);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestPartOverID(IntPtr pObjectManager,
                                  int partID);

            //[DllImport(CPPtoUnityKernel_Constants.DLLName)] internal unsafe static extern IntPtr
            //    requestProductOverID(IntPtr pObjectManager,
            //                         int productID);

            //[DllImport(CPPtoUnityKernel_Constants.DLLName)] internal unsafe static extern IntPtr
            //    requestPartFromObjectOverID(IntPtr pModelobject,
            //                                int modelpartID);

            //[DllImport(CPPtoUnityKernel_Constants.DLLName)] internal unsafe static extern IntPtr
            //    requestPartFromProductOverID(IntPtr pModelproduct,
            //                                 int modelpartID);

            //[DllImport(CPPtoUnityKernel_Constants.DLLName)] internal unsafe static extern IntPtr
            //    requestProductFromObjectOverID(IntPtr pModelobject,
            //                                   int modelproductID);

            //[DllImport(CPPtoUnityKernel_Constants.DLLName)] internal unsafe static extern IntPtr
            //    requestProductFromProductOverID(IntPtr pModelproduct,
            //                                    int modelproductID);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                 requestModelmesh(IntPtr pEdGeModelpart);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestMesh(IntPtr pEdGeModelmesh);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                requestGraphicInfo(IntPtr pEdGeModelmesh);

            }

        // TESTSTSTST
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ProgressCallback(string str);

        [DllImport(CPPtoUnityKernel_Constants.DLLName)]
        public unsafe static extern void
           doWork([MarshalAs(UnmanagedType.FunctionPtr)]ProgressCallback callbackPointer);

        //////////////////////////////////////////////////

        public static unsafe IntPtr
            RequestObjectManagerPointer(int objectManagerID)
        {
            IntPtr pObjectManager
                = CPPKernelDLLCaller.requestObjectManager(CPPKernel_Handle.CPPKernel,
                                                          objectManagerID);

            //if (IntPtr.Zero == pObjectManager)
            //{
            //    // TODO EXCEPTION
            //}

            return pObjectManager;
        }

        public static unsafe IntPtr
            RequestModelobject(IntPtr pObjectManager)
        {
            return CPPKernelDLLCaller.requestModelobject(pObjectManager);
        }

        public static unsafe IntPtr
            RequestSubPartPointerFromObject(IntPtr pParentModelobject,
                                            int i)
        {
            IntPtr pSubPart = CPPKernelDLLCaller.requestSubPartFromObject(pParentModelobject,
                                                                          i);
            //if (IntPtr.Zero == pSubPart)
            //{
            //    // TODO EXCEPTION
            //}

            return pSubPart;
        }

        public static unsafe IntPtr
            RequestSubPartPointerFromProduct(IntPtr pParentModelproduct,
                                             int i)
        {
            IntPtr pSubPart = CPPKernelDLLCaller.requestSubPartFromProduct(pParentModelproduct,
                                                                           i);
            //if (IntPtr.Zero == pSubPart)
            //{
            //    // TODO EXCEPTION
            //}

            return pSubPart;
        }

        public static unsafe IntPtr
            RequestSubProductPointerFromObject(IntPtr pEdGeModelObject,
                                               int i)
        {

            IntPtr pSubproduct = CPPKernelDLLCaller.requestSubProductFromObject(pEdGeModelObject,
                                                                           i);
            //if (IntPtr.Zero == pSubproduct)
            //{
            //    // TODO EXCEPTION
            //}

            return pSubproduct;
        }

        public static unsafe IntPtr
            RequestSubProductPointerFromProduct(IntPtr pParentModelproduct,
                                                int i)
        {
            IntPtr pSubproduct = CPPKernelDLLCaller.requestSubProductFromProduct(pParentModelproduct,
                                                                                 i);
            //if (IntPtr.Zero == pSubproduct)
            //{
            //    // TODO EXCEPTION
            //}

            return pSubproduct;
        }

        public static unsafe IntPtr
            RequestModelmeshPointer(IntPtr pEdGeModelpart)
        {
            IntPtr pModelmesh = CPPKernelDLLCaller.requestModelmesh(pEdGeModelpart);

            //if (IntPtr.Zero == pmodelmesh)
            //{
            //    // TODO EXCEPTION
            //}
            return pModelmesh;
        }

        public static unsafe IntPtr
            RequestMeshPointer(IntPtr pEdGeModelmesh)
        {
            IntPtr pMesh = CPPKernelDLLCaller.requestMesh(pEdGeModelmesh);

            //if (IntPtr.Zero == pMesh)
            //{
            //    // TODO EXCEPTION
            //}

            return pMesh;
        }

        public static unsafe IntPtr
            RequestGraphicinfo(IntPtr pEdGeModelMesh)
        {
            IntPtr pGraphicInfo = CPPKernelDLLCaller.requestGraphicInfo(pEdGeModelMesh);

            //if (IntPtr.Zero == pGraphicInfo)
            //{
            //    // TODO EXCEPTION
            //}

            return pGraphicInfo;
        }

        public static unsafe IntPtr
            RequestPartOverID(IntPtr pObjectManager, int modelpartID)
        {
            return CPPKernelDLLCaller.requestPartOverID( pObjectManager, 
                                                         modelpartID);
        }
    }
}