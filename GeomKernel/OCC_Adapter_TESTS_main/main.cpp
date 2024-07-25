//#include <cstdlib>
//#include <iomanip>
#include <iostream>
#include <cassert>

// ventus_ADAPTER INCLUDES
// OCC
#include "XCAFDoc_DocumentTool.hxx"
#include "STEPControl_Reader.hxx"
#include "BRepMesh_IncrementalMesh.hxx"
#include "BRepTools.hxx"
#include "BRep_Tool.hxx"
#include "TopoDS.hxx"
#include "StlAPI_Writer.hxx"
#include "STEPControl_Writer.hxx"
#include "TDataStd_Name.hxx"
#include "StepRepr_RepresentationItem.hxx"
#include "StepBasic_Product.hxx"
#include "BRep_Builder.hxx"
#include "Interface_Static.hxx"
#include "Interface_InterfaceModel.hxx"
#include "XSControl_WorkSession.hxx"
#include "XSControl_TransferReader.hxx"
#include "Standard_Type.hxx"
#include "StepBasic_ProductDefinition.hxx"
#include "StepShape_TopologicalRepresentationItem.hxx"
#include "StepGeom_GeometricRepresentationItem.hxx"
#include "Transfer_Binder.hxx"
#include "NCollection_DataMap.hxx"
#include "Transfer_TransientProcess.hxx"
#include "TransferBRep.hxx"
#include "StepBasic_ProductDefinitionFormation.hxx"
#include "TDF_ChildIDIterator.hxx"
#include "TNaming_NamedShape.hxx"
#include "TNaming_Builder.hxx"
#include "TDataStd_TreeNode.hxx"


// OCC_ADAPTER INCLUDES
#include "OCCAdapter.h"

using namespace ventus;
using namespace std;

int main() {

    std::string filePath = "../Test_Files/mixed.stp";
    cout << "------------------------------------------------------------------------------------------------------------------------------------" << endl;
    cout << "File to read: " << filePath << endl;

    OCCAdapter occAdapter = OCCAdapter(filePath);
    if (occAdapter.loadStepFile() && occAdapter.triangulate(DEFAULT_LINEAR_DEFLECTION, DEFAULT_ANGULAR_DEFLECTION)) {
        Standard_Boolean stlSaved = occAdapter.safeTriangulationAsStlFile("Test_Files/triangulated_locher.stl");
        occAdapter.findProductsAndParts();
    }

    Handle(XCAFDoc_ColorTool) myColors = XCAFDoc_DocumentTool::ColorTool(occAdapter.getDocument()->Main());
    XCAFDoc_ColorType aColType = XCAFDoc_ColorGen;


    const std::map<int, ModelBaseData>& modulAssambly = occAdapter.getModuleAssamblyMap();
    cout << "------------------------------------------------------------------------------------------------------------------------------------" << endl;
    cout << "Assembley structure read from given file:" << endl;
    cout << "------------------------------------------------------------------------------------------------------------------------------------" << endl;
       
    
    for (auto const& it : modulAssambly) {
        int id = it.first;
        const ModelBaseData& modelBase = it.second;
        const TDF_Label& label = modelBase.label;
        const TopoDS_Shape shape = modelBase.shape;
        const Quantity_ColorRGBA color = modelBase.color;
        int depth = !label.IsNull() ? label.Depth() : 0;
        

        // Colors
        aColType = XCAFDoc_ColorCurv;
        if (!myColors->IsSet(label, aColType)) {
            Quantity_Color aColForShape;           
            // will receive the recorded value (if there is some)
            aColType = XCAFDoc_ColorSurf;
            myColors->GetColor(label, aColType, aColForShape);
            aColForShape.DumpJson(std::cout);
            std::cout << std::endl;
        }


        cout << "| ID : " << id
            << " | Name : " << modelBase.name
            << " | Type : " << modelBase.type
            << " | Parent : " << modelBase.parentId
            << " | Color : ";
        color.DumpJson(cout);
        cout << " |";
        cout << endl;

        TDF_AttributeIndexedMap attrMap = TDF_AttributeIndexedMap();

        for (int itrNb = 1; itrNb <= attrMap.Size(); itrNb++) {
            //Handle(TDF_Attribute) 
            auto key = attrMap.FindKey(itrNb);
            Standard_GUID id = key->ID();
            label.FindAttribute(id, key);
            if (!key.IsNull()) {
                cout << "attrId : " << itrNb << " : " << endl;
                //key->DumpJson(cout);
                TDF_AttributeIndexedMap& map = TDF_AttributeIndexedMap();
                const TDF_IDFilter& fil = TDF_IDFilter(true);
                key->ExtendedDump(cout, fil, map);
                cout << endl;
            }
        }
        
        cout << endl;
        cout << "------------------------------------------------------------------------------------------------------------------------------------" << endl;
    }

    return 0;
}