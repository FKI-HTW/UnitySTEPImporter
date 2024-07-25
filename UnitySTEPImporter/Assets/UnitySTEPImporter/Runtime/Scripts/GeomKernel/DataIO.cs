using System.Runtime.InteropServices;

namespace VENTUS.StepImporter.GeomKernel
{
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public struct BoundingBox
    {
        public double mXmin, mYmin, mZmin, mXmax, mYmax, mZmax;
    };

    public enum ResultFileLoading
    {
        CREATE_SUCCESSFUL,
        WRONG_FILE_EXTENSION,            // current file extesion is not allowed 
        FILE_NOT_FOUND,                  // file not found
        LOADING_EMPTY,
        OCC_ERROR               // developer error
    }

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
    public struct Coordinate3d {
        public double X, Y, Z;
        public Coordinate3d(double v1, double v2, double v3) : this() {
            X = v1;
            Y = v2;
            Z = v3;
        }
    }
}
