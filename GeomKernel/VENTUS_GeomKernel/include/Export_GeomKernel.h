#ifndef _VENTUS_Export_GeomKernel_h_
#define _VENTUS_Export_GeomKernel_h_

// PROJECT INCLUDES

// EXPORT INCLUDES
#include "SetUpsGeneral.h"
#include "SetUpsLoader.h"
#include "SetUpsToUnity.h"

namespace ventus
{
  class GeomKernel;
  class ModelObject;
  class ModelProduct;
  class ModelPart;
  class ModelMesh;
  class GraphicInfo;
  class Color;
  class ModelBase;

  namespace Export
  {
    extern "C"
    {
    /* === Export_GeomKernel === */
      DLL_EXPORT GeomKernel* __stdcall initGeomKernelLOD(int defaultLOD);
      DLL_EXPORT ResultFileLoading __stdcall loadModelObjectFromFile(const char* filepath, GeomKernel*, int& objID);
      DLL_EXPORT const ModelObject* __stdcall getModelObject(const GeomKernel* pGeomKernel, int objID);

    /* === Export_Assembly === */
      DLL_EXPORT int __stdcall transferNumberOfRoots(const ModelObject* modelObject);
      DLL_EXPORT int __stdcall transferChildsNumberOfARoot(const ModelProduct* modelProduct);

    /* === Export_MeshAttribute === */
      DLL_EXPORT int __stdcall transferNumberOfMeshes(const ModelPart* modelPart);
      DLL_EXPORT const ModelMesh* requestModelMesh(const ModelPart* modelPart, int index);
      DLL_EXPORT int __stdcall transferNumberOfTriangles(const ModelMesh* modelMesh);
      DLL_EXPORT void __stdcall transferTriangle(const ModelMesh* modelMesh, int index, int& point1, int& point2, int& point3);
	  DLL_EXPORT int __stdcall transferCoordinateCount(const ModelMesh* modelMesh);
      DLL_EXPORT void __stdcall transferCoordinate(const ModelMesh* modelMesh, int index, ExportCoordinate3d& refCoordinate);
      DLL_EXPORT int __stdcall transferLodStatus(ModelMesh* modelMesh);
      DLL_EXPORT void __stdcall transferColor(const GraphicInfo& pGraphicInfo, ExportRGBAColor& refColor);


      DLL_EXPORT void __stdcall transferIndices( const ModelMesh* pExportMesh, int* indicesArray );

	  DLL_EXPORT int __stdcall transferCoordinateCount(const ModelMesh* pExportMesh);

      DLL_EXPORT void __stdcall transferCoordinate( const ModelMesh* pExportMesh, int index, ExportCoordinate3d& refCoordinate );

      DLL_EXPORT void __stdcall transferColor( const GraphicInfo& pGraphicInfo, ExportRGBAColor& refColor );


    /* === Export_ModelBaseAttribute === */
      DLL_EXPORT int __stdcall transferType(const ModelBase* modelBase);
      DLL_EXPORT void __stdcall transferTransformation(const ModelBase* modelBase, ExportTransformation4d& pExportTransformation);
      DLL_EXPORT void __stdcall transferID(const ModelBase* modelBase, int& ID);
      DLL_EXPORT const char * __stdcall transferName(const ModelBase* modelBase, int& size);
      DLL_EXPORT void __stdcall transferBoundingBox(const ModelBase* pEdGeModelBase, ExportBoundingBox& rBoundingBox);
    

    /* === Export_ModelLODChanger === */
      DLL_EXPORT bool changeLODModelpart(GeomKernel*, ModelPart*, int LOD);

    /* === Export_Pointer === */
      DLL_EXPORT const ModelObject* requestModelobject(const GeomKernel*, int objID);
      DLL_EXPORT const ModelBase* requestRootFromObject(const ModelObject* modelObject, int index);
      DLL_EXPORT const ModelBase* requestChildFromRoot(const ModelProduct* modelProduct, int index);
      DLL_EXPORT const GraphicInfo* requestGraphicInfo(const ModelMesh* modelMesh);
    }
  }
}
#endif