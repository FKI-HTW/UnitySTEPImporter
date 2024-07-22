#include "StepToAssemblyTransferer.h"

//SYSTEM INCLUDES
#include <vector>

// MODULE ASSAMBLY
#include "ModelBase.h"
#include "ModelObject.h"
#include "ModelProduct.h"
#include "Color.h"
#include "ModelPart.h"

// OCC ADAPTER INCLUDES
#include "BRepRequester.h"
#include "BRepLabel.h"
#include "OCCConstants.h"
#include "ColorArray.h"
#include "TransformationMatrixArray.h"
#include "BoundingBoxArray.h"
#include "ColorUtilities.h"
#include "StepTools.h"
#include "StandardOCCTypeConverter.h"

namespace ventus {

  StepToAssemblyTransferer::StepToAssemblyTransferer( 
      const AssemblyRequester& AssemblyRequester, 
      const TriangulationSetup setup ) : 
      mAssemblyRequester( AssemblyRequester ), 
      mSetup( setup ) 
  {}

  ModelObject_sptr StepToAssemblyTransferer::transfer( void ) {
    ModelObject_sptr modelobject = ModelObject::Create();

    return modelobject;
  }

  void StepToAssemblyTransferer::
    triangulate( const AssemblyLabel& AssemblyLabel )
  {
      BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( AssemblyLabel );

    //BRepTransfer::triangulate( mSetup.mTriangulationSetup, bRepLabelOCCAdapter );
  }

  //MeshDataEdGe* StepToAssemblyTransferer::
  //  transferMesh( const AssemblyLabel& AssemblyLabel )
  //{
  //  if ( !mSetup.transferMesh )
  //  {
  //    return nullptr;
  //  }
  //  const AssemblyLabel nonReferenceAssemblyLabel
  //    = mAssemblyRequester.requestNonReferrenceAssemblyLabel( AssemblyLabel );
  //
  //  const BRepLabel bRepLabelOCCAdapter
  //    = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );
  //  return BRepTransfer::transferMesh( bRepLabelOCCAdapter );
  //}

<<<<<<< Updated upstream
  ModelObject_sptr StepToAssemblyTransferer::transferModelObject( ModelObject_sptr& modelObject, AssemblyIterator& iterator ) {
    // Fullfilling of ModelObject has three steps:
    // 1. Stetting mspRootModel
    // 2. Identify all parts for mspParts
    // 3. Identify all products for mspProducts
    AssemblyIterator freeTopLevelAssemblyIterator = mAssemblyRequester.requestFreeTopLevelAssemblyIterator();
    const AssemblyLabel firstAssemblyLabel = freeTopLevelAssemblyIterator.Value(OCCConstants::firstIndexInContainer);

    const ModelBase_sptr& rootModel = ModelProduct::Create();
    // ...
    modelObject->setRootModel(rootModel);

=======
  ModelObject_sptr StepToAssemblyTransferer::transferModelObject( AssemblyIterator& iterator ) {
    ModelObject_sptr pReturnObject = ModelObject::Create();
>>>>>>> Stashed changes
    for ( iterator.Init(); iterator.More(); iterator.Next() ) {
        const AssemblyLabel currentLabelOCCAdapter = iterator.Value();            
        if ( modelProductTransferCondition( currentLabelOCCAdapter )) {
            // !!! -> hier gleich weiter
            ModelProduct_sptr pCurrentProduct = transferModelProduct( currentLabelOCCAdapter );
            if ( nullptr != pCurrentProduct ) {
                if ( nullptr == pReturnObject ) {
                    pReturnObject = new ModelObject();
                }
                pReturnObject->push_backModelproduct( pCurrentProduct );
            }
        } else if (modelPartTransferNecessaryCondition( currentLabelOCCAdapter ) ) {
            ModelPart* pCurrentPartEdGe = transferModelPart( currentLabelOCCAdapter );
            if ( nullptr != pCurrentPartEdGe ) {
                if ( nullptr == pReturnObject ) {
                    pReturnObject = new ModelObject();
                }
                pReturnObject->push_backModelpart( pCurrentPartEdGe );
            }
        }
    }
    
    return pReturnObject;
  }
  
  ModelProduct_sptr StepToAssemblyTransferer::transferModelProduct( const AssemblyLabel& labelOCCAdapter ) {
    ModelProduct_sptr pReturnProduct = ModelProduct::Create();
    AssemblyIterator iterator = requestSubAssemblyIterator( labelOCCAdapter );
    for (iterator.Init(); iterator.More(); iterator.Next() ) {
      const AssemblyLabel currentLabel = iterator.Value();
      if ( modelProductTransferCondition( currentLabel ) ) {
        ModelProduct_sptr pCurrentProduct = transferModelProduct( currentLabel );
        if ( nullptr != pCurrentProduct ) {
          if ( nullptr == pReturnProduct ) {
            pReturnProduct = ModelProduct::Create();
          }
          pReturnProduct->push_backModelproduct( pCurrentProduct );
        }
      }
      else if (modelPartTransferNecessaryCondition( currentLabel ) ) {
        ModelPart_sptr pCurrentPart = transferModelPart( currentLabel );
        if ( nullptr != pCurrentPart ) {
          if ( nullptr == pReturnProduct ) {
            pReturnProduct = ModelProduct::Create();
          }
          pReturnProduct->push_backModelpart( pCurrentPart );
        }
      }
    }
    transferModelBaseAttributes( pReturnProduct, labelOCCAdapter );
    return pReturnProduct;
  }
  
  ModelPart_sptr StepToAssemblyTransferer::transferModelPart( const AssemblyLabel& labelOCCAdapter )
  {
    ModelPart_sptr pReturnPart = nullptr;
    
    if ( modelPartTransferCondition( labelOCCAdapter ) )
    {
      pReturnPartEdGe = new ModelPart();
       
      MeshDataEdGe* mesh = transferMesh( labelOCCAdapter );
      pReturnPartEdGe->getModelmesh()->setMesh( mesh );
      
      transferModelBaseAttributes( pReturnPartEdGe, labelOCCAdapter );
    }
    
    return pReturnPart;
  }

   AssemblyIterator StepToAssemblyTransferer::
    requestSubAssemblyIterator( const AssemblyLabel& labelOCCAdapter )
  {
     const AssemblyLabel nonReferenceAssemblyLabel
       = mAssemblyRequester.requestNonReferrenceAssemblyLabel( labelOCCAdapter );
     return mAssemblyRequester.requestSubAssemblyIterator( nonReferenceAssemblyLabel );
  }

<<<<<<< Updated upstream
  const bool StepToAssemblyTransferer::modelProductTransferCondition( const AssemblyLabel& labelOCCAdapter ) {
=======
  const bool StepToAssemblyTransferer::
      modelProductTransferCondition( const AssemblyLabel& labelOCCAdapter ) {
>>>>>>> Stashed changes
    const AssemblyLabel nonReferenceAssemblyLabel
    = mAssemblyRequester.requestNonReferrenceAssemblyLabel(labelOCCAdapter);
    return mAssemblyRequester.existSubAssemblyLabel( nonReferenceAssemblyLabel );
  }

  const bool StepToAssemblyTransferer::
<<<<<<< Updated upstream
     modelPartTransferNecessaryCondition( const AssemblyLabel& assemblyLabel ) {
    const AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( assemblyLabel );
=======
     modelPartTransferNecessaryCondition( const AssemblyLabel& AssemblyLabel ) {
    const AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( AssemblyLabel );
>>>>>>> Stashed changes
    const BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );   
    return BRepTransfer::BRepLabelMaybeHaveFaces( bRepLabelOCCAdapter );
  }

  const bool StepToAssemblyTransferer::
    modelPartTransferCondition( const AssemblyLabel& AssemblyLabel )
  {
    const AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel(AssemblyLabel );

    const BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );
    
    const bool haveBRepData = mAssemblyRequester.haveBRepData( nonReferenceAssemblyLabel );
    const bool noModelproductCondition = !modelProductTransferCondition( AssemblyLabel );
    const bool existSubFace = BRepRequester::existASubFace( bRepLabelOCCAdapter );

    return haveBRepData && noModelproductCondition && existSubFace;

  }
  
  //void StepToAssemblyTransferer::triangulateSimpleTopLevelAssemblyLabels()
  //{
  //  if ( !mSetup.transferMesh )
  //  {
  //    return;
  //  }
  //
  //  AssemblyIterator topLevelAssemblyIterator
  //    = mAssemblyRequester.requestTopLevelAssemblyIterator();
  //
  //  for ( topLevelAssemblyIterator.Init();
  //    topLevelAssemblyIterator.More();
  //    topLevelAssemblyIterator.Next() )
  //  {
  //    const AssemblyLabel AssemblyLabel
  //      = topLevelAssemblyIterator.Value();
  //
  //    if ( modelPartTransferCondition(AssemblyLabel ) )
  //    {
  //      triangulate(AssemblyLabel );
  //    }
  //  }
  //}

  void StepToAssemblyTransferer::transferTransformation( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel ) {
    if (nullptr == modelBase) {
        return;
    }
    const TransformationMatrixArray tMatrix = mAssemblyRequester.requestTransformationMatrix( assemblyLabel );
    gp_Trsf& transformation = gp_Trsf();
    transformation.SetValues(tMatrix[0], tMatrix[1], tMatrix[2], tMatrix[3],
                             tMatrix[4], tMatrix[5], tMatrix[6], tMatrix[7],
                             tMatrix[8], tMatrix[9], tMatrix[10],tMatrix[12]);
    modelBase->setTransformation( transformation );  
  }

  void StepToAssemblyTransferer::transferName( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel ) {
    if (nullptr == modelBase) {
        return;
    }
    const AssemblyLabel nonReferenceLabelOCCAdapter = mAssemblyRequester.requestNonReferrenceAssemblyLabel( assemblyLabel );
    modelBase->setName(mAssemblyRequester.requestName( nonReferenceLabelOCCAdapter ));
  }
 
  void StepToAssemblyTransferer::transferBoundingBox( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel ) {
    if (nullptr == modelBase) {
        return;
    }
    const AssemblyLabel nonReferenceAssemblyLabel = mAssemblyRequester.requestNonReferrenceAssemblyLabel(assemblyLabel);
    const BRepLabel bRepLabel = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );
    const BoundingBoxArray boxArray = BRepRequester::requestBoundingBox(bRepLabel);
    Bnd_Box bndBox = modelBase->getBoundingBox();
    modelBase->setBoundingBox(bndBox);
  }

  void StepToAssemblyTransferer::transferColor( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel ) {
    if (nullptr == modelBase) {
        return;
    }
    if (modelBase->getType() == ModelType::PART) {
        // TODO : do test -> if it's working?
        ModelPart_sptr modelPart = dynamic_pointer_cast<ModelPart>(modelBase);
        const AssemblyLabel nonReferenceLabel = mAssemblyRequester.requestNonReferrenceAssemblyLabel(assemblyLabel);
        ColorArray colorArray = mAssemblyRequester.requestColor(nonReferenceLabel);
        if (ColorUtilities::isEmpty(colorArray)) {
            if (modelProductTransferCondition(assemblyLabel)) {
                colorArray = ColorUtilities::getDefaultColorArray();
            }
            else {
                colorArray = mAssemblyRequester.requestColorFromBRep(assemblyLabel);
            }
        }
        if (ColorUtilities::isEmpty(colorArray)) {
            return;
        }
        const GraphicInfo& graphicInfo = GraphicInfo(colorArray[0], colorArray[1], colorArray[2], 1);
        ModelMesh_sptr modelMesh = ModelMesh::Create();
        modelMesh.setGraphicInfo(graphicInfo);
        modelPart.setModelMesh(modelMesh);
    }
  }
  
  void StepToAssemblyTransferer::transferAssemblyLabel( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel) {
    if (nullptr == modelBase) {
        return;
    }
    // TODO ! Label is needed to read from OCC
    // modelBase->setThirdPartyItem( new AssemblyLabel( labelOCCAdapter ));
  }

  void StepToAssemblyTransferer::
    colorToSubModels( ModelObject* modelobjectEdGe)
  {
    Color* objectColor = modelobjectEdGe->getColor();
   
    for ( ModelProduct* product : modelobjectEdGe->getEdGeModelproductVector() )
    {
      colorToSubModels( objectColor, product );
    }

    for ( ModelPart* part : modelobjectEdGe->getEdGeModelpartVector() )
    {
      colorToSubModels( objectColor, part );
    }
  }
  
  void StepToAssemblyTransferer::
    colorToSubModels( Color* parentColor,
                      ModelProduct* Modelproduct )
  {
    Color* productColor = Modelproduct->getColor();
   
    if ( nullptr == productColor )
    {
      productColor = parentColor;
      Modelproduct->setColor( parentColor );
    }

    for ( ModelProduct* product : Modelproduct->getEdGeModelproductVector() )
    {
      colorToSubModels( productColor, product );
    }

    for ( ModelPart* part : Modelproduct->getEdGeModelpartVector() )
    {
      colorToSubModels( productColor, part );
    }
  }

  void StepToAssemblyTransferer::
    colorToSubModels( Color* parentColor,
                      ModelPart* ModelPart )
  {
    Color* partColor = ModelPart->getColor();
   
    if ( nullptr == partColor )
    {
      partColor = parentColor;
      ModelPart->setColor( parentColor );
    }

    ModelPart->getModelmesh()->getGraphicInfo()->setColor( partColor );
  }
}