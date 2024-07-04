#ifndef _VENTUS_Export_LODUtilities_h_
#define _VENTUS_Export_LODUtilities_h_

namespace ventus
{
  class LODUtilities
  {
  public:

    static const double getLODLinearDeflectionFactor( const int i );

    static const double getLODAngleDeflection( const int i );
    
    static const int getDefaultLOD();
  };

}
#endif // !_VENTUS_Export_LODUtilities_h_
