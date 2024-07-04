#ifndef ventus_model_mesh_h_
#define ventus_model_mesh_h_

// SYSTEM INCLUDES
#include <vector>
#include <cassert>

// OCC INCLUDES
#include "Poly_Triangulation.hxx"
#include "gp_Pnt.hxx"
#include "Poly_Triangle.hxx"

// VENTUS INCLUDES
#include "GraphicInfo.h"

namespace ventus
{
  using namespace std;

  enum class LoDStatus {
    NOT_LOADED,
    CONTOUR,
    DETAILED,
    COMPLETE
  };

  class ModelMesh;
  using ModelMesh_sptr = shared_ptr<ModelMesh>;
  using indicesList = std::vector<int>;

  class ModelMesh
  {

  public:

    //Achtung: Konstruktion nur ueber Create!
    static ModelMesh_sptr Create();
    static ModelMesh_sptr Create(const Handle(Poly_Triangulation) &hPoly, const GraphicInfo& graphicInfo);

    // Constructor - not to be used from outside
    ModelMesh();
    ModelMesh(const Handle(Poly_Triangulation)& hPoly, const GraphicInfo& graphicInfo);
  
    // Destructor.
    virtual ~ModelMesh();

    // Access
    inline size_t getNumberOfPoints3d() const;

    inline const gp_Pnt& getPoint3d(int index) const;

    inline size_t getNumberOfTriangles() const;

    inline void getTriangle(int index, int& iPoint1, int& iPoint2, int& iPoint3) const;

    inline const GraphicInfo& getGraphicInfo() const;

    inline LoDStatus getLoDStatus();

    inline const indicesList& getIndicesList() const; //deprecated method

  private:

    // Member
    Handle(Poly_Triangulation) mhPoly; 

    GraphicInfo mGraphicInfo;

    LoDStatus mLoDStatus = LoDStatus::NOT_LOADED;

    // Anlegen von Kopien soll nicht moeglich sein
    ModelMesh(const ModelMesh&) = delete;
    const ModelMesh& operator=(const ModelMesh&) = delete;

    // For error cases
    static const gp_Pnt mysteriousPoint;

    indicesList mIndicesList;

  }; // ModelMesh


  class ModelMeshVector;
  using ModelMeshVector_sptr = shared_ptr<ModelMeshVector>;

  class ModelMeshVector : public vector<ModelMesh_sptr>
  {
  public:
      //Achtung: Konstruktion nur ueber Create!
      static ModelMeshVector_sptr Create();

      ModelMeshVector() {}

  private:
      // Anlegen von Kopien soll nicht moeglich sein
      ModelMeshVector(const ModelMeshVector&) = delete;
      const ModelMeshVector& operator=(const ModelMeshVector&) = delete;
  };


// INLINE METHODS
 
  size_t ModelMesh::getNumberOfPoints3d() const
  {
      assert(!mhPoly.IsNull());
      return mhPoly.IsNull() ? 0 : mhPoly->NbNodes();
  }

  const gp_Pnt& ModelMesh::getPoint3d(int index) const
  {
      assert(!mhPoly.IsNull());
      return mhPoly.IsNull() ? mysteriousPoint : mhPoly->Node(index);
  }

  size_t ModelMesh::getNumberOfTriangles() const
  {
      assert(!mhPoly.IsNull());
      return mhPoly.IsNull() ? 0 : mhPoly->NbTriangles();
  }

  void ModelMesh::getTriangle(int index, int& iPoint1, int& iPoint2, int& iPoint3) const
  {
      assert(!mhPoly.IsNull());
      if (!mhPoly.IsNull())
         mhPoly->Triangle(index).Get(iPoint1, iPoint2, iPoint3);
  }

  const GraphicInfo& ModelMesh::getGraphicInfo() const
  {
       return mGraphicInfo;
  }

  LoDStatus ModelMesh::getLoDStatus()
  {
       return mLoDStatus;
  }

  const indicesList& ModelMesh::getIndicesList() const
  {
      return mIndicesList;
  }
}

#endif // ventus_model_mesh_h_