// CLASS IMPLEMENTED
#include "GeomKernel.h"

//PROJECT INCLUDES
#include "LODUtilities.h"
#include "CheckPathString.h"
#include "FileLoaderXDE.h"
#include "EnumAdaptation.h"

namespace ventus
{ 
  GeomKernel::GeomKernel()
    : mDefaultLOD( LODUtilities::getDefaultLOD())
  { }

  GeomKernel::GeomKernel( int defaultLOD )
   : mDefaultLOD( defaultLOD)
  { }

  GeomKernel::~GeomKernel() { }

  ResultFileLoading GeomKernel::objectAddFromFile(const std::string& filepath, int& objID )
  {
    objID = -1;
    if ( !CheckPathString::checkFileExtension( filepath ) )
    {
      return ResultFileLoading::WRONG_FILE_EXTENSION;
    }

    if ( !CheckPathString::checkExistFile( filepath ) )
    {
      return ResultFileLoading::FILE_NOT_FOUND;
    }

    FileLoaderXDE fileLoader(filepath);
    if (!fileLoader.load()) {
        return ResultFileLoading::LOADING_EMPTY;
    }
    ModelXDE model = fileLoader.createModel();
    const AssemblyRequester requester = model.createAssemblyRequester();

    ModelObject_sptr obj = ModelObject::Create();
    // ---------------------------------------------------------------
    // Hier fehlt noch der Code für das Befuellen des Modelobjects !!!
    // ---------------------------------------------------------------
    mObjectVector.push_back(obj);
    objID = mObjectVector.size() - 1;

    return EnumAdaptation::adaptReadStatus(static_cast<IFSelect_ReturnStatus>(fileLoader.requestReadStatus()));
  }

  const ModelObject* GeomKernel::getModelObject(int objID) const
  {
      const ModelObject* pObj = nullptr;
      if (0 <= objID && objID < mObjectVector.size())
      {
          pObj = mObjectVector[objID].get();
      }
      return pObj;
  }

bool GeomKernel::addChangeLODThreadJob(ProgressCallback callback, ModelPart* pModelpart, const int LOD) {
    //if (nullptr == assemblyRequester || ResultFileLoading::CREATE_SUCCESSFUL != resultFileLoading) {
    //    resultTransfer = ResultTransfer::DATA_NOT_IN_READER;
    //    return resultTransfer;
    //}
    //mThreadList.emplace_back(&ObjectManager::changeLODThreadModelpart, this, callback, pModelpart, LOD);
    return true;
}

void GeomKernel::changeLOD(ModelPart* pModelpart, const double linearDeflectionFactor, const double angularDeflection) {

    //const OCC::TriangulationSetup triangulationSetup(linearDeflectionFactor, angularDeflection);
    //OCC::BRepLabel bRepLabelOCCAdapter = assemblyRequester->requestBRepLabel(pModelpart->getAssemblyLabel());
    //const OCC::BoundingBoxArray boxOCCAdapter = OCC::BRepRequester::requestBoundingBox(bRepLabelOCCAdapter);
    //const OCC::TriangulationSetup boxDependentSetup
    //    = OCC::Triangulator::adaptBoundingBoxDependency(triangulationSetup, boxOCCAdapter);
    //OCC::Triangulator::triangulate(boxDependentSetup, bRepLabelOCCAdapter);

    //TODO -> vermutlich recht es schon so, sonst warten auf Margitta!!!
    //MeshLists* mesh = transferer.transferMesh(assemblyLabel->getAssemblyLabelOCCAdapter());
    //MeshLists* mesh();
    //pModelpart->getModelmesh()->setMesh(mesh);
}

ModelPart* GeomKernel::findSubModelpart(const int partID)
{
    const ModelObject* modelObject = getModelObject(partID);

    //for (ModelPart* subPart : modelObject->getModelpartVector())
    //{
    //    if (partID == subPart->getID())
    //    {
    //        return subPart;
    //    }
    //}
    //for (ModelProduct* subProduct : modelObject->getModelproductVector())
    //{
    //    ModelPart* subPart = findSubModelpartInModelproduct(subProduct, partID);
    //    if (partID == subPart->getID())
    //    {
    //        return subPart;
    //    }
    //}
    return nullptr;
}

void GeomKernel::changeLODThreadModelpart(ProgressCallback callback,
    ModelPart* pModelpart,
    const int LOD)
{
    const double linearDeflectionFactor
        = LODUtilities::getLODLinearDeflectionFactor(LOD);
    const double angularDeflection
        = LODUtilities::getLODAngleDeflection(LOD);

    changeLOD(pModelpart,
        linearDeflectionFactor,
        angularDeflection);

    callback(pModelpart, LOD);

}

ModelPart* GeomKernel::findSubModelpartInModelproduct(ModelProduct* product,
    const int partID)
{
    //for (ModelPart subPart : product->getModelBaseVector())
    //{
    //    if (partID == subPart.getID())
    //    {
    //        return &subPart;
    //    }
    //}
    //for (ModelPart subProduct : product->getModelPartVector())
    //{
    //    //ModelPart* subPart = findSubModelpartInModelproduct( subProduct, partID );
    //    if (partID == subProduct.getID())
    //    {
    //        return &subProduct;
    //    }
    //}
    return nullptr;
}
}

