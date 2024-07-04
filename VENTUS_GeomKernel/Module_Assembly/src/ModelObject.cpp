#include "ModelObject.h"
#include "ModelProduct.h"

namespace ventus
{
  /////////////////////////////// STATIC ///////////////////////////////////////
  
  /////////////////////////////// PUBLIC ///////////////////////////////////////


  //============================= LIFECYCLE ====================================

  ModelObject::ModelObject() { }

  ModelObject::~ModelObject() { }

  ModelObject_sptr ModelObject::Create() {
      return make_shared<ModelObject>();
  }

  //============================= OPERATIONS ===================================

  void ModelObject::AddModel2Lists(const ModelBase_sptr& spModel) {
      if (ModelType::PART == spModel->getType()) {
          mspParts->push_back(spModel);
      } else if (ModelType::PRODUCT == spModel->getType()) {
          mspProducts->push_back(spModel);
          ModelProduct_sptr spProduct = dynamic_pointer_cast<ModelProduct>(spModel);
          ModelBaseVector_sptr spChildren = spProduct->getChildren();
          for (ModelBase_sptr& spChild : *spChildren)
              AddModel2Lists(spChild);
      }
  }

  //============================= ACCESS     ===================================
  
  //============================= OPERATORS ====================================

  //============================= OPERATIONS =================================== 

  //============================= INQUIRY    ===================================

  /////////////////////////////// PROTECTED  ///////////////////////////////////

  /////////////////////////////// PRIVATE    ///////////////////////////////////


} // ventus