#include "FaceMeshTransfer.h"

// OCC ADAPTER INCLUDES
#include "BRepRequester.h"
#include "Triangulator.h"

#include <cassert>

namespace ventus
{
  FaceMeshTransfer::
    FaceMeshTransfer( const BRepFaceLabel& bRepFaceLabelOCCAdapter,
                      ModelMesh* meshDataEdGe)
    : mMeshContainerOCCAdapter( ventus::Triangulator::requestMesh( bRepFaceLabelOCCAdapter ) )
    , mIsFaceReversed( ventus::BRepRequester::isFaceReversed( bRepFaceLabelOCCAdapter ) )
    , mMeshDataEdGe( meshDataEdGe )
  {}

  bool FaceMeshTransfer::
    existMeshData( void )
  {
    return !mMeshContainerOCCAdapter.isEmpty();
  }

  void FaceMeshTransfer::
    transfer( void )
  {
    const int indexShift 
      = static_cast<int>(mMeshDataEdGe->getNumberOfTriangles());

    transferCoordinates();
    transferTriangles( indexShift );
  }

  void FaceMeshTransfer::
    transferCoordinates()
  {
      assert(false); //deprecated method
      /*
    MeshCoordinateIterator iteratorOCCAdapter
      = mMeshContainerOCCAdapter.mCoordinateIterator;

    for ( iteratorOCCAdapter.reInit();
          iteratorOCCAdapter.More();
          iteratorOCCAdapter.Next() )
      {
        const ventus::Coordinate coordinateOCCAdapter
          = iteratorOCCAdapter.Value();

        mMeshDataEdGe->push_backCoordinate3d(Poly_Triangle(coordinateOCCAdapter[0],
                                             coordinateOCCAdapter[1],
                                              coordinateOCCAdapter[2])); //keine direkte Methode vorhanden
      }
      */
  }

  void FaceMeshTransfer::
    transferTriangles( const int indexShift )
  {
      assert(false); //deprecated method
      /*
    MeshTriangleIterator iteratorOCCAdapter
      = mMeshContainerOCCAdapter.mTriangleIterator;
    
    for ( iteratorOCCAdapter.reInit();
          iteratorOCCAdapter.More();
          iteratorOCCAdapter.Next() )
    {
      const Triangle triangleOCCAdapter = iteratorOCCAdapter.Value();

      if ( mIsFaceReversed )
      {
        mMeshDataEdGe->push_back3Indices( shiftIndex( triangleOCCAdapter[0], indexShift ),
                                          shiftIndex( triangleOCCAdapter[1], indexShift ),
                                          shiftIndex( triangleOCCAdapter[2], indexShift ) ); //keine direkte Methode vorhanden
      }
      else
      {
        mMeshDataEdGe->push_back3Indices( shiftIndex( triangleOCCAdapter[1], indexShift ),
                                          shiftIndex( triangleOCCAdapter[0], indexShift ),
                                          shiftIndex( triangleOCCAdapter[2], indexShift ) ); //keine direkte Methode vorhanden
      }
    }
  }
  */
}
