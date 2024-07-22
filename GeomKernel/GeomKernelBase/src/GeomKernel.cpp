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
        : mDefaultLOD(LODUtilities::getDefaultLOD())
    { }

    GeomKernel::GeomKernel(int defaultLOD)
        : mDefaultLOD(defaultLOD)
    { }

    GeomKernel::~GeomKernel() { }

    ResultFileLoading GeomKernel::objectAddFromFile(const std::string& filePath, int& modelObjectId) {
        if (!CheckPathString::checkFileExtension(filePath)) {
            return ResultFileLoading::WRONG_FILE_EXTENSION;
        }
        if (!CheckPathString::checkExistFile(filePath)) {
            return ResultFileLoading::FILE_NOT_FOUND;
        }
        occAdapter = OCCAdapter(filePath);
        if (occAdapter.loadStepFile()) {
            ModelObject_sptr modelObject = transferStepToModuleAssembly();
            mObjectVector.push_back(modelObject);
            modelObjectId = mObjectVector.size() - 1;

            return ResultFileLoading::CREATE_SUCCESSFUL;
        }
        return ResultFileLoading::LOADING_EMPTY;
    }

    void GeomKernel::getModel(const TDF_Label& label,
        ModelBase_sptr& model,
        const ModelProduct_sptr& parent,
        const Quantity_ColorRGBA &color) const {

        std::string name = OCCAdapter::findLabelName(label);
        gp_Trsf transform = OCCAdapter::findTransformation(occAdapter.getShapeAssembly(), label);
        TopoDS_Shape shape = occAdapter.getShapeAssembly()->GetShape(label);
        Quantity_ColorRGBA colShape;
        bool hasColor = occAdapter.findColor(label, colShape);
        if (!hasColor) {
           colShape = color;
        }

        if (OCCAdapter::isProduct(occAdapter.getShapeAssembly(), label)) {
            ModelProduct_sptr product = ModelProduct::Create(parent, transform, name);
            if (parent) {
                parent->push_backModelproduct(product);
            }
            model = product;
            TDF_LabelSequence subLabels;
            if (occAdapter.getShapeAssembly()->GetComponents(label, subLabels, true)) {
                for (const TDF_Label& childLabel : subLabels) {
                    ModelBase_sptr child;
                    TDF_Label refLabel;
                    occAdapter.getShapeAssembly()->GetReferredShape(childLabel, refLabel);
                    getModel(refLabel, child, product, colShape);
                }
            }
            else {
                for (TopoDS_Iterator shapeIt(shape); shapeIt.More(); shapeIt.Next()) {
                    const TopoDS_Shape& subShape = shapeIt.Value();
                    TDF_Label subLabel;
                    bool hasLabel = occAdapter.getShapeAssembly()->Search(subShape, subLabel);
                    if (hasLabel) {
                        if (OCCAdapter::isProduct(occAdapter.getShapeAssembly(), subLabel) || 
                            OCCAdapter::isPart(occAdapter.getShapeAssembly(), subLabel)) {
                            ModelBase_sptr child;
                            getModel(subLabel, child, product, colShape);
                        }
                    }
                    else {
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
        }
        else {
            ModelPart_sptr part = ModelPart::Create(parent, transform, name);
            addTriangulation(shape, part, label, colShape);
            part->setBoundingBox(OCCAdapter::findBoundingBox(shape));
            if (parent) {
                parent->push_backModelpart(part);
            }
            model = part;
        }
    }

    void GeomKernel::addTriangulation(
       const TopoDS_Shape& shape, 
       ModelPart_sptr& part, 
       const TDF_Label& label, 
       const Quantity_ColorRGBA& color
    ) const {
        TopAbs_ShapeEnum type = shape.ShapeType();
        if (type == TopAbs_FACE) {
            OCCAdapter::triangulateShape(shape, DEFAULT_LINEAR_DEFLECTION, DEFAULT_ANGULAR_DEFLECTION);
            const TopoDS_Face& face = TopoDS::Face(shape);
            TopLoc_Location location(part.get()->getTransformation());
            Handle(Poly_Triangulation) triangulation = BRep_Tool::Triangulation(face, location);
            // Just to be secure, all normals are pointing to outside of the mesh
            if (face.Orientation() == TopAbs_Orientation::TopAbs_REVERSED
               || face.Orientation() == TopAbs_Orientation::TopAbs_INTERNAL) {
                for (int i = 1; i <= triangulation->NbTriangles(); i++) {
                    Poly_Triangle triangle = triangulation->Triangle(i);
                    Poly_Triangle triangleNew(triangle(2), triangle(1), triangle(3));
                    triangulation->SetTriangle(i, triangleNew);
                }
            }
            Quantity_ColorRGBA colFace;
            bool hasColor = occAdapter.findColor(label, colFace);
            if (!hasColor) {
                colFace = color;
            }
            Quantity_Color rgb = colFace.GetRGB();
            GraphicInfo gInfo(rgb.Red(), rgb.Green(), rgb.Blue(), colFace.Alpha());
            ModelMesh_sptr mesh = ModelMesh::Create(triangulation, gInfo);
            part->addModelMesh(mesh);
        }
        else {
            for (TopoDS_Iterator iter(shape); iter.More(); iter.Next()) {
                addTriangulation(iter.Value(), part, label, color);
            }
        }
    }


    ModelObject_sptr GeomKernel::transferStepToModuleAssembly() {
        ModelObject_sptr modelObject = ModelObject::Create();
        TDF_LabelSequence freeShapes;
        occAdapter.getShapeAssembly()->GetFreeShapes(freeShapes);
        ModelBase_sptr model;
        ModelProduct_sptr nullParent;
        Quantity_ColorRGBA color;
        for (auto itr = freeShapes.begin(); itr != freeShapes.end(); itr++)
        {
            getModel(*itr, model, nullParent, color);
            if (model)
            {
                modelObject->addRootModel(model);
            }
        }
        return modelObject;
    }

    ModelObject_sptr GeomKernel::transferStepToModuleAssambly() {
        occAdapter.findProductsAndParts();
        const std::map<int, ModelBaseData>& modulAssambly = occAdapter.getModuleAssamblyMap();
        ModelObject_sptr modelObject = ModelObject::Create();
        const ModelProduct_sptr rootModelObject = ModelProduct::Create();
        setModelBase(rootModelObject, modulAssambly.find(1)->second);
        modelObject->addRootModel(rootModelObject);
        for (auto const& it : modulAssambly) {
            int id = it.first;
            const ModelBaseData& assemblyData = it.second;
            ModelBase_sptr partOrProduct = setModelPartOrProduct(assemblyData);
            if (partOrProduct) {
                modelObject->addRootModel(partOrProduct);
            }
        }
        return modelObject;
    }

    const ModelBase_sptr GeomKernel::setModelPartOrProduct(const ModelBaseData& assemblyData) {
        if (assemblyData.type == "PRODUCT") {
            ModelProduct_sptr modelProduct = ModelProduct::Create();
            setModelBase(modelProduct, assemblyData);
            if (assemblyData.childs.size() > 0) {
                for (auto const& childId : assemblyData.childs) {
                    const ModelBaseData& child = occAdapter.getModelBaseDataPerId(childId);
                    setModelPartOrProduct(child);
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

    void GeomKernel::changeLODThreadModelpart(ProgressCallback callback, ModelPart* pModelpart, const int LOD) {
        const double linearDeflectionFactor = LODUtilities::getLODLinearDeflectionFactor(LOD);
        const double angularDeflection = LODUtilities::getLODAngleDeflection(LOD);
        changeLOD(pModelpart, linearDeflectionFactor, angularDeflection);
        callback(pModelpart, LOD);
    }
}

