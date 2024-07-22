#ifndef ventus_model_object_h_
#define ventus_model_object_h_

// PROJECT INCLUDE
#include "ModelBase.h"

namespace ventus
{

  using namespace std;

  class ModelPart;
  class ModelProduct;

  class ModelObject;
  using ModelObject_sptr = shared_ptr<ModelObject>;

  class ModelObject
  {

   public:
    // LIFECYCLE
    //Achtung: Konstruktion nur ueber Create!
    static ModelObject_sptr Create();

    ModelObject();

    virtual ~ModelObject();


    // ACCESS
    inline void addRootModel(const ModelBase_sptr& modelMesh);

    inline const vector<ModelBase_sptr>& getRootModels() const;
    
  private:
    
    ModelBaseVector_sptr mspParts;
    ModelBaseVector_sptr mspProducts;
    vector<ModelBase_sptr> mspRootModels;

    void AddModel2Lists(const ModelBase_sptr& spModel);

    // Anlegen von Kopien soll nicht moeglich sein
    ModelObject(const ModelObject&);
    const ModelObject& operator=(const ModelObject&);
  }; // ModelObject

// INLINE METHODS  
  const vector<ModelBase_sptr>& ModelObject::getRootModels() const
  {
    return mspRootModels;
  }

  void ModelObject::addRootModel(const ModelBase_sptr& spRootModel)
  {
    mspRootModels.push_back(spRootModel);
  }
}

#endif // ventus_model_object_h_
