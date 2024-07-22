#ifndef _VENTUS_Export_SetUpsToUnity_h_
#define _VENTUS_Export_SetUpsToUnity_h_

// SYSTEM INCLUDES
#include <tuple>

namespace ventus
{
  namespace Export
  {
    struct ExportTransformation4d
    {
      ExportTransformation4d( float a11, float a12, float a13, float a14,
                              float a21, float a22, float a23, float a24,
                              float a31, float a32, float a33, float a34,
                              float a41, float a42, float a43, float a44 )
        : mA11( a11 ), mA12( a12 ), mA13( a13 ), mA14( a14 ),
        mA21( a21 ), mA22( a22 ), mA23( a23 ), mA24( a24 ),
        mA31( a31 ), mA32( a32 ), mA33( a33 ), mA34( a34 ),
        mA41( a41 ), mA42( a42 ), mA43( a43 ), mA44( a44 )
      {}

      float mA11, mA12, mA13, mA14,
                mA21, mA22, mA23, mA24,
                mA31, mA32, mA33, mA34,
                mA41, mA42, mA43, mA44;
    };

    struct ExportBoundingBox
    {
      ExportBoundingBox( double Xmin, double Ymin, double Zmin,
                         double Xmax, double Ymax, double Zmax )
        : mXmin( Xmin ), mYmin( Ymin ), mZmin( Zmin ),
        mXmax( Xmax ), mYmax( Ymax ), mZmax( Zmax )
      {}

      double mXmin, mYmin, mZmin, mXmax, mYmax, mZmax;
    };


    struct ExportCoordinate3d
    {
      ExportCoordinate3d(double X, double Y, double Z )
        : mX( X ), mY( Y ), mZ( Z )
      {}

      double mX, mY, mZ;
    };

    struct ExportRGBAColor
    {
      ExportRGBAColor( double R, double G, double B, double A )
        : mR( R ), mG( G ), mB( B ), mA( A )
      {}

      double mR, mG, mB, mA;
    };

    struct SetUpsToUnity
    {
    public:
     
      static const std::tuple<int, int, int> coordinateSystemAdaptionOrder;

      static const std::tuple<int, int, int> coordinateSystemAdaptionOrderSign;

      static const int unityFirstIndexNumber;

      static const double lengthDimensionScale;
      
      static const ExportRGBAColor defaultColor;
    };
  }
}

#endif // !_VENTUS_Export_SetUpsToUnity_h_