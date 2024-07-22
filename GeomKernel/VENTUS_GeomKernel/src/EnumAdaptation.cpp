#include "EnumAdaptation.h"

namespace ventus
{
  ResultFileLoading EnumAdaptation::
    adaptReadStatus(const IFSelect_ReturnStatus iFSelect_ReturnStatus )
  {
    switch ( iFSelect_ReturnStatus )
    {
		case(IFSelect_RetDone):
			return CREATE_SUCCESSFUL;
	 	case(IFSelect_RetVoid):
	    	return LOADING_EMPTY;
		case(IFSelect_RetError):
			return OCC_ERROR;
		case(IFSelect_RetFail): 
			return OCC_ERROR;
		case(IFSelect_RetStop):
			return OCC_ERROR;
		default:
			return OCC_ERROR;
    }
  }

 
}