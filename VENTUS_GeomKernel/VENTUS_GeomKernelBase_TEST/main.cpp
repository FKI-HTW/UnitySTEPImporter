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

using namespace ventus;

void showModelBase(const ModelBase_sptr& modelBase)
{
	if (modelBase->getType() == ModelType::PART)
	{
		std::cout << "ModelPart: " << modelBase->getName() << std::endl;
		std::cout << modelBase->getID() << std::endl;
		std::cout << "Bounding Box: " << std::endl;
		modelBase->getBoundingBox().Dump();
		ModelPart_sptr part = std::dynamic_pointer_cast<ModelPart>(modelBase);
		const std::vector<ModelMesh_sptr>& meshes = part->getModelMeshes();
		for (const ModelMesh_sptr& mesh : meshes)
		{
			cout << "color R : " << mesh->getGraphicInfo().getColor().getR() << endl;
			cout << "color G : " << mesh->getGraphicInfo().getColor().getG() << endl;
			cout << "color B : " << mesh->getGraphicInfo().getColor().getB() << endl;
			cout << "color A : " << mesh->getGraphicInfo().getColor().getA() << endl;
			cout << "Face triangulated with " << mesh->getNumberOfPoints3d() << " points and "
				  << mesh->getNumberOfTriangles() << " triangles" << endl;
		}
	}
	else
	{
		std::cout << "ModelProduct: " << modelBase->getName() << std::endl;
		std::cout << modelBase->getID() << std::endl;
	}
}

void showModelBaseStructure(const ModelBase_sptr& modelBase)
{
	showModelBase(modelBase);
	ModelType modeltype = modelBase->getType();
	if (modeltype == ModelType::PRODUCT)
	{
		ModelProduct_sptr prod = std::dynamic_pointer_cast<ModelProduct>(modelBase);
		if (prod != nullptr)
		{
			ModelBaseVector_sptr modelbasevector_sptr = prod->getChildren();
			{
				for (auto itr = modelbasevector_sptr->begin(); itr != modelbasevector_sptr->end(); itr++)
				{
					showModelBaseStructure(*itr);
				}
			}
		}
	}
}

int main()
{
	//std::string filePath = "Test_Files/Locher.stp";
	//std::string filePath = "D:/Modelldaten/STEP_Modelle/VENTUS2_Testmodelle/STP_VW/Radioknopf.stp";
	//std::string filePath = "Test_Files/000000000_mixed.stp"; // mehrere Objekte!!!
	//std::string filePath = "D:/Modelldaten/STEP_Modelle/Verkehrsschilder/RONDE.stp";
	std::string filePath = "../Test_Files/qube.stp";

	std::cout << "------------------------------------------------------------------------------------------------------------------------------------" << std::endl;
	std::cout << "File to read: " << filePath << std::endl;

	int id = -1;
	GeomKernel kernel = GeomKernel();
	ResultFileLoading fileLoading = kernel.objectAddFromFile(filePath, id);

	if (fileLoading == ResultFileLoading::CREATE_SUCCESSFUL)
		std::cout << "File: " << filePath << " was loaded successfully" << endl;
	else
		std::cout << "File: " << filePath << " has following error: " << fileLoading << endl;

	const ModelObject* modelObject = kernel.getModelObject(id);

	vector<ModelBase_sptr> roots = modelObject->getRootModels();

	for (auto root : roots) {
		showModelBaseStructure(root);
	}

	return 0;
}
