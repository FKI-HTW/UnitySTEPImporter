using System;
using System.Runtime.InteropServices;
using UnityEngine;


namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    class CPPToUnityKernel_AttributTransferer
    {
        [StructLayout(LayoutKind.Sequential, Size = 24)]
        public struct ImportBoundingBox
        {
            public float mXmin, mYmin, mZmin, mXmax, mYmax, mZmax;
        };

        internal static class CPPKernelDLLCaller
        {
            // Define the structure to be sequential and with the correct byte size 
            // (16 floats -> 4 bytes * 16 = 64 bytes)
            [StructLayout(LayoutKind.Sequential, Size = 64)]
            public struct ImportTransformation
            {
                public float mA11, mA12, mA13, mA14,
                             mA21, mA22, mA23, mA24,
                             mA31, mA32, mA33, mA34,
                             mA41, mA42, mA43, mA44;
            };

            // Define the structure to be sequential and with the correct byte size (4 float -> 4 bytes * 4 = 16 bytes)
            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct ImportRGBAColor
            {
                public float mR, mG, mB, mA;
            };

            // Define the structure to be sequential and with the correct byte size (3 floats = 4 bytes * 3 = 12 bytes)
            [StructLayout(LayoutKind.Sequential, Size = 12)]
            public struct ImportCoordinate3d
            {
                public float X, Y, Z;
            }

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferIndicesCount(IntPtr pEdGeExportMesh);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                transferIndices( IntPtr pEdGeExportMesh,
                                 int* indicesArray);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferCoordinateCount(IntPtr pEdGeExportMesh);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                transferCoordinate(IntPtr pEdGeExportMesh,
                                   int index,
                                   ref ImportCoordinate3d CoordinateArray);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                transferColor(IntPtr pGraphicinfo,
                             ref ImportRGBAColor refImportRGBAColor);


            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                transferTransformation(IntPtr pModelBase,
                                      ref ImportTransformation ExportTransformation);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                transferID(IntPtr pModelBase,
                          ref int refID);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern IntPtr
                transferName(IntPtr pModelBase);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferNameLength(IntPtr pModelBase);

            //[DllImport(CPPtoUnityKernel_Constants.DLLName)]
            //internal unsafe static extern void
            //    transferName(IntPtr pModelBase, string name);

         [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern void
                transferBoundingBox(IntPtr pEdGeModelBase,
                                    ref ImportBoundingBox refBoundingBox);
        }

        //////////////////////////////////////////////////

        public static unsafe Matrix4x4
            TransferTransformation(IntPtr pEdGeModelBase)
        {
            CPPKernelDLLCaller.ImportTransformation importTransformation
                = new CPPKernelDLLCaller.ImportTransformation();

            CPPKernelDLLCaller.transferTransformation(pEdGeModelBase,
                                                     ref importTransformation);
        

            return ConvertImportTransformationToMatrix4x4(importTransformation);
        }

        public static unsafe int
            TransferID(IntPtr pEdGeModelBase)
        {
            int id = new int();

            CPPKernelDLLCaller.transferID(pEdGeModelBase,
                                          ref id);

            return id;
        }

        public static unsafe string
            TransferName(IntPtr pEdGeModelBase)
        {
            IntPtr transferredNameCharPointer 
                = CPPKernelDLLCaller.transferName(pEdGeModelBase);
            int nameLength
                = CPPKernelDLLCaller.transferNameLength(pEdGeModelBase);
            
            return CharPointerToString(transferredNameCharPointer, nameLength);
        }

        public static unsafe Bounds
            TransferBoundingBox(IntPtr pEdGeModelBase)
        {
            ImportBoundingBox boundingBox = new ImportBoundingBox();

            CPPKernelDLLCaller.transferBoundingBox(pEdGeModelBase,
                                                   ref boundingBox);

            return ConvertImportBoundingBoxToUnityBounds(ref boundingBox);
        }

        public static unsafe int[]
            TransferIndices(IntPtr pEdGeExportMesh)
        {
            int indicesCount = TransferIndicesCount(pEdGeExportMesh);
            int[] indicesArray = new int[indicesCount];
            fixed (int* fixedArrayIndices = indicesArray)
            {
                CPPKernelDLLCaller.transferIndices(pEdGeExportMesh,
                                                    fixedArrayIndices);
            }
            return indicesArray;
        }

        public static Vector3[]
            TransferCoordinates(IntPtr pEdGeMesh)
        {
            int coordinateCount = CPPKernelDLLCaller.transferCoordinateCount(pEdGeMesh);

            CPPKernelDLLCaller.ImportCoordinate3d[] importCoordinateArray
                = new CPPKernelDLLCaller.ImportCoordinate3d[coordinateCount];

            for (int i = 0; i < coordinateCount; ++i)
            {
                CPPKernelDLLCaller.transferCoordinate(pEdGeMesh,
                                                      i,
                                                      ref importCoordinateArray[i]);
            }

            return ImportCoordinateToVector3Array(importCoordinateArray);
        }

        public static unsafe Color
            TransferColor(IntPtr pGraphicinfo)
        {
            CPPKernelDLLCaller.ImportRGBAColor importColor
                = new CPPKernelDLLCaller.ImportRGBAColor();

            CPPKernelDLLCaller.transferColor(pGraphicinfo,
                                              ref importColor);

            return new Color(importColor.mR,
                              importColor.mG,
                              importColor.mB,
                              importColor.mA);
        }

        public static string createDefaultNameModelobject()
        {
            return "Modelobject";
        }

        public static string createDefaultNameModelpart()
        {
            return "Modelpart";
        }

        public static string createDefaultNameModelproduct()
        {
            return "Modelproduct";
        }


        //////////////////////////////////////////////////

        private static unsafe int
            TransferIndicesCount(IntPtr pEdGeExportMesh)
        {
            int indicesCount = CPPKernelDLLCaller.transferIndicesCount(pEdGeExportMesh);

            return indicesCount;
        }

        private static unsafe string
            CharPointerToString(IntPtr transferredNameCharPointer, int len)
        {
            return Marshal.PtrToStringAnsi(transferredNameCharPointer, len);         
        }

        private static Matrix4x4
            ConvertImportTransformationToMatrix4x4(CPPKernelDLLCaller.ImportTransformation importTransformation)
        {
            return new Matrix4x4
                (
                    new Vector4(importTransformation.mA11,
                                 importTransformation.mA12,
                                 importTransformation.mA13,
                                 importTransformation.mA14),
                    new Vector4(importTransformation.mA21,
                                 importTransformation.mA22,
                                 importTransformation.mA23,
                                 importTransformation.mA24),
                    new Vector4(importTransformation.mA31,
                                 importTransformation.mA32,
                                 importTransformation.mA33,
                                 importTransformation.mA34),
                    new Vector4(importTransformation.mA41,
                                 importTransformation.mA42,
                                 importTransformation.mA43,
                                 importTransformation.mA44)
               );
        }

        private static Bounds
            ConvertImportBoundingBoxToUnityBounds(ref ImportBoundingBox importBoundBox)
        {
            float originX = (importBoundBox.mXmax + importBoundBox.mXmin) / 2.0f;
            float originY = (importBoundBox.mYmax + importBoundBox.mYmin) / 2.0f;
            float originZ = (importBoundBox.mZmax + importBoundBox.mZmin) / 2.0f;

            Vector3 origin = new Vector3(originX, originY, originZ);

            float sizeX = importBoundBox.mXmax - importBoundBox.mXmin;
            float sizeY = importBoundBox.mYmax - importBoundBox.mYmin;
            float sizeZ = importBoundBox.mZmax - importBoundBox.mZmin;

            Vector3 size = new Vector3(sizeX, sizeY, sizeZ);

            return new Bounds(origin, size);
        }

        private static unsafe Vector3[]
            ImportCoordinateToVector3Array(CPPKernelDLLCaller.ImportCoordinate3d[] importCoordinateArray)
        {
            Vector3[] Vector3Array = new Vector3[importCoordinateArray.Length];

            for (int i = 0; i < importCoordinateArray.Length; ++i)
            {
                Vector3Array[i] = ImportCoordinateToVector3(importCoordinateArray[i]);
            }
            return Vector3Array;
        }

        private static unsafe Vector3
            ImportCoordinateToVector3(CPPKernelDLLCaller.ImportCoordinate3d importCoordinate)
        {
            return new Vector3(importCoordinate.X,
                                importCoordinate.Y,
                                importCoordinate.Z);
        }

    }

}
