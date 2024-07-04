#ifndef ventus_sub_model_h_
#define ventus_sub_model_h_

// SYSTEM INCLUDES

// EDGE INCLUDES
//#include "Source.h"
//#include "libModeller.h"
//#include "enterexit.h"

// PROJECT INCLUDE
#include "ModelBase.h"

// USING STATEMENTS


// FORWARD REFERENCES

namespace ventus
{
  class //EDGE_ITEM
    SubModel: public ModelBase
  {
    //EDGE_HEADER( SubModel )
 

  public:
  
    /// Initialize runtime type-mechanism and counter. 
    //static void initClass( void );
// LIFECYCLE

    SubModel();

// OPERATORS

// OPERATIONS   

  protected:

    /// Destructor
    virtual ~SubModel();

  }; // SubModel

// INLINE METHODS

// EXTERNAL REFERENCES

  //STL_CONTAINERS( SubModel )

}

#endif // ventus_sub_model_h_