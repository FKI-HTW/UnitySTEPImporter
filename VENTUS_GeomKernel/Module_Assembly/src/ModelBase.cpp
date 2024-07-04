#include "ModelBase.h"

namespace ventus
{

//============================= STATIC =======================================

#if defined (DEBUG_INSTANCE_COUNTER)
    size_t ModelBase::smInstanceCounter = 0; 
#endif // DEBUG_INSTANCE_COUNTER
    int ModelBase::smLastModelID = 0;

  ModelBase::ModelBase(ModelType type)
    : mModelType(type),
      mName("")
  { 
#if defined (DEBUG_INSTANCE_COUNTER)
      smInstanceCounter++;
#endif // DEBUG_INSTANCE_COUNTER
      mID = ++smLastModelID;
  }

  ModelBase::ModelBase(ModelType type, const ModelBase_sptr& spParent, const gp_Trsf& pTransformation, const string& name)
   : mModelType(type),
     mwpParent(spParent),
     mTransformation(pTransformation),
     mName (name)
  { 
#if defined (DEBUG_INSTANCE_COUNTER)
      smInstanceCounter++;
#endif // DEBUG_INSTANCE_COUNTER
      mID = ++smLastModelID;
  }

  ModelBase::~ModelBase() 
  { 
#if defined (DEBUG_INSTANCE_COUNTER)
      smInstanceCounter--;
#endif // DEBUG_INSTANCE_COUNTER
  }
 

  void ModelBase::extendBoundingBox(const Bnd_Box& boundingBox)
  {
      if (mBoundingBox.IsVoid())
      {
          mBoundingBox = boundingBox;
      }
      else
      {
          mBoundingBox.Add(boundingBox);
      }
  }

  void ModelBase::extendBoundingBoxFromChild(const ModelBase_sptr& cspChildModelBase)
  {
    const Bnd_Box& cChildBox3dInCHILDCoordSys
            = cspChildModelBase->getBoundingBox();
    const gp_Trsf& cTransformationToParentCoordSys
            = cspChildModelBase->getTransformation();
    const Bnd_Box cChildBox3dInPARENTCoordSys = cChildBox3dInCHILDCoordSys.Transformed(cTransformationToParentCoordSys);
    this->extendBoundingBox(cChildBox3dInPARENTCoordSys);
  }


  ModelBaseVector_sptr ModelBaseVector::Create()
  {
      return make_shared<ModelBaseVector>();
  }
} // ventus