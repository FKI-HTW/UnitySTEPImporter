#ifndef ventus_model_base_h_
#define ventus_model_base_h_

// SYSTEM INCLUDES
#include <vector>
#include <string>
#include <iostream>
#include <memory>

// PROJECT INCLUDES

// OCC INCLUDES
#include "gp_Pnt.hxx"
#include "gp_Trsf.hxx"
#include "Bnd_Box.hxx"

#define DEBUG_INSTANCE_COUNTER

namespace ventus
{
  using namespace std;

  enum class ModelType {
      PRODUCT,
      PART
  };

  

  class ModelBase;
  using ModelBase_sptr = shared_ptr<ModelBase>;
  using ModelBase_wptr = weak_ptr<ModelBase>;

  class ModelBase
  {
  protected:
      //Konstruktion und Destruktion ueber ModelPart- bzw. ModelProduct-Instanzen
      ModelBase(ModelType type);
      //TODO: Ist die Bounding beim Anlegen einer Part/Product-Instanz schon vorhanden?
      //      Oder wird eigentlich immer eine leere Box uebergeben?
      ModelBase(ModelType type, const ModelBase_sptr& spParent, const gp_Trsf& pTransformation, const string& name/*, const Bnd_Box& box3d*/);

      virtual ~ModelBase();

  public:
  
    // OPERATORS

    // OPERATIONS   

    inline void setBoundingBox(const Bnd_Box& boundingBoxOCC);

    void extendBoundingBox(const Bnd_Box& boundingBoxOCC);

    void extendBoundingBoxFromChild(const ModelBase_sptr& cspChildModelBase);
    
    // ACCESS

    inline ModelType getType() const;

    inline const gp_Trsf& getTransformation() const;

    inline int getID() const;
    
    inline const string& getName() const;
    
    inline const Bnd_Box& getBoundingBox() const;

    inline const ModelBase_wptr& getParent() const;

    // MODIFIER

    inline void setParent(const ModelBase_sptr& spParent);
    
    // deprecated
    inline void setName(const string& name);
    // deprecated
    inline void setTransformation(const gp_Trsf& transformation);


    // INQUIRY

    inline bool isEmptyBoundingBox();

#if defined (DEBUG_INSTANCE_COUNTER)
    static size_t getInstanceCounter() { return smInstanceCounter; }
#endif // DEBUG_INSTANCE_COUNTER

  protected:

    gp_Trsf mTransformation;

    int mID;

    string mName;

    Bnd_Box mBoundingBox;

    ModelBase_wptr mwpParent;

  private:
    ModelType mModelType;

    static int smLastModelID;

#if defined (DEBUG_INSTANCE_COUNTER)
      static size_t smInstanceCounter;
#endif // DEBUG_INSTANCE_COUNTER

  }; // ModelBase

  class ModelBaseVector;
  using ModelBaseVector_sptr = shared_ptr<ModelBaseVector>;

  class ModelBaseVector : public vector<ModelBase_sptr>
  {
  public:
      //Achtung: Konstruktion nur ueber Create!
      static ModelBaseVector_sptr Create();

      ModelBaseVector() {}

  private:

      //TODO: Anmerkung - sollen nicht benutzt werden, daher als private deklarieren, aber nicht definieren
      // Anlegen von Kopien soll nicht moeglich sein
      ModelBaseVector(const ModelBaseVector&);
      const ModelBaseVector& operator=(const ModelBaseVector&);
  };



  // INLINE METHODS

  ModelType ModelBase::getType() const
  {
      return mModelType;
  }

  const gp_Trsf& ModelBase::getTransformation() const
  {
    return mTransformation;
  }

  int ModelBase::getID() const
  {
    return mID;
  }

  const string& ModelBase::getName() const
  {
    return mName;
  }

  const Bnd_Box& ModelBase::getBoundingBox() const
  {
    return mBoundingBox;
  }


  const ModelBase_wptr& ModelBase::getParent() const
  {
      return mwpParent;
  }

  void ModelBase::setParent(const ModelBase_sptr& spParent)
  {
      mwpParent = spParent;
  }

  void ModelBase::setName(const string& name)
  {
    mName = name;
  }
  
  void ModelBase::setTransformation(const gp_Trsf& trsf)
  {
      mTransformation = trsf;
  }

  void ModelBase::setBoundingBox(const Bnd_Box& boundingBox)
  {
    mBoundingBox = boundingBox;
  }
  
  bool ModelBase::isEmptyBoundingBox()
  {
    return mBoundingBox.IsVoid();
  }
} 

#endif // ventus_model_base_h_






















