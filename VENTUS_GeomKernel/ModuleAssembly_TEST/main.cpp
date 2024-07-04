#include <iostream>

#include "ModelObject.h"
#include "ModelProduct.h"
#include "ModelPart.h"

using namespace std;
using namespace ventus;

void CreateTestAssembly()
{
    cout << "CreateTestAssembly - Begin: - counter = " << ModelBase::getInstanceCounter() << endl << flush;

    gp_Trsf dummyTrafo;

    ModelObject_sptr spModelObj = ModelObject::Create();
    ModelProduct_sptr spRootProd = ModelProduct::Create();
    if (nullptr != spRootProd)
        spRootProd->setName(string("Root-Model"));
    if ( nullptr != spModelObj )
        spModelObj->addRootModel(spRootProd);

    ModelProduct_sptr spProd1 = ModelProduct::Create(spRootProd, dummyTrafo, string("Product1"));
    ModelPart_sptr spPart2 = ModelPart::Create(spRootProd, dummyTrafo, string("Part2"));
    if (nullptr != spRootProd)
    {
        spRootProd->push_backModelproduct(spProd1);
        spRootProd->push_backModelpart(spPart2);
    }

    ModelPart_sptr spPart11 = ModelPart::Create(spProd1, dummyTrafo, string("Part11"));
    ModelPart_sptr spPart12 = ModelPart::Create(spProd1, dummyTrafo, string("Part12"));
    if (nullptr != spProd1)
    {
        spProd1->push_backModelpart(spPart11);
        spProd1->push_backModelpart(spPart12);
    }

    if (nullptr != spModelObj)
    {
        //size_t nParts = spModelObj->getNumberOfParts();
        //cout << "Number of Parts: " << nParts << endl << flush;

        //size_t nProducts = spModelObj->getNumberOfProducts();
        //cout << "Number of Products: " << nProducts << endl << flush;

        //for (int i = 0; i < nParts; ++i)
        {
            //const ModelPart* pPart = spModelObj->getPartPointerByIndex(i);
            //if (pPart)
            {
                //string strName = pPart->getName();
                //cout << "Name von Part[" << i << "]: " << strName << endl << flush;
            }
            //else
                //cout << "Part[" << i << "]: nullptr" << endl << flush;
        }
    }

    if (nullptr != spRootProd)
    {
        size_t nParts = spRootProd->getNumberOfChildParts();
        cout << "Number of Child-Parts: " << nParts << endl << flush;

        size_t nProducts = spRootProd->getNumberOfChildProducts();
        cout << "Number of Child-Products: " << nProducts << endl << flush;

        if (nullptr != spPart2)
        {
            int nID = spPart2->getID();
            const ModelPart* pPart = spRootProd->getChildPartPointerByID(nID);
            if (pPart)
            {
                string strName = pPart->getName();
                cout << "Name von Part2 with ID=" << nID << ": " << strName << endl << flush;
            }
            else
                cout << "Part2: nullptr" << endl << flush;
        }

        int i = 1;
        const ModelProduct* pProduct = spRootProd->getChildProductPointerByIndex(i);
        if (pProduct)
        {
            string strName = pProduct->getName();
            cout << "Name von Child-Product[" << i << "]: " << strName << endl << flush;
        }
        else
            cout << "Child-Product[" << i << "]: nullptr" << endl << flush;
    }

    cout << "CreateTestAssembly - End: - counter = " << ModelBase::getInstanceCounter() << endl << flush;
}

int main()
{
    CreateTestAssembly();
    cout << "After CreateTestAssembly: - counter = " << ModelBase::getInstanceCounter() << endl << flush;

    return 0;
}