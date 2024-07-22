#include "OCCAdapter.h"

namespace ventus {

    OCCAdapter::OCCAdapter() {}

    OCCAdapter::OCCAdapter(const std::string filePath ) :
        filePath(filePath),
        loadStatus(IFSelect_ReturnStatus::IFSelect_RetStop),
        loadedDocument(nullptr),
        linearDeflection(DEFAULT_LINEAR_DEFLECTION),
        angularDeflection(DEFAULT_ANGULAR_DEFLECTION)
    {}

    const Standard_Boolean OCCAdapter::loadStepFile() {
        STEPCAFControl_Reader reader;

        reader.SetNameMode(Standard_True);
        reader.SetColorMode(Standard_True);
        reader.SetPropsMode(Standard_True);
        reader.SetGDTMode(Standard_True);
        reader.SetLayerMode(Standard_True);
        reader.SetSHUOMode(Standard_True);
        reader.SetMatMode(Standard_True);

        loadStatus = reader.ReadFile(filePath.c_str());
        if (IFSelect_ReturnStatus::IFSelect_RetDone == loadStatus) {
            loadedDocument = new TDocStd_Document("MDTV-XCAF");
            Standard_Boolean loadStatus = reader.Transfer(loadedDocument) && prepareTriangulation();
            shapeAssembly = XCAFDoc_DocumentTool::ShapeTool(loadedDocument->Main());
            colorTool = XCAFDoc_DocumentTool::ColorTool(loadedDocument->Main());
            return loadStatus;
        }
        return Standard_False;
    }

    const Standard_Boolean OCCAdapter::prepareTriangulation() {
        STEPControl_Reader reader;
        IFSelect_ReturnStatus readStatusForTriangulation = reader.ReadFile(filePath.c_str());
        if (readStatusForTriangulation == IFSelect_RetDone) {
            reader.TransferRoots();
            shapeForTriangulation = reader.OneShape();
            isTriangulationPrepared = Standard_True;
            return isTriangulationPrepared;
        }
        return Standard_False;
    }

    const Standard_Boolean OCCAdapter::triangulate(const Standard_Real& lDeflection, const Standard_Real& aDeflection) {
       if (isTriangulationPrepared)
          return triangulateShape(shapeForTriangulation, lDeflection, aDeflection);
       return Standard_False;
    }

    Standard_Boolean OCCAdapter::triangulateShape(const TopoDS_Shape &shape, const Standard_Real& lDeflection, const Standard_Real& aDeflection) {
        if(!shape.IsNull()) {
            IMeshTools_Parameters parameters;
            parameters.Deflection = lDeflection;
            parameters.Angle = aDeflection;
            BRepTools::Clean(shape);
            BRepMesh_IncrementalMesh triangulator(shape, parameters);
            return Standard_True;
        } 
        return Standard_False;
    }

    Standard_Boolean OCCAdapter::safeTriangulationAsStlFile(std::string stlFilePath) {
        if (isTriangulated && !shapeForTriangulation.IsNull()) {
            StlAPI_Writer writer;
            return writer.Write(shapeForTriangulation, stlFilePath.c_str());
        }
        return Standard_False;
    }

    IFSelect_ReturnStatus OCCAdapter::safeShapeAsStep(std::string stpFilePath) {
        if (!shapeForTriangulation.IsNull()) {
            STEPControl_Writer writer;
            writer.Transfer(shapeForTriangulation, STEPControl_AsIs);
            return writer.Write(stpFilePath.c_str());
        }
        return IFSelect_RetError;
    }

    const std::string OCCAdapter::typeToString(const TopAbs_ShapeEnum& type) {
        switch (type) {
            case TopAbs_COMPOUND:   return "COMPOUND";
            case TopAbs_COMPSOLID:  return "COMPSOLID";
            case TopAbs_SOLID:      return "SOLID";
            case TopAbs_SHELL:      return "SHELL";
            case TopAbs_FACE:       return "FACE";
            case TopAbs_WIRE:       return "WIRE";
            case TopAbs_EDGE:       return "EDGE";
            case TopAbs_VERTEX:     return "VERTEX";
            case TopAbs_SHAPE:      return "SHAPE";
            default:;
        }
        return "ERROR";
    }

    bool OCCAdapter::isProduct(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label) 
    {
        const TopoDS_Shape& shape = assembly->GetShape(label);
        return isProduct(shape);
    }

    bool OCCAdapter::isProduct(const TopoDS_Shape& shape)
    {
       return TopAbs_COMPOUND == shape.ShapeType() || TopAbs_COMPSOLID == shape.ShapeType();
    }

    bool OCCAdapter::isPart(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label)
    {
       const TopoDS_Shape& shape = assembly->GetShape(label);
       return isPart(shape);
    }

    bool OCCAdapter::isPart(const TopoDS_Shape& shape)
    {
        // TODO : part selection logic has to be applied!
        return TopAbs_SOLID == shape.ShapeType();
           //|| TopAbs_SHELL == shape.ShapeType()
           //|| TopAbs_FACE == shape.ShapeType()
           //|| TopAbs_WIRE == shape.ShapeType()
           //|| TopAbs_EDGE == shape.ShapeType()
           //|| TopAbs_VERTEX == shape.ShapeType()
           //|| TopAbs_SHAPE == shape.ShapeType();
    }

    void OCCAdapter::findParentsAndChilds(const Handle(XCAFDoc_ShapeTool)& assembly) {
        for (auto it : moduleAssamblyMap) {
            int parentId = it.first;
            ModelBaseData& parentModelBase = it.second;
            const TDF_Label& parentLabel = parentModelBase.label;
            if (assembly->IsFree(parentLabel)) {
                parentModelBase.parentId = 0;
            }
            if (assembly->NbComponents(parentLabel, true) > 0) {
                TDF_LabelSequence childLabels = TDF_LabelSequence();
                assembly->GetComponents(parentLabel, childLabels, true);
                std::vector<int> childs;
                for (auto const& child : childLabels) {
                    const TDF_Label& childLabel = child;
                    TDF_Label reffered = TDF_Label();
                    assembly->GetReferredShape(childLabel, reffered);
                    for (auto it2 : moduleAssamblyMap) {
                        int childId = it2.first;
                        ModelBaseData& modelBaseChild = it2.second;
                        if (reffered.Tag() == modelBaseChild.label.Tag()) {
                            childs.push_back(childId);
                            modelBaseChild.parentId = parentId;
                        }
                    }
                }
                parentModelBase.childs = childs;
            }
        }
    }

    std::string OCCAdapter::findLabelName(const TDF_Label& label) {
        std::string name("");
        Handle(TDataStd_Name) nameOCC;
        if (!label.IsNull()) {
            const bool nameFound = label.FindAttribute(TDataStd_Name::GetID(), nameOCC);
            if (nameFound) {
                char nameChar[200] = "";
                Standard_PCharacter namePChar = nameChar;
                nameOCC->Get().ToUTF8CString(namePChar);
                name = nameChar;
            }
        }
        return name;
    }

    void OCCAdapter::findProductsAndParts() {
        Handle(XCAFDoc_ShapeTool) assembly = XCAFDoc_DocumentTool::ShapeTool(loadedDocument->Main());

        TDF_LabelSequence shapes;
        assembly->GetShapes(shapes);
        for (TDF_LabelSequence::iterator iter = shapes.begin(); iter != shapes.end(); ++iter) {
            detectObject(assembly, *iter);
        }
        findParentsAndChilds(assembly);
    }

    gp_Trsf OCCAdapter::findTransformation(const Handle(XCAFDoc_ShapeTool)& h_shapeTool, const TDF_Label& label) {
        const TopLoc_Location location = h_shapeTool->GetLocation(label);
        const gp_Trsf transformation = location.Transformation();
        return transformation;
    }

    Bnd_Box OCCAdapter::findBoundingBox(const TopoDS_Shape& shape) {
        Bnd_Box boundingBox = Bnd_Box();
        BRepBndLib::Add(shape, boundingBox);
        boundingBox.SetGap(0.0);
        return boundingBox;
    }

    bool OCCAdapter::findColor(const TDF_Label& label, Quantity_ColorRGBA &color) const {
        bool foundColor = colorTool->GetColor(label, XCAFDoc_ColorType::XCAFDoc_ColorSurf, color);
        if (! foundColor) 
           foundColor = colorTool->GetColor(label, XCAFDoc_ColorType::XCAFDoc_ColorGen, color);
        if (!foundColor)
           foundColor = colorTool->GetColor(label, XCAFDoc_ColorType::XCAFDoc_ColorCurv, color);
        return foundColor;
    }

    void OCCAdapter::insertDataToModuleAssamblyMap(int id, const TDF_Label& label, const Handle(XCAFDoc_ShapeTool)& assembly, TopoDS_Shape& shape) {
        ModelBaseData modelBaseData;
        modelBaseData.iD = id;
        modelBaseData.name = findLabelName(label);
        modelBaseData.type = isProduct(assembly, label) ? "PRODUCT" : "PART";
        modelBaseData.transformation = findTransformation(shapeAssembly, label);
        modelBaseData.boundingBox = findBoundingBox(shape);
        modelBaseData.label = label;
        if (!shape.IsNull())
            modelBaseData.shape = shape;
        findColor(label, modelBaseData.color);

        moduleAssamblyMap.insert(std::make_pair(id, modelBaseData));
    }

    void OCCAdapter::detectObject(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label) {

        TDF_LabelSequence subLabels;
        TopoDS_Shape shape = assembly->GetShape(label);
        TDF_Label subLabel;
        Standard_Integer id = label.Tag();

        insertDataToModuleAssamblyMap(int(id), label, assembly, shape);
        
        if (!assembly->GetComponents(label, subLabels)) {
            for (TopoDS_Iterator shapeIt(shape); shapeIt.More(); shapeIt.Next()){
                const TopoDS_Shape& subShape = shapeIt.Value();                
                bool hasLabel = assembly->Search(subShape, subLabel);
                if (hasLabel && (isProduct(assembly, subLabel) || isPart(assembly, subLabel))) {
                    detectObject(assembly, subLabel);
                }
            }
        }
    }

    // Data structure operations
    const ModelBaseData& OCCAdapter::getModelBaseDataPerId(const int id) {
        auto const& it = moduleAssamblyMap.find(id);
        return it->second;
    }

}