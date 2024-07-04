#include "AssemblyTransferXDE.h"

//SYSTEM INCLUDES
#include <vector>

// PROJECT INCLUDES
//#include "OCCtoEdGe_BRepTransfer.h"
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
#include "BRepTransfer.h"

namespace ventus
{
  AssemblyTransferXDE::
    AssemblyTransferXDE( const AssemblyRequester& AssemblyRequester,
                         const TriangulationSetup setup )
    : mAssemblyRequester( AssemblyRequester ),
      mSetup( setup )
  {}

  ModelObject* AssemblyTransferXDE::
    transfer( void )
  {
    ModelObject* modelobject = nullptr;
    triangulateSimpleTopLevelAssemblyLabels();
    
    AssemblyIterator freeTopLevelAssemblyIterator
      = mAssemblyRequester.requestFreeTopLevelAssemblyIterator();
    
    const int containerSize = freeTopLevelAssemblyIterator.Size();
    const AssemblyLabel firstAssemblyLabel
      = freeTopLevelAssemblyIterator.Value(OCCConstants::firstIndexInContainer );
    if ( modelProductTransferCondition( firstAssemblyLabel ) && containerSize == 1 )
    {
      AssemblyIterator AssemblyIterator
        = mAssemblyRequester.requestSubAssemblyIterator( firstAssemblyLabel );
      
      modelobject = transferModelObject( AssemblyIterator );
      transferModelBaseAttributes( modelobject, firstAssemblyLabel);
     }else if (modelPartTransferNecessaryCondition( firstAssemblyLabel ) )
    {
      modelobject = transferModelObject( freeTopLevelAssemblyIterator );
      transferModelBaseAttributes( modelobject, firstAssemblyLabel);
     }
     else if ( containerSize > 1 ) // model with more then one free top level assembly label are not found, 
                                   // but you can create it (see Steglitz.stp)
     {
       modelobject = transferModelObject( freeTopLevelAssemblyIterator );
       // no transfer of ModelBaseAttribute (from what???)
     }
   
    colorToSubModels( modelobject );

    return modelobject;
  }

  void AssemblyTransferXDE::
    triangulate( const AssemblyLabel& AssemblyLabel )
  {
      BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( AssemblyLabel );

    BRepTransfer::triangulate( mSetup.mTriangulationSetup, bRepLabelOCCAdapter );
  }

  MeshLists* AssemblyTransferXDE::
    transferMesh( const AssemblyLabel& AssemblyLabel )
  {
    if ( !mSetup.transferMesh )
    {
      return nullptr;
    }
    const ventus::AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( AssemblyLabel );

    const BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );
    return BRepTransfer::transferMesh( bRepLabelOCCAdapter );
  }

  ModelObject* AssemblyTransferXDE::
    transferModelObject(AssemblyIterator& AssemblyIterator )
  {
    ModelObject* pReturnObjectEdGe = nullptr;

    for ( AssemblyIterator.Init();
         AssemblyIterator.More();
         AssemblyIterator.Next() )
    {
      const AssemblyLabel currentLabelOCCAdapter 
        = AssemblyIterator.Value();
            
      if ( modelProductTransferCondition( currentLabelOCCAdapter ))
      {
        ModelProduct* pCurrentProductEdGe
          = transferModelProduct( currentLabelOCCAdapter );

        if ( nullptr != pCurrentProductEdGe )
        {
          if ( nullptr == pReturnObjectEdGe )
          {
            pReturnObjectEdGe = new ModelObject();
          }
          pReturnObjectEdGe->push_backModelproduct( pCurrentProductEdGe );
        }
      }
      else if (modelPartTransferNecessaryCondition( currentLabelOCCAdapter ) )
      {
        ModelPart* pCurrentPartEdGe
          = transferModelPart( currentLabelOCCAdapter );
        if ( nullptr != pCurrentPartEdGe )
        {
          if ( nullptr == pReturnObjectEdGe )
          {
            pReturnObjectEdGe = new ModelObject();
          }
          pReturnObjectEdGe->push_backModelpart( pCurrentPartEdGe );
        }
      }
    }
    
    return pReturnObjectEdGe;
  }
  
  ModelProduct* AssemblyTransferXDE::
    transferModelProduct(const AssemblyLabel& labelOCCAdapter )
  {
    ModelProduct* pReturnProductEdGe = nullptr;

    AssemblyIterator AssemblyIterator
      = requestSubAssemblyIterator( labelOCCAdapter );

    for ( AssemblyIterator.Init();
          AssemblyIterator.More();
          AssemblyIterator.Next() )
    {
      const AssemblyLabel currentLabelOCCAdapter
        = AssemblyIterator.Value();

      if ( modelProductTransferCondition( currentLabelOCCAdapter ) )
      {
        ModelProduct* pCurrentProductEdGe
          = transferModelProduct( currentLabelOCCAdapter );
        if ( nullptr != pCurrentProductEdGe )
        {
          if ( nullptr == pReturnProductEdGe )
          {
            pReturnProductEdGe = new ModelProduct;
          }
          pReturnProductEdGe->push_backModelproduct( pCurrentProductEdGe );
        }
      }
      else if (modelPartTransferNecessaryCondition( currentLabelOCCAdapter ) )
      {
        ModelPart* pCurrentPartEdGe
          = transferModelPart( currentLabelOCCAdapter );
        if ( nullptr != pCurrentPartEdGe )
        {
          if ( nullptr == pReturnProductEdGe )
          {
            pReturnProductEdGe = new ModelProduct();
          }
          pReturnProductEdGe->push_backModelpart( pCurrentPartEdGe );
        }
      }
    }

    transferModelBaseAttributes( pReturnProductEdGe, labelOCCAdapter );
    return pReturnProductEdGe;
  }
  
  ModelPart* AssemblyTransferXDE::
    transferModelPart( const AssemblyLabel& labelOCCAdapter )
  {
    ModelPart* pReturnPartEdGe = nullptr;
    
    if ( modelPartTransferCondition( labelOCCAdapter ) )
    {
      pReturnPartEdGe = new ModelPart();
       
      MeshLists* mesh = transferMesh( labelOCCAdapter );
      pReturnPartEdGe->getModelmesh()->setMesh( mesh );
      
      transferModelBaseAttributes( pReturnPartEdGe, labelOCCAdapter );
    }
    
    return pReturnPartEdGe;
  }

  AssemblyIterator AssemblyTransferXDE::
    requestSubAssemblyIterator( const AssemblyLabel& labelOCCAdapter )
  {
     const AssemblyLabel nonReferenceAssemblyLabel
       = mAssemblyRequester.requestNonReferrenceAssemblyLabel( labelOCCAdapter );
     return mAssemblyRequester.requestSubAssemblyIterator( nonReferenceAssemblyLabel );
  }

  const bool AssemblyTransferXDE::
      modelProductTransferCondition( const AssemblyLabel& labelOCCAdapter )
  {
    const AssemblyLabel nonReferenceAssemblyLabel
    = mAssemblyRequester.requestNonReferrenceAssemblyLabel(labelOCCAdapter);

    return mAssemblyRequester.existSubAssemblyLabel( nonReferenceAssemblyLabel );
  }

  const bool AssemblyTransferXDE::
     modelPartTransferNecessaryCondition( const AssemblyLabel& AssemblyLabel )
  {
    const ventus::AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( AssemblyLabel );

    const BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );
   
    return BRepTransfer::BRepLabelMaybeHaveFaces( bRepLabelOCCAdapter );
  }

  const bool AssemblyTransferXDE::
    modelPartTransferCondition( const AssemblyLabel& AssemblyLabel )
  {
    const ventus::AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel(AssemblyLabel );

    const BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );
    
    const bool haveBRepData = mAssemblyRequester.haveBRepData( nonReferenceAssemblyLabel );
    const bool noModelproductCondition = !modelProductTransferCondition( AssemblyLabel );
    const bool existSubFace = BRepRequester::existASubFace( bRepLabelOCCAdapter );

    return haveBRepData && noModelproductCondition && existSubFace;

  }
  
  void AssemblyTransferXDE::
    triangulateSimpleTopLevelAssemblyLabels()
  {
    if ( !mSetup.transferMesh )
    {
      return;
    }

    AssemblyIterator topLevelAssemblyIterator
      = mAssemblyRequester.requestTopLevelAssemblyIterator();

    for ( topLevelAssemblyIterator.Init();
      topLevelAssemblyIterator.More();
      topLevelAssemblyIterator.Next() )
    {
      const AssemblyLabel AssemblyLabel
        = topLevelAssemblyIterator.Value();

      if ( modelPartTransferCondition(AssemblyLabel ) )
      {
        triangulate(AssemblyLabel );
      }
    }
  }

  void AssemblyTransferXDE::
    transferTransformation( ModelBase* ModelBase,
                            const AssemblyLabel& labelOCCAdapter)
  {
    if ( !mSetup.transferTransformation )
    {
      return;
    }

    const TransformationMatrixArray transformationArray
      = mAssemblyRequester.requestTransformationMatrix( labelOCCAdapter );
    
    const TransformationMatrix* matrixEdGe = new TransformationMatrixEdGe(
      transformationArray[0], transformationArray[1], transformationArray[2], transformationArray[3],
      transformationArray[4], transformationArray[5], transformationArray[6], transformationArray[7],
      transformationArray[8], transformationArray[9], transformationArray[10], transformationArray[11],
      0., 0., 0., 1.
    );
   
    ModelBase->getTransformation()->setTransformationMatrix( *matrixEdGe );  
  }

  void AssemblyTransferXDE::
    transferName( ModelBase* ModelBase,
                  const AssemblyLabel& labelOCCAdapter)
  {
    if ( !mSetup.transferName )
    {
      return;
    }

    const AssemblyLabel nonReferenceLabelOCCAdapter
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( labelOCCAdapter );


    ModelBase->setName(mAssemblyRequester.requestName( nonReferenceLabelOCCAdapter ));
  }
 
  void AssemblyTransferXDE::
    transferBoundingBox( ModelBase* ModelBase,
                         const AssemblyLabel& AssemblyLabel )
  {
    if ( !mSetup.transferBoundingBox )
    {
      return;
    }

    const ventus::AssemblyLabel nonReferenceAssemblyLabel
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( AssemblyLabel );

    const BRepLabel bRepLabelOCCAdapter
      = mAssemblyRequester.requestBRepLabel( nonReferenceAssemblyLabel );

    ModelBase->setBoundingBox( BRepTransfer::transferBoundingBox( bRepLabelOCCAdapter ) );
  }

  void AssemblyTransferXDE::
    transferColor( ModelBase* ModelBase,
                   const AssemblyLabel& labelOCCAdapter)
  {
    if ( !mSetup.transferColor )
    {
      return;
    }
    const AssemblyLabel nonReferenceLabelOCCAdapter
      = mAssemblyRequester.requestNonReferrenceAssemblyLabel( labelOCCAdapter );

    ColorArray colorArrayOCCAdapter
      = mAssemblyRequester.requestColor( nonReferenceLabelOCCAdapter );

    if (ColorUtilities::isEmpty( colorArrayOCCAdapter ) )
    {
		if (modelProductTransferCondition(labelOCCAdapter))
		{
			colorArrayOCCAdapter = ColorUtilities::getDefaultColorArray();
		}
		else
		{
			colorArrayOCCAdapter
				= mAssemblyRequester.requestColorFromBRep(labelOCCAdapter);
		}
    }

    if (ColorUtilities::isEmpty( colorArrayOCCAdapter ) )
    {
      return;
    }

    ModelBase->setColor( new Color( colorArrayOCCAdapter[0],
                                            colorArrayOCCAdapter[1], 
                                            colorArrayOCCAdapter[2],
                                            1));
  }
  
  void AssemblyTransferXDE::
    transferAssemblyLabel( ModelBase* ModelBase,
                           const AssemblyLabel& labelOCCAdapter )
  {
    if ( !mSetup.saveAssemblyLabel)
    {
      return;
    }
     ModelBase->setThirdPartyItem( new AssemblyLabel( labelOCCAdapter ));
  }

  void AssemblyTransferXDE::
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
  
  void AssemblyTransferXDE::
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

  void AssemblyTransferXDE::
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