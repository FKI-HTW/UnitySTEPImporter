#ifndef _VENTUS_Export_SetUpsLOD_h_
#define _VENTUS_Export_SetUpsLOD_h_

// SYSTEM INCLUDES
#include <tuple>

namespace ventus
{
  struct SetUpsLOD
  {
    static std::tuple<double, double, double, double, double> LODLinearDeflectionFactor;

    static std::tuple<double, double, double, double, double> LODAngleDeflection;

    static const int defaultLOD;
  };

}

#endif // !_VENTUS_Export_SetUpsLOD_h_