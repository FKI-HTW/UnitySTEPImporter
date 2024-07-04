#ifndef _VENTUS_GeomKernel_h_
#define _VENTUS_GeomKernel_h_

// PROJECT INCLUDES
#include "SetUpsLoader.h"
#include "ModelObject.h"

#include <vector>
#include <thread>

class ModelPart;
class ModelProduct;

namespace ventus
{
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

  }; // GeomKernel


 int GeomKernel::getDefaultLOD() const
 {
   return mDefaultLOD;
 }

}// ventus

#endif // ! _VENTUS_GeomKernel_h_