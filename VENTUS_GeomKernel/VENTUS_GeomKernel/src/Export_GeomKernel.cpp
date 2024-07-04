#include "Export_GeomKernel.h"

// PROJECT INCLUDE
#include "ExportAdaptionUtilities.h"
#include "MeshAttributesToExport.h"
#include "ExportAdaptionUtilities.h"
#include "ModelAttributesToExport.h"

// GeomKernelBase INCLUDES
#include "LODUtilities.h"
#include "GeomKernel.h"

// MODULE_ASSEMBLY INCLUDES
#include "ModelBase.h"
#include "ModelObject.h"
#include "ModelPart.h"
#include "ModelProduct.h"

//OCC Includes
#include "TColgp_HArray1OfPnt.hxx"
//#include <gp_Pnt.hxx>
#include <Bnd_Box.hxx>

// TO KILL INCLUDES
#include <fstream>
#include <ostream>

#include <vector>

// TODO: maybe to kill, test it 
#define _CRT_SECURE_NO_WARNINGS

namespace ventus {
  namespace Export {

  /* === Export_GeomKernel === */

    DLL_EXPORT GeomKernel* __stdcall initGeomKernelLOD(int defaultLOD) {
        return new GeomKernel(defaultLOD);
    }

    DLL_EXPORT ResultFileLoading __stdcall loadModelObjectFromFile(const char* filepath, GeomKernel *pGeomKernel, int& objID) {
        return pGeomKernel->objectAddFromFile(filepath, objID);
    }

    DLL_EXPORT const ModelObject* __stdcall getModelObject(const GeomKernel* pGeomKernel, int objID) {
        return pGeomKernel->getModelObject(objID);
    }


  /* === Export_Assembly === */

    DLL_EXPORT int __stdcall transferNumberOfRoots(const ModelObject* modelObject) {
        if (modelObject)
            return modelObject->getRootModels().size(); 
        return -1;
    }

    DLL_EXPORT int __stdcall transferChildsNumberOfARoot(const ModelProduct* modelProduct) {
        if (modelProduct)
            return modelProduct->getChildren().get()->size();
        return -1;
    }

  /* === Export_MeshAttribute === */

    DLL_EXPORT int __stdcall transferNumberOfMeshes(const ModelPart* modelPart) {
        if (modelPart) {
            return modelPart->getModelMeshes().size();
        }
        return -1;
    }

    DLL_EXPORT const ModelMesh* requestModelMesh(const ModelPart* modelPart, int index) {
        if (modelPart) {
            return modelPart->getModelMeshes()[index].get();
        }
        return nullptr;
    }

    DLL_EXPORT int __stdcall transferNumberOfTriangles(const ModelMesh* modelMesh) {
        const int indexCount = (int)modelMesh->getNumberOfTriangles();
        return indexCount;
    }
    
    DLL_EXPORT void __stdcall transferTriangle(const ModelMesh* modelMesh, int index, int& point1, int& point2, int& point3) {
        modelMesh->getTriangle(index, point1, point2, point3);
    }

    DLL_EXPORT int __stdcall transferCoordinateCount(const ModelMesh* modelMesh) {
        int coordinateCount = (int)modelMesh->getNumberOfPoints3d();
        return coordinateCount;
    }

    DLL_EXPORT void __stdcall transferCoordinate(const ModelMesh* modelMesh, int index, ExportCoordinate3d& refCoordinate) {
        const gp_Pnt point = modelMesh->getPoint3d(index);
        refCoordinate = ExportCoordinate3d(point.X(), point.Y(), point.Z());
    }

    DLL_EXPORT int __stdcall transferLodStatus(ModelMesh* modelMesh) {
        LoDStatus lodStatus = modelMesh->getLoDStatus();
        if (lodStatus == LoDStatus::COMPLETE) {
            return 0;
        } else if (lodStatus ==  LoDStatus::CONTOUR) {
            return 1;
        } else if (lodStatus == LoDStatus::DETAILED) {
            return 2;
        } else if (lodStatus == LoDStatus::NOT_LOADED) {
            return 3;
        }
        return -1;
    }

    DLL_EXPORT void __stdcall transferColor(const GraphicInfo& pGraphicInfo, ExportRGBAColor& refColor) {
        const Color& color = pGraphicInfo.getColor();
        refColor = ExportRGBAColor(color.getR(), color.getG(), color.getB(), color.getA());
    }


  /* === Export_ModelBaseAttribute === */

    DLL_EXPORT int __stdcall transferType(const ModelBase* modelBase) {
        ModelType modelType = modelBase->getType();
        if (modelType == ModelType::PRODUCT) {
            return 1;
        }
        return 2;
    }

    DLL_EXPORT void __stdcall transferTransformation(const ModelBase* modelBase, ExportTransformation4d& rExportTransformation) {
        if (nullptr == modelBase) {
            return;
        }
        const gp_Trsf& transformation = modelBase->getTransformation();
        const ExportTransformation4d exportTransformation = ModelAttributesToExport::transfer(transformation);
        rExportTransformation = exportTransformation;
    }

    DLL_EXPORT void transferID(const ModelBase* modelBase, int& ID) {
        if (nullptr == modelBase) {
            ID = 0;
            return;
        }
        ID = modelBase->getID();
    }

    DLL_EXPORT const char * __stdcall transferName(const ModelBase* modelBase, int& size) {
        if (nullptr != modelBase) {
            size = (int)modelBase->getName().size();
            return modelBase->getName().c_str();
        }   
        return "";
    }

    DLL_EXPORT void __stdcall transferBoundingBox(const ModelBase* modelBase, ExportBoundingBox& rExportBoundingBox) {
        const Bnd_Box cBox = modelBase->getBoundingBox();
        Standard_Real theXmin = 0, theYmin = 0, theZmin = 0, theXmax = 0, theYmax = 0, theZmax = 0;
        if (!cBox.IsVoid()) {
            cBox.Get(theXmin, theYmin, theZmin, theXmax, theYmax, theZmax);
        }
        rExportBoundingBox = ExportBoundingBox(theXmin, theYmin, theZmin, theXmax, theYmax, theZmax);
    }


    /* === Export_ModelLODChanger === */

    DLL_EXPORT bool changeLODModelpart( //ProgressCallback callback,
            GeomKernel* pObjectManager, ModelPart* pModelpart, int LOD)
    {
        const double linearDeflection
            = LODUtilities::getLODLinearDeflectionFactor(LOD);
        const double angularDeflection
            = LODUtilities::getLODAngleDeflection(LOD);
        pObjectManager->changeLOD(pModelpart,
            linearDeflection,
            angularDeflection);
        return true;
        //return pObjectManager->addChangeLODThreadJob( callback, pEdGeModelpart, LOD );
    }

  /* === Export_Pointer === */

    DLL_EXPORT const ModelObject* requestModelobject(const GeomKernel* pGeomKernel, int objID)
    {
        return pGeomKernel->getModelObject(objID);
    }

    DLL_EXPORT const ModelBase* requestRootFromObject(const ModelObject* modelObject, int index) {
        if (modelObject)
            return modelObject->getRootModels().at(index).get();
        return nullptr;
    }

    DLL_EXPORT const ModelBase* requestChildFromRoot(const ModelProduct* modelProduct, int index) {
        if (modelProduct)
            return modelProduct->getChildren().get()->at(index).get();
        return nullptr;
    }

    DLL_EXPORT const ModelPart* requestSubPartFromProduct(const ModelProduct* pProduct, int index)
    {
        if (pProduct)
            return pProduct->getChildPartPointerByIndex(index); //deprecated method
        return nullptr; 
    }

    DLL_EXPORT const ModelProduct* requestSubProductFromProduct(const ModelProduct* pProduct, int index)
    {
        if (pProduct)
            return pProduct->getChildProductPointerByIndex(index); //deprecated method
        return nullptr; 
    }

    DLL_EXPORT const ModelMesh* requestModelmesh(const ModelPart* pPart)
    {
        return pPart->getModelMeshes()[0].get();
    }

    DLL_EXPORT const GraphicInfo* requestGraphicInfo(const ModelMesh* modelMesh)
    {
        return &modelMesh->getGraphicInfo();
    }
  }
}