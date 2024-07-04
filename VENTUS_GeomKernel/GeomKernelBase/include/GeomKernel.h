#ifndef _VENTUS_GeomKernel_h_
#define _VENTUS_GeomKernel_h_

#include <vector>
#include <thread>

// PROJECT INCLUDES
#include "CheckPathString.h"
#include "LODUtilities.h"
#include "SetUpsLoader.h"

// MODUL ASSAMBLY INCLUDES
#include "ModelBase.h"
#include "ModelProduct.h"
#include "ModelPart.h"
#include "ModelObject.h"

// OCC ADAPTER INCLUDES
#include "OCCAdapter.h"

namespace ventus
{
	//class ModelPart;
	//class ModelProduct;
   //class ModelProduct_sptr;

    typedef void(__stdcall* ProgressCallback)(ModelPart*, int);

    using ObjectVector = std::vector<ModelObject_sptr>;

  class GeomKernel
  {
  public:

    GeomKernel();

    GeomKernel(int defaultLOD);
    
    ~GeomKernel();

    ResultFileLoading objectAddFromFile(const std::string& filepath, int &objID);
    
    const ModelObject *getModelObject(int objID) const;

    inline int getDefaultLOD() const;

    bool addChangeLODThreadJob(ProgressCallback callback,
        ModelPart* pModelpart,
        const int LOD);

    void changeLOD(ModelPart* pModelpart,
        const double linearDeflectionFactor,
        const double angularDeflection);

    ModelPart* findSubModelpart(const int partID);


  private:
    
    int mDefaultLOD;

    ObjectVector mObjectVector;

    // TODO destruction handling, delete from list
    std::list<std::thread> mThreadList;

    void changeLODThreadModelpart(ProgressCallback callback,
        ModelPart* pModelpart,
        const int LOD);

    ModelPart* findSubModelpartInModelproduct(ModelProduct*,
        const int partID);

    void getModel(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label, ModelBase_sptr& model, const ModelProduct_sptr& parent) const;

    ModelObject_sptr transferStepToModuleAssambly(OCCAdapter& occAdapter);
    ModelObject_sptr transferStepToModuleAssembly(OCCAdapter& occAdapter);

    void addTriangulation(const TopoDS_Shape& shape, ModelPart_sptr& part, const TDF_Label& label) const;

    const ModelBase_sptr setModelPartOrProduct(const ModelBaseData& assemblyData, OCCAdapter& occAdapter);

    void setModelBase(ModelBase_sptr modelBase, const ModelBaseData& modelBaseData);

  }; // GeomKernel


 int GeomKernel::getDefaultLOD() const
 {
   return mDefaultLOD;
 }

}// ventus

#endif // ! _VENTUS_GeomKernel_h_