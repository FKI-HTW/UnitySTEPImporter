// STRUCT IMPLEMENTED
#include "SetUpsToUnity.h"

namespace ventus
{
  namespace Export
  {
	 enum OccCordinate { OCCX = 1, OCCY, OCCZ };
	 const std::tuple<int, int, int> SetUpsToUnity::
		 coordinateSystemAdaptionOrder = { OCCX, OCCZ, OCCY };

    const std::tuple<int, int, int> SetUpsToUnity::
      coordinateSystemAdaptionOrderSign = { -1, 1, -1 };

    const int SetUpsToUnity::
      unityFirstIndexNumber = 0;
    
	const double SetUpsToUnity::
//      lengthDimensionScale = 1e-3;
//  for city models we use temporarily:
		lengthDimensionScale = 1.0;

    const ExportRGBAColor SetUpsToUnity::defaultColor 
      = ExportRGBAColor( 0.5, 0.5, 0.5, 1. );
  }
}