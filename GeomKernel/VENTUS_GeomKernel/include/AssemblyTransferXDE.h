#ifndef _OCCtoEdGe_AssemblyTransferXDE_h_
#define _OCCtoEdGe_AssemblyTransferXDE_h_

// PROJECT INCLUDES
#include "AssemblyTransferXDESetup.h"

// OCC_ADAPTER INCLUDES
#include "AssemblyRequester.h"
#include "AssemblyLabel.h"
#include "BRepType.h"
#include "AssemblyIterator.h"
#include "TriangulationSetup.h"

//Module Assembly Includes
#include "MeshLists.h"

namespace ventus
{
    class ModelObject;
    class ModelProduct;
    class ModelPart;
    class ModelBase;
    class Color;

  class AssemblyTransferXDE
  {
  public:
    
    AssemblyTransferXDE( const AssemblyRequester&,
                         const TriangulationSetup setup = TriangulationSetup());
       
    ModelObject* transfer();
   
    void triangulate( const AssemblyLabel& );

    MeshLists* transferMesh( const AssemblyLabel& );

  private:

    AssemblyRequester mAssemblyRequester;

    TriangulationSetup mSetup;
    
    ModelObject* transferModelObject(AssemblyIterator&);

    ModelProduct* transferModelProduct( const AssemblyLabel&);

    ModelPart* transferModelPart( const AssemblyLabel&);

    AssemblyIterator requestSubAssemblyIterator( const AssemblyLabel& );

    const bool modelProductTransferCondition( const AssemblyLabel& );

    const bool modelPartTransferNecessaryCondition( const AssemblyLabel& );

    const bool modelPartTransferCondition( const AssemblyLabel& );

    inline void transferModelBaseAttributes(ModelBase*, 
                                      const AssemblyLabel& );

    void transferTransformation( ModelBase*, 
                                 const AssemblyLabel& );

    void transferName( ModelBase*,
                       const AssemblyLabel& );
    
    void transferBoundingBox( ModelBase*,
                              const AssemblyLabel& );
    
    void transferColor( ModelBase*,
                        const AssemblyLabel& );
    
    void transferAssemblyLabel( ModelBase*,
                                const AssemblyLabel& );
  
    void triangulateSimpleTopLevelAssemblyLabels();
      
    //TODO: void transferBRep();
    
    static void colorToSubModels( ModelObject* );
    
    static void colorToSubModels( Color* parentColor,
                                  ModelProduct*);

    static void colorToSubModels( Color* parentColor,
                                  ModelPart* );

  };

  void AssemblyTransferXDE::
    transferModelBaseAttributes( ModelBase* ModelBaseEdGe,
                                 const AssemblyLabel& labelOCCAdapter )
  {
    if ( nullptr == ModelBaseEdGe )
    {
      return;
    }

    transferTransformation( ModelBaseEdGe, 
                            labelOCCAdapter );
    transferName( ModelBaseEdGe, 
                  labelOCCAdapter );

    transferBoundingBox( ModelBaseEdGe, 
                         labelOCCAdapter  );

    transferColor( ModelBaseEdGe,
                   labelOCCAdapter );

    transferAssemblyLabel( ModelBaseEdGe,
                           labelOCCAdapter );
  }

}

#endif // !_OCCtoEdGe_AssemblyTransferXDE_h_

