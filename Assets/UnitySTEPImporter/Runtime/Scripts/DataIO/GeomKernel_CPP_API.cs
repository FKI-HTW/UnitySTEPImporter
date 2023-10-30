using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VENTUS.UnitySTEPImporter.DataIO
{

    // NOTE: change enums and structs only with the enums and structs in exporter(see ExportInfo.h)

    enum ResultFileLoading
    {
        LOADING_SUCCESSFUL,             // execution was successful, data is loaded
        LOADING_EMPTY,                  // execution was successful but nothing to load
        LOADING_ERROR,                  // execution was NOT successful, because of an error in command or in input data
        LOADING_FAIL,                   // execution was successful and has failed
        LOADING_STOP,                   // indicates end or stop (such as Raise)
        WRONG_FILE_EXTENSION,           // current file extesion is not allowed see in 
        URL_NOT_FOUND,                  // file not found
        LOADING_DEFAULT_ERROR           // developer error
    };

    enum ResultTransfer
    {
        TRANSFER_SUCCESSFUL,              // successful transfer
        TRANSFER_EMPTY,                   // initial state or nothing to transfer in data from reader
        TRANSFER_ERROR,                   // transfer was not successful   
        DATA_NOT_IN_READER,               // data was not red from file 
        TRANSFER_DEFAULT_ERROR            // developer error
    };

    // Define the structure to be sequential and with the correct byte size (3 floats = 4 bytes * 3 = 12 bytes)
    [StructLayout(LayoutKind.Sequential, Size = 12)]
    public struct ImportCoordinate3d
    {
        public float X, Y, Z;
    }

    // Define the structure to be sequential and with the correct byte size (4 float -> 4 bytes * 4 = 16 bytes)
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct ImportRGBAColor
    {
        public float mR, mG, mB, mA;
    };

    // Define the structure to be sequential and with the correct byte size (16 ints -> 4 bytes * 16 = 64 bytes)
    [StructLayout(LayoutKind.Sequential, Size = 64)]
    public struct ImportTransformation
    {
        public float
            mA11, mA12, mA13, mA14,
            mA21, mA22, mA23, mA24,
            mA31, mA32, mA33, mA34,
            mA41, mA42, mA43, mA44;
    };
    // Define the structure to be sequential and with the correct byte size (4 float -> 4 bytes * 6 = 24 bytes)
    [StructLayout(LayoutKind.Sequential, Size = 24)]
    public struct ImportBoundingBox
    {
        public float mXmin, mYmin, mZmin, mXmax, mYmax, mZmax;
    };
    static class GeomKernel_CPP_API
    {

        // interface functions to dll-export functions in c++-dll
        // for import c++Model to c#Model
        internal static class ImportModelInterop
        {
            const string DLLName = "VENTUS_GeomKernel";

            // FUNCTIONS : GEOM_KERNEL AND OBJECT MANAGERS

            [DllImport(DLLName)] internal unsafe static extern void                            // DONE
                initGeomKernel();

            [DllImport(DLLName)] internal unsafe static extern void                            // DONE
                initGeomKernelLOD( int defaultLOD);

            [DllImport(DLLName)] internal unsafe static extern ResultFileLoading                // DONE 
                createObjectManager( string filepathFromUnity, 
                                        ref int refObjectManagerID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr                           // DONE 
                getObjectManager(int objectManagerID);


            //// FUNCTIONS : TRANSFER ACTIONS 

            [DllImport(DLLName)] internal unsafe static extern ResultFileLoading               //DONE
                transferFileToOCC( int objectManagerID);

            [DllImport(DLLName)] internal unsafe static extern ResultTransfer                  //DONE
                transferOCCToEdGe( int objectManagerID);

            [DllImport(DLLName)] internal unsafe static extern ResultTransfer                  //DONE
                transferOCCToEdGeLOD( int objectManagerID, 
                                        int LOD);


            // FUNCTIONS : ASSEMBLY GETTER

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getPEdGeModelobject( int objectManagerID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getPartFromObject( IntPtr pEdGeModelBase,
                                    int i);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getPartFromProduct( IntPtr pEdGeModelBase,
                                    int i);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getProductFromProduct( IntPtr pEdGeModelproduct,
                                        int i);

            [DllImport(DLLName)]
            internal unsafe static extern IntPtr                                //DONE
                getProductFromObject( IntPtr pEdGeModelObject,
                                        int i);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getPartOverID( IntPtr pObjectManager,
                                int partID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getProductOverID( IntPtr pObjectManager, 
                                    int productID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getPartFromObjectOverID( IntPtr pModelobject, 
                                            int modelpartID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getPartFromProductOverID( IntPtr pModelproduct, 
                                            int modelpartID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getProductFromObjectOverID( IntPtr pModelobject, 
                                            int modelproductID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr           //DONE
                getProductFromProductOverID( IntPtr pModelproduct, 
                                                int modelproductID);


            // FUNCTIONS: PART ATTRIBUTS GETTER

            [DllImport(DLLName)] internal unsafe static extern IntPtr                     // DONE
            getBoundingBoxCppPointer(IntPtr pModelpart );

            [DllImport(DLLName)] internal unsafe static extern IntPtr                     // DONE 
                getModelmesh( IntPtr pEdGeModelpart);

            [DllImport(DLLName)] internal unsafe static extern IntPtr                     // DONE 
                getMesh( IntPtr pEdGeModelmesh);

            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getIndicesCount( IntPtr pEdGeExportMesh, 
                                    ref int refIndicesCount);

            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getIndices( IntPtr pEdGeExportMesh, 
                            int* indicesArray);

            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getCoordinateCount( IntPtr pEdGeExportMesh, 
                                    ref int refCoordinateCount);

            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getCoordinate( IntPtr pEdGeExportMesh,
                                IntPtr pBoundingBoxOfModelpart,
                                int index, 
                                ref ImportCoordinate3d CoordinateArray);
            
            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getColor( IntPtr pEdGeModelmesh, 
                            ref ImportRGBAColor refImportRGBAColor);

            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getTransformation( IntPtr pModelBase,
                                    ref ImportTransformation ExportTransformation);

            [DllImport(DLLName)] internal unsafe static extern void                     // DONE 
                getID( IntPtr pModelBase, 
                        ref int refID);

            [DllImport(DLLName)] internal unsafe static extern IntPtr                   // DONE 
                getName( IntPtr pModelBase);
            
            [DllImport(DLLName)] internal unsafe static extern void                      // DONE 
                getBoundingBoxOfModelpart( IntPtr pEdGeModelpart,
                                            ref ImportBoundingBox refBoundingBox);

            [DllImport(DLLName)]
            internal unsafe static extern void                                          // DONE
                getBoundingBoxOfModelproduct(IntPtr pEdGeModelproduct,
                                            ref ImportBoundingBox refBoundingBox);

            [DllImport(DLLName)]
            internal unsafe static extern void                                          // DONE
                getBoundingBoxOfModelobject(IntPtr pEdGeModelobject,
                                            ref ImportBoundingBox refBoundingBox);

            // FUNCTIONS: CHANGE LOD 

            [DllImport(DLLName)] internal unsafe static extern bool                     // DONE
                changeLODModelpart( IntPtr pEdGemodelpart, 
                                    int newLOD);

            [DllImport(DLLName)] internal unsafe static extern bool                     // DONE
                changeLODModelproduct( IntPtr pEdGemodelproduct,
                                        int newLOD);

            [DllImport(DLLName)] internal unsafe static extern bool                     // DONE
                changeLODModelobject( IntPtr pEdGemodelobject,
                                        int newLOD);



            // Functions are not used

            //[DllImport(DLLName)] internal unsafe static extern IntPtr 
            //    createStepLoader( string URLFromUnity);

            //[DllImport(DLLName)] internal unsafe static extern ResultFileLoading 
            //    transferStpFileToOCC( IntPtr pStepLoader);

            //[DllImport(DLLName)] internal unsafe static extern ResultFileLoading 
            //    transferOCCToEdGe( IntPtr pStepLoader);

            //[DllImport(DLLName)] internal unsafe static extern IntPtr 
            //    getObject( IntPtr pStepLoader);




            // FUNCTIONS:  DEBUGS (some to kill)

            [DllImport(DLLName)]
            internal unsafe static extern int
                testGetNumberOfObjectManager();

            [DllImport(DLLName)]
            internal unsafe static extern void
                writeHierarchyCodeFromEdGe(string outputFilePath,
                                            int objectManagerID);

            [DllImport(DLLName)]
            internal unsafe static extern void
                writeHierarchyCodeFromOCC(string outputFilePath,
                                            int objectManagerID);

            
            [DllImport(DLLName)]
            internal unsafe static extern void
                DebugLogEdGeModel(string directoryPath,
                                    IntPtr pStepLoader);

            [DllImport(DLLName)]
            internal unsafe static extern void
                DebugLogOCCModel(string directoryPath,
                                    IntPtr pStepLoader);

            [DllImport(DLLName)]
            internal unsafe static extern void
                writeInformationAboutOCCModel(string outputFilePath,
                                                int objectManagerID);

            [DllImport(DLLName)]
            internal unsafe static extern void
                writeInformationAboutEdGeModelobject(string outputFilePath,
                                                        int objectManagerID);
        
            [DllImport(DLLName)]
            internal unsafe static extern void
                TESTString();

            [DllImport(DLLName)] internal unsafe static extern IntPtr 
                TEST_getString();
        }

        public static unsafe void                                                                   // DONE
            InitGeomKernel(int defaultLOD)
        {
            ImportModelInterop.initGeomKernelLOD(defaultLOD);
        }

        public static unsafe int                                             // DONE
            CreateNewObjectManager(string filepath)
        {
            int objectManagerID = 0;

            ResultFileLoading result 
                = ImportModelInterop.createObjectManager( filepath, 
                                                            ref objectManagerID);
            CheckLoadingResult( result, 
                                filepath);

            return objectManagerID;
        }

        public static unsafe Modelobject 
            getModelobject( IntPtr pEdGeModelobject)
        {
            Modelobject modelobject = new Modelobject();

            modelobject.Transformation = getTransformation(pEdGeModelobject);

            modelobject.Id = getID(pEdGeModelobject);

            modelobject.Name = getName(pEdGeModelobject);

            modelobject.CppModelPointer = pEdGeModelobject;

            modelobject.Bounds = getBoundingBoxOfModelobject(pEdGeModelobject);
            
            // START::load sub Modelparts from Modelobject

            bool doGetModelpart = true;

            int i = 0;

            Modelpart modelpart = null;

            do
            {
                modelpart = getModelpartFromObject( pEdGeModelobject, 
                                                    i);

                if (null == modelpart)
                {
                    doGetModelpart = false;
                    continue;
                }

                ++i;

                modelobject.Submodels.Add(modelpart);

            } while (doGetModelpart);
            // END::load sub Modelparts from Modelobject

            // START::load sub Modelproducts from Modelobject
            bool doGetModelproduct = true;
            int j = 0;

            do
            {
                Modelproduct modelproduct 
                    = getModelproductFromObject(pEdGeModelobject, j);

                if (null == modelproduct)
                {
                    doGetModelproduct = false;
                    continue;
                }

                ++j;

                modelobject.Submodels.Add(modelproduct);

            } while (doGetModelproduct);
            // END::load sub Modelproducts from Modelobject

            return modelobject;
        }

        public static unsafe bool                                           // DONE
            LoadObjectFromFileToOCC(int objectManagerID)
        {
            ResultFileLoading resultTransferFileToOCC 
                = ImportModelInterop.transferFileToOCC(objectManagerID);
            CheckLoadingResult(resultTransferFileToOCC);

            return ResultFileLoading.LOADING_SUCCESSFUL 
                == resultTransferFileToOCC;
        }

        public static unsafe bool                                           // DONE
            LoadObjectFromOCCToEdGe(int objectManagerID)
        {
            ResultTransfer resultOCCToEdGe 
                = ImportModelInterop.transferOCCToEdGe(objectManagerID);

            CheckTransferResult(resultOCCToEdGe);

            return true;
        }

        public static unsafe bool
            LoadObjectFromOCCToEdGeLOD(int objectManagerID, 
                                        int LOD)
        {
            ResultTransfer resultOCCToEdGe 
                = ImportModelInterop.transferOCCToEdGeLOD
                    ( objectManagerID, 
                        LOD);

            CheckTransferResult(resultOCCToEdGe);

            ImportModelInterop.writeInformationAboutEdGeModelobject
            (   "C:/Users/tag/Documents/ORDNER/TESTS/UNITY_DEBUG_TESTS/" 
                + "transformation_test.log",
                    objectManagerID
            );

            return true;
        }

        public static unsafe Modelobject
            getModelobjectFromKernel(int objectManagerID)
        {
            IntPtr pEdGeModelobject 
                = ImportModelInterop.getPEdGeModelobject(objectManagerID);

            if (null == pEdGeModelobject)
            {
                return null;
            }

            Modelobject modelobject = getModelobject(pEdGeModelobject);

            TransformationUtilities.TranslateCoordSysOriginToBBoxCenter(modelobject);

            return modelobject;
            
        }

        public static unsafe bool
            ChangeLOD( int objectManagerID, 
                        Modelobject modelobject, 
                        int newLOD)
        {
            IntPtr pEdGeModelobject
                = modelobject.CppModelPointer;

            bool isChanged 
                = ImportModelInterop.changeLODModelobject( pEdGeModelobject, 
                                                            newLOD);

            if (!isChanged)
            {
                return false;
            }

            return TransferNewMesh( pEdGeModelobject, 
                                    modelobject);
        }

        public static unsafe bool 
            ChangeLOD( int objectManagerID, 
                        Modelproduct modelproduct, 
                        int newLOD)
        {
            IntPtr pObjectManager 
                = ImportModelInterop.getObjectManager(objectManagerID);

            IntPtr pEdGeModelproduct
                = modelproduct.CppModelPointer;

            bool isChanged 
                = ImportModelInterop.changeLODModelproduct( pEdGeModelproduct, 
                                                            newLOD);
            if (!isChanged)
            {
                return false;
            }

            return TransferNewMesh( pEdGeModelproduct,
                                    modelproduct);
        }

        public static unsafe bool
            ChangeLOD( int objectManagerID, 
                        Modelpart modelpart, 
                        int newLOD)
        {
            IntPtr pObjectManager 
                = ImportModelInterop.getObjectManager(objectManagerID);



            IntPtr pEdGeModelpart
                = modelpart.CppModelPointer;

            bool isChanged 
                = ImportModelInterop.changeLODModelpart( pEdGeModelpart, 
                                                            newLOD);

            if (!isChanged)
            {
                return false;
            }
            modelpart.Bounds = getBoundingBoxOfModelpart(modelpart.CppModelPointer);

            return TransferNewMesh( pEdGeModelpart,
                                    modelpart);
        }


        /// PRIVATE METHODS 
        private static unsafe void
            setBoundingBoxAsMesh( ImportBoundingBox bBox, 
                                    Modelpart modelpart)
        {
            int[] triangles = new int[36];

            Vector3[] coordinates = new Vector3[8];

            createBoundingBoxMesh( bBox, 
                                    ref triangles, 
                                    ref coordinates);

            modelpart.Modelmesh.Mesh.triangles = triangles;

            modelpart.Modelmesh.Mesh.vertices = coordinates;

            modelpart.Modelmesh.Mesh.RecalculateBounds();
            modelpart.Modelmesh.Mesh.RecalculateNormals();
        }

        private static unsafe void 
            createBoundingBoxMesh( ImportBoundingBox bBox, 
                                    ref int[] triangles, 
                                    ref Vector3[] coordinates)
        {
            // TODO
        }

        private static unsafe Bounds
            getBoundingBoxOfModelpart( IntPtr pEdGeModelpart)
        {
            ImportBoundingBox boundingBox = new ImportBoundingBox();

            ImportModelInterop.getBoundingBoxOfModelpart( pEdGeModelpart, 
                                                            ref boundingBox);
            return importBoundingBox(ref boundingBox);
        }

        private static unsafe Bounds
            getBoundingBoxOfModelproduct( IntPtr pEdGeModelproduct)
        {
            ImportBoundingBox boundingBox = new ImportBoundingBox();

            ImportModelInterop.getBoundingBoxOfModelproduct( pEdGeModelproduct,
                                                                ref boundingBox);
            return importBoundingBox(ref boundingBox);
        }

        private static unsafe Bounds 
            getBoundingBoxOfModelobject( IntPtr pEdGeModelobject)
        {
            ImportBoundingBox boundingBox = new ImportBoundingBox();

            ImportModelInterop.getBoundingBoxOfModelobject(pEdGeModelobject,
                                                            ref boundingBox);
            return importBoundingBox(ref boundingBox);
        }

        private static unsafe Modelpart
            getModelpartFromObject( IntPtr pEdGeModelobject, 
                                    int index)
        {
            Modelpart modelpart = null;

            IntPtr pEdGeModelpart 
                = ImportModelInterop.getPartFromObject( pEdGeModelobject, 
                                                        index);
            if (!pEdGeModelpart.Equals(IntPtr.Zero))
            {
                createModelpart(pEdGeModelpart, ref modelpart);
            }

            return modelpart;
        }

        private static unsafe Modelpart
            getModelpartFromProduct( IntPtr pEdGeModelproduct, 
                                        int index)
        {
            Modelpart modelpart = null;

            IntPtr pEdGeModelpart 
                = ImportModelInterop.getPartFromProduct( pEdGeModelproduct, 
                                                            index);
            if (!pEdGeModelpart.Equals(IntPtr.Zero))
            {
                createModelpart(pEdGeModelpart, ref modelpart);
            }

            return modelpart;
        }

        private static unsafe Modelproduct
            getModelproductFromObject( IntPtr pEdGeModelobject, 
                                        int index)
        {
            Modelproduct modelproduct = null;

            IntPtr pEdGeModelproduct 
                = ImportModelInterop.getProductFromObject( pEdGeModelobject, 
                                                            index);

            if (!pEdGeModelproduct.Equals(IntPtr.Zero))
            {
                modelproduct = null;

                createModelproduct( pEdGeModelproduct, 
                                    ref modelproduct);

                // START::load sub Modelparts
                bool doGetModelpart = true;

                int i = 0;

                do
                {
                    Modelpart modelpart = null;
                    modelpart = getModelpartFromProduct( pEdGeModelproduct, 
                                                            i);

                    if (null == modelpart)
                    {
                        doGetModelpart = false;

                        continue;
                    }

                    ++i;

                    modelproduct.Modelparts.Add(modelpart);

                } while (doGetModelpart);
                // END::load sub Modelparts

                // START::load sub Modelproducts
                bool doGetModelproduct = true;

                int j = 0;

                do
                {
                    Modelproduct subModelproduct
                        = getModelproductFromProduct( pEdGeModelproduct,
                                                        j);
                    if (null == subModelproduct)
                    {
                        doGetModelproduct = false;

                        continue;
                    }

                    ++j;

                    modelproduct.Modelproducts.Add(subModelproduct);

                } while (doGetModelproduct);
                // END::load sub Modelproducts
            }

            return modelproduct;
        }

        private static unsafe Modelproduct
            getModelproductFromProduct( IntPtr pEdGeModelproduct, 
                                        int index)
        {
            Modelproduct modelproduct = null;

            IntPtr pSubEdGeModelproduct 
                = ImportModelInterop.getProductFromProduct( pEdGeModelproduct,
                                                            index);

            if (!pSubEdGeModelproduct.Equals(IntPtr.Zero))
            {
                modelproduct = null;
                createModelproduct( pSubEdGeModelproduct, 
                                    ref modelproduct);

                // START::load sub Modelparts
                bool doGetModelpart = true;
                int i = 0;

                do
                {
                    Modelpart modelpart = null;
                    modelpart = getModelpartFromProduct( pSubEdGeModelproduct, 
                                                            i);

                    if (null == modelpart)
                    {
                        doGetModelpart = false;
                        continue;
                    }

                    ++i;
                    modelproduct.Modelparts.Add(modelpart);

                } while (doGetModelpart);
                // END::load sub Modelparts

                // START::load sub Modelproducts
                bool doGetModelproduct = true;
                int j = 0;
                do
                {
                    Modelproduct subModelproduct = null;
                    subModelproduct 
                        = getModelproductFromProduct( pSubEdGeModelproduct, 
                                                        j);

                    if (null == subModelproduct)
                    {
                        doGetModelproduct = false;
                        continue;
                    }

                    ++j;
                    modelproduct.Modelproducts.Add(subModelproduct);

                } while (doGetModelproduct);
                // END::load sub Modelproducts
            }

            return modelproduct;
        }

        private static unsafe void 
            createModelpart( IntPtr pEdGeModelpart, 
                                ref Modelpart modelpart)
        {
            modelpart = new Modelpart();

            modelpart.Transformation = getTransformation(pEdGeModelpart);

            modelpart.Id = getID(pEdGeModelpart);

            modelpart.Name = getName(pEdGeModelpart);

            modelpart.Modelmesh = getModelmesh(pEdGeModelpart);

            modelpart.CppModelPointer = pEdGeModelpart;

            modelpart.Bounds = getBoundingBoxOfModelpart(pEdGeModelpart);
        }

        private static unsafe void 
            createModelproduct( IntPtr pEdGeModelproduct, 
                                ref Modelproduct modelproduct)
        {
            modelproduct = new Modelproduct();

            modelproduct.Transformation 
                = getTransformation(pEdGeModelproduct);

            modelproduct.Id = getID(pEdGeModelproduct);

            modelproduct.Name = getName(pEdGeModelproduct);

            modelproduct.CppModelPointer = pEdGeModelproduct;

            modelproduct.Bounds 
                = getBoundingBoxOfModelproduct(pEdGeModelproduct);
        }

        private static unsafe Modelmesh 
            getModelmesh(IntPtr pEdGeModelpart)
        {
            IntPtr pEdGeModelMesh 
                = ImportModelInterop.getModelmesh(pEdGeModelpart);

            Modelmesh modelmesh = null;

            IntPtr pEdGeExportMesh 
                = ImportModelInterop.getMesh(pEdGeModelMesh);

            if (!pEdGeExportMesh.Equals(IntPtr.Zero))
            {
                modelmesh = new Modelmesh();

                // load indices 
                int indicesCount = new int();

                ImportModelInterop.getIndicesCount( pEdGeExportMesh, 
                                                    ref indicesCount);
                int[] indicesArray = new int[indicesCount];
                getIndices(pEdGeExportMesh, indicesArray);

                // need for translate coordinate system 
                // to center of bounding box
                IntPtr pBoundingBoxOfModelpart 
                    = ImportModelInterop.getBoundingBoxCppPointer
                        (pEdGeModelpart);
                
                // load coordinates
                Vector3[] coordinateVectorArray 
                    = getCoordinates( pEdGeExportMesh, 
                                        pBoundingBoxOfModelpart);

                // write attributes to C# classes
                modelmesh.Mesh.vertices = coordinateVectorArray;

                modelmesh.Mesh.triangles = indicesArray;

                modelmesh.Mesh.RecalculateBounds();
                modelmesh.Mesh.RecalculateNormals();

                modelmesh.Graphicinfo.Color = getColor(pEdGeModelMesh);
            }

            return modelmesh;
        }

        private static unsafe bool
            TransferNewMesh( IntPtr pEdGeModelobject, 
                                Modelobject modelobject)
        {
            foreach (Submodel currSubmodel in modelobject.Submodels)
            {
                if (typeof(Modelpart) == currSubmodel.GetType())
                {
                    IntPtr pCurrentEdGeModelpart 
                        = ImportModelInterop.getPartFromObjectOverID
                            ( pEdGeModelobject, 
                                currSubmodel.Id);

                    bool isTransfered = 
                        TransferNewMesh( pCurrentEdGeModelpart, 
                                            (Modelpart)currSubmodel);

                    if (!isTransfered)
                    {
                        return false;
                    }
                }

                if (typeof(Modelproduct) == currSubmodel.GetType())
                {
                    IntPtr pCurrentEdGeModelproduct 
                        = ImportModelInterop.getProductFromObjectOverID
                            ( pEdGeModelobject,
                            currSubmodel.Id);

                    bool isTransfered 
                        = TransferNewMesh( pCurrentEdGeModelproduct, 
                                            (Modelproduct)currSubmodel);

                    if (!isTransfered)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static unsafe bool
            TransferNewMesh( IntPtr pEdGeModelproduct, 
                                Modelproduct modelproduct)
        {
            foreach (Modelpart currModelpart in modelproduct.Modelparts)
            {
                IntPtr pCurrentEdGeModelpart 
                    = ImportModelInterop.getPartFromProductOverID
                        ( pEdGeModelproduct, 
                        currModelpart.Id);

                bool isTransfered = TransferNewMesh( pCurrentEdGeModelpart, 
                                                        currModelpart);

                if (!isTransfered)
                {
                    return false;
                }
            }

            foreach ( Modelproduct currModelproduct 
                    in modelproduct.Modelproducts)
            {
                IntPtr pCurrentEdGeModelproduct 
                    = ImportModelInterop.getProductFromProductOverID
                        ( pEdGeModelproduct, 
                            currModelproduct.Id);

                bool isTransfered
                    = TransferNewMesh( pCurrentEdGeModelproduct, 
                                        currModelproduct);

                if (!isTransfered)
                {
                    return false;
                }
            }
            return true;
        }

        private static unsafe bool
            TransferNewMesh(IntPtr pEdGeModelpart,
                            Modelpart modelpart)
        {
            IntPtr pEdGeModelMesh 
                = ImportModelInterop.getModelmesh(pEdGeModelpart);

            IntPtr pEdGeExportMesh 
                = ImportModelInterop.getMesh(pEdGeModelMesh);

            // load indices 
            int indicesCount = new int();
            ImportModelInterop.getIndicesCount( pEdGeExportMesh, 
                                                ref indicesCount);
            int[] indicesArray = new int[indicesCount];
            getIndices(pEdGeExportMesh, indicesArray);

            // load coordinates
            IntPtr pBoundingBoxcppPointer
                = ImportModelInterop.getBoundingBoxCppPointer
                    (pEdGeModelpart);

            Vector3[] coordinateVectorArray 
                = getCoordinates( pEdGeExportMesh,
                                    pBoundingBoxcppPointer);

            // write attributes to C# classes
            modelpart.Modelmesh.Mesh.vertices = coordinateVectorArray;
            modelpart.Modelmesh.Mesh.triangles = indicesArray;

            TransformationUtilities.TranslateCoordSysOriginToBBoxCenterNewMesh(modelpart);

            modelpart.Modelmesh.Mesh.RecalculateBounds();
            modelpart.Modelmesh.Mesh.RecalculateNormals();

            return true;
        }


        ///// Begin: import attributs
        /////
        private static unsafe void 
            getIndices( IntPtr pExportMesh, 
                        int[] arrayIndices)
        {
            fixed (int* fixedArrayIndices = arrayIndices)
            {
                ImportModelInterop.getIndices( pExportMesh, 
                                                fixedArrayIndices);
            }
        }

        private static unsafe Matrix4x4 
            getTransformation(IntPtr pEdGeModelBase)
        {
            ImportTransformation importTransformation 
                = new ImportTransformation();

            ImportModelInterop.getTransformation(pEdGeModelBase, 
                                                    ref importTransformation);

            return importTransformationToMatrix4x4(importTransformation);
        }

        private static unsafe int 
            getID(IntPtr pEdGeModelBase)
        {
            int id = new int();

            ImportModelInterop.getID(pEdGeModelBase, ref id);

            return id;
        }

        private static unsafe string 
            getName(IntPtr pEdGeModelBase)
        {
            IntPtr loadedNameCharPointer = 
                ImportModelInterop.getName(pEdGeModelBase);

            return Marshal.PtrToStringAnsi(loadedNameCharPointer);
        }

        private static unsafe Vector3[] 
            getCoordinates( IntPtr pEdGeExportMesh, 
                            IntPtr pBoundingBoxOfModelpart)
        {
            int coordinateCount = new int();

            ImportModelInterop.getCoordinateCount(pEdGeExportMesh, 
                                                    ref coordinateCount);

            ImportCoordinate3d[] importCoordinateArray 
                = new ImportCoordinate3d[coordinateCount];

            for (int i = 0; i < coordinateCount; ++i)
            {
                ImportModelInterop.getCoordinate(pEdGeExportMesh, 
                                                    pBoundingBoxOfModelpart, 
                                                    i, 
                                                    ref importCoordinateArray[i]);
            }

            return ImportCoordinateToVector3Array(importCoordinateArray);
        }

        private static unsafe Color
            getColor(IntPtr pEdGeModelmesh)
        {
            ImportRGBAColor importColor = new ImportRGBAColor();

            ImportModelInterop.getColor( pEdGeModelmesh, 
                                            ref importColor);

            return new Color( importColor.mR, 
                                importColor.mG, 
                                importColor.mB, 
                                importColor.mA);
        }

        /////
        ///// End: import Attributs

        ///// begin: convert in to unity class
        /////
        // for an array of ImportCoordinate3d
        private static unsafe Vector3[] 
            ImportCoordinateToVector3Array(ImportCoordinate3d[] importCoordinateArray)
        {
            Vector3[] array = new Vector3[importCoordinateArray.Length];

            for (int i = 0; i < importCoordinateArray.Length; ++i)
            {
                array[i] = ImportCoordinateToVector3(importCoordinateArray[i]);
            }
            return array;
        }

        //for only one ImportCoordinate3d
        private static unsafe Vector3 
            ImportCoordinateToVector3(ImportCoordinate3d importCoordinate)
        {
            return new Vector3( importCoordinate.X, 
                                importCoordinate.Y, 
                                importCoordinate.Z);
        }

        private static Matrix4x4 
            importTransformationToMatrix4x4(ImportTransformation importTransformation)
        {
            return new Matrix4x4
                (
                    new Vector4( importTransformation.mA11, 
                                    importTransformation.mA12, 
                                    importTransformation.mA13, 
                                    importTransformation.mA14),
                    new Vector4( importTransformation.mA21, 
                                    importTransformation.mA22, 
                                    importTransformation.mA23, 
                                    importTransformation.mA24),
                    new Vector4( importTransformation.mA31, 
                                    importTransformation.mA32, 
                                    importTransformation.mA33, 
                                    importTransformation.mA34),
                    new Vector4( importTransformation.mA41, 
                                    importTransformation.mA42,
                                    importTransformation.mA43, 
                                    importTransformation.mA44)
                );
        }

        private static Bounds
            importBoundingBox( ref ImportBoundingBox importBoundBox)
        {
            float originX = (importBoundBox.mXmax + importBoundBox.mXmin) / 2;
            float originY = (importBoundBox.mYmax + importBoundBox.mYmin) / 2;
            float originZ = (importBoundBox.mZmax + importBoundBox.mZmin) / 2;

            Vector3 origin = new Vector3(originX, originY, originZ);

            float sizeX = Mathf.Abs(importBoundBox.mXmax - importBoundBox.mXmin);
            float sizeY = Mathf.Abs(importBoundBox.mYmax - importBoundBox.mYmin);
            float sizeZ = Mathf.Abs(importBoundBox.mZmax - importBoundBox.mZmin);

            Vector3 size = new Vector3(sizeX, sizeY, sizeZ);

            return new Bounds(origin,size);
        }

        private static void 
            CheckLoadingResult(ResultFileLoading resultLoad, string URL = "")
        {
            switch (resultLoad)
            {
                case ResultFileLoading.LOADING_SUCCESSFUL:
                    Debug.Log("Model loaded successful from file to OCC.");
                    break;
                case ResultFileLoading.LOADING_EMPTY:
                    //TODO:EXCEPTION
                    Debug.Log( "Model loading successful but there is no data "
                                + "or not faces to triangulate for the import.");
                    break;
                case ResultFileLoading.LOADING_ERROR:
                    //TODO:EXCEPTION
                    Debug.Log("Model loading was incorrect.");
                    break;
                case ResultFileLoading.LOADING_FAIL:
                    //TODO:EXCEPTION
                    Debug.Log("Model loading was started and failed.");
                    break;
                case ResultFileLoading.LOADING_STOP:
                    //TODO:EXCEPTION
                    Debug.Log("Model loading was stoped.");
                    break;
                case ResultFileLoading.WRONG_FILE_EXTENSION:
                    //TODO:EXCEPTION
                    Debug.Log("File extension in " + URL + " is not allowed.");
                    break;
                case ResultFileLoading.URL_NOT_FOUND:
                    //TODO:EXCEPTION
                    Debug.Log("URL " + URL + " was not found.");
                    break;
            }
        }               // DONE 

        private static void 
            CheckTransferResult(ResultTransfer resultTransfer)
        {
            switch (resultTransfer)
            {
                case ResultTransfer.DATA_NOT_IN_READER:
                    // TODO EXCEPTION
                    Debug.Log( "Reading before Transfer was not done " 
                                + "or was not successful.");
                    break;
                case ResultTransfer.TRANSFER_DEFAULT_ERROR:
                    // TODO EXCEPTION
                    Debug.Log("Developer error.");
                    break;
                case ResultTransfer.TRANSFER_EMPTY:
                    // TODO EXCEPTION
                    Debug.Log( "Initial state or nothing to transfer " +
                                "in data from reader.");
                    break;
                case ResultTransfer.TRANSFER_ERROR:
                    // TODO EXCEPTION
                    Debug.Log("Transfer error.");
                    break;
                case ResultTransfer.TRANSFER_SUCCESSFUL:
                    // TODO EXCEPTION
                    Debug.Log("Transfer was successful.");
                    break;
            }
        }                               // DONE

        private static unsafe int 
            TESTGetNumberOfObjectManager()
        {
            return ImportModelInterop.testGetNumberOfObjectManager();
        }

        public static unsafe void 
            CreateDebugLogFileAboutEdGeAssembly( string outputFilePath, 
                                                    int objectManagerID)
        {
            ImportModelInterop.writeInformationAboutEdGeModelobject( outputFilePath, 
                                                                        objectManagerID);
        }
    }
}