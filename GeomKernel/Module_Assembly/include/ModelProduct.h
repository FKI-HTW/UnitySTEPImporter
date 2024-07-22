#ifndef ventus_model_product_h_
#define ventus_model_product_h_

// SYSTEM INCLUDES

// PROJECT INCLUDE
#include "ModelBase.h"
#include "ModelPart.h"


namespace ventus
{
  using namespace std;

  class ModelProduct;
  using ModelProduct_sptr = shared_ptr<ModelProduct>;

  class ModelProduct : public ModelBase
  {
    
  public:
    // LIFECYCLE


    //Achtung: Konstruktion nur ueber Create!
    static ModelProduct_sptr Create();
    static ModelProduct_sptr Create(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name);

    ModelProduct();
    ModelProduct(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name);

    virtual ~ModelProduct();

    // OPERATORS

    // OPERATIONS 
    void push_backModelpart(const ModelPart_sptr& spPart);

    void push_backModelproduct(const ModelProduct_sptr& spProduct);

    // ACCESS

    inline const ModelBaseVector_sptr& getChildren() const;

    size_t getNumberOfChildParts() const;    //deprecated method

    size_t getNumberOfChildProducts() const;    //deprecated method

    const ModelPart* getChildPartPointerByIndex(int i) const;      //deprecated method

    const ModelProduct* getChildProductPointerByIndex(int i) const;    //deprecated method

    const ModelPart* getChildPartPointerByID(int nID) const;     //deprecated method

    const ModelProduct* getChildProductPointerByID(int nID) const;    //deprecated method

    // INQUIRY
  
  private:

    ModelBaseVector_sptr mspChildren;
    
    // Anlegen von Kopien soll nicht moeglich sein
    ModelProduct(const ModelProduct&);
    const ModelProduct& operator=(const ModelProduct&);

  }; // ModelProduct 

  // INLINE METHODS

  const ModelBaseVector_sptr& ModelProduct::getChildren() const
  {
      return mspChildren;
  }

}

#endif // ventus_model_product_h_