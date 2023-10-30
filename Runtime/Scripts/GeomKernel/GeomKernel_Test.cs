using VENTUS.UnitySTEPImporter.DataIO;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    class GeomKernel_Test
    {
        static public void TestGeomKernel(string path)
        {
            CPPKernel_Handle.InitCPPKernel();

            int objectManagerID = CPPKernel_Handle.CreateNewObjectManager(path);
            Debug.Log("objectManagerID: " + objectManagerID);

            int objectManagerID2 = CPPKernel_Handle.CreateNewObjectManager(path);
            Debug.Log("objectManagerID: " + objectManagerID2);

            int objectManagerID3 = CPPKernel_Handle.CreateNewObjectManager("BLA.stp");
            Debug.Log("objectManagerID: " + objectManagerID3);

            int objectManagerID4 = CPPKernel_Handle.CreateNewObjectManager("bla.st");
            Debug.Log("objectManagerID: " + objectManagerID4);

        }

        static public void TestLoad(string path)
        {
            CPPKernel_Handle.InitCPPKernel();
            int objectManagerID = CPPKernel_Handle.CreateNewObjectManager(path);



            bool fileToOCCResult
                = CPPKernel_Loader.LoadObjectFromFileToOCC( CPPKernel_Handle.CPPKernel,
                                                            objectManagerID);
            if (!fileToOCCResult)
            {
                return;
            }

            bool OCCtoEdGeResult
                = CPPKernel_Loader.LoadObjectFromOCCToEdGe(CPPKernel_Handle.CPPKernel,
                                                           objectManagerID);

            if (!OCCtoEdGeResult)
            {
                Debug.Log("load OCC to EdGe was not successful");
                return;
            }
        }

        static public void TestTransferToUnity(string path)
        {
            CPPKernel_Handle.InitCPPKernel();
            int objectManagerID = CPPKernel_Handle.CreateNewObjectManager(path);


            bool fileToOCCResult
                = CPPKernel_Loader.LoadObjectFromFileToOCC(CPPKernel_Handle.CPPKernel,
                                                            objectManagerID);
            if (!fileToOCCResult) return;

            bool OCCtoEdGeResult
                = CPPKernel_Loader.LoadObjectFromOCCToEdGe(CPPKernel_Handle.CPPKernel,
                                                           objectManagerID);

            if (!OCCtoEdGeResult) return;
            

            Modelobject modelobject
                = CPPtoUnityKernel_AssemblyTransferer.TransferModelobject(objectManagerID);

            Debug.Log(modelobject.ToString());
        }
    }
}

