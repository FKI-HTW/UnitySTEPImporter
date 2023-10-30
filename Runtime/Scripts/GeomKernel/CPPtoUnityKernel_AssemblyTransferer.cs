using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using VENTUS.UnitySTEPImporter.DataIO;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.GeomKernel
{
    class CPPtoUnityKernel_AssemblyTransferer
    {
        internal static class CPPKernelDLLCaller
        {
            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferCountOfModelPARTSInMODELOBJECT(IntPtr pEdGeModelobject);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferCountOfModelPRODUCTSInMODELOBJECT(IntPtr pEdGeModelobject);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferCountOfModelPARTSInMODELPRODUCT(IntPtr pEdGeModelobject);

            [DllImport(CPPtoUnityKernel_Constants.DLLName)]
            internal unsafe static extern int
                transferCountOfModelPRODUCTSInMODELPRODUCT(IntPtr pEdGeModelobject);
        }

        //////////////////////////////////////////////////////////////////////

        public static unsafe Modelobject
            TransferModelobject(int objectManagerID)
        {
            IntPtr pObjectManager = CPPToUnityKernel_CPPPointerRequester.RequestObjectManagerPointer(objectManagerID);

            IntPtr pEdGeModelobject = CPPToUnityKernel_CPPPointerRequester.RequestModelobject(pObjectManager);

            Modelobject modelobject = new Modelobject();

            modelobject.ObjectManagerId = objectManagerID;

            modelobject.CppModelPointer = pEdGeModelobject;
            TransferModelBaseAttributesToModelobject(pEdGeModelobject,
                                                      modelobject);

            modelobject.Submodels = TransferSubmodelsToModelobject(pEdGeModelobject);

            TransformationUtilities.TranslateCoordSysOriginToBBoxCenter(modelobject);

            return modelobject;
        }

        public static unsafe Mesh
            TransferMesh(IntPtr pEdGeModelmesh)
        {
            IntPtr pEdGeMesh
                = CPPToUnityKernel_CPPPointerRequester.RequestMeshPointer(pEdGeModelmesh);

            Mesh mesh = new Mesh();

            // attention: don't switch the order
            // first vertices and second triangles
            mesh.vertices
                = CPPToUnityKernel_AttributTransferer.TransferCoordinates(pEdGeMesh);
            mesh.triangles
                = CPPToUnityKernel_AttributTransferer.TransferIndices(pEdGeMesh);

            return mesh;
        }

        public static unsafe Modelpart
          TransferModelpart(IntPtr pEdGeModelpart)
        {
            Modelpart modelpart = new Modelpart();

            modelpart.CppModelPointer = pEdGeModelpart;

            TransferModelBaseAttributsToSubmodel(pEdGeModelpart, modelpart);

            modelpart.Modelmesh = TransferModelmesh(pEdGeModelpart);

            modelpart.Modelmesh.Mesh.RecalculateBounds();
            modelpart.Modelmesh.Mesh.RecalculateNormals();

            return modelpart;
        }

        //////////////////////////////////////////////////////////////////////

        private static unsafe List<Submodel>
            TransferSubmodelsToModelobject(IntPtr pEdGeModelobject)
        {
            List<Submodel> submodelsOfModelobject = new List<Submodel>();

            List<Submodel> subPartsOfModelobject = TransferModelpartLISTFromModelobject(pEdGeModelobject);
            submodelsOfModelobject.AddRange(subPartsOfModelobject);

            List<Submodel> subProductsInModelobject = TransferModelproductLISTFromModelobject(pEdGeModelobject);
            submodelsOfModelobject.AddRange(subProductsInModelobject);

            return submodelsOfModelobject;
        }

        private static unsafe List<Submodel>
            TransferModelpartLISTFromModelobject(IntPtr pEdGeModelobject)
        {
            List<Submodel> modelPartsOfModelobject = new List<Submodel>();

            int subModelPartsCount
                = CPPKernelDLLCaller.transferCountOfModelPARTSInMODELOBJECT(pEdGeModelobject);
            
            for (int i = 0; i < subModelPartsCount; ++i)
            {
                Modelpart modelpart = TransferModelpartFromModelobject(pEdGeModelobject, i);
                modelPartsOfModelobject.Add(modelpart);
            }

            return modelPartsOfModelobject;
        }

        private static unsafe List<Modelpart>
            TransferModelpartLISTFromModelproduct(IntPtr pEdGeModelproduct)
        {
            List<Modelpart> modelPartsOfModelproduct = new List<Modelpart>();

            int subModelPartsCount
                = CPPKernelDLLCaller.transferCountOfModelPARTSInMODELPRODUCT(pEdGeModelproduct);

            for (int i = 0; i < subModelPartsCount; ++i)
            {
                Modelpart modelpart = TransferModelpartFromModelproduct(pEdGeModelproduct, i);
                modelPartsOfModelproduct.Add(modelpart);
            }

            return modelPartsOfModelproduct;
        }

        private static unsafe List<Submodel>
            TransferModelproductLISTFromModelobject(IntPtr pEdGeModelobject)
        {
            List<Submodel> modelproductsOfModelobject = new List<Submodel>();

            int subModelPartsCount
                = CPPKernelDLLCaller.transferCountOfModelPRODUCTSInMODELOBJECT(pEdGeModelobject);

            for (int i = 0; i < subModelPartsCount; ++i)
            {
                Modelproduct modelpart = TransferModelproductFromModelobject(pEdGeModelobject, i);
                modelproductsOfModelobject.Add(modelpart);
            }

            return modelproductsOfModelobject;
        }

        private static unsafe List<Modelproduct>
            TransferModelproductLISTFromModelproduct(IntPtr pEdGeModelproduct)
        {

            List<Modelproduct> modelproductsOfModelproduct = new List<Modelproduct>();

            int subModelPartsCount
                = CPPKernelDLLCaller.transferCountOfModelPRODUCTSInMODELPRODUCT(pEdGeModelproduct);

            for (int i = 0; i < subModelPartsCount; ++i)
            {
                Modelproduct modelproduct = TransferModelproductFromModelproduct(pEdGeModelproduct, i);
                modelproductsOfModelproduct.Add(modelproduct);
            }

            return modelproductsOfModelproduct;
        }

        //////////////////////////////////////////////////////////////////////

        private static unsafe Modelpart
            TransferModelpartFromModelobject( IntPtr pEdGeModelobject,
                                              int index)
        {
            IntPtr pEdGeModelpart
                = CPPToUnityKernel_CPPPointerRequester.
                  RequestSubPartPointerFromObject( pEdGeModelobject,
                                                   index);
            return TransferModelpart(pEdGeModelpart);
        }

        private static unsafe Modelpart
            TransferModelpartFromModelproduct(IntPtr pEdGeModelproduct,
                                              int index)
        {
            IntPtr pEdGeModelpart
             = CPPToUnityKernel_CPPPointerRequester.RequestSubPartPointerFromProduct(pEdGeModelproduct,
                                                                              index);
            return TransferModelpart(pEdGeModelpart);
        }

        private static unsafe Modelproduct
            TransferModelproductFromModelobject(IntPtr pEdGeModelobject,
                                                int index)
        {
            IntPtr pEdGeSubModelproduct
                = CPPToUnityKernel_CPPPointerRequester.RequestSubProductPointerFromObject(pEdGeModelobject,
                                                                                   index);
            return TransferModelproduct(pEdGeSubModelproduct);
        }

        private static unsafe Modelproduct
            TransferModelproductFromModelproduct(IntPtr pEdGeModelproduct,
                                                 int index)
        {
            IntPtr pEdGeSubModelproduct
                = CPPToUnityKernel_CPPPointerRequester.RequestSubProductPointerFromProduct(pEdGeModelproduct,
                                                                                    index);
            return TransferModelproduct(pEdGeSubModelproduct);
        }

        //////////////////////////////////////////////////////////////////////
      
        private static unsafe Modelproduct
            TransferModelproduct(IntPtr pEdGeModelproduct)
        {
            Modelproduct modelproduct = new Modelproduct();

            modelproduct.CppModelPointer = pEdGeModelproduct;

            TransferModelBaseAttributsToSubmodel(pEdGeModelproduct, modelproduct);
            
            modelproduct.Modelproducts = TransferModelproductLISTFromModelproduct(pEdGeModelproduct);
            
            modelproduct.Modelparts = TransferModelpartLISTFromModelproduct(pEdGeModelproduct);

            return modelproduct;
        }

        //////////////////////////////////////////////////////////////////////

        private static unsafe void
            TransferModelBaseAttributesToModelobject(IntPtr pEdGeModelobject,
                                                     Modelobject modelobject)
        {
            modelobject.Transformation
                = CPPToUnityKernel_AttributTransferer.TransferTransformation(pEdGeModelobject);

            modelobject.Id
                = CPPToUnityKernel_AttributTransferer.TransferID(pEdGeModelobject);

            modelobject.Name
                = CPPToUnityKernel_AttributTransferer.TransferName(pEdGeModelobject);

            if (0 == modelobject.Name.Length)
            {
                modelobject.Name = CPPToUnityKernel_AttributTransferer.createDefaultNameModelobject();
            }

            modelobject.Bounds
                = CPPToUnityKernel_AttributTransferer.TransferBoundingBox(pEdGeModelobject);
        }

        private static unsafe void
            TransferModelBaseAttributsToSubmodel(IntPtr pEdGeModelBase,
                                                 Submodel submodel)
        {
            submodel.Transformation
                = CPPToUnityKernel_AttributTransferer.TransferTransformation(pEdGeModelBase);

            submodel.Id
                = CPPToUnityKernel_AttributTransferer.TransferID(pEdGeModelBase);

            submodel.Name
                = CPPToUnityKernel_AttributTransferer.TransferName(pEdGeModelBase);

            if (0 == submodel.Name.Length)
            {
                if (typeof(Modelpart) == submodel.GetType())
                {
                    submodel.Name = CPPToUnityKernel_AttributTransferer.createDefaultNameModelpart();
                }
                else
                {
                    submodel.Name = CPPToUnityKernel_AttributTransferer.createDefaultNameModelproduct();
                }
            }


            submodel.Bounds
                = CPPToUnityKernel_AttributTransferer.TransferBoundingBox(pEdGeModelBase);
        }

        //////////////////////////////////////////////////////////////////////

        private static unsafe Modelmesh
            TransferModelmesh(IntPtr pEdGeModelpart)
        {
            IntPtr pEdGeModelMesh
                = CPPToUnityKernel_CPPPointerRequester.RequestModelmeshPointer(pEdGeModelpart);

            Modelmesh modelmesh = new Modelmesh();

            modelmesh.Mesh = TransferMesh(pEdGeModelMesh);
            modelmesh.Mesh.RecalculateBounds();
            modelmesh.Mesh.RecalculateNormals();

            modelmesh.Graphicinfo = TransferGraphicinfo(pEdGeModelMesh);

            modelmesh.CppModelPointer = pEdGeModelMesh;

            return modelmesh;
        }

        private static unsafe Graphicinfo
            TransferGraphicinfo(IntPtr pEdGeModelMesh)
        {
            IntPtr pGraphicinfo = CPPToUnityKernel_CPPPointerRequester.RequestGraphicinfo(pEdGeModelMesh);

            Graphicinfo graphicinfo = new Graphicinfo();

            graphicinfo.Color = CPPToUnityKernel_AttributTransferer.TransferColor(pGraphicinfo);

            // currently no texture from cppKernel
            graphicinfo.Texture = new Texture2D(1, 1);

            return graphicinfo;
        }

    }
}
