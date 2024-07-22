#ifndef _OCC_ADAPTER_h_
#define _OCC_ADAPTER_h_

// SYSTEM INCLUDES
#include <ostream>
#include <map>

// OCC includes
#include "STEPCAFControl_Reader.hxx"
#include "TDocStd_Document.hxx"
#include "IFSelect_ReturnStatus.hxx"
#include "Bnd_Box.hxx"
#include "BRepBndLib.hxx"
#include "TopoDS_Builder.hxx"
#include "XCAFDoc_DocumentTool.hxx"
#include "IMeshTools_Parameters.hxx"
#include "BRepTools.hxx"
#include "BRepMesh_IncrementalMesh.hxx"
#include "StlAPI_Writer.hxx"
#include "STEPControl_Writer.hxx"
#include "STEPControl_StepModelType.hxx"
#include "TDataStd_Name.hxx"
#include "XCAFDoc_ShapeTool.hxx"
#include "TopoDS_Iterator.hxx"
#include "TopLoc_Location.hxx"
#include "NCollection_DataMap.hxx"
#include "XCAFDoc_ColorTool.hxx"
#include "Quantity_Color.hxx"
#include "Quantity_ColorRGBA.hxx"

//maybe onlx for tests
#include "TDF_IDFilter.hxx"
#include "NCollection_StlIterator.hxx"
#include "NCollection_IndexedMap.hxx"
#include "TDF_AttributeIndexedMap.hxx"
#include "TNaming_NamedShape.hxx"

#include "Standard_Dump.hxx"

namespace ventus {

	// GLOBAL CONSTANTS
	static const Standard_Real DEFAULT_LINEAR_DEFLECTION = 1.e-2;
	static const Standard_Real DEFAULT_ANGULAR_DEFLECTION = 1.;

	struct ModelBaseData {

		ModelBaseData():
			name(""),
			type(""),
			transformation(gp_Trsf()),
			boundingBox(Bnd_Box()),
			label(TDF_Label())
		{}

		int iD = 0;
		int parentId = 0; // all who has parent 0 is a root
		std::vector<int> childs;
		std::string name;
		std::string type;
		gp_Trsf transformation;
		Bnd_Box boundingBox;
		TDF_Label label;
		TopoDS_Shape shape;
		Quantity_ColorRGBA color;

		bool operator<(const ModelBaseData model) {
			return iD < model.iD;
		}
	};

	class OCCAdapter {

	public:

		OCCAdapter();

		OCCAdapter(const std::string filePath);

		const Standard_Boolean loadStepFile();

		const std::string typeToString(const TopAbs_ShapeEnum& type);

		const Standard_Boolean prepareTriangulation();

		const Standard_Boolean triangulate(const Standard_Real& linearDeflection, const Standard_Real& angularDeflection);

		static Standard_Boolean triangulateShape(const TopoDS_Shape&, const Standard_Real& linearDeflection, const Standard_Real& angularDeflection);

		inline const Handle(TDocStd_Document)& getDocument() const;

		inline const TopoDS_Shape& getShape() const;

		inline const std::map<int, ModelBaseData>& getModuleAssamblyMap() const;

		inline const Handle(XCAFDoc_ShapeTool)& getShapeAssembly() const;

		static bool isProduct(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label);
		
		static bool isProduct(const TopoDS_Shape &shape);

		static bool isPart(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label);
		
		static bool isPart(const TopoDS_Shape& shape);

		void findParentsAndChilds(const Handle(XCAFDoc_ShapeTool)& assembly);

		static std::string findLabelName(const TDF_Label& label);

		void findProductsAndParts();

		static gp_Trsf findTransformation(const Handle(XCAFDoc_ShapeTool) &h_shapeTool, const TDF_Label& label);

		static Bnd_Box findBoundingBox(const TopoDS_Shape& shape);

		bool findColor(const TDF_Label& label, Quantity_ColorRGBA& color) const;

		void insertDataToModuleAssamblyMap(int id, const TDF_Label& label, const Handle(XCAFDoc_ShapeTool)& assembly, TopoDS_Shape& shape);
		
		void detectObject(const Handle(XCAFDoc_ShapeTool)& assembly, const TDF_Label& label);

		// Data structure operations
		const ModelBaseData& getModelBaseDataPerId(const int id);

		// This is much more for tests
		Standard_Boolean safeTriangulationAsStlFile(std::string stlFilePath);
		IFSelect_ReturnStatus safeShapeAsStep(std::string stpFilePath);

	private:
		
		std::map<int, ModelBaseData> moduleAssamblyMap = std::map<int, ModelBaseData>();

		std::string filePath;

		Handle(TDocStd_Document) loadedDocument;
		Handle(XCAFDoc_ShapeTool) shapeAssembly;
		Handle(XCAFDoc_ColorTool) colorTool;

		IFSelect_ReturnStatus loadStatus;

		Standard_Real linearDeflection;
		Standard_Real angularDeflection;
		Standard_Boolean isTriangulationPrepared = Standard_False;
		Standard_Boolean isTriangulated = Standard_False;
		TopoDS_Shape shapeForTriangulation;
	};

	inline const Handle(TDocStd_Document)& OCCAdapter::getDocument() const {
		return loadedDocument;
	}

	inline const Handle(XCAFDoc_ShapeTool)& OCCAdapter::getShapeAssembly() const {
		return shapeAssembly;
	}

	inline const TopoDS_Shape& OCCAdapter::getShape() const {
		return shapeForTriangulation;
	}

	inline const std::map<int, ModelBaseData>& OCCAdapter::getModuleAssamblyMap() const
	{
		return moduleAssamblyMap;
	}
}
#endif
