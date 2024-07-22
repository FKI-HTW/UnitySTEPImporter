#include <iostream>
#include <string>

//Project Includes 
#include "SetUpsLoader.h"
#include "SetUpsLOD.h"

//Module Assembyl Includes

#include "ModelBase.h"
#include "ModelObject.h"
#include "ModelProduct.h"
#include "ModelPart.h"

//GeomKernel Includes
#include "GeomKernel.h"
#include "Export_GeomKernel.h"

using namespace std;
using namespace ventus;
using namespace Export;


void showModelBase(const ModelBase_sptr& modelBase) {
	std::cout << "Type : " << modelBase->getName();
	std::cout << ", ID : " << modelBase->getID() << std::endl;
	cout << "BoundingBox : ";
	modelBase->getBoundingBox().Dump();
	
	if (modelBase->getType() ==  ModelType::PART) {		
		 ModelPart_sptr part = std::dynamic_pointer_cast< ModelPart>(modelBase);
		const std::vector< ModelMesh_sptr>& meshes = part->getModelMeshes();
		for (const  ModelMesh_sptr& mesh : meshes) {
		//	cout << "color R : " << mesh->getGraphicInfo().getColor().getR() << endl;
		//	cout << "color G : " << mesh->getGraphicInfo().getColor().getG() << endl;
		//	cout << "color B : " << mesh->getGraphicInfo().getColor().getB() << endl;
		//	cout << "color A : " << mesh->getGraphicInfo().getColor().getA() << endl;
			cout << "Face triangulated with " << mesh->getNumberOfPoints3d() << " points and "
				<< mesh->getNumberOfTriangles() << " triangles" << endl;

			int nb = mesh->getNumberOfTriangles();
			const int nbOfInd = transferNumberOfTriangles(mesh.get());		
		}
	}
}

void showModelBaseStructure(const  ModelBase_sptr& modelBase) {
	showModelBase(modelBase);
	 ModelType modeltype = modelBase->getType();
	if (modeltype ==  ModelType::PRODUCT) {
		 ModelProduct_sptr prod = std::dynamic_pointer_cast< ModelProduct>(modelBase);
		if (prod != nullptr) {
			 ModelBaseVector_sptr modelbasevector_sptr = prod->getChildren();
			for (auto itr = modelbasevector_sptr->begin(); itr != modelbasevector_sptr->end(); itr++) {
				showModelBaseStructure(*itr);
			}
		}
	}
}

int main() {
	string filePath = "../Test_Files/mixed.stp";

	GeomKernel* geomKernel = initGeomKernelLOD(2);

	int id = -1;
	ResultFileLoading loadResult = loadModelObjectFromFile(filePath.c_str(), geomKernel, id);
	if (loadResult != ResultFileLoading::CREATE_SUCCESSFUL) {
		return -1;
	}
	const ModelObject* modelObject = getModelObject(geomKernel, id);

	int numberOfRoots = transferNumberOfRoots(modelObject);
	vector<ModelBase_sptr> roots = modelObject->getRootModels();

	for (auto root : roots) {
		ExportBoundingBox box(0, 0, 0, 0, 0, 0);
		transferBoundingBox(root.get(), box);
		cout << box.mXmax << endl;
		showModelBaseStructure(root);
	}

	return 0;
}
