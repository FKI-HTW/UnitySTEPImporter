#include "LODUtilities.h"

// PROJECT INCLUDEs
#include "SetUpsLOD.h"

namespace ventus
{
  const double LODUtilities::
    getLODLinearDeflectionFactor( const int i )
  {
    if ( i < 1 || i> 5 ) return -1.;

    const size_t iTuple = i - 1;
    switch ( iTuple )
    {
    case 0:
      return std::get<0>( SetUpsLOD::LODLinearDeflectionFactor );
    case 1:
      return std::get<1>( SetUpsLOD::LODLinearDeflectionFactor );
    case 2:
      return std::get<2>( SetUpsLOD::LODLinearDeflectionFactor );
    case 3:
      return std::get<3>( SetUpsLOD::LODLinearDeflectionFactor );
    case 4:
      return std::get<4>( SetUpsLOD::LODLinearDeflectionFactor );
    }
    return -1.;
  }

  const double LODUtilities::
    getLODAngleDeflection( const int i )
  {
    if ( i < 1 || i > 5 ) return -1.;

    const size_t iTuple = i - 1;
    switch ( iTuple )
    {
    case 0:
      return std::get<0>( SetUpsLOD::LODAngleDeflection );
    case 1:
      return std::get<1>( SetUpsLOD::LODAngleDeflection );
    case 2:
      return std::get<2>( SetUpsLOD::LODAngleDeflection );
    case 3:
      return std::get<3>( SetUpsLOD::LODAngleDeflection );
    case 4:
      return std::get<4>( SetUpsLOD::LODAngleDeflection );
    }
    return -1.;
  }

  const int LODUtilities::
    getDefaultLOD()
  {
    return SetUpsLOD::defaultLOD;
  }
}