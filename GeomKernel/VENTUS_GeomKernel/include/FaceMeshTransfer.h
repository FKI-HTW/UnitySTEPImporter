#ifndef _OCCtoEdGe_FaceMeshTransfer_h_
#define _OCCtoEdGe_FaceMeshTransfer_h_

// OCC ADAPTER INCLUDES
#include "BRepFaceLabel.h"
#include "SubFaceBRepIterator.h"
#include "MeshContainer.h"

// Module Assembly INCLUDES
#include "MeshLists.h"

namespace ventus
{
  class FaceMeshTransfer
  {
  public:

    FaceMeshTransfer( const BRepFaceLabel&,
                      MeshLists* meshDataEdGe = nullptr);
      
     bool existMeshData( void);

    // if don't set meshDataEdGe in Constructor 
    // then use setMeshDataEdGe method before 
    // using method transfer
    void transfer( void );
    
    inline void setMeshDataEdGe( MeshLists* );

  private:

    FaceMeshTransfer();

    MeshContainer mMeshContainerOCCAdapter;
    const bool mIsFaceReversed;
    MeshLists* mMeshDataEdGe;

    void transferCoordinates();
    void transferTriangles( const int indexShift );
    static inline const int shiftIndex( const int index,
                                        const int shift );
  };

  const int FaceMeshTransfer::
    shiftIndex( const int index,
                const int shift )
  {
    return index + shift;
  }
  
  void FaceMeshTransfer::
    setMeshDataEdGe( MeshLists* meshDataEdGe)
  {
    mMeshDataEdGe = meshDataEdGe ;
  }
}

#endif
