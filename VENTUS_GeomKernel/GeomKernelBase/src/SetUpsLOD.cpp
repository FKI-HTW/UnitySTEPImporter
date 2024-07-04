// STRUCT IMPLEMENTED
#include "SetUpsLOD.h"

namespace ventus
{

  std::tuple<double, double, double, double, double>
    SetUpsLOD::LODLinearDeflectionFactor = { 1. ,2.5e-1, 2.e-2, 7.e-3, 5.e-4 };

  std::tuple<double, double, double, double, double>
    SetUpsLOD::LODAngleDeflection = { 5., 4., 2.8 , 0.9, 0.3 };

  const int SetUpsLOD::
    defaultLOD = 2;

}