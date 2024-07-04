#ifndef _VENTUS_Export_ModelAttributesToExport_h_
#define _VENTUS_Export_ModelAttributesToExport_h_

//SYSTEM INCLUDES
#include <string>

// PROJECT INCLUDES
#include "SetUpsToUnity.h"
#include "gp_Trsf.hxx"
#include "Bnd_Box.hxx"
namespace ventus
{
  namespace Export
  {
    class ModelAttributesToExport
    {
    public:

      static const ExportTransformation4d
        transfer( const gp_Trsf& crTransformationMatrix );

      static const ExportBoundingBox
        transfer( const Bnd_Box& );

     /* static char*
        transfer( const string cName );*/
    };
  }
}
#endif