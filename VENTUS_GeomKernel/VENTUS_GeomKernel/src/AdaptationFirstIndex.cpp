#include "AdaptationFirstIndex.h"

// PROJECT INCLUDES
#include "SetUpsToUnity.h"

// MODULE_ASSEMBLY INCLUDES

namespace ventus
{
  namespace Export
  {
    int AdaptationFirstIndex::
      edGeToUnityAdapt( const int edGe_index )
    {
      const int cEdGeFirstIndexNumber = 1;
      const int cUnityFirstIndexNumber = SetUpsToUnity::unityFirstIndexNumber;
      int indexshift = cEdGeFirstIndexNumber - cUnityFirstIndexNumber;
        
      return edGe_index - indexshift;
    }

  }
}