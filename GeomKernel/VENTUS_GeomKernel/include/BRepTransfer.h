#ifndef _OCCtoEdGe_BRepTransfer_h_
#define _OCCtoEdGe_BRepTransfer_h_

// OCC ADAPTER INCLUDE
#include "BRepLabel.h"
#include "TriangulationSetup.h"
#include "SubFaceBRepIterator.h"

// OCC INCLUDES
#include "Bnd_Box.hxx"

// Module Assembly Includes
#include "ModelMesh.h"


namespace ventus
{
  class BRepTransfer
  {
  public:

    static const bool BRepLabelMaybeHaveFaces( const BRepLabel& );
    
    static const Bnd_Box transferBoundingBox( const BRepLabel& );
    
    static void triangulate( const TriangulationSetup,
                            const BRepLabel& );

    static ModelMesh* transferMesh( const BRepLabel& );
    
  private:

    static const TriangulationSetup
      toOCCAdapterTriangulationSetup( const TriangulationSetup );

    static void transferMeshFromSubBRepLabels(ModelMesh*&,
                                               SubFaceBRepIterator& );
    
    static const bool existASubFace(SubFaceBRepIterator& );

    static void initMeshDataEdGe(ModelMesh*& );
  };
  
}

#endif 
