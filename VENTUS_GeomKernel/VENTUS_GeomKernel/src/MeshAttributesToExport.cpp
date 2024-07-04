#include "MeshAttributesToExport.h"

// EDGE_ASSEMBLY INCLUDES
#include "Color.h"

namespace ventus
{
  namespace Export
  {
    const ExportCoordinate3d MeshAttributesToExport::
      edGeToExportTransfer( const gp_Pnt& crPoint )
    {
      return ExportCoordinate3d( (float)crPoint.X(), 
                                 (float)crPoint.Y(), 
                                 (float)crPoint.Z() );
    }

    const ExportRGBAColor MeshAttributesToExport::
      edGeToExportTransfer( const Color& cpColor)
    {
       /*
      if ( nullptr == cpColor )
      {
        return SetUpsToUnity::defaultColor;
      }
      */
      return ExportRGBAColor( (float)cpColor.getR(), 
                              (float)cpColor.getG(), 
                              (float)cpColor.getB(), 
                              (float)cpColor.getA() );
    }
  }
}