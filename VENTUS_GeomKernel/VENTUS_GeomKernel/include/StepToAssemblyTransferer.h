#ifndef _StepToAssemblyTransferer_h_
#define _StepToAssemblyTransferer_h_

// MODULE ASSAMBLY
#include "ModelObject.h"
#include "ModelProduct.h"

// OCC_ADAPTER INCLUDES
#include "AssemblyRequester.h"
#include "AssemblyLabel.h"
#include "BRepType.h"
#include "AssemblyIterator.h"
#include "TriangulationSetup.h"


namespace ventus {

    class ModelObject;
    class ModelProduct;
    class ModelPart;
    class ModelBase;
    class Color;

  class StepToAssemblyTransferer {
  public:
    
    StepToAssemblyTransferer( const AssemblyRequester& assemblyRequester,
                         const TriangulationSetup setup = TriangulationSetup());
       
    ModelObject_sptr transfer();
   
    void triangulate( const AssemblyLabel& assemblyLabel );

   // MeshDataEdGe* transferMesh( const ventus::AssemblyLabel& );

  private:

    AssemblyRequester mAssemblyRequester;

    TriangulationSetup mSetup;
    
    ModelObject_sptr transferModelObject( AssemblyIterator& iterator);

    ModelProduct_sptr transferModelProduct( const AssemblyLabel& assemblyLabel);

    ModelPart_sptr transferModelPart( const AssemblyLabel& assemblyLabel );

    AssemblyIterator requestSubAssemblyIterator( const AssemblyLabel& assemblyLabel );

    const bool modelProductTransferCondition( const AssemblyLabel& assemblyLabel );

    const bool modelPartTransferNecessaryCondition( const AssemblyLabel& assemblyLabel );

    const bool modelPartTransferCondition( const AssemblyLabel& assemblyLabel );

    inline void transferModelBaseAttributes( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel );

    void transferTransformation( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel );

    void transferName( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel );
    
    void transferBoundingBox( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel );
    
    void transferColor( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel );
    
    void transferAssemblyLabel( ModelBase_sptr& modelBase, const AssemblyLabel& assemblyLabel );
  
    //void triangulateSimpleTopLevelAssemblyLabels();
      
    //TODO: void transferBRep();
    
    static void colorToSubModels( ModelObject* modelObject );
    
    static void colorToSubModels( Color* parentColor, ModelProduct* modelObject );

    static void colorToSubModels( Color* parentColor, ModelPart* modelObject );

  };

  void StepToAssemblyTransferer::transferModelBaseAttributes( ModelBase_sptr& modelBase, const ventus::AssemblyLabel& assemblyLabel) {
    if ( nullptr == modelBase ) {
      return;
    }
    transferTransformation( modelBase, assemblyLabel );
    transferName( modelBase, assemblyLabel );
    transferBoundingBox( modelBase, assemblyLabel );
    transferColor( modelBase, assemblyLabel );
    transferAssemblyLabel( modelBase, assemblyLabel );
  }

}

#endif // !_StepToAssemblyTransferer_h_

