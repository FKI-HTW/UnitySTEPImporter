#ifndef _VENTUS_EnumAdaptation_h_
#define _VENTUS_EnumAdaptation_h_

// PROJECT INCLUDES
#include "SetUpsLoader.h"

// OCC ADAPTER INCLUDES
#include "IFSelect_ReturnStatus.hxx"

namespace ventus
{
  struct EnumAdaptation
  {
  	static ResultFileLoading adaptReadStatus( const IFSelect_ReturnStatus iFSelect_ReturnStatus );
  };
}

#endif 