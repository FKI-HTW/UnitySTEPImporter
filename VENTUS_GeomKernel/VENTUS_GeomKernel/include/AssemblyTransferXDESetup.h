#ifndef _OCCtoEdGe_AssemblyTransferXDESetup_h_
#define _OCCtoEdGe_AssemblyTransferXDESetup_h_

// SYSTEM INCLUDES
#include <ostream>
#include <string>

// PROJECT INCLUDES
#include "TriangulationSetup.h"

namespace ventus
{
  struct AssemblyTransferXDESetup
  {
    AssemblyTransferXDESetup();
    
    AssemblyTransferXDESetup( const TriangulationSetup);
    
    friend std::ostream & operator << ( std::ostream &,
                                        const AssemblyTransferXDESetup&);
    
    std::string print( void ) const;

    bool transferTransformation;
    bool transferName;
    bool transferBoundingBox;

    bool transferColor;
    bool transferMesh;
    TriangulationSetup mTriangulationSetup;

    bool transferBRep;
    
    bool saveAssemblyLabelOCCAdapter;// usefull for delay transfer (e.g. LOD in VENTUS)

  };

}

#endif // !_OCCtoEdGe_AssemblyTransferXDESetup_h_
