using System.Runtime.InteropServices;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
   [StructLayout(LayoutKind.Sequential, Size = 24)]
   public struct BoundingBox
   {
      public double mXmin, mYmin, mZmin, mXmax, mYmax, mZmax;
   };

   public enum ResultFileLoading
   {
      CREATE_SUCCESSFUL,
      WRONG_FILE_EXTENSION,            // current file extension is not allowed 
      FILE_NOT_FOUND,                  // file not found
      LOADING_EMPTY,
      OCC_ERROR               // developer error
   }

   //public enum ModelObjectType
   //{
   //   MODEL_PRODUCT = 1,
   //   MODEL_PART = 2
   //}

   // Define the structure to be sequential and with the correct byte size 
   // (16 floats -> 4 bytes * 16 = 64 bytes)
   [StructLayout(LayoutKind.Sequential, Size = 64)]
   public struct Transformation
   {
      public float mA11, mA12, mA13, mA14,
                   mA21, mA22, mA23, mA24,
                   mA31, mA32, mA33, mA34,
                   mA41, mA42, mA43, mA44;
   };

   // Define the structure to be sequential and with the correct byte size (4 float -> 4 bytes * 4 = 16 bytes)
   [StructLayout(LayoutKind.Sequential, Size = 16)]
   public struct RGBAColor
   {
      public double mR, mG, mB, mA;
   };

   // Define the structure to be sequential and with the correct byte size (3 floats = 4 bytes * 3 = 12 bytes)
   [StructLayout(LayoutKind.Sequential, Size = 12)]
   public struct Coordinate3d
   {
      public double X, Y, Z;
      public Coordinate3d(double v1, double v2, double v3) : this()
      {
         X = v1;
         Y = v2;
         Z = v3;
      }
   }

   public static class OCCToUnity
   {
      private static readonly Matrix4x4 mAdaptionMatrix = new Matrix4x4(new Vector4(-1f, 0f, 0f, 0f), new Vector4(0f, 0f, -1f, 0f), new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 0f, 1f));
      private static readonly float mScaleFactor = 0.001f;

      public static int adapt(int index)
      {
         // respect index shift from OCC to Unity
         return index - 1;
      }

      public static Matrix4x4 adapt(Matrix4x4 mat)
      {
         // respect coordinate system transformation
         mat = mAdaptionMatrix * mat * mAdaptionMatrix.transpose;
         // respect scaling of translation vector
         mat.m03 *= mScaleFactor;
         mat.m13 *= mScaleFactor;
         mat.m23 *= mScaleFactor;
         return mat;
      }

      public static Coordinate3d adapt(Coordinate3d vec3D)
      {
         // respect coordinate system transformation and scaling
         return new Coordinate3d(-mScaleFactor * vec3D.X, mScaleFactor * vec3D.Z, -mScaleFactor * vec3D.Y);
      }
   }
}
