#include "AssemblyTransferXDESetup.h"

namespace ventus
{
  AssemblyTransferXDESetup::
    AssemblyTransferXDESetup()
     : transferTransformation(true)
     , transferName(true)
     , transferBoundingBox(true)
     , transferColor(true)
     , transferMesh(false)
     , transferBRep(true)
     , saveAssemblyLabelOCCAdapter(true)
  {}

  AssemblyTransferXDESetup::
    AssemblyTransferXDESetup( const TriangulationSetup triangulationSetup )
    : transferTransformation( true )
    , transferName( true )
    , transferBoundingBox( true )
    , transferColor( true )
    , transferMesh( true )
    , mTriangulationSetup(triangulationSetup)
    , transferBRep( true )
    , saveAssemblyLabelOCCAdapter( true )
    {}
  
  std::ostream & operator << ( std::ostream & os,
    const AssemblyTransferXDESetup& crSetup )
  {
    if ( !&crSetup ) {
      os << "AssemblyTransferXDESetup is a null";
      return os;
    }

    os << crSetup.print( );
  }

  std::string AssemblyTransferXDESetup::
    print( void ) const
  {
    return std::string(
      "transferTransformation = "       + std::to_string( transferTransformation )
    + "\ntransferName = "               + std::to_string( transferName )
    + "\ntransferBoundingBox = "        + std::to_string( transferBoundingBox )
    + "\ntransferColor = "              + std::to_string( transferColor )
    + "\ntransferMesh = "               + std::to_string( transferMesh )
    + "\ntransferBRep = "               + std::to_string( transferBRep )
    + "\nsaveAssemblyLabelOCCAdapter = "+ std::to_string( saveAssemblyLabelOCCAdapter )

    );
  }
}