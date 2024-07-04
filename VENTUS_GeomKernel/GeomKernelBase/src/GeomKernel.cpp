// CLASS IMPLEMENTED
#include "GeomKernel.h"

#include "ModelBase.h"

#include "BRep_Tool.hxx"
#include "TopoDS.hxx"

#include "ShapeFix_Shape.hxx"
#include "TopAbs_Orientation.hxx"

namespace ventus
{ 
    GeomKernel::GeomKernel()
    : mDefaultLOD( LODUtilities::getDefaultLOD())
    { }

    GeomKernel::GeomKernel( int defaultLOD )
    : mDefaultLOD( defaultLOD)
    { }

    GeomKernel::~GeomKernel() { }

    ResultFileLoading GeomKernel::objectAddFromFile(const std::string& filePath, int& modelObjectId ) {
        //modelObjectId = -1; // ???
        if ( !CheckPathString::checkFileExtension( filePath ) ) {
            return ResultFileLoading::WRONG_FILE_EXTENSION;
        }
        if ( !CheckPathString::checkExistFile( filePath ) ) {
            return ResultFileLoading::FILE_NOT_FOUND;
        }
        OCCAdapter occAdapter(filePath);
        if (occAdapter.loadStepFile()) {
            ModelObject_sptr modelObject = transferStepToModuleAssembly(occAdapter);

            mObjectVector.push_back(modelObject);
            modelObjectId = mObjectVector.size() - 1;

            return ResultFileLoading::CREATE_SUCCESSFUL;
        }
        return ResultFileLoading::LOADING_EMPTY;
    }

    void GeomKernel::getModel(const Handle(XCAFDoc_ShapeTool)& assembly, 
                              const TDF_Label& label, 
                              ModelBase_sptr& model, 
                              const ModelProduct_sptr& parent) const {
       std::string name = OCCAdapter::findLabelName(label);
       gp_Trsf transform = OCCAdapter::findTransformation(assembly, label);
       TopoDS_Shape shape = assembly->GetShape(label);

       if (OCCAdapter::isProduct(assembly, label)) {
          ModelProduct_sptr product = ModelProduct::Create(parent, transform, name);
          if (parent) {
              parent->push_backModelproduct(product);
          }
          model = product;
          TDF_LabelSequence subLabels;
          if (assembly->GetComponents(label, subLabels)) {
             for (const TDF_Label& childLabel : subLabels) {
                ModelBase_sptr child;
                getModel(assembly, childLabel, child, product);
             }
          }
          else {
             for (TopoDS_Iterator shapeIt(shape); shapeIt.More(); shapeIt.Next()) {
                const TopoDS_Shape& subShape = shapeIt.Value();
                TDF_Label subLabel;
                bool hasLabel = assembly->Search(subShape, subLabel);
                if (hasLabel) {
                   if (OCCAdapter::isProduct(assembly, subLabel) || OCCAdapter::isPart(assembly, subLabel)) {
                      ModelBase_sptr child;
                      getModel(assembly, subLabel, child, product);
                   }
                } else {
                   // TO DO: inquire subshapes without label
                   // 
                   //if (OCCAdapter::isProduct(subShape) || OCCAdapter::isPart(shape))
                   //{
                   //   ModelBase_sptr child;
                   //   getModel(subShape, child, product);
                   //}
                }
             }
          }
       } else {
          ModelPart_sptr part = ModelPart::Create(parent, transform, name);
          addTriangulation(shape, part, label);
          part->setBoundingBox(OCCAdapter::findBoundingBox(shape));
          if (parent) {
              parent->push_backModelpart(part);
          }
          model = part;
       }
    }

    void GeomKernel::addTriangulation(const TopoDS_Shape& shape, ModelPart_sptr &part, const TDF_Label& label) const {
       TopAbs_ShapeEnum type = shape.ShapeType();
       if (type == TopAbs_FACE) {
          OCCAdapter::triangulateShape(shape, DEFAULT_LINEAR_DEFLECTION, DEFAULT_ANGULAR_DEFLECTION);
          
          // const 
          TopoDS_Face face = TopoDS::Face(shape);
          TopLoc_Location location(part.get()->getTransformation());

          label.Dump(std::cout);
          std::cout << std::endl;

          face.DumpJson(std::cout);
          std::cout << std::endl;

          TopAbs_Orientation orientation = face.Orientation();
          //if (orientation == TopAbs_REVERSED || orientation == TopAbs_EXTERNAL || orientation == TopAbs_INTERNAL) {
          //    face.Reverse();
          //    face.Compose(face.Orientation());
          //}
          std::cout << "Orientation : " << face.Orientation() << std::endl;
          
          Handle(Poly_Triangulation) triangulation = BRep_Tool::Triangulation(face, location);
          
          triangulation->DumpJson(std::cout);
          std::cout << std::endl;

          for (int i = 1; i <= triangulation->NbTriangles(); i++) {
              Poly_Triangle triangle = triangulation->Triangle(i);
              if (orientation != TopAbs_FORWARD) {
                  Poly_Triangle triangleNew(triangle(2), triangle(1), triangle(3));
                  triangulation->SetTriangle(i, triangleNew);
              }
              triangle = triangulation->Triangle(i);
              std::cout << "Triangle " << i << " : (" << triangle(1) << ", " << triangle(2) << ", " << triangle(3) << ")" << std::endl;
          }
          
          Quantity_ColorRGBA color = OCCAdapter::findColor(label);
          Quantity_Color rgb = color.GetRGB();
          GraphicInfo gInfo(rgb.Red(), rgb.Green(), rgb.Blue(), color.Alpha()); 
          // TODO: get color of shape and set gInfo appropriately 
          ModelMesh_sptr mesh = ModelMesh::Create(triangulation, gInfo);
          part->addModelMesh(mesh);
       } else {
          for (TopoDS_Iterator iter(shape); iter.More(); iter.Next()) {
             addTriangulation(iter.Value(), part, label);
          }
       }
    }


    ModelObject_sptr GeomKernel::transferStepToModuleAssembly(OCCAdapter& occAdapter) {
       ModelObject_sptr modelObject = ModelObject::Create();
       Handle(XCAFDoc_ShapeTool) assembly = XCAFDoc_DocumentTool::ShapeTool(occAdapter.getDocument()->Main());
       TDF_LabelSequence freeShapes;
       assembly->GetFreeShapes(freeShapes);
       ModelBase_sptr model;
       ModelProduct_sptr nullParent;
       for (auto itr = freeShapes.begin(); itr != freeShapes.end(); itr++)
       {
           getModel(assembly, *itr, model, nullParent);
           if (model)
           {
               modelObject->addRootModel(model);
           }
       }
       return modelObject;
    }

    ModelObject_sptr GeomKernel::transferStepToModuleAssambly(OCCAdapter& occAdapter) {
        occAdapter.findProductsAndParts();
        const std::map<int, ModelBaseData>& modulAssambly = occAdapter.getModuleAssamblyMap();
        ModelObject_sptr modelObject = ModelObject::Create();
        const ModelProduct_sptr rootModelObject = ModelProduct::Create();
        setModelBase(rootModelObject, modulAssambly.find(1)->second);
        modelObject->addRootModel(rootModelObject);        
        for (auto const& it : modulAssambly) {
            int id = it.first;
            const ModelBaseData& assemblyData = it.second;
            ModelBase_sptr partOrProduct = setModelPartOrProduct(assemblyData, occAdapter);
            if (partOrProduct) {
                modelObject->addRootModel(partOrProduct);
            }
        }        
        return modelObject;
    }

    const ModelBase_sptr GeomKernel::setModelPartOrProduct(const ModelBaseData& assemblyData, OCCAdapter& occAdapter) {
        if (assemblyData.type == "PRODUCT") {
            ModelProduct_sptr modelProduct = ModelProduct::Create();
            setModelBase(modelProduct, assemblyData);
            if (assemblyData.childs.size() > 0) {
                for (auto const& childId : assemblyData.childs) {
                    const ModelBaseData& child = occAdapter.getModelBaseDataPerId(childId);
                    setModelPartOrProduct(child, occAdapter);
                }
            }
            return modelProduct;
        }
        else if (assemblyData.type == "PART") {
            const ModelPart_sptr modelPart = ModelPart::Create();
            setModelBase(modelPart, assemblyData);
            return modelPart;
        }
    
    }

    void GeomKernel::setModelBase(ModelBase_sptr modelBase, const ModelBaseData& modelBaseData) {
        modelBase->setTransformation(modelBaseData.transformation);
        //modelBase->setId(modelBaseData.iD);
        modelBase->setBoundingBox(modelBaseData.boundingBox);
        modelBase->setName(modelBaseData.name);
        //modelBase->setType(modelBaseData.type);
    }

    const ModelObject* GeomKernel::getModelObject(int objID) const {
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

    ModelPart* GeomKernel::findSubModelpart(const int partID) {
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

    void GeomKernel::changeLODThreadModelpart(ProgressCallback callback, ModelPart* pModelpart, const int LOD) {
        const double linearDeflectionFactor = LODUtilities::getLODLinearDeflectionFactor(LOD);
        const double angularDeflection = LODUtilities::getLODAngleDeflection(LOD);
        changeLOD(pModelpart, linearDeflectionFactor, angularDeflection);
        callback(pModelpart, LOD);
    }

    ModelPart* GeomKernel::findSubModelpartInModelproduct(ModelProduct* product, const int partID) {
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

