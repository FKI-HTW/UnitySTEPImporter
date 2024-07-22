#ifndef _VENTUS_Export_edGeToExportTransfer_MeshAttributesToExport_h_
#define _VENTUS_Export_edGeToExportTransfer_MeshAttributesToExport_h_

// PROJECT INCLUDES
#include "SetUpsToUnity.h"
#include "gp_Pnt.hxx"

namespace ventus
{
  //class Point3d;
  class Color;
  namespace Export
  {
      class MeshAttributesToExport
      {
      public:

        static const ExportCoordinate3d
          edGeToExportTransfer( const gp_Pnt& ); 
                
        static const ExportRGBAColor
          edGeToExportTransfer( const Color& );
      };
  }
}

#endif // !_VENTUS_Export_edGeToExportTransfer_MeshAttributesToExport_h_
