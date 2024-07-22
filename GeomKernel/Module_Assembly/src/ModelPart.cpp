#include "ModelPart.h"

namespace ventus {

//  size_t ModelPart::smCounterModelpart = 300000001;

//============================= LIFECYCLE ====================================

  ModelPart::ModelPart()
      : ModelBase(ModelType::PART)
  {
  }

  ModelPart::ModelPart(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name)
      : ModelBase(ModelType::PART, spParent, transformation, name)
  {
  }


  ModelPart::~ModelPart() { }


  ModelPart_sptr ModelPart::Create()
  {
      return make_shared<ModelPart>();
  }

  ModelPart_sptr ModelPart::Create(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name)
  {
      return make_shared<ModelPart>(spParent, transformation, name);
  }

//============================= OPERATORS ====================================

//============================= OPERATIONS ===================================

//============================= ACCESS     ===================================

//============================= INQUIRY    ===================================

} // ventus