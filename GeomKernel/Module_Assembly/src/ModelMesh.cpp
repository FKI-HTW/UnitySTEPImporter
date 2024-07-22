#include "ModelMesh.h"

namespace ventus
{
    const gp_Pnt ModelMesh::mysteriousPoint = gp_Pnt(1707.0505, 1707.0505, 1707.0505);

    ModelMesh::ModelMesh()
    {}

    ModelMesh::ModelMesh(const Handle(Poly_Triangulation)& hPoly, const GraphicInfo& graphicInfo)
        : mhPoly(hPoly),
          mGraphicInfo(graphicInfo)
    {
        mIndicesList.resize(3 * mhPoly->NbTriangles());
    }

    ModelMesh::~ModelMesh() 
    {}

    ModelMesh_sptr ModelMesh::Create()
    {
        return make_shared<ModelMesh>();
    }

    ModelMesh_sptr ModelMesh::Create(const Handle(Poly_Triangulation)& hPoly, const GraphicInfo& graphicInfo)
    {
        return make_shared<ModelMesh>(hPoly, graphicInfo);
    }

    ModelMeshVector_sptr ModelMeshVector::Create()
    {
        return make_shared<ModelMeshVector>();
    }
} // ventus