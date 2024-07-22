#include "BRepTransfer.h"

// PROJECT INCLUDES
#include "FaceMeshTransfer.h"

// OCC ADAPTER INCLUDES
#include "BRepRequester.h"
#include "BoundingBoxArray.h"
#include "Triangulator.h"

namespace ventus
{
  const bool BRepTransfer::
    BRepLabelMaybeHaveFaces( const BRepLabel& bRepLabelOCCAdapter)
  {
    return BRepRequester::isBRepLabelACompound( bRepLabelOCCAdapter )
        || BRepRequester::isBRepLabelACompSolid( bRepLabelOCCAdapter )
        || BRepRequester::isBRepLabelASolid( bRepLabelOCCAdapter )
        || BRepRequester::isBRepLabelAShell( bRepLabelOCCAdapter )
        || BRepRequester::isBRepLabelAFace( bRepLabelOCCAdapter );
 }
  
  const Bnd_Box BRepTransfer::
    transferBoundingBox( const BRepLabel& bRepLabel )
  {
    BoundingBoxArray boxArrayOCCAdapter 
      = BRepRequester::requestBoundingBox( bRepLabel );

    Bnd_Box BoundingBox;
    BoundingBox.Update(boxArrayOCCAdapter[0],
        boxArrayOCCAdapter[1],
        boxArrayOCCAdapter[2],
        boxArrayOCCAdapter[3],
        boxArrayOCCAdapter[4],
        boxArrayOCCAdapter[5]);
  }

  void BRepTransfer::
    triangulate( const TriangulationSetup setup,
                 const BRepLabel& bRepLabelOCCAdapter )
  {
    const BoundingBoxArray boxOCCAdapter
      = BRepRequester::requestBoundingBox( bRepLabelOCCAdapter );

    const TriangulationSetup boxDependentSetup
      = Triangulator::adaptBoundingBoxDependency( toOCCAdapterTriangulationSetup( setup ),
        boxOCCAdapter );

    Triangulator::triangulate( boxDependentSetup,
                                         bRepLabelOCCAdapter );
  }

  const TriangulationSetup BRepTransfer::
    toOCCAdapterTriangulationSetup( const TriangulationSetup setup)
  {
    return TriangulationSetup( setup.mLinearDeflection,
                                         setup.mAngularDeflection);
  }

  ModelMesh* BRepTransfer::
    transferMesh( const BRepLabel&  bRepLabelOCCAdapter)
  {
    SubFaceBRepIterator subFaceBRepIteratorOCC
      = BRepRequester::requestAllSubFacesBRepIterator( bRepLabelOCCAdapter );

    if ( !existASubFace(subFaceBRepIteratorOCC) )
    {
      return nullptr;
    }
    ModelMesh* meshDataEdGe = nullptr;
    transferMeshFromSubBRepLabels( meshDataEdGe, subFaceBRepIteratorOCC);
   
    return meshDataEdGe;
  }

  const bool BRepTransfer::
    existASubFace( SubFaceBRepIterator& subFaceBRepIteratorOCCAdapter )
  {
    subFaceBRepIteratorOCCAdapter.ReInit();
    return subFaceBRepIteratorOCCAdapter.More();
  }
  
  void BRepTransfer::
    transferMeshFromSubBRepLabels(ModelMesh*& meshDataEdGe,
                                   SubFaceBRepIterator& faceIteratorOCCAdapter )
  {
    for ( faceIteratorOCCAdapter.ReInit();
          faceIteratorOCCAdapter.More();
          faceIteratorOCCAdapter.Next() )
    {
      FaceMeshTransfer faceMeshTransfer( faceIteratorOCCAdapter.Value()); 
      if ( faceMeshTransfer.existMeshData())
      {
        initMeshDataEdGe( meshDataEdGe );
        faceMeshTransfer.setMeshDataEdGe( meshDataEdGe );
        faceMeshTransfer.transfer();
      }
    }
  }
  
  void BRepTransfer::
      initMeshDataEdGe(ModelMesh*& meshDataEdGe)
  {
      assert(false); //deprecated method
      /*
    if ( nullptr == meshDataEdGe )
    {
      meshDataEdGe = new ModelMesh;
      meshDataEdGe->Create();
    }
  } 
  */
}