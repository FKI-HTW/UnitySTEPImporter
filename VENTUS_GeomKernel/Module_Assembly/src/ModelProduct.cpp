#include "ModelProduct.h"

namespace ventus
{
  
  ModelProduct::ModelProduct() 
      : ModelBase(ModelType::PRODUCT)
  {
      mspChildren = ModelBaseVector::Create();
  }

  ModelProduct::ModelProduct(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name)
      : ModelBase(ModelType::PRODUCT, spParent, transformation, name)
  {
      mspChildren = ModelBaseVector::Create();
  }

  ModelProduct::~ModelProduct() { }

  ModelProduct_sptr ModelProduct::Create()
  {
      return make_shared<ModelProduct>();
  }

  ModelProduct_sptr ModelProduct::Create(const ModelBase_sptr& spParent, const gp_Trsf& transformation, const string& name)
  {
      return make_shared<ModelProduct>(spParent, transformation, name);
  }

//============================= OPERATORS ====================================

//============================= OPERATIONS ===================================

  void ModelProduct::push_backModelpart(const ModelPart_sptr& spPart)
  {
      if (nullptr != mspChildren && nullptr != spPart)
          mspChildren->push_back(dynamic_pointer_cast<ModelBase>(spPart));
  }

  void ModelProduct::push_backModelproduct(const ModelProduct_sptr& spProduct)
  {
      if (nullptr != mspChildren && nullptr != spProduct)
          mspChildren->push_back(dynamic_pointer_cast<ModelBase>(spProduct));
  }


//============================= ACCESS     ===================================

  size_t ModelProduct::getNumberOfChildParts()const
  {
      size_t nParts = 0;
      if (nullptr != mspChildren)
      {
          for (ModelBase_sptr& spChild : *mspChildren)
          {
              if (ModelType::PART == spChild->getType())
                  ++nParts;
          }
      }
      return nParts;
  }

  size_t ModelProduct::getNumberOfChildProducts() const
  {
      size_t nProducts = 0;
      if (nullptr != mspChildren)
      {
          for (ModelBase_sptr& spChild : *mspChildren)
          {
              if (ModelType::PRODUCT == spChild->getType())
                  ++nProducts;
          }
      }
      return nProducts;
  }

  const ModelPart* ModelProduct::getChildPartPointerByIndex(int i) const
  {
      int iPart = 0;
      if (nullptr != mspChildren)
      {
          for (ModelBase_sptr& spChild : *mspChildren)
          {
              if (ModelType::PART == spChild->getType())
              {
                  if (i == iPart)
                  {
                      ModelPart_sptr spPart = dynamic_pointer_cast<ModelPart>(spChild);
                      const ModelPart* pPart = spPart.get();
                      return pPart;
                  }
                  ++iPart;
              }
          }
      }
      return nullptr;
  }

  const ModelProduct* ModelProduct::getChildProductPointerByIndex(int i) const
  {
      int iProduct = 0;
      if (nullptr != mspChildren)
      {
          for (ModelBase_sptr& spChild : *mspChildren)
          {
              if (ModelType::PRODUCT == spChild->getType())
              {
                  if (i == iProduct)
                  {
                      ModelProduct_sptr spProduct = dynamic_pointer_cast<ModelProduct>(spChild);
                      const ModelProduct* pProduct = spProduct.get();
                      return pProduct;
                  }
                  ++iProduct;
              }
          }
      }
      return nullptr;
  }

  const ModelPart* ModelProduct::getChildPartPointerByID(int nID) const
  {
      if (nullptr != mspChildren)
      {
          for (ModelBase_sptr& spChild : *mspChildren)
          {
              if (ModelType::PART == spChild->getType() && nID == spChild->getID())
              {
                  ModelPart_sptr spPart = dynamic_pointer_cast<ModelPart>(spChild);
                  const ModelPart* pPart = spPart.get();
                  return pPart;
              }
          }
      }
      return nullptr;
  }

  const ModelProduct* ModelProduct::getChildProductPointerByID(int nID) const
  {
      if (nullptr != mspChildren)
      {
          for (ModelBase_sptr& spChild : *mspChildren)
          {
              if (ModelType::PRODUCT == spChild->getType() && nID == spChild->getID())
              {
                  ModelProduct_sptr spProduct = dynamic_pointer_cast<ModelProduct>(spChild);
                  const ModelProduct* pProduct = spProduct.get();
                  return pProduct;
              }
          }
      }
      return nullptr;
  }




//TODO: kann evtl. weg bzw. nullptr abfangen, komplett ueberarbeiten
  /*
  ventus::ModelPart* ModelProduct::
    getModelpart( const int modelpartID )
  {
    // TODO change linear search to binary search

    // search linear for ModelPart
    for ( ventus::ModelPart* pCurrPart : mModelpartVector )
    {
      if ( modelpartID == pCurrPart->getID() )
      {
        return pCurrPart;
      }
    }
    return nullptr;
  }

  ventus::ModelProduct* ModelProduct::
    getModelproduct( const int modelproductID )
  {
    // TODO change linear search to binary search

    // search linear for ModelProduct
    for ( ventus::ModelProduct* pCurrProduct : mModelproductVector )
    {
      if ( modelproductID == pCurrProduct->getID() )
      {
        return pCurrProduct;
      }
    }
    return nullptr;
  }

  ventus::ModelPart* ModelProduct::
    getModelpartFromSubmodels(const int modelpartID )
  {
    ventus::ModelPart* pFoundedPart = nullptr;

    pFoundedPart = this->getModelpart( modelpartID );
    if ( nullptr == pFoundedPart )
    {
      for (ventus::ModelProduct* pCurrProduct : mModelproductVector )
      {
        pFoundedPart = pCurrProduct->getModelpartFromSubmodels( modelpartID );

        if ( nullptr != pFoundedPart )
        {
          break;
        }
      }
    }

    return pFoundedPart;
  }

  
  ventus::ModelProduct* ModelProduct::
    getModelproductFromSubmodels(const int modelproductID )
  {
    ventus::ModelProduct* pFoundedProduct = nullptr;
   
    pFoundedProduct = this->getModelproduct( modelproductID );
    if ( nullptr == pFoundedProduct )
    {
      for ( ventus::ModelProduct* pCurrProduct : mModelproductVector )
      {
        pFoundedProduct = pCurrProduct->getModelproductFromSubmodels( modelproductID );

        if ( nullptr != pFoundedProduct )
        {
          break;
        }
      }
    }

    return pFoundedProduct;
  }
  */

//============================= INQUIRY    ===================================

/////////////////////////////// PROTECTED  ///////////////////////////////////

/////////////////////////////// PRIVATE    ///////////////////////////////////

} // ventus