#ifndef ventus_model_part_h_
#define ventus_model_part_h_

// SYSTEM INCLUDES

// PROJECT INCLUDE
#include "ModelBase.h"
#include "ModelMesh.h"

namespace ventus
{
 
  using namespace std;
    
  class ModelPart;
  using ModelPart_sptr = shared_ptr<ModelPart>;

  class ModelPart : public ModelBase
  {

  public:
    // LIFECYCLE

    //Achtung: Konstruktion nur ueber Create!
    static ModelPart_sptr Create();
    static ModelPart_sptr Create(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name);

    ModelPart();
    ModelPart(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name);

    // Destructor.
    virtual ~ModelPart();

    // OPERATORS

    // OPERATIONS 

    // ACCESS
    inline void addModelMesh(const ModelMesh_sptr& modelMesh);

    inline const vector<ModelMesh_sptr> &getModelMeshes() const;
    
  private:

    vector<ModelMesh_sptr> mspModelMeshes;
         
    //TODO: wo wird dieser Counter gepflegt 
     // PRIVATE STATIC MEMBER
     //static size_t smCounterModelpart;

    // Anlegen von Kopien soll nicht moeglich sein
    ModelPart(const ModelPart&);
    const ModelPart& operator=(const ModelPart&);

  }; // ModelPart

// INLINE METHODS
  void ModelPart::addModelMesh(const ModelMesh_sptr& modelMesh ) {
      mspModelMeshes.push_back(modelMesh);
  }

  const vector<ModelMesh_sptr>& ModelPart::getModelMeshes() const
  {
    return mspModelMeshes;
  }
}

#endif // ventus_model_part_h_
